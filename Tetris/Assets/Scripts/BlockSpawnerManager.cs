using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnerManager : MonoBehaviour
{
    [SerializeField] BlockPiecesHandler[] _tetrominoBlockPrefabs;
    [SerializeField] Transform _spawnPoint;
    public static BlockSpawnerManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public BlockPiecesHandler SpawnRandomBlock=> Instantiate(_tetrominoBlockPrefabs[Random.Range(0, _tetrominoBlockPrefabs.Length)], _spawnPoint);



}
