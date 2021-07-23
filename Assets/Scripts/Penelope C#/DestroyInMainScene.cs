//////////////////////////////////////////////////////////////
// DestroyInMainScene.cs
// Penelope iPhone Tutorial
//
// DestroyInMainScene destroys objects that have this script 
// attached when they are loaded additively into a larger scene.
// By default, the first loaded scene is given an index of 0,
// so we use that to distinguish whether this scene is being
// loaded by itself or additively to another scene.
//////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class DestroyInMainScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
        if (Application.loadedLevel == 0)
            Destroy(gameObject);
	
	}
	
}
