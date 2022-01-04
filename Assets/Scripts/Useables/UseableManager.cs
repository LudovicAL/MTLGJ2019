using UnityEngine;

public class UseableManager : MonoBehaviour
{
    [SerializeField] DataAccessor dataAccessor;

    #region SingletonPattern
    public static UseableManager Instance { get; private set; }

    void Awake()
    {
        //Makes this script a singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion SingletonPattern

    #region Initialize
    public void SetUseableEffect(Useables useables)
    {
        switch (useables.useableType)
        {
            case UseableType.Ammo:
                SetAmmo();
                break;
            case UseableType.Armor:
                SetArmor();
                break;
            case UseableType.Healing:
                SetHealing();
                break;
            case UseableType.Weapon:
                SetWeapons(useables);
                break;
        }
    }

    public void GetUseableEffect(Transform item, GameObject avatar, Useables useables)
    {
        Player player = avatar.GetComponent<Player>();
        //if (player.playerId.looting) // always loot for now!
        {
            bool used = false;
            switch (useables.useableType)
            {
                case UseableType.Ammo:
                    used = GetAmmo(player, useables.valueOfUseable);
                    break;
                case UseableType.Armor:
                    used = GetArmor(player, useables.valueOfUseable);
                    break;
                case UseableType.Healing:
                    used = GetHealing(player, useables.valueOfUseable);
                    break;
                case UseableType.Weapon:
                    used = GetWeapon(avatar, useables.valueOfUseable);
                    break;
            }
            if (used)
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    public enum UseableType
    {
        Healing,   //Healing stuff
        Ammo,   //More ammo
        Armor,    //Move a level down or up
        MountedWeapon, //Activate a mounted weapon
        Weapon  //Does nothing or do something quite custom
    };
    #endregion

    #region UseableSet
    private void SetArmor()
    {

    }

    private void SetHealing()
    {

    }

    private void SetAmmo()
    {

    }

    private void SetWeapons(Useables useables)
    {
        int weaponIndex = dataAccessor.GetRandomWeaponIndex();

        WeaponData weaponData;
        bool foundWeapon = dataAccessor.TryGetWeapon(weaponIndex, out weaponData);
        if (foundWeapon && weaponData != null)
        {
            useables.useableSprite = weaponData.WeaponSprite;
            useables.valueOfUseable = weaponIndex;
        }
    }
    #endregion

    #region UseableGet
    private bool GetArmor(Player player, int value)
    {
        player.playerId.armor = value;
        return true;
    }

    private bool GetHealing(Player player, int value)
    {
        if (player.playerId.currentHealth < player.playerId.maxHealth )
        {
            float cHealth = player.playerId.currentHealth + value;
            float mHealth = player.playerId.maxHealth;
            player.playerId.currentHealth = Mathf.Min(cHealth, mHealth);
            return true;
        }
        return false;
    }

    private bool GetAmmo(Player player, int value)
    {
        if (player.playerId.ammo < player.playerId.maxAmmo)
        {
            int cAmmo = player.playerId.ammo + value;
            int mAmmo = player.playerId.maxAmmo;
            player.playerId.ammo = Mathf.Min(cAmmo , mAmmo);
            return true;
        }
        return false;
    }

    private bool GetWeapon(GameObject player, int value)
    {
        WeaponUtility.PickupWeapon(player.GetComponent<Player>(), value);
        return true;
    }
    #endregion
}
