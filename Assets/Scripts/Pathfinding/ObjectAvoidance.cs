using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAvoidance : MonoBehaviour {
    Rigidbody rb;
    public LayerMask wallMask;
    float m_feelerRange = 3.0f;
    float m_feelerSpread = 0.5f; //radians
    float m_objectRepulsionForce = 4.0f;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
    }

	
	// Update is called once per frame
	void Update () {

    }

    void FixedUpdate()
    {
        rb.AddForce(objectAvoidance());
    }

    public void setParameters(float feelerRange,float feelerSpread,float objectRepulsionForce)
    {
        m_feelerRange = feelerRange;
        m_feelerSpread = feelerSpread;
        m_objectRepulsionForce = objectRepulsionForce;
    }

    Vector3 objectAvoidance()
    {
        Vector3 force = new Vector3();
        float directionAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z);
        Vector3 forward = new Vector3(Mathf.Sin(directionAngle), 0, Mathf.Cos(directionAngle));
        Vector3 diagonalLeft = new Vector3(Mathf.Sin(directionAngle - m_feelerSpread), 0, Mathf.Cos(directionAngle - m_feelerSpread));
        Vector3 diagonalRight = new Vector3(Mathf.Sin(directionAngle + m_feelerSpread), 0, Mathf.Cos(directionAngle + m_feelerSpread));
        Debug.DrawLine(transform.position, transform.position + forward* m_feelerRange);
        Debug.DrawLine(transform.position, transform.position + diagonalLeft * m_feelerRange);
        Debug.DrawLine(transform.position, transform.position + diagonalRight * m_feelerRange);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, forward, out hit, m_feelerRange, wallMask))
        {
            force += hit.normal * m_objectRepulsionForce;
        }
        if (Physics.Raycast(transform.position, diagonalLeft, out hit, m_feelerRange, wallMask))
        {
            force += hit.normal* m_objectRepulsionForce;
        }
        if (Physics.Raycast(transform.position, diagonalRight, out hit, m_feelerRange, wallMask))
        {
            force += hit.normal* m_objectRepulsionForce;
        }
        return force;
    }

}
