using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public enum TileDirection
{
    Top,
    Right,
    Bot,
    Left
}

public class PriorityQueue<Element, Priority>
{
    SortedDictionary<Priority, Queue<Element>> data = new();

    public void Enqueue(Element element, Priority priority)
    {
        if (!data.ContainsKey(priority))
        {
            data.Add(priority, new Queue<Element>());
        }

        data[priority].Enqueue(element);
    }

    public Element Dequeue()
    {
        var pair = data.First();
        Element result = pair.Value.Dequeue();
        if (pair.Value.Count == 0)
        {
            data.Remove(pair.Key);
        }
        return result;
    }

    public bool IsEmpty() => data.Count == 0;
}


public class TileManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI modeText;
    [SerializeField] GameObject tilePrefab;
    Tile[,] tiles;
    int rowCount = 10;
    int colCount = 10;

    internal static bool IsModeBFS = true;

    PriorityQueue<string, int> pq = new();

    private void Start()
    {
        tiles = new Tile[rowCount, colCount];

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
            {
                var pos = new Vector3(j, 0, i);
                tiles[i, j] = Instantiate(tilePrefab, pos, Quaternion.identity).GetComponent<Tile>();
                tiles[i, j].gameObject.name = $"Tile({i},{j})";
                tiles[i, j].tileManager = this;
            }
        }

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
            {
                Tile currentTile = tiles[i, j];
                if (i < rowCount - 1)
                    currentTile.neighbors[(int)TileDirection.Top] = tiles[i + 1, j];

                if (i > 0)
                    currentTile.neighbors[(int)TileDirection.Bot] = tiles[i - 1, j];

                if (j < colCount - 1)
                    currentTile.neighbors[(int)TileDirection.Right] = tiles[i, j + 1];

                if (j > 0)
                    currentTile.neighbors[(int)TileDirection.Left] = tiles[i, j - 1];
            }
        }
    }

    public void ToggleMode()
    {
        IsModeBFS = !IsModeBFS;
        modeText.text = IsModeBFS ? "BFS" : "DFS";
    }

    internal void Dijkstra(Tile tile)
    {
        foreach (var t in tiles)
            t.InfiniteDistance();

        Dictionary<Tile, int> bestDistance = new();
        PriorityQueue<Tile, int> nexts = new();
        bestDistance.Add(tile, 0);
        tile.ShowDistance(0);
        nexts.Enqueue(tile, 0);

        while (!nexts.IsEmpty())
        {
            Tile next = nexts.Dequeue();
            for (int i = 0; i < next.neighbors.Length; i++)
            {
                if (next.neighbors[i] == null)
                    continue;
                if (next.isWalled[i])
                    continue;
                int distance = bestDistance[next] + 1;

                if (bestDistance.ContainsKey(next.neighbors[i]) && bestDistance[next.neighbors[i]] < distance)
                    continue;

                next.neighbors[i].ShowDistance(distance);
                bestDistance[next.neighbors[i]] = distance;
                nexts.Enqueue(next.neighbors[i], distance);
            }
        }
    }
}
