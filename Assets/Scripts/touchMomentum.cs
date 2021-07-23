using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchMomentum : MonoBehaviour
{

    Vector3 cameraVelocity = new Vector3();

    void LateUpdate()
    {
        if(Input.touchCount == 1) 
        {
            Touch touch = Input.GetTouch(0);
            cameraVelocity.x += touch.deltaPosition.x;
            cameraVelocity.y += touch.deltaPosition.y;
        }

        //Camera.main.transform.position += cameraVelocity * Time.deltaTime * 0.001;
        Camera.main.transform.Translate(-cameraVelocity.x * 0.0001f, -cameraVelocity.y * Time.deltaTime * 0.0001f,0);
    }
}
