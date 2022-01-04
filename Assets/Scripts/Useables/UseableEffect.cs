using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class UseableEffect : MonoBehaviour
{
    [SerializeField] Useables useables;
    [SerializeField] SpriteRenderer itemArt;

    private void Start()
    {
        UseableManager.Instance.SetUseableEffect(useables);
        if (itemArt)
        {
            itemArt.sprite = useables.useableSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            UseableManager.Instance.GetUseableEffect(gameObject.transform.parent, collider.gameObject, useables);
        }
    }
}
