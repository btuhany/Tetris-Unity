using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnerManager : MonoBehaviour
{
    [SerializeField] BlockPiecesHandler[] _tetrominoBlockPrefabs;
    [SerializeField] RectTransform _nextBlockPreviewTransform;
    
    Vector3 randomBlockLocalPos = new Vector3(0, -10, 0);
    Vector3 randomBlockLocalScale = new Vector3(20, 20, 10);
    public static BlockSpawnerManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    // public BlockPiecesHandler SpawnRandomBlock=> Instantiate(_tetrominoBlockPrefabs[Random.Range(0, _tetrominoBlockPrefabs.Length)], _spawnPoint);

    public BlockPiecesHandler RandomNewBlock()
    {
        BlockPiecesHandler randomBlock = Instantiate(_tetrominoBlockPrefabs[Random.Range(0, _tetrominoBlockPrefabs.Length)], this.transform);
        randomBlock.transform.SetParent(_nextBlockPreviewTransform);
        randomBlock.transform.localPosition = randomBlockLocalPos;
        randomBlock.transform.localScale = randomBlockLocalScale;
        return randomBlock;
    }

}
