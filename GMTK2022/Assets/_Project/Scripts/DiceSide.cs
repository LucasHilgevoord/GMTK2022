using TMPro;
using UnityEngine;

public class DiceSide : MonoBehaviour
{
    internal TextMeshPro diceSideText;
    internal int diceSideNumber;

    private void Awake()
    {
        diceSideText = this.GetComponent<TextMeshPro>();
    }
}