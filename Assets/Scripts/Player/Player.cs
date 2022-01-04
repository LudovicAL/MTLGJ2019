using UnityEngine;

public class Player : MonoBehaviour {
	public PlayerId playerId;
	public PlayerHealth playerHealth;
	public PlayerHealthBar playerHealthBar;
	public PlayerActions playerActions;
    public PlayerAnim playerAnim;
	public AudioSource audioSource;
    public float walkSpeed = 30.0f;
    public float crouchSpeed = 10.0f;
    public float jumpForce = 60.0f;
    public GameObject weaponPrefab;

    private DataAccessor dataAccessor;
    private GameObject equippedWeapon;
    private bool isFlipped = false;
	[HideInInspector]public bool m_IsJumpQueued = false;
	[HideInInspector]public bool m_IsJumping = false;

	//Hack hack hack!
	[HideInInspector]public float m_BazookaTimer;
	[HideInInspector]public float m_GrenadeTimer;


    private void Start () {
        dataAccessor = GameObject.FindObjectOfType<DataAccessor>();
        playerAnim = GetComponent<PlayerAnim>();
		m_IsJumpQueued = false;
		m_BazookaTimer = 0.0f;
		m_GrenadeTimer = 0.0f;
    }

    private void OnBecameInvisible()
    {
		if (GameStatesManager.gameState == StaticData.AvailableGameStates.Playing) {
            if (transform.position.y < 0.0f)
			    playerHealth.TakeDamage(9999.0f);
		}
    }

    public DataAccessor GetDataAccessor() { return dataAccessor; }
    public GameObject GetEquippedWeapon() { return equippedWeapon; }
    public void SetEquippedWeapon(GameObject _weaponObject) { equippedWeapon = _weaponObject; }
    public void SetIsFlipped(bool _flipped) { isFlipped = _flipped; }
    public bool GetIsFlipped() { return isFlipped; }
}
