using UnityEngine;

public static class MovementUtility
{
    // To communicate result with other systems (weapon, anim, sprite renderer)
    public struct MoveResult
    {
        public bool isWalking;
        public bool isCrouching;
        public bool isJumping;
        public bool justFlipped;
    }
		
    public static void ApplyMovementInput(Player _player, out MoveResult _result)
    {
        Rigidbody2D rigidBody = _player.GetComponent<Rigidbody2D>();
        Collider2D collider = _player.GetComponent<Collider2D>();
        _result = new MoveResult();

        bool crouch = _player.playerId.controls.GetLBumper();
		if (_player.playerId.controls.GetLBumperDown())
		{
            AudioManager.Instance.PlayClip(AudioManager.Instance.crouchClip);
        }

        float horizontal = _player.playerId.controls.GetLHorizontal();
        float minToFlip = 0.215f; // could too easily end up flipping when released stick abruptly
        bool justFlipped = false;
        if (horizontal != 0.0f && rigidBody)
        {
			if (Mathf.Abs(horizontal) > minToFlip)
			{
				bool oldFlip = _player.GetIsFlipped();
				_player.SetIsFlipped(horizontal > 0.0f);
				justFlipped = true;
			}

            float speed = crouch ? _player.crouchSpeed : _player.walkSpeed;
            rigidBody.AddForce(new Vector2(horizontal * speed, 0.0f));
        }

		_player.m_IsJumpQueued = _player.playerId.controls.GetButtonA();


        // result
        _result.isWalking = Mathf.Abs(horizontal) > minToFlip;
        _result.isCrouching = crouch;
		_result.isJumping = _player.m_IsJumpQueued;
        _result.justFlipped = justFlipped;
    }

	public static void TriggerCallback(Collider2D other, Player _player)
	{
		if (_player.m_IsJumpQueued)
        {
			_player.m_IsJumping = true;
			_player.m_IsJumpQueued = false;
			AudioManager.Instance.PlayClip (AudioManager.Instance.GetRandomClipFromList (AudioManager.Instance.listOfJumpClips));
			_player.gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0.0f, _player.jumpForce));
		}
        else
        {
            _player.m_IsJumping = false;
        }
	}
}
