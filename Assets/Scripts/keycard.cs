using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keycard : MonoBehaviour {

	public bool keycardFound = false;

	void onTriggerEnter(Collider collisionInfo) {
		if(collisionInfo.gameObject.tag == "keycard") {
			keycardFound = true;
			Destroy(collisionInfo.gameObject);
			Debug.Log(keycardFound);
		}

		if(collisionInfo.gameObject.tag == "door") {
			Debug.Log("door");
			if(keycardFound) {
				Destroy(collisionInfo.gameObject);
				Debug.Log("Door Opening");
			}
		}
	}
}
