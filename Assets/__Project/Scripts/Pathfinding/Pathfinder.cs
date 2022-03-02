using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField]
    private NavGraphGenerator navGraph;

    public List<Vector3> RequestPath(Vector3 start, Vector3 end)
    {
        List<Node> path = FindPath(start, end);
        List<Vector3> output = new List<Vector3>();
        
        foreach (Node node in path)
        {
            output.Add(node.Position);
        }

        return output;
    }

    public Vector3 GetRandomPositionInsideNavGraph(Vector3 origin, int radius)
    {
        Node node = navGraph.GetNodeFromWorldPosition(origin);
        List<Node> nodes = navGraph.GetNeighbors(node, radius);
        List<Node> resultNodes = nodes.FindAll(n => n.IsWalkable);
        int index = Random.Range(0, resultNodes.Count);

        return resultNodes[index].Position;
    }

    public Vector3 GetPositionInsideNavGraph(Vector3 start, Vector3 direction, int lenght)
    {
        List<Node> nodes = new List<Node>();

        while (lenght > 0)
        {
            Node node = navGraph.GetNodeFromWorldPosition(start);

            if (node != null && node.IsWalkable)
            {
                nodes.Add(node);
            }

            start += direction.normalized;

            lenght--;
        }
        
        return nodes[nodes.Count - 1].Position;
    }

    public List<Node> FindPath(Vector3 start, Vector3 end)
    {
        List<Node> outputPath = new List<Node>();

        Node startNode = navGraph.GetNodeFromWorldPosition(start);
        Node endNode = navGraph.GetNodeFromWorldPosition(end);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            foreach (Node node in openSet)
            {
                if (node.fCost < currentNode.fCost || node.fCost == currentNode.fCost && node.hCost < currentNode.hCost)
                {
                    currentNode = node;
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                outputPath = GetPath(startNode, endNode);
                break;
            }

            foreach (Node neighbor in navGraph.GetNeighborsSimple(currentNode))
            {
                if (neighbor.IsWalkable == false || closedSet.Contains(neighbor))
                {
                    continue;
                }

                float gCostToNeighbor = currentNode.gCost + Heuristic(currentNode.Position, neighbor.Position);

                if (gCostToNeighbor < neighbor.gCost || openSet.Contains(neighbor) == false)
                {
                    neighbor.gCost = gCostToNeighbor;
                    neighbor.hCost = Heuristic(currentNode.Position, neighbor.Position);
                    neighbor.Parent = currentNode;

                    if (openSet.Contains(neighbor) == false)
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return outputPath;
    }

    private List<Node> GetPath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();

        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();

        return path;
    }

    private float Heuristic(Vector3 pointA, Vector3 pointB)
    {
        return Mathf.Abs(pointA.x - pointB.x) + Mathf.Abs(pointA.y - pointB.y);
    }
}
