using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataAccessor : MonoBehaviour
{
    Dictionary<int, WeaponData> weaponCache;

    private void Awake()
    {
        weaponCache = new Dictionary<int, WeaponData>();
        WeaponData[] weaponList = Resources.LoadAll<WeaponData>("Weapons"); // move to player? cache
        for (int i = 0; i < weaponList.Length; ++i)
        {
            weaponCache.Add(weaponList[i].WeaponIndex, weaponList[i]);
        }
    }

    public bool TryGetWeapon(int _index, out WeaponData _weaponData)
    {
        return weaponCache.TryGetValue(_index, out _weaponData);
    }

    public int GetRandomWeaponIndex()
    {
        List<WeaponData> datas = Enumerable.ToList(weaponCache.Values);
        if (datas.Count == 0)
            return -1;

        return Random.Range(0, datas.Count);
    }
}
