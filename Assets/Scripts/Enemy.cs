using UnityEngine;

public class Enemy : MonoBehaviour
{
// start
    void Start()
    {
        EnemyManager.instance.AddEnemy(this);
    }
    
    void OnDestroy()
    {
        EnemyManager.instance.RemoveEnemy(this);
    }
}

