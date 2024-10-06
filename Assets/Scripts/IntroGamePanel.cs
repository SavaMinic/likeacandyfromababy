using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class IntroGamePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleLabel;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.interactable = _canvasGroup.blocksRaycasts = false;
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

        callback?.Invoke();
    }
}