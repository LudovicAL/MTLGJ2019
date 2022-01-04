using UnityEngine;

public class Utility
{
    public static float GroundDistance(GameObject _target)
    {
        RaycastHit2D hit = Physics2D.Raycast(_target.transform.position, new Vector2(0.0f, -100.0f));

        if (hit.collider != null && hit.transform.CompareTag("Ground"))
        {
            return hit.distance;
        }
        return 0;
    }

    public static float GetTerrainHeightAtPosition(Vector3 _targetPos)
    {
        Vector3 castOrigin = _targetPos + new Vector3(0.0f, 50.0f, 0.0f);
        RaycastHit2D hit = Physics2D.Raycast(castOrigin, new Vector2(0.0f, -100.0f));

        if (hit.collider != null && hit.transform.CompareTag("Ground"))
        {
            return hit.point.y;
        }
        return 0;
    }
}
