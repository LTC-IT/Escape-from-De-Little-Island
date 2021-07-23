using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    FLOOR,
    WALL,
    WATER,
}

public enum TileStatus
{
    UNSET,
    OPEN,
    CLOSED,
    ON_ROUTE,
    START,
    END
}



public class AIStarWorld : MonoBehaviour
{
    public int m_worldWidth;
    public int m_worldHeight;
    public GameObject tilePrefab;
    public TileScript[,] tileMapScripts;
    public GameObject[,] tileMap;
    public Material[,] tileMapMaterials;
    public Vector3 tileSize;
    System.Random m_rand;
    public float m_wallHeight = 10;
    public float m_waterChance = 0.5f;
    public float m_wallChance = 0.1f;
    public Vector3 m_routeStart;
    public Vector3 m_routeEnd;
    bool m_newRoute = false;
    bool m_routeFound = false;
    List<TileScript> m_openList;
    List<TileScript> m_closedList;
    TileScript m_routeEndTile;
    TileScript m_routeStartTile;
    List<TileScript> m_currentPath;
    public GameObject agent;
    public LayerMask floorMask;
    public float m_pathBuildingSlowDown = 0.0f;
    public float m_waterCost = 10.0f;
    // Use this for initialization



    void createTileMap()
    {
        //create the map
        tileMapScripts = new TileScript[m_worldWidth, m_worldHeight];
        tileMap = new GameObject[m_worldWidth, m_worldHeight];
        tileMapMaterials = new Material[m_worldWidth, m_worldHeight];
        for (int x = 0; x < m_worldWidth; x++)
        {
            for (int z = 0; z < m_worldHeight; z++)
            {
                Vector3 position = new Vector3(x * tileSize.x, 0, z * tileSize.z);
                GameObject newTile = Instantiate(tilePrefab, position, Quaternion.identity);
                tileMap[x, z] = newTile;
                TileScript tileScript = newTile.GetComponent<TileScript>();
                tileScript.x = x;
                tileScript.z = z;
                tileScript.tileType = TileType.FLOOR;
                tileScript.tileStatus = TileStatus.UNSET;
                tileMapScripts[x, z] = tileScript;
                Renderer renderer = newTile.GetComponent<Renderer>();
                if (m_rand.NextDouble() < m_wallChance && x != 0 && z != 0)
                {
                    Vector3 scale = new Vector3(tileSize.x, m_wallHeight, tileSize.z);
                    newTile.transform.localScale = scale;
                    position.y += m_wallHeight / 2.0f;
                    newTile.transform.position = position;
                    tileScript.tileType = TileType.WALL;
                    tileScript.gameObject.layer = 13;
                }
                else if (x> (m_worldWidth * 0.25f) && (x< m_worldWidth * 0.75f) && z > (m_worldHeight * 0.25f) && (z < m_worldHeight * 0.75f))
                {
                    tileScript.tileType = TileType.WATER;
                }
            }
        }
        //build connection graph in tiles
        int tileID = 0;
        for (int x = 0; x < m_worldWidth; x++)
        {
            for (int z = 0; z < m_worldHeight; z++)
            {
                TileScript tileScript = tileMapScripts[x, z];
                tileScript.initLinks(tileID);
                //tileScript.westLink = tileScript;
                addConnection(x - 1, z, ref tileScript.links[(int)Link.NORTH]);
                addConnection(x + 1, z, ref tileScript.links[(int)Link.SOUTH]);
                addConnection(x, z - 1, ref tileScript.links[(int)Link.EAST]);
                addConnection(x, z + 1, ref tileScript.links[(int)Link.WEST]);
                tileID++;
            }
        }
    }

    void resetTileScores()
    {
        for (int x = 0; x < m_worldWidth; x++)
        {
            for (int z = 0; z < m_worldHeight; z++)
            {
                TileScript tileScript = tileMapScripts[x, z];
                tileScript.resetScores();
            }
        }
    }

    void addConnection(int x, int z, ref TileScript link)
    {
        if (x >= 0 && x < m_worldWidth && z >= 0 && z < m_worldHeight)
        {
            if (tileMapScripts[x, z].tileType == TileType.WALL)
            {
                link = null;
            }
            else
            {
                link = tileMapScripts[x, z];
            }

        }
        else
        {
            link = null;
        }
    }
    void Start()
    {
        m_rand = new System.Random(1);
        createTileMap();
    }

    void prepRoute(TileScript startingTile, TileScript endingTile)
    {
        m_openList = new List<TileScript>();
        m_closedList = new List<TileScript>();
        resetTileScores();
        m_routeStartTile.tileStatus = TileStatus.START;
        m_routeEndTile.tileStatus = TileStatus.END;

        m_routeStartTile.m_gScore = 0; //costs nothing to get to start tile
        m_routeStartTile.m_fScore = m_routeStartTile.heuristic(m_routeEndTile.transform.position);
        m_openList.Add(m_routeStartTile);
    }


    TileScript findBestTile()
    {
        float bestFScore = Mathf.Infinity;
        TileScript bestTile = null;
        foreach (TileScript tileScript in m_openList)
        {
            if (tileScript.m_fScore < bestFScore)
            {
                bestFScore = tileScript.m_fScore;
                bestTile = tileScript;
            }
        }
        return bestTile;
    }
    void reconstructPath(TileScript current)
    {
        m_currentPath = new List<TileScript>();
        m_currentPath.Add(current);
        while (current != m_routeStartTile)
        {
            current = current.m_cameFrom;
            m_currentPath.Insert(0, current);
        }
        setTilesOnPath(TileStatus.ON_ROUTE);
    }
    IEnumerator findRoute()
    {
        while (m_openList.Count > 0 && !m_routeFound)
        {
            TileScript currentTile = findBestTile();
            m_openList.Remove(currentTile);
            m_closedList.Add(currentTile);
            currentTile.tileStatus = TileStatus.CLOSED;
            if (currentTile == m_routeEndTile)
            {
                m_routeFound = true;
                reconstructPath(currentTile);
            }
            else
            {
                foreach (TileScript neighbourTile in currentTile.links)
                {
                    if (neighbourTile != null)
                    {
                        if (!m_closedList.Contains(neighbourTile))
                        {
                            if (!m_openList.Contains(neighbourTile))
                            {
                                m_openList.Add(neighbourTile);
                                neighbourTile.tileStatus = TileStatus.OPEN;
                            }
                            float distanceToNeighbour = (currentTile.transform.position - neighbourTile.transform.position).magnitude;
                            if(neighbourTile.tileType == TileType.WATER)
                            {
                                distanceToNeighbour *= m_waterCost;
                            }
                            float tentativeGScore = currentTile.m_gScore + distanceToNeighbour;
                            if (tentativeGScore < neighbourTile.m_gScore)
                            {
                                neighbourTile.m_cameFrom = currentTile;
                                neighbourTile.m_gScore = tentativeGScore;
                                neighbourTile.m_fScore = neighbourTile.m_gScore + neighbourTile.heuristic(m_routeEndTile.transform.position);
                            }
                        }
                    }
                }
            }
            if(m_pathBuildingSlowDown>0)
            {
                yield return new WaitForSeconds(m_pathBuildingSlowDown);
            }

        }

    }

    // Update is called once per frame
    void setTilesOnPath(TileStatus tileStatus)
    {
        if (m_currentPath == null)
        {
            return;
        }
        foreach (TileScript tileScript in m_currentPath)
        {
            tileScript.tileStatus = tileStatus;
        }
    }

    void Update()
    {
        if (m_newRoute)
        {
            setTilesOnPath(TileStatus.UNSET);
            m_currentPath = null;
            prepRoute(m_routeStartTile, m_routeEndTile);
            m_newRoute = false;
            m_routeFound = false;
            StartCoroutine(findRoute());
        }
    }

    GameObject GetClickedGameObject()
    {
        // Builds a ray from camera point of view to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // Casts the ray and get the first game object hit
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorMask))
            return hit.transform.gameObject;
        else
            return null;
    }

    void checkForNewRoute()
    {
        GameObject clickedGmObj = null;
        if (Input.GetMouseButtonDown(0))
        {
            clickedGmObj = GetClickedGameObject();
            if (clickedGmObj != null)
            {
                m_routeEndTile = clickedGmObj.GetComponent<TileScript>();
                if(m_routeEndTile.tileType != TileType.WALL)
                {
                    m_routeStartTile = agent.GetComponent<AStarAgent>().getCurrentTile(tileMapScripts);
                    m_newRoute = true;
                }

            }

            //display = true;
        }
    }

        void FixedUpdate()
    {
        if (m_currentPath != null)
        {
            agent.GetComponent<AStarAgent>().applySteeringForce(m_currentPath);
        }

        checkForNewRoute();
    }
}
