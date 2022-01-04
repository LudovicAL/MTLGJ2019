using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public GameObject gunProjectilePrefab;
    public GameObject bazookaProjectilePrefab;
	public GameObject grenadeProjectilePrefab;
	private int frameCounter;

    private void Start()
    {
		frameCounter = 0;
    }

    private void Update()
    {
        if (PlayerListManager.Instance.isDebugging)
        {
            DebugShooting();
        }
    }

    #region DebugShooting
    private void DebugShooting()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector3 startPoint = transform.position;//Camera.main.ScreenToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        Vector3 shootAtPoint = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x + Random.Range(-30.0f, 30.0f), mousePos.y + Random.Range(-30.0f, 30.0f), 0.0f));
        GLDebug.DrawCircle2d(shootAtPoint, 0.5f, Color.green, 1.0f);
        Vector3 shootDir = shootAtPoint - startPoint;

        if (Input.GetMouseButton(0))
        {
            if (frameCounter == 0)
            {
                RequestAddProjectile(gunProjectilePrefab, startPoint, shootDir);
            }

            frameCounter++;
            if (frameCounter > 5)
                frameCounter = 0;
        }

        if (Input.GetMouseButtonDown(1))
        {
            RequestAddProjectile(bazookaProjectilePrefab, startPoint, shootDir);
        }

        if (Input.GetMouseButtonDown(2))
        {
            RequestAddProjectile(grenadeProjectilePrefab, startPoint, shootDir);
        }
    }
    #endregion

    public static void RequestAddProjectile(GameObject _prefab, Vector2 _startPosition, Vector2 _direction)
    {
        if (_prefab == null)
        {
            Debug.LogWarning("Cannot find projectile prefab, link it to the ProjectileManager Monobehaviour.");
            return;
        }

        GameObject spawnedProjectile = GameObject.Instantiate(_prefab, _startPosition, Quaternion.FromToRotation(Vector3.up, _direction));
        ProjectileData projectileData = spawnedProjectile.GetComponent<ProjectileData>();
        Rigidbody2D rigidBody = spawnedProjectile.GetComponent<Rigidbody2D>();
        if (rigidBody == null || projectileData == null)
        {
            Debug.LogWarning("Spawned projectile needs a Rigidbody2D and a ProjectileData comnponent.");
            return;
        }

        rigidBody.gravityScale = projectileData.GravityScale;
        rigidBody.AddForce(_direction.normalized * projectileData.Speed);
    }
}
