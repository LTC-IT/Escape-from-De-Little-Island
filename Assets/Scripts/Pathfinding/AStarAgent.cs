using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAgent : MonoBehaviour {

    // Use this for initialization
    public Rigidbody m_rb;
    public float m_acceleration = 3.0f;
    public float floorDrag = 0.6f;
    public float waterDrag = 1.2f;
    void Start () {
        m_rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public TileScript getCurrentTile(TileScript[,] tileMapScripts)
    {
        TileScript currentTile = null;
        float bestDistance = Mathf.Infinity;
        int currentTileIndex = 0;
        int tileIndex = 0;
        for(int x=0;x < tileMapScripts.GetLength(0);x++)
        {
            for (int y = 0; y < tileMapScripts.GetLength(1); y++)
            {
                TileScript tileScript = tileMapScripts[x, y];

                float distance = (tileScript.transform.position - transform.position).magnitude;
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    currentTile = tileScript;
                    currentTileIndex = tileIndex;
                }
                tileIndex++;
            }
        }
        return currentTile;
    }

    public void applySteeringForce(List<TileScript> path)
    {
        TileScript currentTile = null;
        float bestDistance = Mathf.Infinity;
        int currentTileIndex = 0;
        int tileIndex  = 0;
        TileScript destinationTile;
        foreach (TileScript tileScript in path)
        {
            float distance = (tileScript.transform.position - transform.position).magnitude;
            if (distance < bestDistance)
            {
                bestDistance = distance;
                currentTile = tileScript;
                currentTileIndex = tileIndex;
            }
            tileIndex++;
        }
        if (currentTileIndex < path.Count - 1)
        {
            destinationTile = path[currentTileIndex + 1];
        }
        else
        {
            destinationTile = path[currentTileIndex];
        }
        
        if (currentTile.tileType == TileType.WATER)
        {
            m_rb.drag = waterDrag;
        }
        else
        {
            m_rb.drag = floorDrag;
        }
        Vector3 destPos = destinationTile.gameObject.transform.position;
        destPos.y = transform.position.y;
        Vector3 delta = destPos - transform.position;
        float distanceToTarget = delta.magnitude;
        delta = delta.normalized;
        Vector3 force = new Vector3();
        if (distanceToTarget > 0.2f)
        {
            force = delta * m_acceleration;
        }
        m_rb.AddForce(force);
    }
}
