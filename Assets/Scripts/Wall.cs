using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] Tile tile;
    [SerializeField] bool isTop;
    MeshRenderer meshRenderer;
    bool isWalling;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = new Color(0, 0, 0, 0);
    }

    private void OnMouseDown()
    {
        isWalling = !isWalling;
        meshRenderer.material.color = new Color(0, 0, 0, isWalling ? 1f : 0);
        tile.SetWall(isTop, isWalling);
    }
}
