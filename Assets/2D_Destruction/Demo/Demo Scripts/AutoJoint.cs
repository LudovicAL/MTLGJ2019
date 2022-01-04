using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoJoint : MonoBehaviour
{
	public List<string> jointPartners;
    // Start is called before the first frame update
    void Start()
    {
		jointPartners = new List<string> ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.GetComponent<AutoJoint> ()) {
			if (!jointPartners.Contains (coll.gameObject.name)) {
				jointPartners.Add (coll.gameObject.name);
				gameObject.AddComponent<FixedJoint2D>().connectedBody = coll.transform.GetComponent<Rigidbody2D> ();;
			}
		}
	}
}
