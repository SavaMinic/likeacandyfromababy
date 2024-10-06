using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private readonly int[] _scores = { 0, 0, 0, 0 };

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
    }
}