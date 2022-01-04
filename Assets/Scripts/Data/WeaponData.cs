using UnityEngine;

[CreateAssetMenu(menuName = "Player/Weapons")]
public class WeaponData : ScriptableObject
{
    public int WeaponIndex;
    public Sprite WeaponSprite;
    public Vector2 AttachOffset;
    public float BulletSpawnDistance;
    public GameObject ProjectilePrefab;
    public float AccuracyDistance;
    public float AccuracyAngleRange;
    public bool IsAutomatic;
    public float MinTimeBetweenShots;
    // ammo, recoil, fire rate
}
