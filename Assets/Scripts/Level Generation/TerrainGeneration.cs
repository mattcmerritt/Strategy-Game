using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

// Procedurally generates the terrain on a single mesh
// Using the tiles would cause issues with the NavMesh not spawning correctly
// Additionally, the NavMesh had strange issues on the edges of tiles

public class TerrainGeneration : MonoBehaviour
{
    // Map size
    [SerializeField] private int width, depth; // number of squares in the mesh
    [SerializeField] private float heightLayers; // number of vertical slices in the map
    [SerializeField] private float layerHeight; // height of a single layer
    [SerializeField] private float squareSize; // size of the tiles

    // Map grid data
    private Vector3[] vertices; // vertices for our mesh
    private float[] heightMap; // heights of the tiles
    
    // Height map values
    [SerializeField] private float mapScale; // scale applied to perlin noise function

    // Building information
    [SerializeField] private float villageRadius; // determines how far out the buildings can be placed
    [SerializeField] private int numBuildings; // number of buildings to place on the map
    [SerializeField] private GameObject buildingPrefab; // generic building to place

    // Floor layer
    [SerializeField] private LayerMask floorLayer; // layer to ignore with box overlap
    [SerializeField] private GameObject markerPrefab; // debug tool for testing bounds

    private void Start()
    {
        // Generate height map
        heightMap = new float[(width + 1) * (depth + 1)];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                heightMap[x + z * width] = Mathf.Floor(Mathf.PerlinNoise((float) x / width * mapScale, (float) z / depth * mapScale) * heightLayers);
            }
        }

        //// Output height map in readable format
        //string heightMapString = "";
        //for (int x = 0; x < width; x++)
        //{
        //    for (int z = 0; z < depth; z++)
        //    {
        //        heightMapString += heightMap[x + z * width];
        //    }
        //    heightMapString += "\n";
        //}
        //Debug.Log(heightMapString);

        // Configure new mesh
        // Code for building the mesh data done using tutorial: https://catlikecoding.com/unity/tutorials/procedural-grid/
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        // Calculate all the vertices
        // There is one more row and column than there are squares in the map
        vertices = new Vector3[(width + 1) * (depth + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        for (int vertex = 0, z = 0; z <= depth; z++)
        {
            for (int x = 0; x <= width; x++, vertex++)
            {
                vertices[vertex] = new Vector3(x * squareSize, heightMap[x + z * width] * layerHeight, z * squareSize);
                uv[vertex] = new Vector2((float) x / width, (float) z / depth);
                tangents[vertex] = tangent;
            }
        }
        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.uv = uv;
        meshFilter.mesh.tangents = tangents;

        // Triangulating all the squares in the mesh
        // Each square has 2 triangles
        int[] triangles = new int[width * depth * 6];
        for (int square = 0, vertex = 0, z = 0; z < depth; z++, vertex++)
        {
            for (int x = 0; x < width; x++, square++, vertex++)
            {
                // Bottom triangle (joins the bottom left, top right, and bottom right)
                triangles[square * 6] = vertex;
                triangles[square * 6 + 1] = vertex + width + 1;
                triangles[square * 6 + 2] = vertex + 1;
                // Top triangle (joins the bottom right, top right, and top left)
                triangles[square * 6 + 3] = vertex + 1;
                triangles[square * 6 + 4] = vertex + width + 1;
                triangles[square * 6 + 5] = vertex + width + 2;
            }
        }
        meshFilter.mesh.triangles = triangles;
        meshFilter.mesh.RecalculateNormals();

        // Generating the NavMesh
        NavMeshSurface surface = GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();

        // Generating the collider
        MeshCollider collider = GetComponent<MeshCollider>();
        collider.sharedMesh = meshFilter.sharedMesh;

        // Placing the buildings onto the terrain
        GameObject tempBuilding = Instantiate(buildingPrefab, new Vector3(-100f, -100f, -100f), Quaternion.identity);
        // need to grab the bounds of the building from a copy of the prefab
        BoxCollider buildingCollider = tempBuilding.GetComponent<BoxCollider>();
        // cannot grab the values if it is not instantiated
        Bounds bound = buildingCollider.bounds;

        // finding distances to all four corners of a building
        Vector3[] corners = {
            new Vector3(bound.extents.x, 0f, bound.extents.z),
            new Vector3(-bound.extents.x, 0f, -bound.extents.z),
            new Vector3(-bound.extents.x, 0f, bound.extents.z),
            new Vector3(bound.extents.x, 0f, -bound.extents.z),
        };

        // Debug.Log($"max: {bound.max}, min: {bound.min}, extents: {bound.extents}");
        DestroyImmediate(tempBuilding); // remove from game once data is collected

        // placing the buildings
        for (int i = 0; i < numBuildings; i++)
        {
            bool placed = false;
            while (!placed)
            {
                // calculate where to put the building
                Vector3 direction = Vector3.Normalize(Random.Range(-1f, 1f) * Vector3.right + Random.Range(-1f, 1f) * Vector3.forward);
                Vector3 displacement = direction * Random.Range(0f, villageRadius);

                Vector3 mapCenter = transform.position + (Vector3.right * width * squareSize / 2) + (Vector3.forward * depth * squareSize / 2);
                Vector3 gridTarget = mapCenter + displacement; // location to attempt placing a house, less the vertical component

                // checking if the building location is valid
                Collider[] blockingColliders = Physics.OverlapBox(gridTarget, buildingPrefab.transform.localScale / 2, Quaternion.identity, ~floorLayer);

                // if nothing is blocking the building, place it down at the height of its lowest corner
                if (blockingColliders.Length == 0)
                {
                    // debug markers to ensure corners are in right place
                    float minimumHeight = (heightLayers + 1) * layerHeight;
                    foreach (Vector3 corner in corners)
                    {
                        // raycast down from the sky above the corners of the building to determine if it needs to be lower in the ground
                        Physics.Raycast((heightLayers + 1) * layerHeight * Vector3.up + gridTarget + corner, Vector3.down, out RaycastHit hit, (heightLayers + 1) * layerHeight, floorLayer);
                        // Instantiate(markerPrefab, gridTarget + corner + hit.point.y * Vector3.up, Quaternion.identity); // place debug markers
                        if (hit.point.y <= minimumHeight)
                        {
                            minimumHeight = hit.point.y;
                        }
                    }

                    Instantiate(buildingPrefab, gridTarget + (minimumHeight + bound.extents.y) * Vector3.up, Quaternion.identity);
                    placed = true;
                }               
            }
        }
    }
}
