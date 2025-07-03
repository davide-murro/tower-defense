using UnityEngine;

[System.Serializable]
public class Node
{
    public Vector3Int coordinates;
    public Node connectedTo;
    public bool isWalkable;
    public bool isExplored;
    public bool isPath;

    public Node(Vector3Int coodinates, bool isWalkable)
    {
        this.coordinates = coodinates;
        this.isWalkable = isWalkable;
    }
}
