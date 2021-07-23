using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeshPathFindingAgent : MonoBehaviour
{
    public GameObject target;
    public float distanceToTriggerMovement = 5;
    NavMeshAgent m_agent;
    private Transform FPS;           // Links to the First Person Controller
    // Use this for initialization
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        FPS = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (transform.position - FPS.position).magnitude;
        //Debug.Log(distance);
        if (distance < distanceToTriggerMovement)
        {
            m_agent.destination = FPS.position;
        }
    }
}


/*



 */
