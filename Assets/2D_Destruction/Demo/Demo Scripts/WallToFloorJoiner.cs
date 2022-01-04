using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallToFloorJoiner : MonoBehaviour
{
	bool m_FoundAJoin;
	GameObject m_JoinedObject;
    // Start is called before the first frame update
    void Start()
    {
		m_FoundAJoin = false;
		m_JoinedObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (!m_FoundAJoin) 
		{
			if (coll.gameObject.CompareTag ("ConcreteFloor") && (coll.transform.position.y < transform.position.y) && (Mathf.Abs(coll.transform.position.x - transform.position.x) < 5.0f)) {
				m_JoinedObject = coll.gameObject;
				gameObject.AddComponent<FixedJoint2D> ().connectedBody = coll.transform.GetComponent<Rigidbody2D> ();
				m_FoundAJoin = true;
				gameObject.layer = 10; //"BackgroundDestructibles"
			}
		} 
		else 
		{
			if (m_JoinedObject == null) {
				gameObject.GetComponent<Explodable> ().explode ();
				ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
				ef.doCustomExplosion(transform.position, 200);
			}
		}
	}
}
