using UnityEngine;

public class DiceSide : MonoBehaviour
{
    public CardAbility type;
    internal SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }
}