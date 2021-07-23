#pragma strict

var isFPS : boolean = false;	// set to true if game is fps
var Range : int = 100;
var decal : GameObject;

function OnGUI () {
	if(isFPS == false){
	    if(GUI.Button(Rect(Screen.width*0.5,Screen.height*0.5,100,100),"Fire")){
	        Shoot();
	    }
    }
}

function Update(){
	if(Input.GetMouseButtonDown(0)){
		if(isFPS == true){
			Shoot();
		}
	}
}

function Shoot()
{
    var fwd = transform.TransformDirection (Vector3.forward);
    var particleClone : Transform;
    var hit : RaycastHit;
    var decalClone : Transform;
    
	
    if(Physics.Raycast (transform.position, fwd, hit, Range))
    {
        var hitrotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        if(hit.collider)
        {
            Debug.Log("Hit");
            decalClone = Instantiate(decal.transform, hit.point, hitrotation);
        }
    }
}