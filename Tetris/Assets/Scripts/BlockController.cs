using UnityEngine;

public class BlockController : MonoBehaviour
{
    [SerializeField] float _downButtonSpeedIncreaseScale = 3f;
    [SerializeField] BlockPiecesHandler _nextBlockTransform;
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
        _nextBlockTransform = BlockSpawnerManager.Instance.SpawnRandomBlock;
        NewBlockProcess();
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
        if (PlayerInput.Rotate)
        {
            RotateBlock();
        }
        else
        {
            _moveTimeCounter -= Time.deltaTime;
        }

        
        if(_moveTimeCounter<0)
        {
            if(IsMovableOnVeritcal())
            {
                VerticalMove();
            }
            else
            {
                GridManager.Instance.AddBlockTilesToGrid(_currentBlock.TilePieces);
                NewBlockProcess();
            }             
            _moveTimeCounter = _movePeriod;
        }
    }
    void RotateBlock()
    {
        float lastRotatedAngle = _currentBlock.transform.rotation.eulerAngles.z;
        
        if (_currentBlock.MaxRotationCount == 0) return;
        if (_currentBlock.MaxRotationCount == 2)
        {
            if(_currentBlock.transform.rotation.eulerAngles.z == 0)
            {
                _currentBlock.transform.rotation = Quaternion.Euler(0, 0, _currentBlock.transform.rotation.eulerAngles.z + 90);
            }
            else if(_currentBlock.transform.rotation.eulerAngles.z == 90f)
            {
                _currentBlock.transform.rotation = Quaternion.Euler(0, 0, _currentBlock.transform.rotation.eulerAngles.z - 90);
            }
        }
        if(_currentBlock.MaxRotationCount == 4)
        {
            _currentBlock.transform.rotation = Quaternion.Euler(0, 0, _currentBlock.transform.rotation.eulerAngles.z + 90);
        }
        for (int i = 0; i < 4; i++)
        {
            int y = Mathf.RoundToInt(_currentBlock.TilePieces[i].transform.position.y);
            int x = Mathf.RoundToInt(_currentBlock.TilePieces[i].transform.position.x);
            if (GridManager.Instance.IsGridPositionFull(x, y) || x < GridManager.Instance.MinGridPoint.x || x > GridManager.Instance.MaxGridPoint.x || y<GridManager.Instance.MinGridPoint.y || y>GridManager.Instance.MaxGridPoint.y)
            {
               
                _currentBlock.transform.rotation = Quaternion.Euler(0, 0, lastRotatedAngle);
                return;
            }
        }   
    }
    void NewBlockProcess()
    {
        if (_currentBlock)
        {
            foreach (GameObject tile in _currentBlock.TilePieces)
            {
                tile.transform.SetParent(null);
            }
            Destroy(_currentBlock.gameObject, 0.2f);
        }
           
        _currentBlock = _nextBlockTransform;
        _currentBlock.transform.SetParent(null);
        _nextBlockTransform = BlockSpawnerManager.Instance.SpawnRandomBlock;
    }
    void VerticalMove()
    {
        _currentBlock.transform.position += _downVector;
    }
    void HorizontalMove(Vector3Int dir)
    {
        if(IsMovableOnHorizontal(dir))
            _currentBlock.transform.position += dir;
    }
    bool IsMovableOnVeritcal()
    {
       
        for (int i = 0; i < 4; i++)
        {
            int x = Mathf.RoundToInt(_currentBlock.TilePieces[i].transform.position.x);
            int y = Mathf.RoundToInt(_currentBlock.TilePieces[i].transform.position.y);
            y--;
            
            if (y<GridManager.Instance.MinGridPoint.y)
            {
                //Debug.Log("stop");          
                return false;
            }
            if(GridManager.Instance.IsGridPositionFull(x,y))
            {
                return false;
            }
        }
        return true;
    }
    bool IsMovableOnHorizontal(Vector3Int dir)
    {
        for (int i = 0; i < 4; i++)
        {
            int y = Mathf.RoundToInt(_currentBlock.TilePieces[i].transform.position.y);
            int x = Mathf.RoundToInt(_currentBlock.TilePieces[i].transform.position.x);
            if (dir == _rightVector)
            {
                x++;
                if(x > GridManager.Instance.MaxGridPoint.x)
                {
                    return false;
                }
                if (GridManager.Instance.IsGridPositionFull(x, y))
                {
                    return false;
                }
            }
            else if (dir == _leftVector)
            {
                x--;
                if (x < GridManager.Instance.MinGridPoint.x)
                {
                    return false;
                }
                if (GridManager.Instance.IsGridPositionFull(x, y))
                {
                    return false;
                }
            }
        }
        return true;

    }
}
