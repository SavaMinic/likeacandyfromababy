using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleLabel;
    [SerializeField] private TextMeshProUGUI scoreLabel;
    [SerializeField] private float animationDuration = 0.4f;
    [SerializeField] private float scoreScale = 1.5f;
    private int _currentScore;

    private IEnumerator _scoreAnimation;
    private int PlayerIndex => transform.GetSiblingIndex();

    private void Awake()
    {
        titleLabel.text = $"Toddler {PlayerIndex + 1}:";
        scoreLabel.text = _currentScore.ToString();
    }

    public void UpdateScore(int newScore)
    {
        _currentScore = newScore;
        if (_scoreAnimation != null) StopCoroutine(_scoreAnimation);
        StartCoroutine(_scoreAnimation = AnimateScoring());
    }

    private IEnumerator AnimateScoring()
    {
        // reset it
        scoreLabel.transform.localScale = Vector3.one;
        
        var time = 0f;
        while (time < animationDuration / 2f)
        {
            scoreLabel.transform.localScale = Vector3.one * Mathf.Lerp(1f, scoreScale, time / (animationDuration / 2f));
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        scoreLabel.text = _currentScore.ToString();

        time = 0f;
        while (time < animationDuration / 2f)
        {
            scoreLabel.transform.localScale = Vector3.one * Mathf.Lerp(scoreScale, 1f, time / (animationDuration / 2f));
            time += Time.deltaTime;
            yield return null;
        }

        // just in case
        scoreLabel.transform.localScale = Vector3.one;
    }
}