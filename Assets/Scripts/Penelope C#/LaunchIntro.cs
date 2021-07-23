//////////////////////////////////////////////////////////////
// LaunchIntro.cs
// Penelope iPhone Tutorial
//
// LaunchIntro marhsals the separate elements that compose the
// introductory sequence to the game.
//////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class LaunchIntro : MonoBehaviour {
    
    public AudioClip rumbleSound;
    public AudioClip boomSound;
    //public ParticleEmitter spawnParticleEmitter;
    
    private Transform thisTransform;
    private AudioSource thisAudio;

	// Use this for initialization
	void Start () {
        
        // Cache component lookups at startup instead of doing this every frame
        thisTransform = transform;
        thisAudio = GetComponent<AudioSource>();
        
        // Play the rumble sound, which leads up to the boom
        thisAudio.PlayOneShot(rumbleSound, 1.0f);
        
        // Repeatedly shake the camera randomly until the boom
        InvokeRepeating("CameraShake", 0f, 0.05f);
        
        // Launch the particles after the rumble sound has completed
        Invoke("Launch", rumbleSound.length);
	
	}
    
    void CameraShake()
    {
        // Pick a random rotation to shake the camera
        Vector3 eulerAngles = new Vector3 ( Random.Range(0,5), Random.Range(0,5), 0);
        thisTransform.localEulerAngles = eulerAngles;
    }
    
    void Launch()
    {
        // Launch the (fake) particles, play the boom sound and cancel any further
        // camera shaking
        //spawnParticleEmitter.emit = true;
        thisAudio.PlayOneShot(boomSound, 1.0f);
        Invoke("CancelInvoke", 0.5f);
        
    }
	
}
