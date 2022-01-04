using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour {

	public Player player;
	public PlayerTakingDamage playerTakingDamage = new PlayerTakingDamage();

	[System.Serializable]
	public class PlayerTakingDamage : UnityEvent<PlayerId, float> {}

	// Use this for initialization
	void Start () {
		player.playerId.currentHealth = player.playerId.maxHealth;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		float collisionSpeed = coll.relativeVelocity.magnitude;
		if (collisionSpeed > 30.0f) {
            float damageAmount = Mathf.Clamp(collisionSpeed / 5.0f, 0.0f, 10.0f);
			TakeDamage(damageAmount);
		}
	}

	//Call this function when dealing damage to this player
	public void TakeDamage(float amount) {
		if (player.playerId.currentHealth > 0f) {
			player.playerId.currentHealth = Mathf.Clamp((player.playerId.currentHealth - amount), 0.0f, player.playerId.maxHealth);
			playerTakingDamage.Invoke(player.playerId, player.playerId.currentHealth / player.playerId.maxHealth);
			if (Mathf.Approximately(player.playerId.currentHealth, 0.0f)) {
				Debug.Log(player.playerId.name + " has died.");
				player.playerId.currentHealth = 0.0f;
				if (player.playerId.sex == StaticData.Sex.Female) {
					AudioManager.Instance.PlayClip(AudioManager.Instance.GetRandomClipFromList(AudioManager.Instance.listOfFemaleDeathScreamClips));
				} else {
					AudioManager.Instance.PlayClip(AudioManager.Instance.GetRandomClipFromList(AudioManager.Instance.listOfMaleDeathScreamClips));
				}
				PlayerListManager.Instance.PlayerDied(player.playerId);
			}
		}
	}
}
