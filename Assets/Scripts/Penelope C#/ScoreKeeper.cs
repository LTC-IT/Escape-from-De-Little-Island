//////////////////////////////////////////////////////////////
// Scorekeeper.cs
// Penelope iPhone Tutorial
//
// Scorekeeper keeps track of the player's score, both the deposited
// and carried points. It also manages the game timer to keep
// track of how long until the game ends. The scorekeeper keeps
// references to the gui elements which display the score and time
// so that those gui elements can be updated whenever the values
// change.
//////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Text;

public class ScoreKeeper : MonoBehaviour
{


    public int carrying;
    public int carryLimit;
    public int deposited;
    public int winScore;                                // How many orbs must be deposited to win

    public int gameLength;                              // Length in seconds

    public GameObject guiMessage;                       // Prefab for one-shot messages


    // GUIText objects that must be assigned in editor
    public GUIText carryingGui;
    public GUIText depositedGui;
    public GUIText timerGui;


    // Sound fx and voices for different events
    public AudioClip[] collectSounds;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip pickupSound;
    public AudioClip depositSound;
    private float timeSinceLastPlay;                    // Last time we played a voice for pickup
    private float timeLeft;

    void Start()
    {
        timeLeft = gameLength;
        timeSinceLastPlay = Time.time;
        UpdateCarryingGui();
        UpdateDepositedGui();
        StartCoroutine(CheckTime());
    }

    void UpdateCarryingGui()
    {
        carryingGui.text = "Carrying: " + carrying + " of " + carryLimit;   
    }

    void UpdateDepositedGui()
    {
        depositedGui.text = "Deposited: " + deposited + " of " + winScore;  
    }

    void UpdateTimerGui()
    {
        timerGui.text = "Time: " + TimeRemaining();     
    }

    IEnumerator CheckTime()
    {
        // Rather than using Update(), use a co-routine that controls the timer.
        // We only need to check the timer once every second, not multiple times
        // per second.
        while (timeLeft > 0)
        {
            UpdateTimerGui();       
            yield return new WaitForSeconds(1);
            timeLeft -= 1;
        }
        UpdateTimerGui();
        StartCoroutine(EndGame());
    }    

    // This is a utility function to a play one shot audio at a specific position
    // and at a specific volume
    AudioSource PlayAudioClip(AudioClip clip, Vector3 position, float volume)
    {
        GameObject go = new GameObject("One shot audio");
        go.transform.position = position;
        AudioSource source = go.AddComponent<AudioSource>();
        source.rolloffMode = AudioRolloffMode.Logarithmic;
        source.clip = clip;
        source.volume = volume;
        source.Play();
        Destroy(go, clip.length);
        return source;
    }

    IEnumerator EndGame()
    {
        AnimationController animationController = GetComponent<AnimationController>();
        GameObject prefab = Instantiate(guiMessage) as GameObject;
        GUIText endMessage = prefab.GetComponent<GUIText>();

        if (deposited >= winScore)
        {
            //Player wins
            endMessage.text = "You win!";
            PlayAudioClip(winSound, Vector3.zero, 1.0f);
            animationController.animationTarget.Play("WIN");
        } else
        {
            //Player loses
            endMessage.text = "Oh no...You lose!";
            PlayAudioClip(loseSound, Vector3.zero, 1.0f);      
            animationController.animationTarget.Play("LOSE");     
        }
 
        // Alert other components on this GameObject that the game has ended
        SendMessage("OnEndGame");
 
        while (true)
        {
            // Wait for a touch before reloading the intro level
            yield return new WaitForFixedUpdate();
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                break;
        }
 
        Application.LoadLevel(0);
    }

    public void Pickup(ParticlePickup pickup)
    {
        if (carrying < carryLimit)
        {
            carrying++;
            UpdateCarryingGui();        
     
            // We don't want a voice played for every pickup as this would be annoying.
            // Only allow a voice to play with a random percentage of chance and only
            // after a minimum time has passed.
            var minTimeBetweenPlays = 5;
            if (Random.value < 0.1 && Time.time > (minTimeBetweenPlays + timeSinceLastPlay))
            {
                PlayAudioClip(collectSounds[Random.Range(0, collectSounds.Length)], Vector3.zero, 0.25f);
                timeSinceLastPlay = Time.time;
            }
     
            pickup.Collected(); 
            PlayAudioClip(pickupSound, pickup.transform.position, 1.0f);
        } else
        {
            GameObject warning = Instantiate(guiMessage) as  GameObject;
            warning.GetComponent<GUIText>().text = "You can't carry any more";
            Destroy(warning, 2);
        }
 
        // Show the player where to deposit the orbs
        // if (carrying >= carryLimit)
        //     pickup.emitter.SendMessage("ActivateDepository");     
    }

    public void Deposit()
    {
        deposited += carrying;
        carrying = 0;
        UpdateCarryingGui();
        UpdateDepositedGui();
        PlayAudioClip(depositSound, transform.position, 1.0f);     
    }

    string TimeRemaining()
    {
        int remaining = (int)timeLeft;
        StringBuilder val = new StringBuilder();
        if (remaining > 59) 
           // val += remaining / 60 + ".";
            val.AppendFormat("{0}.",remaining/60); // Insert # of minutes
 
        if (remaining >= 0) // Add # of seconds
        {
            string seconds = (remaining % 60).ToString();
            if (seconds.Length < 2)
                //val += "0" + seconds; 
                val.AppendFormat("0{0}",seconds); // insert leading 0
            else
                //val += seconds;
                val.Append(seconds);
        }
     
        return val.ToString();
    }
}
