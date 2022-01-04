using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetOfSoundFx", menuName = "SetOfSoundFx")]
public class SetOfSoundsFx : ScriptableObject {

	public List<AudioClip> damage;
	public List<AudioClip> attack;
	public List<AudioClip> death;

	private AudioClip GetRandomSoundFxFromList(List<AudioClip> list) {
		if (list.Count > 0) {
			return list[Random.Range(0, list.Count)];
		}
		return null;
	}

	public AudioClip GetDamageSoundFx() {
		return GetRandomSoundFxFromList(damage);
	}
	public AudioClip GetAttackSoundFx() {
		return GetRandomSoundFxFromList(attack);
	}
	public AudioClip GetDeathSoundFx() {
		return GetRandomSoundFxFromList(death);
	}
}
