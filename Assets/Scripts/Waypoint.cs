using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] bool isPlaceable;
    [SerializeField] Tower towerPrefab;

    public bool IsPlaceable
    {
        get { return isPlaceable; }
    }

    void OnMouseDown()
    {
        if (isPlaceable)
        {
            //Debug.Log("You clicked on: " + transform.name);
            bool isPlaced = towerPrefab.CreateTower(towerPrefab, transform.position);
            isPlaceable = !isPlaced; // after prefab was instantiated on this tile, disable the isPlaceable, so the player will not be able to place multiple towers at the same spot
        }
    }
}
