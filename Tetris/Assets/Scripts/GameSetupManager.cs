using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetupManager : MonoBehaviour
{
    [SerializeField] Transform _hiderImg;
    [SerializeField] Camera _camera;
    [SerializeField] Transform _spawnPointTransform;
    [SerializeField] SpriteRenderer _borderSprite;
    [SerializeField] float _borderOffset = 0.45f;
    [SerializeField] Transform _gridTransform;

    public static GameSetupManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    public void SetupTheGameParameters()
    {
        _borderSprite.size = new Vector2(GridManager.Instance.GridWidth + _borderOffset, GridManager.Instance.GridHeight + _borderOffset);
        _spawnPointTransform.position = new Vector3(1, Mathf.RoundToInt(_borderSprite.size.y + 1), 0);
        _hiderImg.localScale = new Vector3(_borderSprite.size.x * 2, _borderSprite.size.y * 2, 1);
        _hiderImg.localPosition = new Vector3(0, (_borderSprite.size.y / 2 + _hiderImg.localScale.y / 2) - 0.033f, 0);
        _camera.transform.position = new Vector3(_gridTransform.position.x, _gridTransform.position.y, -10);
        if (_borderSprite.size.y > _borderSprite.size.x)
        {
            _camera.orthographicSize = _borderSprite.size.y / 2 + _borderSprite.size.y / 5;
        }
        else
        {
            _camera.orthographicSize = _borderSprite.size.x / 2 + _borderSprite.size.x / 5;
        }
    }
}
