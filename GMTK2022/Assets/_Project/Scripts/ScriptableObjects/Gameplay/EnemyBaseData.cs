using UnityEngine;

/// <summary>
/// Scriptable object that holds the base hero data to only initially load enemies from
/// </summary>
[CreateAssetMenu(fileName = "EnemyBaseData", menuName = "ScriptableObjects/BaseData")]
public class EnemyBaseData : ScriptableObject
{
    [SerializeField]
    private int enemyID;

    [SerializeField]
    private string enemyName;

    [SerializeField]
    private int maxHealth;

    public int GetEnemyID() => enemyID;
    public string GetEnemyName() => enemyName;
    public int GetMaxHealth() => maxHealth;
}


