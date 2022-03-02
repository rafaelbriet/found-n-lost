using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NavGraphGenerator : MonoBehaviour
{
    [SerializeField]
    private float tileOffset = 0.5f;
    [SerializeField]
    private LayerMask layerMask;

    private Dictionary<Vector3, Node> nodes = new Dictionary<Vector3, Node>();

    private void Awake()
    {
        Generate();
    }

    private void OnDrawGizmos()
    {
        foreach (var item in nodes)
        {
            if (item.Value.IsWalkable)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }

            Gizmos.DrawWireCube(item.Value.Position, Vector3.one);
        }
    }

    public void Generate()
    {
        nodes.Clear();

        List<Tilemap> tilemaps = new List<Tilemap>();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out Tilemap tilemap))
            {
                tilemaps.Add(tilemap);
            }
        }

        foreach (Tilemap tilemap in tilemaps)
        {
            for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++)
            {
                for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)
                {
                    Vector3Int cellPostion = new Vector3Int(x, y, 0);
                    Vector3 cellWorldPostion = tilemap.CellToWorld(cellPostion);
                    Vector3 nodePosition = new Vector3(cellWorldPostion.x + tileOffset, cellWorldPostion.y + tileOffset, cellWorldPostion.z);

                    if (tilemap.HasTile(cellPostion) && nodes.ContainsKey(nodePosition) == false)
                    {
                        Collider2D collider = Physics2D.OverlapBox(nodePosition, Vector2.one * 0.75f, 0f, layerMask);

                        Node node = new Node();
                        node.Position = nodePosition;
                        node.IsWalkable = collider == null;

                        nodes.Add(nodePosition, node);
                    }
                }
            }
        }

        foreach (KeyValuePair<Vector3, Node> node in nodes)
        {
            node.Value.Neighbors = GetNeighbors(node.Value);
        }
    }

    public static Vector3 WorldToNodePosition(Vector3 position)
    {
        return new Vector3(position.x < 0 ? Mathf.Ceil(position.x) - 0.5f : Mathf.Floor(position.x) + 0.5f,
                           position.y < 0 ? Mathf.Ceil(position.y) - 0.5f : Mathf.Floor(position.y) + 0.5f,
                           0);
    }

    public Node GetNodeFromWorldPosition(Vector3 position)
    {
        Node output = null;
        position = WorldToNodePosition(position);

        if (nodes.ContainsKey(position))
        {
            output = nodes[position];
        }

        return output;
    }

    public List<Node> GetNeighbors(Node node, int radius = 1)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -radius; x < radius; x++)
        {
            for (int y = -radius; y < radius; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                Vector3 position = new Vector3(node.Position.x + x, node.Position.y + y, node.Position.z);

                if (nodes.ContainsKey(position))
                {
                    neighbors.Add(nodes[position]);
                }
            }
        }

        return neighbors;
    }

    public List<Node> GetNeighborsSimple(Node node)
    {
        List<Node> neighbors = new List<Node>();

        List<Vector3> possibleNodesPosition = new List<Vector3>()
        {
            new Vector3(node.Position.x + 1, node.Position.y, node.Position.z),
            new Vector3(node.Position.x - 1, node.Position.y, node.Position.z),
            new Vector3(node.Position.x, node.Position.y + 1, node.Position.z),
            new Vector3(node.Position.x, node.Position.y - 1, node.Position.z),
        };

        foreach (Vector3 position in possibleNodesPosition)
        {
            if (nodes.ContainsKey(position))
            {
                neighbors.Add(nodes[position]);
            }
        }

        return neighbors;
    }
}
