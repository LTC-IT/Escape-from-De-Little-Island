using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Points : MonoBehaviour {

	private Text textfield;
	private int score;
	public AudioSource[] audioSources;
	private AudioSource pointsAudio;
	public AudioClip pointsSound;


	void Awake() {
		textfield = GameObject.Find("Canvas/scoreText").GetComponent<Text>();
		//audioSources = GetComponents<AudioSource>();
		//pointsAudio = audioSources[1];
	}

	void OnTriggerEnter (Collider collisionInfo) {
    	if (collisionInfo.gameObject.tag == "points") {
			//pointsAudio.clip = pointsSound;
			//pointsAudio.Play();
    		Debug.Log("You collected a ball");
        	Destroy(collisionInfo.gameObject);
        	commonValues.points++;
        	textfield.text = commonValues.points.ToString();
        	if (commonValues.points >=3) {
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
            	//SceneManager.LoadScene("levelTwo"); // Change Level2 to the name of the next scene
        	} // end of if
    	} // end of if
	} // end of function
} // end of class