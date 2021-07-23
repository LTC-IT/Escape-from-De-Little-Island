using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Plant : MonoBehaviour {
    System.Random rand;
    public float worldExtent = 20;
    GameObject[] walls;
    public LayerMask NotFloor;
    // Use this for initialization
    void Start () {
        rand = new System.Random();
        walls = GameObject.FindGameObjectsWithTag("Wall");
        randomWorldCoordinate();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //finds a new random location for the food
    //make sure it's not close to, or inside a, wall,player or another plant
    Vector3 randomWorldCoordinate()
    {
        bool badPosition = true;
        Vector3 foodPosition = new Vector3(0.0f,0.5f,0.0f);
        //check that we haven't spawned inside of anything or close to an agent
        int loopCheck = 10;
        while (badPosition && loopCheck>0)
        {
            foodPosition.x = ((float)rand.NextDouble() - 0.5f) * worldExtent;
            foodPosition.z = ((float)rand.NextDouble() - 0.5f) * worldExtent;
      //      foodPosition.y = ((float)rand.NextDouble() - 0.5f) * worldExtent;
            if (Physics.CheckSphere(foodPosition, 2,NotFloor))
            {
                badPosition = true;
            }
            else
            {
                badPosition = false;
            }
            loopCheck--;
        }

        return foodPosition;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Herbivore")
        {
            eatPlant();
        }
    }

    public void eatPlant()
    {
        transform.position = randomWorldCoordinate();
    }
}
