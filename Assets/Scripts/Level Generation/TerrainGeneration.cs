using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Procedurally generates the terrain on a single mesh
// Using the tiles would cause issues with the NavMesh not spawning correctly
// Additionally, the NavMesh had strange issues on the edges of tiles



public class TerrainGeneration : MonoBehaviour
{
    [SerializeField] private int width, depth; // Number of squares in the mesh
    private Vector3[] vertices; // Vertices for our mesh
    private float[] heightMap; // heights of the tiles
    [SerializeField] private float heightLayers; // vertical span of the map
    [SerializeField] private float mapScale; // scale applied to perlin noise function

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
                vertices[vertex] = new Vector3(x, heightMap[x + z * width], z);
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
    }
}
