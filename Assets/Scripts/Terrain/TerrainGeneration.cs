using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code based on example from class (https://github.com/greggddqu/ProceduralGenExs/blob/main/Assets/Scripts/MyLevelGen.cs)
// Also based on tutorial provided in class example (https://gamedevacademy.org/complete-guide-to-procedural-level-generation-in-unity-part-1/)

public class TerrainGeneration : MonoBehaviour
{
    [SerializeField] private int MapWidthInTiles, MapDepthInTiles, NumBuildings;
    [SerializeField] private GameObject TilePrefab, BuildingPrefab, TreePrefab;
    [SerializeField] private bool RandomSeed;
    [SerializeField] List<GameObject> Tiles, Buildings;

    private void Start()
    {
        Tiles = new List<GameObject>();
        Buildings = new List<GameObject>();
        GenerateMap();
    }

    private void Update()
    {
        if(Buildings.Count < NumBuildings)
        {
            PlaceBuilding();
        }
    }

    private void GenerateMap()
    {
        Vector3 tileSize = TilePrefab.GetComponent<MeshRenderer>().bounds.size;
        int tileWidth = (int)tileSize.x;
        int tileDepth = (int)tileSize.z;

        // Randomizing the tile seeds
        if (RandomSeed)
        {
            TileGeneration tg = TilePrefab.GetComponent<TileGeneration>();
            tg.RandomizeLevelSeeds();
        }

        for (int xTileIndex = 0; xTileIndex < MapWidthInTiles; xTileIndex++)
        {
            for (int zTileIndex = 0; zTileIndex < MapDepthInTiles; zTileIndex++)
            {
                Vector3 tilePosition = new Vector3(transform.position.x + xTileIndex * tileWidth, transform.position.y, transform.position.z + zTileIndex * tileDepth);
                GameObject tile = Instantiate(TilePrefab, tilePosition, Quaternion.identity);
                Tiles.Add(tile);
            }
        }
    }

    private void PlaceBuilding()
    {
        // pick tile to spawn building on
        int buildingSpawnTileX = Random.Range(0, MapWidthInTiles);
        int buildingSpawnTileZ = Random.Range(0, MapDepthInTiles);
        int buildingSpawnTileIndex = buildingSpawnTileX * MapWidthInTiles + buildingSpawnTileZ;

        // if the tile is fully generated, place the building above it
        if((Tiles[buildingSpawnTileIndex].GetComponent<TileGeneration>() != null && (Tiles[buildingSpawnTileIndex].GetComponent<TileGeneration>().GetFinishedGenerating())))
        {
            GameObject bld = Instantiate(BuildingPrefab);
            Vector3[] vertices = Tiles[buildingSpawnTileIndex].GetComponent<MeshFilter>().mesh.vertices;
            Debug.Log(Tiles.Count);
            int buildingSpawnVertexIndex = Random.Range(0, vertices.Length);
            bld.transform.position = vertices[buildingSpawnVertexIndex] + Tiles[buildingSpawnTileIndex].transform.position + new Vector3(0, 3f, 0);
            Buildings.Add(bld);
        }
    }

    // Accessor methods to determine where to spawn the player and goal
    public int GetMapWidth()
    {
        return MapWidthInTiles;
    }

    public int GetMapDepth()
    {
        return MapDepthInTiles;
    }

    public int GetTileWidth()
    {
        return (int)TilePrefab.GetComponent<MeshRenderer>().bounds.size.x;
    }

    public int GetTileDepth()
    {
        return (int)TilePrefab.GetComponent<MeshRenderer>().bounds.size.z;
    }
}
