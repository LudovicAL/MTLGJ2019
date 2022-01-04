using UnityEngine;

public class ProjectileData : MonoBehaviour
{
    public float Speed = 10f;
    public float GravityScale = 1.0f;
    public float MaxLifeTime = 8.0f;
	public bool m_DestroyOnImpact;

    private float LifeTimeRemaining;
    void Start()
    {
        LifeTimeRemaining = MaxLifeTime;
    }

    void Update()
    {
        LifeTimeRemaining -= Time.deltaTime;
        if (LifeTimeRemaining <= 0.0f)
            GameObject.Destroy(gameObject);
    }

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (m_DestroyOnImpact)
			GameObject.Destroy(gameObject);
	}
}
