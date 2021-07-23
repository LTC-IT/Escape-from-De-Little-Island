using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class causeDamage : MonoBehaviour {

	private void OnTriggerEnter(Collider hit) {
		if(hit.gameObject.tag == "enemy"){
			enemyHits.damage++;
			Debug.Log(enemyHits.damage);
			if(enemyHits.damage >= 3) {
				Destroy(hit.gameObject);
				enemyHits.damage = 0;
			}
		}
	}
}

