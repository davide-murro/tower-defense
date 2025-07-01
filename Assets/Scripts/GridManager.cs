using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    public Dictionary<Vector2Int, Node> grid { get; private set; } = new Dictionary<Vector2Int, Node>();


    //Dictionary <Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    //public Dictionary<Vector2Int, Node> Grid { get { return grid; } };

    void Awake()
    {
        CreateGrid();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int coordinates = new Vector2Int(x, y);
                grid.Add(coordinates, new Node(coordinates, true));
                Debug.Log(grid[coordinates].coordinates + " = " + grid[coordinates].isWalkable);
            }
        }
    }

    public Node GetNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
             return grid[coordinates];
        }
        return null;
    }
}
