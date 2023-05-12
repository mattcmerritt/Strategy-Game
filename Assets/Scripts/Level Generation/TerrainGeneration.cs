using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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

    // Player worker data
    [SerializeField] private GameObject workerPrefab; // prefab to instantiate to add workers
    [SerializeField] private int numWorkers; // starting number of workers

    // Tree resource data
    [SerializeField] private GameObject treePrefab; // prefab to instantiate to add trees
    [SerializeField] private int numTrees; // starting number of trees
    [SerializeField] private int numTreesInVillage; // determines how many of the trees start close to the player

    // Box casts
    private List<Vector3> locations = new List<Vector3>(), extents = new List<Vector3>();

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
        Bounds buildingBound = buildingCollider.bounds;

        // finding distances to all four corners of a building
        Vector3[] buildingCorners = {
            new Vector3(buildingBound.extents.x, 0f, buildingBound.extents.z),
            new Vector3(-buildingBound.extents.x, 0f, -buildingBound.extents.z),
            new Vector3(-buildingBound.extents.x, 0f, buildingBound.extents.z),
            new Vector3(buildingBound.extents.x, 0f, -buildingBound.extents.z),
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

                // find corners of the objects model to check height
                float minimumHeight = (heightLayers + 1) * layerHeight;
                foreach (Vector3 corner in buildingCorners)
                {
                    // raycast down from the sky above the corners of the building to determine if it needs to be lower in the ground
                    Physics.Raycast((heightLayers + 1) * layerHeight * Vector3.up + gridTarget + corner, Vector3.down, out RaycastHit hit, (heightLayers + 1) * layerHeight, floorLayer);
                    // Instantiate(markerPrefab, gridTarget + corner + hit.point.y * Vector3.up, Quaternion.identity); // place debug markers
                    if (hit.point.y <= minimumHeight)
                    {
                        minimumHeight = hit.point.y;
                    }
                }
                // apply height to target location
                Vector3 target = gridTarget + (minimumHeight + buildingBound.extents.y) * Vector3.up;

                // checking if the building location is valid
                Collider[] blockingColliders = Physics.OverlapBox(target, buildingPrefab.transform.localScale / 2, Quaternion.identity, ~floorLayer);

                // if nothing is blocking the building, place it down at the height of its lowest corner
                if (blockingColliders.Length == 0)
                {
                    locations.Add(target);
                    extents.Add(buildingPrefab.transform.localScale / 2);

                    Instantiate(buildingPrefab, target, Quaternion.identity);
                    placed = true;
                }               
            }
        }

        // Placing in player workers
        GameObject tempWorker = Instantiate(workerPrefab, new Vector3(-100f, -100f, -100f), Quaternion.identity);
        // need to grab the bounds of the building from a copy of the prefab
        CapsuleCollider workerCollider = tempWorker.GetComponent<CapsuleCollider>();
        // cannot grab the values if it is not instantiated
        Bounds workerBound = workerCollider.bounds;

        // finding distances to all four corners of space around worker
        Vector3[] workerCorners = {
            new Vector3(workerBound.extents.x, 0f, workerBound.extents.z),
            new Vector3(-workerBound.extents.x, 0f, -workerBound.extents.z),
            new Vector3(-workerBound.extents.x, 0f, workerBound.extents.z),
            new Vector3(workerBound.extents.x, 0f, -workerBound.extents.z),
        };

        // Debug.Log($"max: {workerBound.max}, min: {workerBound.min}, extents: {workerBound.extents}");
        DestroyImmediate(tempWorker); // remove from game once data is collected

        // placing the workers
        for (int i = 0; i < numWorkers; i++)
        {
            bool placed = false;
            while (!placed)
            {
                // calculate where to put the worker
                Vector3 direction = Vector3.Normalize(Random.Range(-1f, 1f) * Vector3.right + Random.Range(-1f, 1f) * Vector3.forward);
                Vector3 displacement = direction * Random.Range(0f, villageRadius);

                Vector3 mapCenter = transform.position + (Vector3.right * width * squareSize / 2) + (Vector3.forward * depth * squareSize / 2);
                Vector3 gridTarget = mapCenter + displacement; // location to attempt placing a worker, less the vertical component

                // find corners of the objects model to check height
                float minimumHeight = (heightLayers + 1) * layerHeight;
                foreach (Vector3 corner in workerCorners)
                {
                    // raycast down from the sky above the corners of the building to determine if it needs to be lower in the ground
                    Physics.Raycast((heightLayers + 1) * layerHeight * Vector3.up + gridTarget + corner, Vector3.down, out RaycastHit hit, (heightLayers + 1) * layerHeight, floorLayer);
                    // Instantiate(markerPrefab, gridTarget + corner + hit.point.y * Vector3.up, Quaternion.identity); // place debug markers
                    if (hit.point.y <= minimumHeight)
                    {
                        minimumHeight = hit.point.y;
                    }
                }
                // apply height to target location
                Vector3 target = gridTarget + (minimumHeight + workerBound.extents.y) * Vector3.up;

                // checking if the worker location is valid
                Collider[] blockingColliders = Physics.OverlapBox(target, workerPrefab.transform.localScale / 2, Quaternion.identity, ~floorLayer);
                
                // if nothing is blocking the building, place it down at the height of its lowest corner
                if (blockingColliders.Length == 0)
                {
                    locations.Add(target);
                    extents.Add(workerPrefab.transform.localScale / 2);

                    GameObject newWorker = Instantiate(workerPrefab, target, Quaternion.identity);
                    NavMeshAgent agent = newWorker.GetComponent<NavMeshAgent>();
                    agent.enabled = true;
                    placed = true;
                }
            }
        }

        // Placing in resources (trees)
        GameObject tempTree = Instantiate(treePrefab, new Vector3(-100f, -100f, -100f), Quaternion.identity);
        // need to grab the bounds of the building from a copy of the prefab
        CapsuleCollider treeCollider = tempTree.GetComponent<CapsuleCollider>();
        // cannot grab the values if it is not instantiated
        Bounds treeBound = treeCollider.bounds;

        // finding distances to all four corners of space around worker
        Vector3[] treeCorners = {
            new Vector3(treeBound.extents.x, 0f, treeBound.extents.z),
            new Vector3(-treeBound.extents.x, 0f, -treeBound.extents.z),
            new Vector3(-treeBound.extents.x, 0f, treeBound.extents.z),
            new Vector3(treeBound.extents.x, 0f, -treeBound.extents.z),
        };

        // Debug.Log($"max: {treeBound.max}, min: {treeBound.min}, extents: {treeBound.extents}");
        DestroyImmediate(tempTree); // remove from game once data is collected

        // placing the workers
        for (int i = 0; i < numTrees; i++)
        {
            bool placed = false;
            while (!placed)
            {
                // calculate where to put the worker
                Vector3 direction = Vector3.Normalize(Random.Range(-1f, 1f) * Vector3.right + Random.Range(-1f, 1f) * Vector3.forward);
                Vector3 displacement = direction * Random.Range(i <= numTreesInVillage ? 0f : villageRadius, i <= numTreesInVillage ? villageRadius : width * squareSize / 2);

                Vector3 mapCenter = transform.position + (Vector3.right * width * squareSize / 2) + (Vector3.forward * depth * squareSize / 2);
                Vector3 gridTarget = mapCenter + displacement; // location to attempt placing a worker, less the vertical component

                // find corners of the objects model to check height
                float minimumHeight = (heightLayers + 1) * layerHeight;
                foreach (Vector3 corner in treeCorners)
                {
                    // raycast down from the sky above the corners of the building to determine if it needs to be lower in the ground
                    Physics.Raycast((heightLayers + 1) * layerHeight * Vector3.up + gridTarget + corner, Vector3.down, out RaycastHit hit, (heightLayers + 1) * layerHeight, floorLayer);
                    // Instantiate(markerPrefab, gridTarget + corner + hit.point.y * Vector3.up, Quaternion.identity); // place debug markers
                    if (hit.point.y <= minimumHeight)
                    {
                        minimumHeight = hit.point.y;
                    }
                }
                // apply height to target location
                Vector3 target = gridTarget + (minimumHeight + treeBound.extents.y) * Vector3.up;

                // checking if the worker location is valid
                Collider[] blockingColliders = Physics.OverlapBox(target, treePrefab.transform.localScale / 2, Quaternion.identity, ~floorLayer);

                // if nothing is blocking the building, place it down at the height of its lowest corner
                if (blockingColliders.Length == 0)
                {
                    locations.Add(target);
                    extents.Add(treePrefab.transform.localScale / 2);

                    Instantiate(treePrefab, target, Quaternion.identity);
                    placed = true;
                }
            }
        }
    }

    private void Update()
    {
        while (locations.Count > 0)
        {
            string type = "";
            if (numBuildings > 0)
            {
                type = "building";
                numBuildings--;
            }
            else if (numWorkers > 0)
            {
                type = "worker";
                numWorkers--;
            }
            else if (numTrees > 0)
            {
                type = "tree";
                numTrees--;
            }

            Collider[] blockingColliders = Physics.OverlapBox(locations[0], extents[0], Quaternion.identity, ~floorLayer);

            Debug.Log($"Checking {type} at {locations[0]} with size {extents[0]}, found {blockingColliders.Length} objects");

            locations.RemoveAt(0);
            extents.RemoveAt(0);
        }
    }
}
