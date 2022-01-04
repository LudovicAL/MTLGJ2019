using UnityEngine;

[CreateAssetMenu(fileName = "Useable")]
public class Useables : ScriptableObject
{
    public UseableManager.UseableType useableType;
    public int valueOfUseable;
    public Sprite useableSprite;
}
