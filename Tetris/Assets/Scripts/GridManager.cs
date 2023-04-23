using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] int _gridWidth = 10, _gridHeight = 20;
    [SerializeField] float _offset = 0.45f;
    [SerializeField] GameObject _debugUnfilledPrefab;
    [SerializeField] GameObject _debugFilledPrefab;
    SpriteRenderer _gridSprite;
    SpriteRenderer _borderSprite;
    SpriteRenderer[] _sprites;
    bool[,] Grid;
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
        Grid = new bool[_gridWidth, _gridHeight];
        MinGridPoint = new Vector2Int(-(_gridWidth - 2) / 2, 0);
        MaxGridPoint = new Vector2Int(((_gridWidth - 2) / 2) + 1, _gridHeight - 1);
        transform.position = new Vector3(transform.position.x, _gridHeight / 2 - 0.5f, transform.position.z);
        _gridSprite.size = new Vector2(_gridWidth,_gridHeight);
        _borderSprite.size = new Vector2(_gridWidth + _offset, _gridHeight + _offset);
        if (ShowDebugPoints)
            UpdateDebug();
    }
    public void FillTheGrid(Vector2Int[] filledTilesPos)
    {
        for (int i = 0; i < 4; i++)
        {
            Grid[filledTilesPos[i].x - MinGridPoint.x, filledTilesPos[i].y - MinGridPoint.y] = true;
        }
        if(ShowDebugPoints)
            UpdateDebug();
    }
    private void UpdateDebug()
    {
        for (int i = MinGridPoint.x; i <= MaxGridPoint.x; i++)
        {
            for (int j = MinGridPoint.y; j <= MaxGridPoint.y; j++)
            {
                if (Grid[i - MinGridPoint.x, j - MinGridPoint.y])
                    Instantiate(_debugFilledPrefab, new Vector3(i, j, 0), this.transform.rotation);
                else
                    Instantiate(_debugUnfilledPrefab, new Vector3(i, j, 0), this.transform.rotation);

            }
        }
    }

}
