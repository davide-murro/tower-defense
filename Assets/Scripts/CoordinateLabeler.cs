using TMPro;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color disabledColor = Color.black;
    [SerializeField] Color blockedColor = Color.grey;
    [SerializeField] Color exploredColor = Color.yellow;
    [SerializeField] Color pathColor = Color.green;

    GridManager gridManager;
    TextMeshPro label;
    Vector3Int coordinates = new Vector3Int();
    Tile tile;

    // Awake is called once before the Start()
    void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();

        label = GetComponent<TextMeshPro>();
        label.enabled = false;

        tile = GetComponentInParent<Tile>();

        UpdateCoordinates();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            UpdateCoordinates();
            UpdateObjectName();
        }
        ToggleLabels();
        SetLabelColor();
    }

    void UpdateCoordinates()
    {
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);
        coordinates.z = Mathf.RoundToInt(transform.parent.position.y / UnityEditor.EditorSnapSettings.move.y);

        label.text = $"{coordinates.x};{coordinates.y};{coordinates.z}";
    }

    void UpdateObjectName()
    {
        transform.parent.name = coordinates.ToString();
    }

    void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            label.enabled = !label.IsActive();
        }
    }

    void SetLabelColor()
    {
        if (gridManager == null) return;

        Node node = gridManager.GetNode(coordinates);

        if ((node != null && !node.isWalkable)
            && (tile != null && !tile.IsPlaceable))
        {
            label.color = disabledColor;
        }
        else if ((node != null && !node.isWalkable)
            || (tile != null && !tile.IsPlaceable))
        {
            label.color = blockedColor;
        }
        else if ((node != null && node.isPath))
        {
            label.color = pathColor;
        }
        else if ((node != null && node.isExplored))
        {
            label.color = exploredColor;
        }
        else
        {
            label.color = defaultColor;
        }
    }
}
