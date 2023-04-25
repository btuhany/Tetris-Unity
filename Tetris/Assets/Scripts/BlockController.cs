using System;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    [SerializeField] float _downButtonSpeedIncreaseScale = 3f;
    [SerializeField] BlockPiecesHandler _nextBlock;
    [SerializeField] BlockPiecesHandler _currentBlock;
    [SerializeField] float _verticalMoveSpeed;
    [SerializeField] float _horizontalMoveSpeed;
    [SerializeField] RectTransform _nextBlockParent;
    [SerializeField] Transform _spawnPoint;
    float _verticalMoveTimeCounter;
    float _verticalMovePeriod;
    float _horizontalMoveTimeCounter;
    float _horizontalMovePeriod;
    Vector3Int _downVector = Vector3Int.down;
    Vector3Int _rightVector = Vector3Int.right;
    Vector3Int _leftVector = Vector3Int.left;

    public float VerticalMoveSpeed { get => _verticalMoveSpeed; set => _verticalMoveSpeed = value; }
    public float VerticalMovePeriod { get => _verticalMovePeriod; set => _verticalMovePeriod = value; }

    private void OnEnable()
    {
        GameManager.Instance.OnGameOver += HandleOnGamerOver;
        _horizontalMovePeriod = 5 / _horizontalMoveSpeed;
        _verticalMovePeriod = 5 / _verticalMoveSpeed;
        _verticalMoveTimeCounter = _verticalMovePeriod;
        if(!_nextBlock)
            _nextBlock = BlockSpawnerManager.Instance.RandomNewBlock();
        NewBlockProcess();
    }



    private void Update()
    {
        if(PlayerInput.Right)
        {
            _horizontalMoveTimeCounter -= Time.deltaTime;
            if(_horizontalMoveTimeCounter<=0f)
            {
                _horizontalMoveTimeCounter = _horizontalMovePeriod;
                HorizontalMove(_rightVector);
            }
        }
        if(PlayerInput.Left)
        {
            _horizontalMoveTimeCounter -= Time.deltaTime;
            if (_horizontalMoveTimeCounter <= 0f)
            {
                _horizontalMoveTimeCounter = _horizontalMovePeriod;
                HorizontalMove(_leftVector);
            }
        }
        if(PlayerInput.LeftRelease || PlayerInput.RightRelease) 
        {
            _horizontalMoveTimeCounter = 0f;
        }  
        if(PlayerInput.Down)
        {
            _verticalMoveTimeCounter -= Time.deltaTime * _downButtonSpeedIncreaseScale;
        }
        if (PlayerInput.Rotate)
        {
            RotateBlock();
        }
        else
        {
            _verticalMoveTimeCounter -= Time.deltaTime;
        }

        
        if(_verticalMoveTimeCounter<0)
        {
            if(IsMovableOnVeritcal())
            {
                VerticalMove();
            }
            else if(!GameManager.Instance.IsGameStopped)
            {
                if(_currentBlock && _currentBlock.TilePieces.Length>0)
                    GridManager.Instance.AddBlockTilesToGrid(_currentBlock.TilePieces);
                NewBlockProcess();
            }             
            _verticalMoveTimeCounter = _verticalMovePeriod;
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
                tile.transform.SetParent(GridManager.Instance.TilesParent);
            }
            Destroy(_currentBlock.gameObject, 0.2f);
        }
        
        _currentBlock = _nextBlock;
        _currentBlock.transform.SetParent(null);
        _currentBlock.transform.position = _spawnPoint.position;
        _currentBlock.transform.localScale = Vector3.one;
        _nextBlock = BlockSpawnerManager.Instance.RandomNewBlock();
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
    private void HandleOnGamerOver()
    {
        if (_currentBlock)
            Destroy(_currentBlock.gameObject);
        if (_nextBlock)
            Destroy(_nextBlock.gameObject);
    }
}
