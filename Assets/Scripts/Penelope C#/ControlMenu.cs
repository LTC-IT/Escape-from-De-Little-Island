//////////////////////////////////////////////////////////////
// ControlMenu.cs
// Penelope iPhone Tutorial
//
// ControlMenu creates the menu from which the player can choose
// which control scheme to play. It makes use of Unity's GUILayout
// system to create buttons. The menu loads a background
// image so that the player can't see the transitions between
// the different scenes which contain the control schemes.
//////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class ControlMenu : MonoBehaviour
{
    [System.Serializable]
    public class ControllerScene
    {
        public string label;                                // The label to show on the button
        public string controlScene;                         // The file name of the unity scene without extension
    }
    
    public ControllerScene[] controllers;
    public Texture2D backGround;                        // A background to show to cover loading the control setup levels
    public bool display;                                // Whether to display the button menu or not
    public Font font;                                   // Font used for the buttons
    public Transform[] destroyOnLoad;                   // Objects in scene that should be destroyed when control scheme is loaded
    public GameObject launchIntro;                      // The GameObject hierarchy for the launch intro
    public GameObject orbEmitter;                       // The GameObject that launches the real collectibles
    

    private int selection = -1;                         // Button selected
    private bool displayBackground = false;             // Toggle for background display
    
    
    
    // Co-routine to hold any further execution while an object still exists
    IEnumerator WaitUntilObjectDestroyed(Object o)
    {
        while (o)
            yield return new WaitForFixedUpdate();
    }
    
    IEnumerator ChangeControls()
    {
        
        // Destroy objects that are no longer needed
        foreach (Transform t in destroyOnLoad)
            Destroy(t.gameObject);
        
        // Kick off the launch intro and wait until it has finished
        launchIntro.SetActiveRecursively(true);
        
        yield return StartCoroutine(WaitUntilObjectDestroyed(launchIntro));
        
        displayBackground = true; // display a background image to cover the load
        
        // Emit the real orbs and load the control scheme
        orbEmitter.GetComponent<Renderer>().enabled = true; 
        Application.LoadLevelAdditive(controllers [selection].controlScene);
        
        // wait at least a second to allow level to load
        Destroy(gameObject, 1.0f);
    }
    
    void OnGUI()
    {
        GUI.skin.font = font;
        if (displayBackground)
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backGround, ScaleMode.StretchToFill, false);
        
        if (display)
        {
            int hit = -1;
            float minHeight = 60.0f;
            float areaWidth = 400.0f;
            GUILayout.BeginArea(new Rect((Screen.width - areaWidth) / 2, (Screen.height - minHeight) / 2, areaWidth, minHeight));
            GUILayout.BeginHorizontal();
            
            for (int i = 0; i < controllers.Length; i++)
            {
                // Show the buttons for all the separate control schemes
                if (GUILayout.Button(controllers [i].label, GUILayout.MinHeight(minHeight)))
                {
                    hit = i;
                }
            }
            
            // If we received a selection, then load those controls
            if (hit >= 0)
            {
                selection = hit;
                GetComponent<GUITexture>().enabled = false;
                display = false;
                displayBackground = false;
                StartCoroutine(ChangeControls());
                
            }
            
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    
    }
    
    void Start()
    {
        // Make sure these are disabled initially
        launchIntro.SetActiveRecursively(false);
        orbEmitter.GetComponent<Renderer>().enabled = false;
    }
    
    void Update()
    {
        if (!display && selection == -1 && Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                
                // Check whether we are getting a touch and that it is within the bounds of
                // the title graphic
                if (touch.phase == TouchPhase.Began && GetComponent<GUITexture>().HitTest(touch.position))
                {
                    display = true;
                    displayBackground = false;
                    GetComponent<GUITexture>().enabled = false;
                }
            }
        }
    }

}
