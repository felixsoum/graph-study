using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] MeshRenderer floorMesh;
    internal Tile[] neighbors = new Tile[4];
    internal bool[] isWalled = new bool[4];

    static List<Tile> nextToVisit = new();
    static HashSet<Tile> alreadyVisited = new();

    private void OnMouseDown()
    {
        IEnumerator TraversalCoroutine()
        {
            nextToVisit.Add(this);
            alreadyVisited.Add(this);

            while (nextToVisit.Count > 0)
            {
                Tile tile = nextToVisit[0];
                nextToVisit.RemoveAt(0);

                tile.Highlight();
                yield return new WaitForSeconds(0.1f);
                for (int i = 0; i < neighbors.Length; i++)
                {
                    var neighbor = tile.neighbors[i];
                    if (neighbor == null)
                        continue;

                    if (tile.isWalled[i])
                        continue;

                    if (alreadyVisited.Contains(neighbor))
                        continue;

                    alreadyVisited.Add(neighbor);
                    if (TileManager.IsModeBFS)
                    {
                        nextToVisit.Add(neighbor);
                    }
                    else
                    {
                        nextToVisit.Insert(0, neighbor);
                    }
                }
            }
        }
        StartCoroutine(TraversalCoroutine());
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
