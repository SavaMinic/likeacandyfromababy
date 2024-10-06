using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class IntroGamePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleLabel;
    [SerializeField] private TextMeshProUGUI countdownLabel;

    private CanvasGroup _canvasGroup;
    private CanvasGroup _countdownCanvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.interactable = _canvasGroup.blocksRaycasts = false;

        _countdownCanvasGroup = countdownLabel.GetComponent<CanvasGroup>();
        _countdownCanvasGroup.alpha = 0f;
    }

    public void ShowIntro(int count)
    {
        _canvasGroup.alpha = 1f;
        titleLabel.text = $"Collect {count} candies from those tiny creatures to win";
    }

    public void HideIntro(Action callback)
    {
        StartCoroutine(AnimateAlpha(callback));
    }

    private IEnumerator AnimateAlpha(Action callback)
    {
        _canvasGroup.interactable = _canvasGroup.blocksRaycasts = true;
        var time = 0f;
        while (time < 0.4f)
        {
            time += Time.unscaledDeltaTime;
            _canvasGroup.alpha = Mathf.Lerp(1f, 0f, time / 0.4f);
            yield return null;
        }

        _countdownCanvasGroup.alpha = 1f;
        countdownLabel.rectTransform.localScale = Vector3.one;
        
        for (int remainingSeconds = 3; remainingSeconds > 0; remainingSeconds--)
        {
            countdownLabel.text = remainingSeconds.ToString();
            time = 0f;
            while (time < 1f)
            {
                time += Time.unscaledDeltaTime;
                countdownLabel.rectTransform.localScale = Vector3.one * Mathf.Lerp(1f, 1.5f, time);
                yield return null;
            }
        }

        countdownLabel.text = "GO";
        callback?.Invoke();

        time = 0f;
        while (time < 0.5f)
        {
            time += Time.unscaledDeltaTime;
            _countdownCanvasGroup.alpha = Mathf.Lerp(1f, 0f, time / 0.5f);
            countdownLabel.rectTransform.localScale = Vector3.one * Mathf.Lerp(1f, 3f, time);
            yield return null;
        }
    }
}