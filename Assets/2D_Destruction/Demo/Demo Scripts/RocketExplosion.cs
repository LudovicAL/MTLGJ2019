using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketExplosion : MonoBehaviour
{
	public float m_ExplosionRadius;
	public float m_TimeAfterImpactToGoOff;
	public bool m_IsGrenadeType;
	public float m_TimeAfterThrowToGoOff;
	private float m_ExplosionTimer;
	private bool m_ExplosionQueued;
	public GameObject m_ExplosionPrefab;
    // Start is called before the first frame update
    void Start()
    {
		m_ExplosionQueued = false;
		if (m_IsGrenadeType) {
			m_ExplosionQueued = true;
			m_ExplosionTimer = m_TimeAfterThrowToGoOff;
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (m_ExplosionQueued) {
			m_ExplosionTimer -= Time.deltaTime;
			if (m_ExplosionTimer <= 0.0f) {
				Explode (m_ExplosionRadius);
			}
		}
    }

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (!m_IsGrenadeType && !m_ExplosionQueued) {
			m_ExplosionQueued = true;
			m_ExplosionTimer = m_TimeAfterImpactToGoOff;
		}
	}

	void Explode(float radius)
	{
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll ((Vector2)transform.position, radius);
		int i = 0;
		while (i < hitColliders.Length)
		{
			Explodable _explodable = hitColliders[i].gameObject.GetComponent<Explodable>();
			if (_explodable) 
			{
				_explodable.explode ();
			}
			i++;
		}

		float rocketForceRadius = radius * 2.0f;
		Collider2D[] hitCollidersMore = Physics2D.OverlapCircleAll ((Vector2)transform.position, rocketForceRadius);
		int j = 0;
		while (j < hitCollidersMore.Length)
		{
			Vector3 vectorToTarget = transform.position - hitCollidersMore [j].transform.position;
			float magnitudeToTarget = vectorToTarget.magnitude;
			float forceToExert = (rocketForceRadius - magnitudeToTarget) * 600.0f;
			Rigidbody2D rb = hitCollidersMore [j].gameObject.GetComponent<Rigidbody2D> ();
			if (rb) 
			{
				rb.AddForce (vectorToTarget.normalized * forceToExert * -1.0f);
			}
			j++;
		}

		RequestAddExplosion (m_ExplosionPrefab, transform.position, m_IsGrenadeType);

		GameObject.Destroy(gameObject);
	}

	public static void RequestAddExplosion(GameObject _prefab, Vector2 _startPosition, bool smallSound)
	{
		if (_prefab == null)
		{
			Debug.LogWarning("Cannot find explosion prefab, link it to the Rocket Explosion script.");
			return;
		}

		if (smallSound) {
			AudioManager.Instance.PlayClip (AudioManager.Instance.explosion4);
		} else {
			AudioManager.Instance.PlayClip (AudioManager.Instance.explosion5);
		}
		Camera.main.gameObject.GetComponent<EZCameraShake.CameraShaker>().ShakeOnce(2.0f, 4.0f, 0.1f, 2.0f);
			
		GameObject spawnedExplosion = GameObject.Instantiate(_prefab, _startPosition, Quaternion.identity);
	}
}
