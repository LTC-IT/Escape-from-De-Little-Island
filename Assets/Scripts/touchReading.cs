using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchReading : MonoBehaviour
{
    GUIStyle guiStyle = new GUIStyle();
    Vector2 highSpeed = new Vector2();

    void OnGUI()
    {
        guiStyle.fontSize = 25; //Adjust as necessary to fit the screen size

        for (int touchNumber = 0; touchNumber < Input.touchCount; touchNumber++)
        {
            Vector2 touch = Input.GetTouch(touchNumber).position;
            float xCoord = touch.x;
            float yCoord = touch.y;

            Vector2 tSpeed = Input.GetTouch(touchNumber).deltaPosition;


            if (tSpeed.magnitude > highSpeed.magnitude)
            {
                highSpeed = tSpeed;
            }

            GUI.Label(new Rect(xCoord, Screen.height - yCoord, 3000, 200), "Touch #" + (touchNumber + 1) + " Position " + touch, guiStyle);
        }
        GUI.Label(new Rect(0, 0, 500, 20), "Fastest Finger Moved " + highSpeed.magnitude + " pixels per frame!");
    }
}


/* Version 1

void OnGUI()
    {
        guiStyle.fontSize = 25; //Adjust as necessary to fit the screen size

        for ( int touchNumber = 0; touchNumber< Input.touchCount;touchNumber++) 
        {
             Vector2 touch = Input.GetTouch(touchNumber).position;
             float xCoord = touch.x;
             float yCoord = touch.y;

             GUI.Label(new Rect(xCoord, Screen.height - yCoord, 3000, 200), "Touch #" + (touchNumber +1) + " Position " + touch, guiStyle);
        }
    }



 */
