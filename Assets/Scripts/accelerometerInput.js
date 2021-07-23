#pragma strict

function Update () 
{
    transform.Translate(Input.acceleration.x, 0, -Input.acceleration.z);
}


