using System;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] MeshRenderer floorMesh;
    [SerializeField] TMP_Text text;
    internal Tile[] neighbors = new Tile[4];
    internal bool[] isWalled = new bool[4];

    internal TileManager tileManager;

    private void OnMouseDown()
    {
        tileManager.Dijkstra(this);
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

    internal void ShowDistance(int distance)
    {
        text.text = distance.ToString();
    }

    internal void InfiniteDistance()
    {
        text.text = "∞";
    }
}
