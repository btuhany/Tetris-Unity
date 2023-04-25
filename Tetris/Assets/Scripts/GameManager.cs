using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int _scoreIncrement;
    [SerializeField] BlockController _blockController;
 

    [SerializeField] GameObject _inputPanel;
    [SerializeField] GameObject _inGamePanel;
    private int _score;
    public bool IsGameStopped;
    public event System.Action<int> OnScoreChanged;
    public event System.Action OnGameOver;
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void IncreaseScore()
    {
        _score += _scoreIncrement;
        OnScoreChanged?.Invoke(_score);
        HandleDifficulty();
    }

    private void HandleDifficulty()
    {
        if(_score == _scoreIncrement * 5)
        {
            _blockController.VerticalMoveSpeed *= 1.2f;
            _blockController.VerticalMovePeriod = 5/_blockController.VerticalMoveSpeed;
        }
        else if(_score == _scoreIncrement * 10) 
        {
            _blockController.VerticalMoveSpeed *= 1.5f;
            _blockController.VerticalMovePeriod = 5 / _blockController.VerticalMoveSpeed;
        }
        else if (_score == _scoreIncrement * 20)
        {
            _blockController.VerticalMoveSpeed *= 2f;
            _blockController.VerticalMovePeriod = 5 / _blockController.VerticalMoveSpeed;
        }
        else if (_score == _scoreIncrement * 30)
        {
            _blockController.VerticalMoveSpeed *= 2.5f;
            _blockController.VerticalMovePeriod = 5 / _blockController.VerticalMoveSpeed;
        }
    }

    public void GameOver()
    {
        if(!IsGameStopped)
        {
            IsGameStopped = true;
            OnGameOver?.Invoke();
            Debug.Log("GameOver!");
            Time.timeScale = 0f;
            StopTheGame();
        }

    }
    public void StartTheGame()  
    {
        IsGameStopped = false;
        _inputPanel.SetActive(false);
        _inGamePanel.SetActive(true);
        //order is important
        GridManager.Instance.SetupParameters();
        GameSetupManager.Instance.SetupTheGameParameters();
        _blockController.gameObject.SetActive(true);
    }
    private void StopTheGame()
    {
        _inputPanel.SetActive(true);
        _inGamePanel.SetActive(false);
        _blockController.gameObject.SetActive(false);
        GridManager.Instance.ClearTheGrid();
        Time.timeScale = 1f;
    }
}
