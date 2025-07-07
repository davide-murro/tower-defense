using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector3Int startCoordinates;
    [SerializeField] Vector3Int endCoordinates;

    public Vector3Int StartCoordinates { get { return startCoordinates; } }
    public Vector3Int EndCoordinates { get { return endCoordinates; } }

    GridManager gridManager;

    Node startNode;
    Node endNode;
    Node currentSearchNode;

    Queue<Node> frontier = new Queue<Node>();

    //Vector3Int[] directions = { Vector3Int.right, Vector3Int.left, Vector3Int.forward, Vector3Int.back, Vector3Int.up, Vector3Int.down };
    //Vector3Int[] directions = { Vector3Int.right, Vector3Int.left, new Vector3Int(1, 0, 1), new Vector3Int(-1, 0, 1), new Vector3Int(1, 0, -1), new Vector3Int(-1, 0, -1), new Vector3Int(0, 1, 1), new Vector3Int(0, -1, 1), new Vector3Int(0, 1, -1), new Vector3Int(0, -1, -1), Vector3Int.up, Vector3Int.down };
    Vector3Int[] directions = {
        Vector3Int.right, Vector3Int.left,  // right / left
        Vector3Int.up, Vector3Int.down,     // forward / back
        Vector3Int.forward + Vector3Int.right, Vector3Int.forward + Vector3Int.left, Vector3Int.forward + Vector3Int.up, Vector3Int.forward + Vector3Int.down,  // up - right / left / forward / back
        Vector3Int.back + Vector3Int.right, Vector3Int.back + Vector3Int.left, Vector3Int.back + Vector3Int.up, Vector3Int.back + Vector3Int.down   // down - right / left / forward / back
    };
    Dictionary<Vector3Int, Node> grid = new Dictionary<Vector3Int, Node>();
    Dictionary<Vector3Int, Node> reached = new Dictionary<Vector3Int, Node>();

    void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        if (gridManager != null)
        {
            grid = gridManager.Grid;
            startNode = grid[startCoordinates];
            endNode = grid[endCoordinates];
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startNode = gridManager.Grid[startCoordinates];
        endNode = gridManager.Grid[endCoordinates];
        GetNewPath();
    }

    void ExploreNeighbors()
    {
        List<Node> neighbors = new List<Node>();

        foreach (Vector3Int direction in directions)
        {
            Vector3Int neighborCoords = currentSearchNode.coordinates + direction;
            if (grid.ContainsKey(neighborCoords))
            {
                neighbors.Add(grid[neighborCoords]);
            }
        }

        foreach (Node neighbor in neighbors)
        {
            if (!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                neighbor.connectedTo = currentSearchNode;
                reached.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }

    void BreadthFirstSearch(Vector3Int coordinates)
    {
        // clean all
        startNode.isWalkable = true;
        endNode.isWalkable = true;
        frontier.Clear();
        reached.Clear();

        // start search
        bool isRunning = true;

        frontier.Enqueue(grid[coordinates]);
        reached.Add(coordinates, grid[coordinates]);

        while (frontier.Count > 0 && isRunning)
        {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;

            ExploreNeighbors();

            if (currentSearchNode.coordinates == endCoordinates)
            {
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();

        Node currentNode = endNode;
        currentNode.isPath = true;
        path.Add(currentNode);

        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            currentNode.isPath = true;
            path.Add(currentNode);
        }

        path.Reverse();

        return path;
    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinates);
    }

    public List<Node> GetNewPath(Vector3Int coordinates)
    {
        gridManager.ResetNodes();
        BreadthFirstSearch(coordinates);
        return BuildPath();
    }

    public bool WillBlockPath(Vector3Int coordinates)
    {
        // Check if the tile exists in the grid
        // If it doesn't, we can't block it = just return false
        if (!grid.ContainsKey(coordinates)) return false;

        // Check if the tile is already blocked (not walkable)
        // If it's already blocked, then placing a tower there won't change anything
        if (!grid[coordinates].isWalkable) return false;

        // Simulate what would happen if we placed a tower here.
        // This means we temporarily mark the tile as unwalkable
        grid[coordinates].isWalkable = false;

        // Get all currently active enemies in the scene
        // We want to test if each one would still have a valid path to the end tile
        var allEnemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);

        // Loop through each enemy to check their current path
        foreach (var enemy in allEnemies)
        {
            // Convert the enemy's world position into grid coordinates
            Vector3Int enemyCoord = gridManager.GetCoordinatesFromPosition(enemy.transform.position);

            // Try to find a new path from this enemy's current position to the goal
            List<Node> path = GetNewPath(enemyCoord);

            //If the path is null or has only one or fewer nodes, that means this enemy is now completely blocked
            if (path == null || path.Count <= 1)
            {
                // In this case, restore the original walkable state before exiting
                grid[coordinates].isWalkable = true;
                // also reset the visuals or internal path
                GetNewPath();

                // Since this tower would trap an enemy, we block placement (WillBlockAnyEnemyPath(true))
                return true;
            }
        }

        // If no enemy was trapped, put the tile back to walkable
        grid[coordinates].isWalkable = true;

        // Recalculate the path to update it
        GetNewPath();

        // Tower placement is safe, no enemy got stuck (WillBlockAnyEnemyPath(false))
        return false;
    }


    public void NotifyReceivers()
    {
        BroadcastMessage(nameof(EnemyController.RecalculatePath), false, SendMessageOptions.DontRequireReceiver);
    }
}
