using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private int _startIndex = 0;
    [SerializeField] private float _animationSpeed = 10;
    private float swapTime;
    private Image _img;
    private int _index = 0;

    private void Awake()
    {
        _img = GetComponent<Image>();
        swapTime = Time.time;
    }

    private void Update()
    {
        if (_sprites.Length == 0)
            return;
        if (Time.time > swapTime + _animationSpeed)
        {
            _index = (_index + 1) % _sprites.Length;
            _img.sprite = _sprites[_index];
            swapTime = Time.time;
        }
    }
}
