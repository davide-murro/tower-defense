using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] bool isPlaceable;
    [SerializeField] bool isWalkable;
    [SerializeField] Tower towerPrefab;
    [SerializeField] GameObject towerInstantiationParent;

    GridManager gridManager;
    PathFinder[] pathFinders;
    Animator animator;

    Vector3Int coordinates = new Vector3Int();

    bool isPlaced = false;

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

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("You clicked on: " + transform.name);

        // check if is already placed
        if (isPlaced) return;

        // check if placeable
        if (!isPlaceable)
        {
            animator.SetTrigger("blocked");
            return;
        }

        // check if block at list 1 path finder
        foreach (var pathFinder in pathFinders)
        {
            if (pathFinder.WillBlockPath(coordinates))
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
        isPlaced = true;
        animator.SetTrigger("place");
        gridManager.BlockNode(coordinates);
        foreach (PathFinder pathFinder in pathFinders)
        {
            pathFinder.NotifyReceivers();
        }
    }
}
