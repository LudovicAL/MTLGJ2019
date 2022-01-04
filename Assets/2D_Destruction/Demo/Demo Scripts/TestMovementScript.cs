using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovementScript : MonoBehaviour
{
	private Rigidbody2D m_RigidBody2D;
    // Start is called before the first frame update
    void Start()
    {
		m_RigidBody2D = gameObject.GetComponent<Rigidbody2D> ();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKey("left"))
		{
			m_RigidBody2D.AddForce (new Vector2 (-2500.0f, 0.0f));
		}

		if (Input.GetKey("right"))
		{
			m_RigidBody2D.AddForce (new Vector2 (2500.0f, 0.0f));
		}

		if (Input.GetKey("up"))
		{
			m_RigidBody2D.AddForce (new Vector2 (0.0f, 2500.0f));
		}

		if (Input.GetKey("down"))
		{
			m_RigidBody2D.AddForce (new Vector2 (0.0f, -2500.0f));
		}
    }
}
