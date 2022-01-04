using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieWhenHitByBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.relativeVelocity.magnitude > 10.0f)
			GameObject.Destroy(gameObject);
	}
}
