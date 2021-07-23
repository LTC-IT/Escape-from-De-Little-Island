using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {

    private bool dead = false;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "respawn") {
            dead = true;
        }
    }
    
    // Update is called once per framew
    void LateUpdate () {
        if (dead) {
            transform.position = new Vector3(0,4,0);
            dead = false;
        }
    }
}