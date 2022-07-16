using UnityEngine;

public class DiceSide : MonoBehaviour
{
    public DiceAbility type;
    internal SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }
}