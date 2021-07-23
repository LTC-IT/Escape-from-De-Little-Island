/////////////////////////////////////////////////////////////
// PickupManager.cs
// Penelope iPhone Tutorial
//
// PickupManager handles positioning the pickup particles.
// The PickupManager uses the children of its GameObject as
// the spawn locations for the pickups in game. It randomly
// selects a child, and then places a particle on top of it.
//////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

//[RequireComponent (typeof(ParticleEmitter))]
public class PickUpManager : MonoBehaviour
{
    
    public GameObject ColliderPrefab;
    public DepositTrigger depositTrigger;

    // The Start function is called at the beginning of the level,
    // and is where the placing of the particles is handedled.
    void Start()
    {
        
        //ParticleEmitter emitter = particleEmitter;
        //emitter.ClearParticles();
        //emitter.Emit();
        
        //Vector3 location;
        
        // Particle[] myParticles = emitter.particles;
        GameObject toDestroy = new GameObject("ObjectsToDestroy");
        GameObject ColliderContainer = new GameObject("ParticleColliders");
        
        // for (int i = 0; i < emitter.particleCount; i++)
        // {
        //     if (transform.childCount <= 0)
        //         break;
            
        //     Transform child = transform.GetChild(Random.Range(0, transform.childCount));
        //     // myParticles [i].position = child.position;
            
        //     // Move the particle to another parent, so it isn't selected again
        //     // for another orb position
        //     child.parent = toDestroy.transform;
            
        //     GameObject preFab = Instantiate(ColliderPrefab, transform.position, Quaternion.identity) as GameObject;
        //     // ParticlePickup pickup = preFab.GetComponent<ParticlePickup>();
        //     // pickup.emitter = emitter;
        //     // pickup.index = i;
            
        //     preFab.transform.parent = ColliderContainer.transform;
        // }
        
        Destroy(toDestroy);
        //emitter.particles = myParticles;
 
    }
 
    void ActivateDepository()
    {
        depositTrigger.ActivateDepository();
    }
}
