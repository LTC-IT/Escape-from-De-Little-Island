using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShooting : MonoBehaviour {
	public Transform projectilePrefab;
	public float forceMultiplier = 5000.0f;

	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Transform projectile = Instantiate(projectilePrefab, transform.Find("playerShootSpawn").transform.position, Quaternion.identity);
			projectile.gameObject.tag = "projectile";
			projectile.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, transform.up);
			projectile.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * forceMultiplier);
		}
	}
}
