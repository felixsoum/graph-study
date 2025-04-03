using TMPro;
using UnityEngine;

public enum TileDirection
{
    Top,
    Right,
    Bot,
    Left
}

public class TileManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI modeText;
    [SerializeField] GameObject tilePrefab;
    Tile[,] tiles;
    int rowCount = 10;
    int colCount = 10;

    internal static bool IsModeBFS = true;

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
}
