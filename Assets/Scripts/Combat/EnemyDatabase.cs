using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDatabase : MonoBehaviour
{
    public List<EnemyData> Enemies = new List<EnemyData>();

    // Return EnemyData by enemy id
    public EnemyData GetEnemyById(int id)
    {
        foreach (var enemyData in Enemies)
        {
            if (enemyData.Id == id)
            {
                return enemyData;
            }
        }

        return null;
    }

    // Return a list of EnemyData with matching type and rarity
    public List<EnemyData> GetEnemyByTypeRarity(EnemyType type, EnemyRarity rarity)
    {
        var enemies = new List<EnemyData>();
        foreach (var enemyData in Enemies)
        {
            if (enemyData.Type == type && enemyData.Rarity == rarity)
            {
                enemies.Add(enemyData);
            }
        }

        return enemies;
    }
}
