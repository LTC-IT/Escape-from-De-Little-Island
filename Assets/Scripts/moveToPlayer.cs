using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveToPlayer : MonoBehaviour
{
    public int moveSpeed;           // How quickly the object moves
    public int rotationSpeed;       // How quickly the object turns
    public Transform FPS;           // Links to the First Person Controller
    private Transform enemy;        // Refers to the enemy object (itself)
    public int maxDistance;

    public void Awake()
    {
        print(transform.position);
        //enemy = transform;  //sets the enemy variable to the current game object that the script is attached to.
    }

    public void Start()
    {
        FPS = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update()
    {
        FPS = GameObject.FindGameObjectWithTag("Player").transform;
        float distance = Vector3.Distance(FPS.position, transform.position);
        if (distance < maxDistance)
        {
            Debug.DrawLine(FPS.position, transform.position, Color.green);
            Vector3 lookPos = FPS.position - transform.position;
            lookPos.y = 0;
            Quaternion rotate = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * rotationSpeed);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }


    }
}





