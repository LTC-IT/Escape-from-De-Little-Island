using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carnivore : MonoBehaviour
{
    int agentID = 0;
    static int agentCount = 0;
    public float maxAccelerationForce = 1.0f;
    public float attackAccelerationForce = 5.0f;
    GameObject[] herbivores;
    public GameObject target;
    public LayerMask wallMask;
    Vector3 m_totalForce;
    Vector3 m_wanderTarget;
    System.Random m_rand;
    Rigidbody rb;
    public float worldExtent = 30;
    public float m_maximumWanderDistance = 12;
    // Use this for initialization
    void Start()
    {
        agentID = agentCount++;
        rb = GetComponent<Rigidbody>();
        herbivores = GameObject.FindGameObjectsWithTag("Herbivore");
        m_rand = new System.Random(agentID);
    }

    GameObject findBestPrey()
    {
        float bestDistance = Mathf.Infinity;
        GameObject currentTarget = null;
        foreach (GameObject prey in herbivores)
        {
            float distance = (prey.transform.position - transform.position).magnitude;
            bool behindWall = Physics.Raycast(transform.position, prey.transform.position - transform.position, distance, wallMask);
            if (distance < bestDistance && !behindWall)
            {
                bestDistance = distance;
                currentTarget = prey;
            }
        }
        return currentTarget;
    }

    void getNewWanderTarget()
    {
        float x = ((float)m_rand.NextDouble() - 0.5f) * worldExtent;
        float z = ((float)m_rand.NextDouble() - 0.5f) * worldExtent;
        m_wanderTarget = new Vector3(x, 0.5f, z);
        Vector3 delta = m_wanderTarget - transform.position;
        Vector3 direction = delta.normalized;


        float distance = delta.magnitude;
        if (distance > m_maximumWanderDistance)
        {
            distance = m_maximumWanderDistance;
            m_wanderTarget = transform.position + direction * distance;
        }
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, distance, wallMask))
        {
            m_wanderTarget = hit.point - direction;
        }

    }

    void wander()
    {
        float distance = 0;
        bool behindWall = false;
        Vector3 delta = m_wanderTarget - transform.position;
        Vector3 drection = delta.normalized;
        distance = delta.magnitude;
        behindWall = Physics.Raycast(transform.position, m_wanderTarget - transform.position, distance, wallMask);
        while (distance < 2.0 || behindWall)
        {
            getNewWanderTarget();
            delta = m_wanderTarget - transform.position;
            drection = delta.normalized;
            distance = delta.magnitude;
            behindWall = Physics.Raycast(transform.position, m_wanderTarget - transform.position, distance, wallMask);

        }
        float accelerationForce = maxAccelerationForce;
        m_totalForce += (drection * accelerationForce);
        Debug.DrawLine(transform.position, m_wanderTarget, Color.red);
    }

    bool seakPrey()
    {
        GameObject bestFood = findBestPrey();
        if (bestFood == null)
        {
            return false;
        }
        Vector3 delta = bestFood.transform.position - transform.position;
        Vector3 drection = delta.normalized;
        float distance = delta.magnitude;
        float accelerationForce = maxAccelerationForce;
        if (distance < 3)
        {
            accelerationForce = attackAccelerationForce;
            bestFood.GetComponent<BasicAgent>().beingattacked(this.gameObject);
        }
        m_totalForce += (drection * accelerationForce);
        return true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        addMovementForce();

    }

    void addMovementForce()
    {
        m_totalForce = new Vector3();
        bool foundFood = seakPrey();
        if (!foundFood)
        {
            wander();
        }
        rb.AddForce(m_totalForce);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Herbivore")
        {
            collision.gameObject.GetComponent<BasicAgent>().killHerbivore();
        }


    }
}
