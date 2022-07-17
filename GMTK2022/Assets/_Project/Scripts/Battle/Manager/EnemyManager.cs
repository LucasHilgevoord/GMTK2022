using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static event Action NextEnemyAppeared;

    public string[] characterSkins;
    public RectTransform enemyField;
    public Character enemyPrefab;
    private float startPosX;

    private Character currentEnemy;
    public Character FocussedEnemy => currentEnemy;
    private int _focussedEnemyIndex;
    
    private void Awake()
    {
        startPosX = enemyField.anchoredPosition.x;
        _focussedEnemyIndex = -1;
    }

    internal bool NextEnemy()
    {
        if (currentEnemy != null)
            Destroy(currentEnemy.gameObject);

        // Reset field
        enemyField.anchoredPosition = new Vector2(500, enemyField.anchoredPosition.y);

        _focussedEnemyIndex++;
        
        // No more enemies to fight!
        if (_focussedEnemyIndex >= characterSkins.Length)
        {
            _focussedEnemyIndex = 0;
            return false;
        }

        currentEnemy = Instantiate(enemyPrefab, enemyField.transform);
        currentEnemy.spineHandler.ChangeSkin(characterSkins[_focussedEnemyIndex]);

        // Move enemy to start position
        enemyField.DOAnchorPos(new Vector2(startPosX, enemyField.anchoredPosition.y), 0.5f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => { NextEnemyAppeared?.Invoke(); });

        return true;
    }
}
