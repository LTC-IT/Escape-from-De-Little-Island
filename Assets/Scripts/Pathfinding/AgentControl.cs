using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentControl : MonoBehaviour {
    System.Random m_rand;
    public GameObject agentPrefab;
    public GameObject carnivorePrefab;
    public int flockWidth = 5;
    public int flockHeight = 5;
    public int numberCarnivores = 2;
    public bool m_doFlocking = true;
    public float m_movementForce = 2.0f;
    public float m_fleeForce = 8;
    public float m_cohesionForce = 1.0f;
    public float m_alignmentForce = 1.0f;
    public float m_seperationForce = 1.0f;
    public float m_neighborhoodRadius = 2.5f;
    public float m_feelerRange = 4.0f;
    public float m_feelerSpread = 0.6f;
    public float m_objectRepulsionForce = 4.0f;
    List<BasicAgent> agentScripts;

    // Use this for initialization
    void Start () {
        m_rand = new System.Random();
        spawnHerbivores();
        spawnCarnivores();
    }
	
    void spawnHerbivores()
    {
        int agentID = 0;
        agentScripts = new List<BasicAgent>();
        for (int x = 0;x< flockWidth;x++)
        {
            for (int z = 0; z < flockHeight; z++)
            {
                Vector3 agentPosition = transform.position + new Vector3(x*2, 0, z*2); //spawn agents into a grid
                GameObject newAgent = GameObject.Instantiate(agentPrefab, agentPosition, Quaternion.identity);
                BasicAgent agentScript = newAgent.GetComponent<BasicAgent>();
                agentScript.setParameters(m_doFlocking, m_movementForce, m_fleeForce, m_cohesionForce, m_alignmentForce, m_seperationForce, m_neighborhoodRadius, m_feelerRange, m_feelerSpread, m_objectRepulsionForce);
                agentScript.m_agentID = agentID;
                agentScript.m_rand = m_rand;
                agentScripts.Add(agentScript); //add to our list of agent scripts so we can easily find this again later
                agentID++;
            }
        }
    }

    void spawnCarnivores()
    {
        for (int i = 0; i < numberCarnivores; i++)
        {
            Vector3 agentPosition = transform.position + new Vector3(10, 0, i * 3);
            GameObject newAgent = GameObject.Instantiate(carnivorePrefab, agentPosition, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update () {
       StartCoroutine(updateAgentParameters()); //concurrency
	}


    IEnumerator updateAgentParameters()
    {
        //continuously loop over all agents
        while(true)
        {
            foreach(BasicAgent agent in agentScripts)
            {
                agent.setParameters(m_doFlocking, m_movementForce, m_fleeForce, m_cohesionForce, m_alignmentForce, m_seperationForce, m_neighborhoodRadius, m_feelerRange, m_feelerSpread,m_objectRepulsionForce);
                //halt coroutine every 1/100 of a second per agent
                yield return new WaitForSeconds(.01f);
            }

        }
    }

    void setUpAgentsParameters()
    {
        foreach (BasicAgent agent in agentScripts)
        {

        }
    }

}
