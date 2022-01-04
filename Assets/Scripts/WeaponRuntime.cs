using UnityEngine;

public class WeaponRuntime : MonoBehaviour
{
    private WeaponData weaponData;
    private float fireRateCooldown = 0.0f;

    void Update()
    {
        if (fireRateCooldown > 0.0f) fireRateCooldown -= Time.deltaTime;
    }

    public bool IsInCooldown() { return fireRateCooldown > 0.0f; }
    public void SetFireRateCooldown(float _cooldown) { fireRateCooldown = _cooldown; }
    public void SetWeaponData(WeaponData _weaponData) { weaponData = _weaponData; }
    public WeaponData GetWeaponData() { return weaponData; }
}
