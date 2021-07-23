using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour {

	public int allowedTime = 10;
	private Text textField;
	private int currentTime;

	void Awake () {
		commonValues.currentTime = allowedTime;
		textField = GetComponent<Text>();
		UpdateTimerText();	
		StartCoroutine(Tick());
	}
	
	// Update the GUI
	void UpdateTimerText() {
		textField.text = commonValues.currentTime.ToString();
	}

	IEnumerator Tick() {
		Debug.Log(commonValues.currentTime);
		while (commonValues.currentTime > 0) {
			yield return new WaitForSeconds(1);
			commonValues.currentTime--;
			UpdateTimerText();
	}
	yield return new WaitForSeconds(3);
	SceneManager.LoadScene(0);
	}
}


