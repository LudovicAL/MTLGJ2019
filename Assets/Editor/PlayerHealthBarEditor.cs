using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerHealthBar))]
public class PlayerHealthBarEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		PlayerHealthBar playerHealthBar = (PlayerHealthBar)target;
		if (GUILayout.Button("OnTakingDamage")) {
			playerHealthBar.OnTakingDamage(playerHealthBar.player.playerId, playerHealthBar.player.playerId.currentHealth / playerHealthBar.player.playerId.maxHealth);
		}
	}
}