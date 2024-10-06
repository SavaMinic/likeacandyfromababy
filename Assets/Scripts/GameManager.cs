using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<PlayerScore> playerScoresUI;
    [SerializeField] private int scoreTarget = 10;
    [SerializeField] private EndGamePanel endGamePanel;
    
    private readonly int[] _scores = { 0, 0, 0, 0 };
    private CandySpawner _candySpawner;
    
    public bool IsActive { get; private set; }

    private void Awake()
    {
        _candySpawner = FindObjectOfType<CandySpawner>();
        IsActive = true;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            // TODO: show menu instead of reload
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IncreaseScore(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= _scores.Length) return;

        _scores[playerIndex]++;
        playerScoresUI[playerIndex].UpdateScore(_scores[playerIndex]);

        if (_scores[playerIndex] >= scoreTarget)
        {
            // end game
            IsActive = false;
            endGamePanel.ShowEndGame(playerIndex);
        }
        else
        {
            // spawn a new candy somewhere
            StartCoroutine(SpawnNewCandyAfterSeconds(Random.Range(2f, 4f)));
        }
        
    }

    private IEnumerator SpawnNewCandyAfterSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        _candySpawner.SpawnCandy(1);
    }
}