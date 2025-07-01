using UnityEngine;

[System.Serializable]
public class Node
{
    public Vector2Int coordinates;
    public Node connectedTo;
    public bool isWalkable;
    public bool isExplored;
    public bool isPath;

    public Node(Vector2Int coodinates, bool isWalkable)
    {
        this.coordinates = coodinates;
        this.isWalkable = isWalkable;
    }
}
