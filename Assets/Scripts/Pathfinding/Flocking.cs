using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : MonoBehaviour {
    public int m_agentID ;

    GameObject target;
    Rigidbody rb;
    GameObject[] herbivores;
    public System.Random m_rand;
    bool m_doFlocking = true;
    float m_cohesionForce = 1.0f;
    float m_alignmentForce = 1.0f;
    float m_seperationForce = 1.0f;
    float m_neighborhoodRadius = 2.5f;
    int m_age = 0;
    List<GameObject> neighborAgents;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        herbivores = GameObject.FindGameObjectsWithTag("Herbivore");
        neighborAgents = new List<GameObject>();
    }

    void findNeighbours()
    {
        neighborAgents = new List<GameObject>();
        foreach(GameObject herbivore in herbivores)
        {
            float distance = Vector3.Distance(herbivore.transform.position, transform.position);
            if (distance <= m_neighborhoodRadius)
            {
                neighborAgents.Add(herbivore);
            }
        }
    }
    public void setParameters(bool doFlocking,float cohesionForce,float alignmentForce,float seperationForce, float neighborhoodRadius )
    {
        m_doFlocking = doFlocking;
        m_cohesionForce = cohesionForce;
        m_alignmentForce = alignmentForce;
        m_seperationForce = seperationForce;
        m_neighborhoodRadius = neighborhoodRadius;
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_age++;
    }

    void FixedUpdate()
    {
        rb.AddForce(flock());
    }

    Vector3 flock()
    {
        if(m_doFlocking)
        {
            findNeighbours();
            return alignment() + cohesion() + separation();
        }
        return new Vector3();
    }

    Vector3 alignment()
    {
        Vector3 force = new Vector3();
        if(neighborAgents.Count==0)
        {
            return force;
        }
        foreach(GameObject agent in neighborAgents)
        {
            {
                force += agent.GetComponent<Rigidbody>().velocity;
            }
        }
        return Vector3.Lerp(force.normalized * m_alignmentForce, GetComponent<Rigidbody>().velocity,0.5f);
    }
    Vector3 cohesion()
    {
        if (neighborAgents.Count == 0)
        {
            return new Vector3();
        }
        Vector3 centre = new Vector3();
        foreach (GameObject agent in neighborAgents)
        {
            {
                centre += (agent.transform.position-transform.position);
            }
        }
        centre /= neighborAgents.Count;
        Vector3 force = centre - transform.position;
        return force.normalized * m_cohesionForce;
    }

    Vector3 separation()
    {
        if (neighborAgents.Count == 0)
        {
            return new Vector3();
        }
        Vector3 centre = new Vector3();
        foreach (GameObject agent in neighborAgents)
        {
            {
                centre += agent.transform.position;
            }
        }
        centre /= neighborAgents.Count;
        Vector3 force = centre - transform.position;
        return force.normalized*-m_seperationForce;
    }
}
