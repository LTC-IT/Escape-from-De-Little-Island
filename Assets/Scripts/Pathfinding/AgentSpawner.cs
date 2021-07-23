using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpawner : MonoBehaviour {
    public GameObject agentPrefab;
    public int flockWidth = 5;
    public int flockHeight = 5;
	// Use this for initialization
	void Start () {
        spawnAgents();
	}
	
    void spawnAgents()
    {
        for(int x = 0;x< flockWidth;x++)
        {
            for (int z = 0; z < flockHeight; z++)
            {
                Vector3 agentPosition = transform.position + new Vector3(x*2, 0, z*2);
                GameObject newAgent = GameObject.Instantiate(agentPrefab, agentPosition, Quaternion.identity);
            }
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
