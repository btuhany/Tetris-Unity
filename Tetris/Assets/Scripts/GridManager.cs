using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] int _gridWidth = 10, _gridHeight = 20;
    [SerializeField] float _offset = 0.45f;
    [SerializeField] GameObject _debugUnfilledPrefab;
    [SerializeField] GameObject _debugFilledPrefab;
    [SerializeField] Transform _tilesParent;
    [SerializeField] Transform _debugTilesParent;
    SpriteRenderer _gridSprite;
    SpriteRenderer _borderSprite;
    SpriteRenderer[] _sprites;
    GameObject[,] _grid;
    GameObject[,] _debugGrid;

    public static GridManager Instance;

    public Vector2Int MinGridPoint;
    public Vector2Int MaxGridPoint;

    public bool ShowDebugPoints;
    private void Awake()
    {
        Instance = this;
        _sprites = GetComponentsInChildren<SpriteRenderer>();
        _gridSprite = _sprites[0];
        _borderSprite = _sprites[1];
    }
    private void OnEnable()
    {
        _grid = new GameObject[_gridWidth, _gridHeight];
        _debugGrid = new GameObject[_gridWidth, _gridHeight];
        MinGridPoint = new Vector2Int(-(_gridWidth - 2) / 2, 0);
        MaxGridPoint = new Vector2Int(((_gridWidth - 2) / 2) + 1, _gridHeight - 1);
        transform.position = new Vector3(transform.position.x, _gridHeight / 2 - 0.5f, transform.position.z);
        _gridSprite.size = new Vector2(_gridWidth,_gridHeight);
        _borderSprite.size = new Vector2(_gridWidth + _offset, _gridHeight + _offset);
        if (ShowDebugPoints)
            UpdateDebug();
    }
    public void FillTheGrid(GameObject[] filledTilesPos)
    {
        for (int i = 0; i < 4; i++)
        {
            _grid[Mathf.RoundToInt(filledTilesPos[i].transform.position.x) - MinGridPoint.x, Mathf.RoundToInt(filledTilesPos[i].transform.position.y) - MinGridPoint.y] = filledTilesPos[i];
            
            
        }
        
        if (ShowDebugPoints)
            UpdateDebug();
    }
    public void IsThereFullRows()
    {
        int counter=0;
        for (int i = 0; i < _gridHeight; i++)
        {
            counter = 0;
            for (int j = 0; j < _gridWidth; j++)
            {
                if (_grid[j, i])
                {
                    counter++;
                    //Debug.Log("----------------");
                    //Debug.Log("row: " + i);
                    //Debug.Log(counter);
                    if (counter == _gridWidth)
                    {
                        //Debug.Log("finishCounter: " + counter);
                        ClearFullRow(i);
                    }
                }

            }
        }
    }
    public void ClearFullRow(int column)
    {
        for (int i = 0; i < _gridWidth; i++)
        {
        //    GameObject deleteObj = _grid[i, column];
            _grid[i, column] = _grid[i+1,column];
            Destroy(_grid[i, column]);

           

        }
        
    }
    public bool IsGridPositionFull(int x, int y)
    {
        y = Mathf.Clamp(y, MinGridPoint.y, MaxGridPoint.y);
        x = Mathf.Clamp(x, MinGridPoint.x, MaxGridPoint.x);
        if(_grid[x - MinGridPoint.x, y - MinGridPoint.y])
        {
            return true;
        }
        return false;
    }
    private void UpdateDebug()
    {
        for (int i = MinGridPoint.x; i <= MaxGridPoint.x; i++)
        {
            for (int j = MinGridPoint.y; j <= MaxGridPoint.y; j++)
            {
                if (_grid[i - MinGridPoint.x, j - MinGridPoint.y] && _debugGrid[i - MinGridPoint.x, j - MinGridPoint.y])
                {
                    Destroy(_debugGrid[i - MinGridPoint.x, j - MinGridPoint.y]);           //ObjectPooling
                    _debugGrid[i - MinGridPoint.x, j - MinGridPoint.y] = Instantiate(_debugFilledPrefab, new Vector3(i, j, 0), this.transform.rotation);
                    _debugGrid[i - MinGridPoint.x, j - MinGridPoint.y].transform.SetParent(_debugTilesParent);
                }
                else if(!_debugGrid[i - MinGridPoint.x, j - MinGridPoint.y])
                {
                    
                    _debugGrid[i - MinGridPoint.x, j - MinGridPoint.y] = Instantiate(_debugUnfilledPrefab, new Vector3(i, j, 0), this.transform.rotation);
                    _debugGrid[i - MinGridPoint.x, j - MinGridPoint.y].transform.SetParent(_debugTilesParent);
                    
                }

            }
        }
    }

}
