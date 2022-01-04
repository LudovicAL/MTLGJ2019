using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Explodable))]
public class ExplodeOnClick : MonoBehaviour {

	private Explodable _explodable;
	public float healthPoints;
	private float tempHealthPoints;
	SpriteRenderer m_SpriteRenderer;
	Color m_BaseColor;

	void Start()
	{
		_explodable = GetComponent<Explodable>();
		m_SpriteRenderer = GetComponent<SpriteRenderer> ();
		tempHealthPoints = healthPoints;
		m_BaseColor = m_SpriteRenderer.color;
	}
	void OnMouseDown()
	{
		_explodable.explode();
		ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
		ef.doExplosion(transform.position);
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		tempHealthPoints -= coll.relativeVelocity.magnitude;
		Color newColor = m_BaseColor * (Mathf.Max(0.5f, (float)(tempHealthPoints / healthPoints)));
		newColor.a = 1.0f;
		m_SpriteRenderer.color = newColor;

		if (tempHealthPoints < 0)
			_explodable.explode ();
	}
}
