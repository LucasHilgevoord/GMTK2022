using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Character> Enemies => _enemies;
    private List<Character> _enemies;

    public Character FocussedEnemy => testEnemy;//_enemies[_focussedEnemyIndex];
    private int _focussedEnemyIndex;

    public Character testEnemy;

    private void Initialize(CharacterData[] characterData)
    {
        foreach (CharacterData character in characterData)
        {
            // Create character
        }
        
        _focussedEnemyIndex = 0;
    }
}
