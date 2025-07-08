using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector3Int gridSize;
    //[SerializeField] Vector3 gridSpan = UnityEditor.EditorSnapSettings.move;  // this is unavailable in production
    [SerializeField] Vector3 gridSpan = new Vector3(10f, 10f, 10f);
    
    Dictionary<Vector3Int, Node> grid = new Dictionary<Vector3Int, Node>();
    public Dictionary<Vector3Int, Node> Grid { get { return grid; } }


    void Awake()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int z = 0; z < gridSize.z; z++)
                {
                    Vector3Int coordinates = new Vector3Int(x, y, z);
                    grid.Add(coordinates, new Node(coordinates, false));
                    //Debug.Log(grid[coordinates].coordinates + " = " + grid[coordinates].isWalkable);
                }
            }
        }
    }

    public Node GetNode(Vector3Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }
        return null;
    }

    public void BlockNode(Vector3Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates].isWalkable = false;
        }
    }

    public void UnblockNode(Vector3Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates].isWalkable = true;
        }
    }

    public Vector3Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector3Int coordinates = new Vector3Int();
        coordinates.x = Mathf.RoundToInt(position.x / gridSpan.x);
        coordinates.y = Mathf.RoundToInt(position.z / gridSpan.z);
        coordinates.z = Mathf.RoundToInt(position.y / gridSpan.y);

        return coordinates;
    }

    public Vector3 GetPositionFromCoordinates(Vector3Int coordinates)
    {
        Vector3 position = new Vector3();

        position.x = coordinates.x * gridSpan.x;
        position.z = coordinates.y * gridSpan.z;
        position.y = coordinates.z * gridSpan.y;

        return position;
    }

    public void ResetNodes()
    {
        foreach (KeyValuePair<Vector3Int, Node> entry in grid)
        {
            entry.Value.connectedTo = null;
            entry.Value.isExplored = false;
            entry.Value.isPath = false;
        }
    }
}
