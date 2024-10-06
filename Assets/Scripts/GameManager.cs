using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<PlayerScore> playerScoresUI;
    [SerializeField] private int scoreTarget = 10;
    [SerializeField] private IntroGamePanel introGamePanel;
    [SerializeField] private EndGamePanel endGamePanel;

    private readonly int[] _scores = { 0, 0, 0, 0 };
    private CandySpawner _candySpawner;
    private GameState _gameState;

    public bool IsActive => _gameState == GameState.Playing;

    private void Awake()
    {
        _candySpawner = FindObjectOfType<CandySpawner>();
        _gameState = GameState.Intro;
    }

    private void Start()
    {
        Time.timeScale = 0f;
        introGamePanel.ShowIntro(scoreTarget);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && (_gameState == GameState.End || _gameState == GameState.Playing))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (_gameState == GameState.Intro && Input.GetKeyUp(KeyCode.Return))
            introGamePanel.HideIntro(() =>
            {
                _gameState = GameState.Playing;
                Time.timeScale = 1f;
            });
    }

    public void IncreaseScore(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= _scores.Length) return;

        _scores[playerIndex]++;
        playerScoresUI[playerIndex].UpdateScore(_scores[playerIndex]);

        if (_scores[playerIndex] >= scoreTarget)
        {
            // end game
            _gameState = GameState.End;
            endGamePanel.ShowEndGame(playerIndex);
            Time.timeScale = 0f;
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

    private enum GameState
    {
        Intro,
        Playing,
        End
    }
}