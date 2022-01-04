using UnityEngine;

public static class WeaponUtility
{
    public struct WeaponResult
    {
        public bool justAttacked;
    }

    public static void ApplyWeaponInput(Player _player, out WeaponResult _result)
    {
        GameObject equippedWeapon = _player.GetEquippedWeapon();
        WeaponRuntime weaponRuntime = equippedWeapon ? equippedWeapon.GetComponent<WeaponRuntime>() : null;
        WeaponData weaponData = weaponRuntime ? weaponRuntime.GetWeaponData() : null;
        _result = new WeaponResult();

        // aiming (right stick)
        HandleAiming(_player, equippedWeapon);

        // shooting
        HandleShooting(_player, equippedWeapon, ref _result);

    }

    private static void HandleShooting(Player _player, GameObject _equippedGun, ref WeaponResult _result)
    {
		//HACK HACK HACK HACK
		bool attack = false;

		_player.m_BazookaTimer -= Time.deltaTime;
		_player.m_GrenadeTimer -= Time.deltaTime;

		if (_player.playerId.controls.GetButtonXDown ()) {
			PickupWeapon(_player, 0);
			attack = true;
		}

		if (_player.playerId.controls.GetButtonYDown () && _player.m_GrenadeTimer <= 0.0f) {
			PickupWeapon(_player, 3);
			attack = true;
			_player.m_GrenadeTimer = 2.0f;
		}

		if (_player.playerId.controls.GetButtonBDown ()  && _player.m_BazookaTimer <= 0.0f) {
			PickupWeapon(_player, 1);
			attack = true;
			_player.m_BazookaTimer = 2.0f;
		}


		//HACK HACK HACK HACK

        WeaponRuntime weaponRuntime = _equippedGun ? _equippedGun.GetComponent<WeaponRuntime>() : null;
        WeaponData weaponData = weaponRuntime ? weaponRuntime.GetWeaponData() : null;
        if (!weaponData)
            return;


        //bool attack = weaponData.IsAutomatic ? _player.playerId.controls.GetRBumper() : _player.playerId.controls.GetRBumperDown();
        if (!attack)
            return;

        if (weaponRuntime.IsInCooldown())
            return;

        Vector3 shootDirection = !_player.GetIsFlipped() ? Vector3.left : Vector3.right;
        shootDirection = _equippedGun.transform.rotation * shootDirection;

        Vector3 muzzlePos = _equippedGun.transform.position;
        muzzlePos += shootDirection * weaponData.BulletSpawnDistance; // small offset for shot

        Vector3 targetPosition = muzzlePos;
        targetPosition += shootDirection * weaponData.AccuracyDistance;
        targetPosition += new Vector3(Random.Range(-weaponData.AccuracyAngleRange, weaponData.AccuracyAngleRange), Random.Range(-weaponData.AccuracyAngleRange, weaponData.AccuracyAngleRange), 0.0f);
        //GLDebug.DrawCircle2d(muzzlePos, 0.5f, Color.red, 1.0f);
        //GLDebug.DrawCircle2d(targetPosition, 0.5f, Color.green, 1.0f);

        ProjectileManager.RequestAddProjectile(weaponData.ProjectilePrefab, muzzlePos, targetPosition - muzzlePos);
        weaponRuntime.SetFireRateCooldown(weaponData.MinTimeBetweenShots);
        _result.justAttacked = true;
    }

    private static void HandleAiming(Player _player, GameObject _equippedGun)
    {
        if (!_equippedGun)
            return;

        float hortizontal = _player.playerId.controls.GetLHorizontal();
        float vertical = _player.playerId.controls.GetLVertical();
        float meaningfulMovmenet = 0.215f;
        if (Mathf.Abs(hortizontal) <= meaningfulMovmenet && Mathf.Abs(vertical) <= meaningfulMovmenet)
            return;

        if (!_player.GetIsFlipped())
        {
            hortizontal *= -1;
            vertical *= -1;
        }
        hortizontal = Mathf.Max(0.1f, hortizontal); // clamp to not go around

        float eulerAngle = Mathf.Atan2(vertical, hortizontal) * Mathf.Rad2Deg;
        _equippedGun.transform.rotation = Quaternion.Euler(0.0f, 0.0f, eulerAngle);
    }

    public static void PickupWeapon(Player _player, int _weaponIdx)
    {
        DataAccessor dataAccessor = _player.GetDataAccessor();
        if (!dataAccessor)
        {
            Debug.LogWarning("Trying to pickup weapon but player did not load the data accessor!");
            return;
        }

        WeaponData weaponData;
        bool foundWeapon = dataAccessor.TryGetWeapon(_weaponIdx, out weaponData);
        if (!foundWeapon || weaponData == null)
        {
            Debug.LogWarning("Trying to pickup weapon " + _weaponIdx + " could not find its data");
            return;
        }

        GameObject attachedWeapon = _player.GetEquippedWeapon();
        if (attachedWeapon == null)
        {
            attachedWeapon = GameObject.Instantiate(_player.weaponPrefab, _player.transform);
            _player.SetEquippedWeapon(attachedWeapon);
        }

        SpriteRenderer weaponRenderer = attachedWeapon.GetComponent<SpriteRenderer>();
        if (!weaponRenderer)
            weaponRenderer.sprite = weaponData.WeaponSprite;

        Vector2 attachOffset = weaponData.AttachOffset;
        if (!_player.GetIsFlipped()) attachOffset.x *= -1f;
        attachedWeapon.transform.localPosition = attachOffset;

        weaponRenderer.flipX = !_player.GetIsFlipped();
        weaponRenderer.sprite = weaponData.WeaponSprite;
        attachedWeapon.GetComponent<WeaponRuntime>().SetWeaponData(weaponData);

        AudioManager.Instance.PlayClip(AudioManager.Instance.weaponSwitch);
    }
}
