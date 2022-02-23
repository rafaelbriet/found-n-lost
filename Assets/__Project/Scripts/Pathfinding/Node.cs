using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 Position { get; set; }
    public bool IsWalkable { get; set; }
    public List<Node> Neighbors { get; set; }
    public Node Parent { get; set; }
    public float gCost { get; set; }
    public float hCost { get; set; }
    public float fCost => gCost + hCost;
}
