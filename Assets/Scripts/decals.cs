using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class decals : MonoBehaviour
{
    public GameObject decal;
    public bool isFPS = false;
    int range = 100;

    RaycastHit hit;

    void OnGUI()
    {
        if (!isFPS)
        {
            if (GUI.Button(new Rect(Screen.width * 0.5f, Screen.height * 0.5f, 100, 100), "Fire"))
            {
                Shoot();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
            if (isFPS)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        Debug.Log("Shoot");
        Vector3 fwd = Camera.main.transform.forward;

        Transform decalClone;

        if (Physics.Raycast(transform.position, fwd, out hit, range))
        {
            Quaternion hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            if (hit.collider)
            {
                Debug.Log("Hit");
                
                decalClone = Instantiate(decal.transform, hit.point, hitRotation);
                decalClone.transform.parent = hit.transform;
            }
        }
    }
}
