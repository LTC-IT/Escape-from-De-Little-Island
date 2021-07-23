//////////////////////////////////////////////////////////////
// WaterMovement.cs
// Penelope iPhone Tutorial
//
// WaterMovement is not explained in the tutorial, however,
// is a simple script that animates the textures on two meshes
//////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class WaterMovement : MonoBehaviour
{

    public Renderer waterSurface;
    public Renderer pipeWater;

    void Update()
    {
        var myTime = Time.time;

        // Sin is expensive to use on iPhone, so we use PingPong instead
        // var mover = Mathf.Sin( myTime * 0.2 );
        var mover = Mathf.PingPong(myTime * 0.2f, 1.0f) * 0.05f;
        waterSurface.material.mainTextureOffset = new Vector2(mover, mover);  
        pipeWater.material.mainTextureOffset = new Vector2((myTime * 0.2f) % 1.0f, (myTime * 1.3f) % 1.0f);
    }
}
