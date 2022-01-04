using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketGoFast : MonoBehaviour
{
	private Vector2 rocketHeading;
	public float maxJitter;
	public float forceToApply;
	public float maxSpeed;
	private Rigidbody2D m_RigidBody2D;
    // Start is called before the first frame update
    void Start()
    { 
		m_RigidBody2D = gameObject.GetComponent<Rigidbody2D> ();
		UpdateRocketHeading ();
    }

    // Update is called once per frame
    void Update()
    {
		UpdateRocketHeading ();
		if (m_RigidBody2D.velocity.magnitude < maxSpeed) 
		{
			m_RigidBody2D.AddForce (rocketHeading.normalized * forceToApply);
		}
    }

	private void UpdateRocketHeading()
	{
		float radianAngle = (transform.eulerAngles.z + Random.Range (-maxJitter, maxJitter)) * Mathf.Deg2Rad;
		rocketHeading.x = -Mathf.Sin (radianAngle);
		rocketHeading.y = Mathf.Cos (radianAngle);
		m_RigidBody2D.MoveRotation (radianAngle * Mathf.Rad2Deg);
	}
}
