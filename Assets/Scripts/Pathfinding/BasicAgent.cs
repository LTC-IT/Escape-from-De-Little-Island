using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAgent : MonoBehaviour {
    public int m_agentID ;

    GameObject target;
    Rigidbody rb;
    GameObject[] plants;
    GameObject[] herbivores;
    public LayerMask wallMask;
    public LayerMask notFloor;
    public Vector3 m_totalForce;
    Vector3 m_wanderTarget;
    public System.Random m_rand;
    public float worldExtent = 30;
    public float m_maximumWanderDistance = 12;
    GameObject m_attacker;
    float fleeTimer;
    float m_movementForce = 2.0f;
    float m_fleeForce = 8;
    float m_feelerRange = 3.0f;
    float m_feelerSpread = 0.5f; //radians
    float m_objectRepulsionForce = 4.0f;
    float m_wanderTargetSetTime;
    Flocking m_flocking = null; //this is where we'll store a reference to the flocking behaviour if it has been added
    ObjectAvoidance m_objectAvoidance = null;
    int m_age = 0;
    List<GameObject> neighborAgents;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        plants = GameObject.FindGameObjectsWithTag("Plant");
        herbivores = GameObject.FindGameObjectsWithTag("Herbivore");
        m_flocking = gameObject.GetComponent<Flocking>();
        m_objectAvoidance = gameObject.GetComponent<ObjectAvoidance>();
        m_wanderTarget = new Vector3();
        neighborAgents = new List<GameObject>();
    }
    public void setParameters(bool doFlocking,float movementForce,float fleeForce,float cohesionForce,float alignmentForce,float seperationForce, float neighborhoodRadius,float feelerRange,float feelerSpread,float objectRepulsionForce)
    {
        m_movementForce = movementForce;
        m_fleeForce = fleeForce;
        m_feelerRange = feelerRange;
        m_feelerSpread = feelerSpread;
        m_objectRepulsionForce = objectRepulsionForce;
        if(m_flocking!= null)
        {
            m_flocking.setParameters(doFlocking,cohesionForce,alignmentForce,seperationForce,neighborhoodRadius);
        }
        if (m_objectAvoidance != null)
        {
            m_objectAvoidance.setParameters(m_feelerRange, m_feelerSpread, m_objectRepulsionForce);
        }
    }
	
	// Update is called once per frame
	void Update () {

        fleeTimer -= Time.deltaTime;
        if(fleeTimer<=0)
        {
            fleeTimer = 0;
            m_attacker = null;
        }
        m_age++;
    }

    void FixedUpdate()
    {
        addMovementForce();

    }

    GameObject findBestFood()
    {
        float bestDistance = Mathf.Infinity;
        GameObject currentTarget = null;
        foreach(GameObject plant in plants)
        {
            float distance = (plant.transform.position - transform.position).magnitude;
            bool behindWall =  Physics.Raycast(transform.position,  plant.transform.position - transform.position, distance, wallMask);
            if(distance < bestDistance && !behindWall)
            {
                bestDistance = distance;
                currentTarget = plant;
            }
        }
        return currentTarget;
    }

    void getNewWanderTarget()
    {
        m_wanderTargetSetTime = Time.time;
        RaycastHit hit;
        float x = ((float)m_rand.NextDouble() - 0.5f) * worldExtent;
        float z = ((float)m_rand.NextDouble() - 0.5f) * worldExtent;
        m_wanderTarget = new Vector3(x, 0.5f, z);
        Vector3 delta = m_wanderTarget - transform.position;
        Vector3 direction = delta.normalized;
        float distance = delta.magnitude;
        if (distance>m_maximumWanderDistance)
        {
            distance = m_maximumWanderDistance;
            m_wanderTarget = transform.position + direction * distance;
        }
        if (Physics.Raycast(transform.position, direction, out hit,distance,wallMask))
        {
            m_wanderTarget = hit.point - direction;
        }

    }

    void wander()
    {
        if(m_wanderTargetSetTime+5.0f<Time.time)
        {
            getNewWanderTarget();
        }
        float distance = 0;
        bool behindWall = false;
        Vector3 delta = m_wanderTarget - transform.position;
        Vector3 direction = delta.normalized;
        distance = delta.magnitude;
        behindWall = Physics.Raycast(transform.position, m_wanderTarget - transform.position, distance, wallMask);
        if(distance > 2 && !behindWall)
        {
            m_totalForce += (direction * m_movementForce);
            return;
        }
        int loopCheck = 10;
        do
        {
            getNewWanderTarget();
            delta = m_wanderTarget - transform.position;
            direction = delta.normalized;
            distance = delta.magnitude;
            loopCheck--;
        } while (distance < 2.0 && loopCheck > 10);

        Debug.DrawLine(transform.position, m_wanderTarget, Color.red);
    }

    bool seakFood()
    {
        GameObject bestFood = findBestFood();
        if(bestFood== null)
        {
            return false;
        }
        Vector3 delta = bestFood.transform.position - transform.position;
        Vector3 direction = delta.normalized;
        m_totalForce+= (direction * m_movementForce);
        return true;
    }

    void flee()
    {
        Vector3 delta =  transform.position - m_attacker.transform.position;
        Vector3 drection = delta.normalized;
        float distance = delta.magnitude;
        float accelerationForce = m_fleeForce;
        m_totalForce += (drection * accelerationForce);
    }

    void addMovementForce()
    {
        m_totalForce = new Vector3();
        if(m_attacker!= null)
        {
            flee();
        }
        else
        {
            bool foundFood = seakFood();
            if (!foundFood)
            {
                wander();
            }
        }
        rb.AddForce(m_totalForce);
    }

    //finds a new random location to spawn out on the edge of the map
    Vector3 randomWorldCoordinate()
    {
        Vector3 displacement = new Vector3();

        displacement.x = ((float)m_rand.NextDouble() - 0.5f);
        displacement.z = ((float)m_rand.NextDouble() - 0.5f);
        displacement = displacement.normalized;
        Vector3 spawnLocation = displacement * worldExtent;
        spawnLocation.y = 0.5f;
        return spawnLocation;
    }

    public void beingattacked(GameObject attacker)
    {
        m_attacker = attacker;
        fleeTimer = 2.0f;
    }

    public void killHerbivore()
    {
        transform.position = randomWorldCoordinate();
        TrailRenderer trailRenderer = gameObject.GetComponent<TrailRenderer>();
        trailRenderer.Clear();
    }

}
