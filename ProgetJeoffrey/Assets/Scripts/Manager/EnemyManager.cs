using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    internal void EnemyTakeDamage(Enemy enemy, int damage) 
    {
        enemy.TakeDamage(damage);
    }

    internal void EnemyDied(GameObject enemy) 
    {
        Destroy(enemy);
    }
}
