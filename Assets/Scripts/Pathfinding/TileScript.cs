using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Link
{
    NORTH,
    SOUTH,
    EAST,
    WEST,
}

public class TileScript : MonoBehaviour {
    public int x;
    public int z;
   // public bool wall;
    public TileType tileType;
    public TileStatus tileStatus;
    Renderer m_renderer;
    Material m_tileMapMaterial;
    public TileScript[] links;
    public int m_tileID;
    public float m_gScore;
    public float m_fScore;
    public TileScript m_cameFrom;
    public float m_heuristic = 0;

    // Use this for initialization
    void Start () {


        m_renderer = GetComponent<Renderer>();
        m_tileMapMaterial = m_renderer.material;
    }
	
    public void initLinks(int tileID)
    {
        resetScores();
        links = new TileScript[4];
        for (int i = 0; i < 4; i++)
        {
            links[i] = null;
        }
        m_tileID = tileID;
    }
	// Update is called once per frame
	void Update () {
        setColor();

    }

    public void resetScores()
    {
        m_gScore = Mathf.Infinity;
        m_fScore = Mathf.Infinity;
        tileStatus = TileStatus.UNSET;
        m_cameFrom = null;
    }

    public float heuristic(Vector3 end)
    {
        //need to add a heuristic to optimize the search in this function
        //note:
        //"end" is the position of the goal
        //"transform.position" is the tiles position. 
        m_heuristic = Vector3.Distance(end, transform.position);
        //m_heuristic = 0;
        return m_heuristic;
    }

    Color getTileColor(int x, int z)
    {
        Color tileColor = new Color(0.2f, 0.2f, 0.2f);

        switch(tileType)
        {

            case TileType.WALL:
                tileColor = new Color(0.1f, 0.1f, 0.1f);
                break;
            case TileType.WATER:
                tileColor = new Color(0.2f, 0.2f, 0.6f);
                break;
            default:
                if ((x + z) % 2 == 0)
                {
                    tileColor = new Color(0.3f, 0.3f, 0.3f);
                }
                break;
        }
        switch (tileStatus)
        {
            case TileStatus.END:
                tileColor += new Color(0.2f, 0.2f, 0.7f);
                break;
            case TileStatus.START:
                tileColor += new Color(0.2f, 0.8f, 0.2f);
                break;
            case TileStatus.OPEN:
                tileColor += new Color(0.0f, 0.0f, 0.3f);
                break;
            case TileStatus.CLOSED:
                float debugShade = 1 - (m_heuristic / m_fScore);
                tileColor += new Color(debugShade*.8f, 0.0f, 0.0f);
                break;
            case TileStatus.ON_ROUTE:
                tileColor += new Color(0.0f, 0.5f, 0.5f);
                break;

        }
        return tileColor;
    }

    Color getTileDebugColor(int x, int z)
    {
        float tint = 0;
        foreach(TileScript tileScript in links)
        {
            if(tileScript!= null)
            {
                tint++;
            }
        }
        return new Color(tint,tint, tint);
    }

    public void setColor()
    {
        m_tileMapMaterial.color = getTileColor(x, z);


    }


}
