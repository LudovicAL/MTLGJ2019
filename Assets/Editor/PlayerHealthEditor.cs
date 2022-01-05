using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerHealth))]
public class PlayerHealthEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		PlayerHealth playerHealth = (PlayerHealth)target;
		if (GUILayout.Button("TakeDamage")) {
			playerHealth.TakeDamage(10f);
		}
	}
}