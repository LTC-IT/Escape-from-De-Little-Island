using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUILives : MonoBehaviour {

	public Sprite lives1;
	public Sprite lives2;
	public Sprite lives3;

	public static int GUIDeath = 3;

	private Image livesImage;

	// Use this for initialization
	void Start () {
		livesImage = gameObject.GetComponentInChildren<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		switch(GUIDeath) { // Which value is GUIDeath?
		case 3: 
			livesImage.sprite = lives3; 
			break; // If 3, then change to the lives3 graphic
		case 2: 
			livesImage.sprite = lives2; 
			break; // If 2, then change to the lives2 graphic
		case 1: 
			livesImage.sprite = lives1; 
			break; // If 1, then change to the lives1 graphic
		case 0: 
			//Application.LoadLevel(0); 
			break;    // If 0, then Go back to the main menu
	}
	}
}



