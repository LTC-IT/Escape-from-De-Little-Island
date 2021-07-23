//////////////////////////////////////////////////////////////
// ParticlePickup.cs
// Penelope iPhone Tutorial
//
// ParticlePickup allows the colliders for the particles to keep
// a reference to the emitter, and the index of the particle
// that the collider is representing. When the player collides with
// the ParticlePickup's GameObject, the ParticlePickup passes
// itself to the player's ScoreKeeper so that the player can
// determine whether or not to pickup the item.
//////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class ParticlePickup : MonoBehaviour
{
    //public ParticleEmitter emitter;
    public int index;
    public GameObject collectedParticle;

    // OnTriggerEnter is called whenever a Collider hits this GameObject's collider
    void OnTriggerEnter(Collider other)
    {
        ScoreKeeper sk = other.GetComponent<ScoreKeeper>();
        sk.Pickup(this);
    }

    // Collected is called when the player picks up this item.
    public void Collected()
    {
        // Spawn particles where the orb was collected
        Instantiate(collectedParticle, transform.position, Quaternion.identity);
 
        // Scale the particle down, so it is no longer visible
        //Particle[] particles = emitter.particles;     
        //particles [index].size = 0;        
        //emitter.particles = particles;
 
        // Destroy the collider for this orb
        Destroy(gameObject);
    }
}
