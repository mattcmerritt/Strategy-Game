using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // map size params, steal from TerrainGeneration
    private int width, depth;
    private float squareSize;

    [SerializeField] private Enemy[] AllEnemies;
    [SerializeField] private float WaveSpawnTimer, CoordinatedAttackTimer;
    private float WaveSpawnCooldown = 45f, CoordinatedAttackCooldown = 90f;

    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private TerrainGeneration Map;

    [SerializeField] private int enemyBase = 2;
    [SerializeField] private int waveNumber = 0;

    // Setting up singleton
    public static EnemyManager Instance;

    private void Start()
    {
        Instance = this;

        WaveSpawnTimer = WaveSpawnCooldown;
        CoordinatedAttackTimer = CoordinatedAttackCooldown;

        // collect map data
        width = Map.GetWidth();
        depth = Map.GetDepth();
        squareSize = Map.GetSquareSize();
    }

    private void Update() 
    {
        WaveSpawnTimer -= Time.deltaTime;
        CoordinatedAttackTimer -= Time.deltaTime;

        if (WaveSpawnTimer <= 0f)
        {
            Debug.Log("Spawning Wave!");
            // trees spawn in a radius around the center, meaning that the corner 1/64ths of the map are clear
            Vector3[] minCorners =
            {
                new Vector3(0f, 0f, 0f),
                new Vector3(7f/8f * width * squareSize, 0f, 0f),
                new Vector3(0f, 0f, 7f/8f * depth * squareSize),
                new Vector3(7f/8f * width * squareSize, 0f, 7f/8f * depth * squareSize),
            };
            Vector3[] maxCorners =
            {
                new Vector3(1f/8f * width * squareSize, 0f, 1f/8f * depth * squareSize),
                new Vector3(width * squareSize, 0f, 1f/8f * depth * squareSize),
                new Vector3(1f/8f * width * squareSize, 0f, depth * squareSize),
                new Vector3(width * squareSize, 0f, depth * squareSize),
            };

            // pick a corner to spawn the wave in
            int corner = Random.Range(0, 4);

            int numberEnemiesInWave = Mathf.RoundToInt(Mathf.Pow(enemyBase, waveNumber));
            for (int i = 0; i < numberEnemiesInWave; i++)
            {
                float x = Random.Range(minCorners[corner].x, maxCorners[corner].x);
                float z = Random.Range(minCorners[corner].z, maxCorners[corner].z);
                float y = Map.FindHeightAtLocation(new Vector3(x, 0, z));

                Instantiate(EnemyPrefab, new Vector3(x, y, z), Quaternion.identity);
            }

            // start the timer again
            WaveSpawnTimer = WaveSpawnCooldown;
            waveNumber++;
        }

        if(CoordinatedAttackTimer <= 0f)
        {
            AllEnemies = GameObject.FindObjectsOfType<Enemy>();
            if(AllEnemies.Length > 2)
            {
                float minDistance = Mathf.Infinity;
                Agent target = null;
                // perform coordinated attack
                foreach(Enemy e in AllEnemies)
                {
                    if(e.GetMinDistance() <= minDistance)
                    {
                        minDistance = e.GetMinDistance();
                        target = e.GetTarget();
                    }
                }
                // Debug.Log($"Coordinating attack on {target.gameObject.name}");
                foreach(Enemy e in AllEnemies)
                {
                    e.SetTarget(target);
                    e.ChangeState(new EnemyPursueState());
                }
            }

            // start the timer again
            CoordinatedAttackTimer = CoordinatedAttackCooldown;
        }
    }

    public int GetWaveCounter()
    {
        return waveNumber;
    }

    public float GetTimeForWave()
    {
        return WaveSpawnTimer;
    }
}