using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // map size params, steal from TerrainGeneration
    [SerializeField] private int width, depth;
    [SerializeField] private float squareSize;
    [SerializeField] private Enemy[] AllEnemies;
    [SerializeField] private float WaveSpawnTimer, CoordinatedAttackTimer;

    private void Start()
    {
        WaveSpawnTimer = 3f;
        CoordinatedAttackTimer = 2f;
    }

    private void Update() 
    {
        WaveSpawnTimer -= Time.deltaTime;
        CoordinatedAttackTimer -= Time.deltaTime;

        if(WaveSpawnTimer <= 0f)
        {
            
        }

        if(CoordinatedAttackTimer <= 0f)
        {
            AllEnemies = GameObject.FindObjectsOfType<Enemy>();
            if(AllEnemies.Length > 2)
            {
                float minDistance = Mathf.Infinity;
                Worker target = null;
                // perform coordinated attack
                foreach(Enemy e in AllEnemies)
                {
                    if(e.GetMinDistance() <= minDistance)
                    {
                        minDistance = e.GetMinDistance();
                        target = e.GetTarget();
                    }
                }
                Debug.Log($"Coordinating attack on {target.gameObject.name}");
                foreach(Enemy e in AllEnemies)
                {
                    e.SetTarget(target);
                    e.ChangeState(new EnemyPursueState());
                }
            }
        }
    }

}