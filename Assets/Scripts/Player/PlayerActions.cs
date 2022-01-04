using UnityEngine;
using UnityEngine.Events;

public class PlayerActions : MonoBehaviour {

	public Player player;
    public GameObject playerAvatar;
	public PlayerAttacking playerAttacking = new PlayerAttacking();

	[System.Serializable]
	public class PlayerAttacking : UnityEvent<PlayerId> {}

    private SpriteRenderer spriteRenderer; 
    private Rigidbody2D rigidBody;
    private void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update () {
		if (GameStatesManager.gameState == StaticData.AvailableGameStates.Playing) {
			if (player.playerId.controls.GetButtonStartDown()) {
				GameStatesManager.Instance.ChangeGameStateTo(StaticData.AvailableGameStates.Pausing);
			} else {
				
                // TODO: real pickup, not pickup mode
                if (player.playerId.controls.GetButtonYDown()) {
                    player.playerId.looting = !player.playerId.looting;
                }

                MovementUtility.MoveResult moveResult;
                MovementUtility.ApplyMovementInput(player, out moveResult);

                WeaponUtility.WeaponResult weaponResult;
                WeaponUtility.ApplyWeaponInput(player, out weaponResult);

                if (weaponResult.justAttacked)
                    playerAttacking.Invoke(player.playerId);

                // Do at the end to affect weapon
                if (moveResult.justFlipped)
                    FlipPlayer(player.GetIsFlipped());

                // anim states
                player.playerAnim.Walking = moveResult.isWalking;
                player.playerAnim.Jumping = moveResult.isJumping;
                player.playerAnim.Crouching = moveResult.isCrouching;
            }
		} else if (GameStatesManager.gameState == StaticData.AvailableGameStates.Pausing) {
			if (player.playerId.controls.GetButtonStartDown()) {
				GameStatesManager.Instance.ChangeGameStateTo(StaticData.AvailableGameStates.Playing);
			} else if (player.playerId.controls.GetButtonBackDown()) {
				GameStatesManager.Instance.ChangeGameStateTo(StaticData.AvailableGameStates.Menu);
			}
		}
    }

    private void FlipPlayer(bool _newFlip)
    {
        int y = _newFlip ? -180 : 0;
        Quaternion playerAvatarRotation = playerAvatar.transform.rotation;
        playerAvatarRotation.y = y;
        playerAvatar.transform.rotation = playerAvatarRotation;

        GameObject equippedWeapon = player.GetEquippedWeapon();
        WeaponRuntime weaponRuntime = equippedWeapon ? equippedWeapon.GetComponent<WeaponRuntime>() : null;
        WeaponData weaponData = weaponRuntime ? weaponRuntime.GetWeaponData() : null;
        if (weaponData)
        {
            Vector2 attachOffset = weaponData.AttachOffset;
            if (!_newFlip) attachOffset.x *= -1f;
            equippedWeapon.transform.localPosition = attachOffset;

            SpriteRenderer weaponRenderer = equippedWeapon.GetComponent<SpriteRenderer>();
            if (weaponRenderer) weaponRenderer.flipX = !_newFlip;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
	{
		MovementUtility.TriggerCallback(other, player);
	}

    private void OnTriggerStay2D(Collider2D other)
	{
		MovementUtility.TriggerCallback(other, player);
	}
}
