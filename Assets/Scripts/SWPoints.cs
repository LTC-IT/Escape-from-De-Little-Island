using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SWPoints : MonoBehaviour {

	private Text textfield;
	private int score;
	public AudioSource[] audioSources;
	private AudioSource pointsAudio;
	public AudioClip pointsSound;


	void Awake() {
		textfield = GameObject.Find("Canvas/scoreText").GetComponent<Text>();
		audioSources = GetComponents<AudioSource>();
		pointsAudio = audioSources[1];
	}

	void OnControllerColliderHit (Collider collisionInfo) {
    	if (collisionInfo.gameObject.tag == "swpoints") {
			pointsAudio.clip = pointsSound;
			pointsAudio.Play();
    		Debug.Log("You collected a ball");
        	Destroy(collisionInfo.gameObject);
        	score++;
        	textfield.text = score.ToString();
        	if (score >=3) {
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
            	//SceneManager.LoadScene("levelTwo"); // Change Level2 to the name of the next scene
        	} // end of if
    	} // end of if
	} // end of function
} // end of class