using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PlayerId", menuName = "PlayerId")]
public class PlayerId : ScriptableObject {
    public string playerName;
    public Controls controls;
	public SetOfSoundsFx setOfSoundsFx;
	public float currentHealth = 100f;
	public float maxHealth = 100f;
	public Color color = Color.white;
	public GameObject spawnPosition;
    public int ammo;
    public int maxAmmo;
    public float armor;
    public float startingArmor;
    public int weapon;
    public int startingWeapon;
    public bool looting;
	public StaticData.Sex sex = StaticData.Sex.Female;
	public Sprite controlUiImage;

	[HideInInspector]
	public Player player;
	[HideInInspector]
    public GameObject panelJoin;
	[HideInInspector]
	public GameObject panelHealthBar;
	[HideInInspector]
	public Image greenHealthBar;
	[HideInInspector]
	public GameObject avatar;
	[HideInInspector]
	public StaticData.AvailableTeams team = StaticData.AvailableTeams.Undecided;
}