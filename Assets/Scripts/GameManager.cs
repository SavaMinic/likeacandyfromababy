using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<PlayerScore> playerScoresUI;
    [SerializeField] private int scoreTarget = 10;
    [SerializeField] private IntroGamePanel introGamePanel;
    [SerializeField] private EndGamePanel endGamePanel;
    [SerializeField] private List<PlayerData> playersData;
    [SerializeField] private int initialCandyCount = 10;

    [Serializable]
    public class PlayerData
    {
        public GameObject playerObject;
        public TextMeshProUGUI playerScoreText;
        public TextMeshProUGUI playerMovementText;
        public List<KeyCode> keycodeForToggle;
        public string playerMovementHint;
        public string pressToPlayHint;
    }

    private readonly int[] _scores = { 0, 0, 0, 0 };
    private CandySpawner _candySpawner;
    private GameState _gameState;

    public bool IsActive => _gameState == GameState.Playing;

    private void Awake()
    {
        _candySpawner = FindObjectOfType<CandySpawner>();
        _gameState = GameState.Menu;
    }

    private void Start()
    {
        TogglePlayer(0, true);
        TogglePlayer(1, true);
        TogglePlayer(2, false);
        TogglePlayer(3, false);
        
        Time.timeScale = 0f;
        introGamePanel.ShowIntro(scoreTarget);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.JoystickButton3))
        {
            if (_gameState == GameState.End || _gameState == GameState.Playing)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                Application.Quit();
            }
        }

        if (_gameState == GameState.Menu && (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.JoystickButton9)))
        {
            var activePlayers = playersData.Count(t => t.playerObject.activeSelf);
            if (activePlayers >= 2)
            {
                _gameState = GameState.Intro;
                _candySpawner.SpawnCandy(Mathf.RoundToInt(initialCandyCount * (activePlayers / 4f)));
                introGamePanel.HideIntro(() =>
                {
                    _gameState = GameState.Playing;
                    Time.timeScale = 1f;
                });
            }
        }

        if (_gameState == GameState.Menu && Input.anyKeyDown)
        {
            for (int playerIndex = 0; playerIndex < playersData.Count; playerIndex++)
            {
                var keys = playersData[playerIndex].keycodeForToggle;
                for (int j = 0; j < keys.Count; j++)
                {
                    if (Input.GetKeyDown(keys[j]))
                    {
                        var isActive = playersData[playerIndex].playerObject.activeSelf;
                        TogglePlayer(playerIndex, !isActive);
                        break;
                    }
                }
            }
        }
    }

    private void TogglePlayer(int index, bool isActive)
    {
        playersData[index].playerObject.SetActive(isActive);
        playersData[index].playerScoreText.text = isActive ? "0" : "/";
        playersData[index].playerMovementText.text = isActive ? playersData[index].playerMovementHint : playersData[index].pressToPlayHint;
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
        Menu,
        Intro,
        Playing,
        End
    }
}