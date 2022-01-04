using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PlayerActions))]
public class PlayerActionsEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		PlayerActions playerActions = (PlayerActions)target;
		if (GUILayout.Button("Attack")) {
			//playerActions.Attack(); // sorry, changed this to WeaponUtility, won't be super clean to port this and it doesn't look used
		}
	}
}