using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            // TODO: show menu instead of reload
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}