//////////////////////////////////////////////////////////////
// FadeInFadeOut.cs
// Penelope iPhone Tutorial
//
// FadeInFadeOut modifies the material on the depository to create
// a smooth transition when the player steps on and off of the
// platform. The alpha channel on the material lerps in and out
// rather than toggling on and off.
//////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class FadeInFadeOut : MonoBehaviour
{
    private Material[] childMaterials;
    private float currentAlpha = 1.0f;
    private int fading = 1;
    private float timeStep = 0.05f;
    private float blendTime = 4.0f;
    private float blend;
    private string colorName = "_TintColor";

    void Start()
    {
        // Cache the materials from the depository overlay meshes.
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        childMaterials = new Material[ renderers.Length ];
        for (int i = 0; i < renderers.Length; i++)
        {
            Renderer r = renderers [i];
            childMaterials [i] = r.material;
        }
    }

    void FadeIn()
    {    
        // Cancel any previous InvokeRepeating() calls
        CancelInvoke(); 

        // Set fading direction (in) and reset the blend timer
        fading = 1;
        blend = 0.0f;

        // Set up a custom method to be invoked repeatedly until fading has finished
        InvokeRepeating("CustomUpdate", 0.0f, timeStep);
    }

    void FadeOut()
    {
        // Cancel any previous InvokeRepeating() calls
        CancelInvoke(); 
 
        // Set fading direction (in) and reset the blend timer
        fading = -1;
        blend = 0;
 
        // Set up a custom method to be invoked repeatedly until fading has finished
        InvokeRepeating("CustomUpdate", 0.0f, timeStep);
    }

    void CustomUpdate()
    {
        // Add the time elapsed to our blend timer
        blend += timeStep;
 
        // Accumulate alpha difference for this time step
        if (fading > 0)
            currentAlpha += timeStep / blendTime;
        else
            currentAlpha -= timeStep / blendTime;
     
        // Alpha must be between 0 and 1
        currentAlpha = Mathf.Clamp(currentAlpha, 0, 1);       
 
        // Update the alpha on the materials
        for (int i = 0; i < childMaterials.Length; i++)
        {
            Material m = childMaterials [i];
            Color c = m.GetColor(colorName);
            c.a = currentAlpha;
            m.SetColor(colorName, c);
        }
 
        // If we're done fading, then kill any future update calls 
        if (blend >= blendTime)
            CancelInvoke();
    }
}
