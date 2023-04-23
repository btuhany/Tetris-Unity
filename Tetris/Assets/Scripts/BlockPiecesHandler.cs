
using UnityEngine;

public class BlockPiecesHandler : MonoBehaviour
{
    [SerializeField] Transform[] _tilePieces = new Transform[4];
   
    public Vector2Int[] TilePiecePositions()
    {
        Vector2Int[] tilePiecesPos = new Vector2Int[4];
        for (int i = 0; i < 4; i++)
        {
            
            int x = Mathf.RoundToInt(_tilePieces[i].position.x ) ;
            int y = Mathf.RoundToInt(_tilePieces[i].localPosition.y + transform.position.y);
     
            tilePiecesPos[i] = new Vector2Int(x, y);
        }
        return tilePiecesPos;

    }
}
