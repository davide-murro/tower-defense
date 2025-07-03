using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] bool isPlaceable;
    [SerializeField] bool isWalkable;
    [SerializeField] Tower towerPrefab;

    GridManager gridManager;
    PathFinder pathFinder;

    Vector3Int coordinates = new Vector3Int();


    public bool IsPlaceable
    {
        get { return isPlaceable; }
    }

    void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        pathFinder = FindFirstObjectByType<PathFinder>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

            if (isWalkable)
            {
                gridManager.UnblockNode(coordinates);
            }
        }
    }

    void OnMouseDown()
    {
        if (isPlaceable && !pathFinder.WillBlockPath(coordinates))
        {
            //Debug.Log("You clicked on: " + transform.name);
            bool isSuccessful = towerPrefab.CreateTower(towerPrefab, transform.position);
            if (isSuccessful)
            {
                gridManager.BlockNode(coordinates);
                pathFinder.NotifyReceivers();
            }
        }
    }
}
