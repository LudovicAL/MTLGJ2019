using UnityEngine;

public class TestDebug : MonoBehaviour
{
    void Update()
    {
        Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
        Vector3 currentPos = rigidbody.transform.position;
        Vector3 posPlusVelocity = currentPos + new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0.0f);
        GLDebug.DrawArrow(currentPos, posPlusVelocity, 0.25f, 20, Color.green);
    }
}
