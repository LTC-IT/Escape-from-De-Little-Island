using UnityEngine;
using System.Collections;

public class CursorScript : MonoBehaviour
{
    // Use this for initialization
    void Update()
    {
        //Set Cursor to not be visible
		Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}




