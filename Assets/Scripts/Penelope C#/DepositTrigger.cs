//////////////////////////////////////////////////////////////
// DepositTrigger.cs
// Penelope iPhone Tutorial
//
// The DepositTrigger handles the region in which the player
// can deposit their pickups. When the player enters the 
// DepositTrigger area, the particle effects for the deposit
// area are activated and the player's carried items are 
// deposited.
//////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class DepositTrigger : MonoBehaviour
{

    //public ParticleEmitter[] emitters;      // These are the particle systems associated with the depository
    public GameObject depository;            // The root GameObject for the depository
    private bool arrowShown = false;

    void Start()
    {
        // Disable everything by default
        //foreach (ParticleEmitter emitter in emitters)
           // emitter.emit = false;
     
        DeactivateDepository();
 
        foreach (Transform child in transform)
            child.gameObject.SetActiveRecursively(false);
    }

    void OnTriggerEnter(Collider other)
    {
        // Activate depository objects and emitters
        ActivateDepository();
        // foreach (ParticleEmitter emitter in emitters)
        //     emitter.emit = true;
     
        // Tell the player that they have entered the depository
        other.SendMessage("Deposit");

        // Destroy the arrow designating the depository, now that we know the player
        // has found and entered the depository.    
        if (!arrowShown)
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
         
            arrowShown = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Disable depository when player leaves
        // foreach (ParticleEmitter emitter in emitters)
        //     emitter.emit = false;
        DeactivateDepository(); 
    }

    public void ActivateDepository()
    {
        if (!arrowShown)
            gameObject.SetActiveRecursively(true);
 
        depository.SendMessage("FadeIn");
    }

    void DeactivateDepository()
    {        
        depository.SendMessage("FadeOut");
    }
}
