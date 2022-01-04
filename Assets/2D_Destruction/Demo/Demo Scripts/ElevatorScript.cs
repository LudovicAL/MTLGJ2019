using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
	public float m_ElevatorSpeed;
	private float m_CurrentElevatorSpeed;
	public float m_ElevatorTopHeight;
	public float m_ElevatorBottomHeight;
	private Rigidbody2D m_RigidBody2D;
    // Start is called before the first frame update
    void Start()
    {
		m_RigidBody2D = gameObject.GetComponent<Rigidbody2D> ();
		m_CurrentElevatorSpeed = m_ElevatorSpeed;
    }

    // Update is called once per frame
    void Update()
    {
		if (transform.position.y > m_ElevatorTopHeight || transform.position.y < m_ElevatorBottomHeight) {
			m_CurrentElevatorSpeed *= -1.0f;
		}
		Vector3 targetOffset = new Vector2 (0.0f, m_CurrentElevatorSpeed);
		m_RigidBody2D.MovePosition (transform.position += targetOffset);
    }
}
