//////////////////////////////////////////////////////////////
// OrbGizmo.cs
// Penelope iPhone Tutorial
//
// OrbGizmo is a simple script to visualize the locations of
// individual orb spawn points. This script should go on the parent
// object that is used to group the orb spawn points.
//////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class OrbGizmo : MonoBehaviour
{


    void Start()
    {
        print("Orb count: " + transform.childCount);
    }

    void OnDrawGizmos()
    {
        if (enabled)
        {
            foreach (Transform child in transform)
            {
                Gizmos.DrawIcon(child.position, "orbIcon.tif");
            }
        }
    }

}
