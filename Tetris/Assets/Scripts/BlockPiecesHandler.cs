
using UnityEngine;

public class BlockPiecesHandler : MonoBehaviour
{
    public int MaxRotationCount;
    [SerializeField] GameObject[] _tilePieces = new GameObject[4];

    private Vector2Int[] _tilePiecesPoses = new Vector2Int[4];

    public Vector2Int[] TilePiecesPoses { get => _tilePiecesPoses; }
    public GameObject[] TilePieces { get => _tilePieces; }

    public void CheckTilePieces()
    {
        Vector2Int[] tilePiecesPos = new Vector2Int[4];
        for (int i = 0; i < 4; i++)
        {
            int x = Mathf.RoundToInt(_tilePieces[i].transform.position.x);
            int y = Mathf.RoundToInt(_tilePieces[i].transform.position.y);

            _tilePiecesPoses[i] = new Vector2Int(x, y);
        }
       
    }
    
}
