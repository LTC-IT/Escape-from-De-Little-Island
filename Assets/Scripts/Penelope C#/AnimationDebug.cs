//////////////////////////////////////////////////////////////
// AnimationDebug.cs
// Penelope iPhone Tutorial
//
// AnimationDebug is not explained in the tutorial, however,
// was useful by us in debugging animations while building 
// controls.
//////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections;

[RequireComponent (typeof(AnimationController))]
public class AnimationDebug : MonoBehaviour
{


    private AnimationController animationController;
    private CharacterController character;

    void Start()
    {
        character = GetComponent<CharacterController>();
        animationController = GetComponent<AnimationController>();
        if (!Application.isEditor)
            Destroy(this);
    }

    void OnGUI()
    {
        GUI.skin.font = null;
 
        float largestWeight = 0;
        AnimationState animState = new AnimationState();
        foreach (AnimationState state in animationController.animationTarget)
        {
            if (state.weight > largestWeight)
            {
                largestWeight = state.weight;
                animState = state;
            }
        }

        Vector3 vel = character.velocity;
        Vector3 horizontalVel = vel;
        horizontalVel.y = 0;
        float speed = horizontalVel.magnitude;

        if (animState)
            GUI.Label(new Rect(10, 70, 400, 60), String.Format("Vel: {5}  Speed: {0:0.000}\nAnimation: {1}\n  * weight {2:0.00}  speed {3:0.00} time {4:0.00}",
                        speed, animState.name, animState.weight, animState.speed, animState.normalizedTime, vel));
     
        //   GUI.Label( Rect( 10, 70, 100, 20 ), String.Format( "{0}", vel ) );
    }
}
