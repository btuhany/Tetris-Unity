using UnityEngine;

public class BlockController : MonoBehaviour
{
    [SerializeField] float _downButtonSpeedIncreaseScale = 3f;
    [SerializeField] Transform _nextBlockTransform;
    [SerializeField] BlockPiecesHandler _currentBlock;
    [SerializeField] float _moveSpeed;
    float _moveTimeCounter;
    float _movePeriod;

    Vector3Int _downVector = Vector3Int.down;
    Vector3Int _rightVector = Vector3Int.right;
    Vector3Int _leftVector = Vector3Int.left;
    private void OnEnable()
    {
        _movePeriod = 5 / _moveSpeed;
        _moveTimeCounter = _movePeriod; 
    }
    private void Update()
    {
        if(PlayerInput.Right)
        {
            HorizontalMove(_rightVector);
        }
        if(PlayerInput.Left)
        {
            HorizontalMove(_leftVector);
        }
        if(PlayerInput.Down)
        {
            _moveTimeCounter -= Time.deltaTime * _downButtonSpeedIncreaseScale;
        }
        else
        {
            _moveTimeCounter -= Time.deltaTime;
        }

        
        if(_moveTimeCounter<0)
        {
            if(IsMovable())
                VerticalMove();
            _moveTimeCounter = _movePeriod;
        }
    }
    void VerticalMove()
    {
        _currentBlock.transform.position += _downVector;
    }
    void HorizontalMove(Vector3Int dir)
    {
        _currentBlock.transform.position += dir;
    }
    bool IsMovable()
    {
        
        for (int i = 0; i < 4; i++)
        {
            int y = _currentBlock.TilePiecePositions()[i].y;
            y--;
            
            if (y<GridManager.Instance.MinGridPoint.y)
            {
                //Debug.Log("stop");
                GridManager.Instance.FillTheGrid(_currentBlock.TilePiecePositions());
                return false;
            }

        }
        return true;
    }
}
