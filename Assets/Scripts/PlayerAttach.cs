using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttach : MonoBehaviour {

	public GameObject Player;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject == Player){
			Debug.Log("Attach");
			Player.transform.parent = transform;
		}
	}

		void OnTriggerExit(Collider other) {
		if (other.gameObject == Player){
			Debug.Log("Detach");
			Player.transform.parent = null;
		}
	}
}
