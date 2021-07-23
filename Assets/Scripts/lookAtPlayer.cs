using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtPlayer : MonoBehaviour
{
    public Transform lookAtTarget;
    public Transform projectilePrefab;
    public float damp = 0.6f;
    public int shotInterval = 5;
    public AudioSource[] audioSources;
    private AudioSource cannonAudio;
    public AudioClip cannonSound;
    public int firingRange = 20;
     private Transform FPS;           // Links to the First Person Controller

    public void Awake()
    {
        //StartCoroutine(Tick());
        //audioSources = GetComponents<AudioSource>();
        //cannonAudio = audioSources[0];
    }

    public void Update()
    {
        float distance = Vector3.Distance(FPS.position, transform.position);
        if (lookAtTarget && distance <= firingRange)
        {
            Debug.DrawLine(lookAtTarget.position, transform.position, Color.yellow);
            Vector3 lookPos = lookAtTarget.position - transform.position;
            lookPos.y = 0;
            Quaternion rotate = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * damp);
        }
    }

    public void Start()
    {
        FPS = GameObject.FindGameObjectWithTag("Player").transform;
    }

    IEnumerator Tick()
    {
        while (true)
        {
            yield return new WaitForSeconds(shotInterval);
            float distance = Vector3.Distance(FPS.position, transform.position);
            if (projectilePrefab && distance <= firingRange)
            {
                Transform projectile = Instantiate(projectilePrefab, transform.Find("gunShootSpawn").transform.position, Quaternion.identity);
                //.gameObject.tag = "projectile";
                //projectile.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);
                //projectile.GetComponent<Rigidbody>().AddForce(transform.forward * 5000);
                //cannonAudio.clip = cannonSound;
                //cannonAudio.Play();

            }
        }
    }
}


