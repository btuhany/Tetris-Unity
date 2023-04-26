using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    int _gridWidth = 10, _gridHeight = 20;

    [SerializeField] GameObject _debugUnfilledPrefab;
    [SerializeField] GameObject _debugFilledPrefab;
    [SerializeField] Transform _tilesParent;
    [SerializeField] Transform _debugTilesParent;
    
    SpriteRenderer _gridSprite;
    GameObject[,] _grid;
    GameObject[,] _debugGrid;

    public static GridManager Instance;

    public Vector2Int MinGridPoint;
    public Vector2Int MaxGridPoint;

    public bool ShowDebugPoints;


    public Transform TilesParent { get => _tilesParent; }
    public int GridWidth { get => _gridWidth; set => _gridWidth = value; }
    public int GridHeight { get => _gridHeight; set => _gridHeight = value; }

    private void Awake()
    {
        Instance = this;
        _gridSprite = GetComponent<SpriteRenderer>();

    }
    private void OnEnable()
    {
        GameManager.Instance.OnGameOver += HandleOnGameOver;
    }

    private void HandleOnGameOver()
    {
        for (int i = 0; i < TilesParent.transform.childCount; i++)
        {
            Destroy(TilesParent.GetChild(i).gameObject);
      
        }
    }

    public void SetupParameters()
    {
        _grid = new GameObject[_gridWidth, _gridHeight];
        _debugGrid = new GameObject[_gridWidth, _gridHeight];
        MinGridPoint = new Vector2Int(-(_gridWidth - 2) / 2, 0);
        MaxGridPoint = new Vector2Int(((_gridWidth - 2) / 2) + 1, _gridHeight - 1);
        transform.position = new Vector3(transform.position.x, _gridHeight / 2 - 0.5f, transform.position.z);
        _gridSprite.size = new Vector2(_gridWidth, _gridHeight);

        if (ShowDebugPoints)
            UpdateDebug();
    }
    public void AddBlockTilesToGrid(GameObject[] tilePositions)
    {
        CheckAndClearFilledRows(FillTheGrid(tilePositions));
        if (ShowDebugPoints)
            UpdateDebug();
    }
    public void ClearTheGrid()
    {

        for (int i = 0; i < _gridWidth; i++)
        {
            for (int j = 0; j < _gridHeight; j++)
            {
                if (_grid[i,j])
                {
                    Destroy(_grid[i, j].gameObject);
                    _grid[i, j] = null;
                    _debugGrid[i, j] = null;
                }
            }
        }
    }
    private List<int> FillTheGrid(GameObject[] tilePositions)
    {
        bool isOutsideTheGrid = false;
        List<int> newTilesRows = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            int tilePosXRelativeToGrid = Mathf.RoundToInt(tilePositions[i].transform.position.x) - MinGridPoint.x;
            int tilePosYRelativeToGrid = Mathf.RoundToInt(tilePositions[i].transform.position.y) - MinGridPoint.y;

            if (tilePosXRelativeToGrid < _gridWidth && tilePosYRelativeToGrid < _gridHeight)
            {
                if (tilePositions[i])   // --> prevents error
                {
                    _grid[tilePosXRelativeToGrid, tilePosYRelativeToGrid] = tilePositions[i];
                    if (!newTilesRows.Contains(tilePosYRelativeToGrid))
                        newTilesRows.Add(tilePosYRelativeToGrid);

                }
                
            }
            else
            {
                isOutsideTheGrid = true;
                if (tilePositions[i])
                {
                    Destroy(tilePositions[i].gameObject);
                }
            }
        }
        if(isOutsideTheGrid)
        {
            GameManager.Instance.GameOver();
        }
        return newTilesRows;
    }
    private void CheckAndClearFilledRows(List<int> newTileRows)
    {
        
        List<int> rowsToClear = new List<int>();
        if (newTileRows.Count == 0) return; 
        for (int i = newTileRows.Min(); i < newTileRows.Max()+1; i++)
        {
            int filledCellCounter = 0;
            for (int j = 0; j < _gridWidth; j++)
            {
                if (_grid[j, i])
                {
                    filledCellCounter++;
                    //Debug.Log("----------------");
                    //Debug.Log("row: " + i);
                    //Debug.Log(counter);
                    if (filledCellCounter == _gridWidth)
                    {
                        //Debug.Log("finishCounter: " + counter);
                        rowsToClear.Add(i);
                    }
                }
            }
        }
        if (rowsToClear.Count > 0)
        {
            ClearAndMove(rowsToClear);
       
            
        }
    }
    private void ClearAndMove(List<int> rowsToClear)
    {
        
        for (int x = rowsToClear.Count - 1; x >= 0; x--)
        {
            for (int i = 0; i < _gridWidth; i++)
            {
                Destroy(_grid[i, rowsToClear[x]].gameObject);
                _grid[i, rowsToClear[x]] = null;
                Debug.Log("Deneme");
                for (int j = rowsToClear[x] + 1; j < _gridWidth; j++)
                {
                    if (_grid[i, j])
                    {
                        _grid[i, j].gameObject.transform.position += Vector3.down;
                        _grid[i, j - 1] = _grid[i, j];
                        _grid[i, j] = null;
                    }

                }

            }
            GameManager.Instance.IncreaseScore();
        }
    }
    //private void MoveRowsDown(List<int> rowsToClear)
    //{
       
        
    //    for (int i = rowsToClear.Max() + 1; i < _gridHeight; i++)
    //    {
    //        for (int j = 0; j < _gridWidth; j++)
    //        {
    //            if (_grid[j, i])  //if grid cell contains tile
    //            {
    //                _grid[j, i].gameObject.transform.position += Vector3.down * rowsToClear.Count;
    //                _grid[j, i - rowsToClear.Count] = _grid[j, i];
    //                _grid[j, i] = null;
    //            }
    //        }
    //    }
        
      

    //}

    //public void ClearFullRow(List<int> rowsToClear)
    //{
    //    for (int x = 0; x < rowsToClear.Count; x++)
    //    {
    //        for (int i = 0; i < _gridWidth; i++)
    //        {
    //            Destroy(_grid[i, rowsToClear[x]].gameObject);  
    //            _grid[i, rowsToClear[x]] = null;
                
    //        }
    //        GameManager.Instance.IncreaseScore();
    //    }

    //}
    public bool IsGridPositionFull(int x, int y)
    {
        y = Mathf.Clamp(y, MinGridPoint.y, MaxGridPoint.y);
        x = Mathf.Clamp(x, MinGridPoint.x, MaxGridPoint.x);
        if (_grid[x - MinGridPoint.x, y - MinGridPoint.y])
        {
            return true;
        }
        return false;
    }
    public void UpdateDebug()
    {
        for (int i = MinGridPoint.x; i <= MaxGridPoint.x; i++)
        {
            for (int j = MinGridPoint.y; j <= MaxGridPoint.y; j++)
            {
                if (_debugGrid[i - MinGridPoint.x, j - MinGridPoint.y])
                {
                    if (_grid[i - MinGridPoint.x, j - MinGridPoint.y])
                    {

                        Destroy(_debugGrid[i - MinGridPoint.x, j - MinGridPoint.y]);           //ObjectPooling
                        _debugGrid[i - MinGridPoint.x, j - MinGridPoint.y] = Instantiate(_debugFilledPrefab, new Vector3(i, j, 0), this.transform.rotation);
                        _debugGrid[i - MinGridPoint.x, j - MinGridPoint.y].transform.SetParent(_debugTilesParent);
                    }
                    else
                    {

                        Destroy(_debugGrid[i - MinGridPoint.x, j - MinGridPoint.y]);
                        _debugGrid[i - MinGridPoint.x, j - MinGridPoint.y] = Instantiate(_debugUnfilledPrefab, new Vector3(i, j, 0), this.transform.rotation);
                        _debugGrid[i - MinGridPoint.x, j - MinGridPoint.y].transform.SetParent(_debugTilesParent);
                    }
                }
                else
                {

                    _debugGrid[i - MinGridPoint.x, j - MinGridPoint.y] = Instantiate(_debugUnfilledPrefab, new Vector3(i, j, 0), this.transform.rotation);
                    _debugGrid[i - MinGridPoint.x, j - MinGridPoint.y].transform.SetParent(_debugTilesParent);

                }

            }
        }
    }

}
