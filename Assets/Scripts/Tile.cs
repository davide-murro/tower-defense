using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] bool isPlaceable;
    [SerializeField] bool isWalkable;
    [SerializeField] Tower towerPrefab;
    [SerializeField] GameObject towerInstantiationParent;

    GridManager gridManager;
    PathFinder[] pathFinders;
    Animator animator;

    Vector3Int coordinates = new Vector3Int();

    bool inputEnabled = true;

    public bool IsPlaceable
    {
        get { return isPlaceable; }
    }

    void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        pathFinders = FindObjectsByType<PathFinder>(FindObjectsSortMode.None);
        animator = GetComponent<Animator>();
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

    void OnDisable()
    {
        inputEnabled = false;
    }

    void OnEnable()
    {
        inputEnabled = true;
    }

    void OnMouseDown()
    {
        //Debug.Log("You clicked on: " + transform.name);

        // check input
        if (!inputEnabled) return;

        // check if placeable
        if (!isPlaceable)
        {
            animator.SetTrigger("blocked");
            return;
        }


        // check if block at list 1 path finder
        foreach (var pathFinder in pathFinders)
        {
            if (pathFinder.WillBlockPath(coordinates) || pathFinder.WillBlockAnyEnemyPath(coordinates))
            {
                animator.SetTrigger("blocked");
                return;
            }
        }

        // try to create tower
        bool isSuccessful = towerPrefab.CreateTower(towerPrefab, towerInstantiationParent.transform);
        if (!isSuccessful)
        {
            animator.SetTrigger("unavailable");
            return;
        }

        // tower created
        animator.SetTrigger("place");
        gridManager.BlockNode(coordinates);
        foreach (var pathFinder in pathFinders)
        {
            pathFinder.NotifyReceivers();
        }
    }
}
