using TMPro;
using UnityEngine;

public class DiceSide : MonoBehaviour
{
    internal SpriteRenderer spriteRenderer;
    internal TextMeshPro diceSideText;
    internal int diceSideNumber;

    private void Awake()
    {
        diceSideText = this.GetComponent<TextMeshPro>();
    }
}