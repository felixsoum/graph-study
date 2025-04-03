using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] MeshRenderer floorMesh;
    internal Tile[] neighbors = new Tile[4];
    internal bool[] isWalled = new bool[4];

    static Queue<Tile> nextToVisit = new();
    static HashSet<Tile> alreadyVisited = new();

    private void OnMouseDown()
    {
        nextToVisit.Enqueue(this);

        while (nextToVisit.Count > 0)
        {
            Tile tile = nextToVisit.Dequeue();
            tile.Highlight();
            alreadyVisited.Add(tile);
            for (int i = 0; i < neighbors.Length; i++)
            {
                var neighbor = tile.neighbors[i];
                if (neighbor == null)
                    continue;

                if (tile.isWalled[i])
                    continue;

                if (alreadyVisited.Contains(neighbor))
                    continue;

                nextToVisit.Enqueue(neighbor);
            }
        }
    }

    private void Highlight()
    {
        floorMesh.material.color = Color.cyan;
    }

    internal void SetWall(bool isTop, bool isWalling)
    {
        if (isTop)
        {
            isWalled[(int)TileDirection.Top] = isWalling;
            if (neighbors[(int)TileDirection.Top] != null)
            {
                neighbors[(int)TileDirection.Top].isWalled[(int)TileDirection.Bot] = isWalling;
            }
        }
        else
        {
            isWalled[(int)TileDirection.Right] = isWalling;
            if (neighbors[(int)TileDirection.Right] != null)
            {
                neighbors[(int)TileDirection.Right].isWalled[(int)TileDirection.Left] = isWalling;
            }
        }
    }
}
