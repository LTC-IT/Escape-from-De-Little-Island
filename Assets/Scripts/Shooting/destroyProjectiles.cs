using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyProjectiles : MonoBehaviour {

	public float lifeTime = 1.0f;

	// Use this for initialization
	void Awake () {
		Destroy(gameObject, lifeTime);
	}
	
}


