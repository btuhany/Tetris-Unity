using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] int _gridWidth = 10, _gridHeight = 20;
    [SerializeField] float _offset = 0.45f;
    SpriteRenderer _gridSprite;
    SpriteRenderer _borderSprite;
    SpriteRenderer[] _sprites;

    public static GridManager Instance;

    public int MinGridY = 0;

    private void Awake()
    {
        Instance = this;
        _sprites = GetComponentsInChildren<SpriteRenderer>();
        _gridSprite = _sprites[0];
        _borderSprite = _sprites[1];
        
    }
    private void OnEnable()
    {
        transform.position = new Vector3(transform.position.x, _gridHeight / 2 - 0.5f, transform.position.z);
        _gridSprite.size = new Vector2(_gridWidth,_gridHeight);
        _borderSprite.size = new Vector2(_gridWidth + _offset, _gridHeight + _offset);
    }

}
