using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
        _gridSprite.size = new Vector2(_gridWidth, _gridHeight);
        _borderSprite.size = new Vector2(_gridWidth + _offset, _gridHeight + _offset);
        if (ShowDebugPoints)
            UpdateDebug();
    }
    public void AddBlockTilesToGrid(GameObject[] tilePositions)
    {
        CheckClearFilledRows(FillTheGrid(tilePositions));
        if (ShowDebugPoints)
            UpdateDebug();
    }
    private List<int> FillTheGrid(GameObject[] tilePositions)
    {
        List<int> newTilesRows = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            int tilePosXRelativeToGrid = Mathf.RoundToInt(tilePositions[i].transform.position.x) - MinGridPoint.x;
            int tilePosYRelativeToGrid = Mathf.RoundToInt(tilePositions[i].transform.position.y) - MinGridPoint.y;

            if (tilePosXRelativeToGrid < _gridWidth && tilePosYRelativeToGrid < _gridHeight)
            {
                _grid[tilePosXRelativeToGrid, tilePosYRelativeToGrid] = tilePositions[i];
                if (!newTilesRows.Contains(tilePosYRelativeToGrid))
                    newTilesRows.Add(tilePosYRelativeToGrid);
            }
            else
            {
                Debug.Log("finish");
                Time.timeScale = 0f;
                //GameManager Stop
            }
        }
        return newTilesRows;
    }
    private void CheckClearFilledRows(List<int> newTileRows)
    {
        List<int> rowsToClear = new List<int>();
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
            ClearFullRow(rowsToClear);
            MoveRowsDown(rowsToClear);

        }
    }

    private void MoveRowsDown(List<int> rowsToClear)
    {
        for (int i = rowsToClear.Max() + 1; i < _gridHeight; i++)
        {
            for (int j = 0; j < _gridWidth; j++)
            {
                if (_grid[j, i])  //if grid cell contains tile
                {
                    _grid[j, i].gameObject.transform.position += Vector3.down * rowsToClear.Count;
                    _grid[j, i - rowsToClear.Count] = _grid[j, i];
                    _grid[j, i] = null;
                }
            }

        }
    }

    public void ClearFullRow(List<int> rowsToClear)
    {
        for (int x = 0; x < rowsToClear.Count; x++)
        {
            for (int i = 0; i < _gridWidth; i++)
            {
                Destroy(_grid[i, rowsToClear[x]].gameObject);  
                _grid[i, rowsToClear[x]] = null;
            }
        }

    }
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
