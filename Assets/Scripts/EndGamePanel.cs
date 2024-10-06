using System.Collections;
using TMPro;
using UnityEngine;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleLabel;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.interactable = _canvasGroup.blocksRaycasts = false;
    }

    public void ShowEndGame(int winnerIndex)
    {
        titleLabel.text = $"Toddler {winnerIndex + 1} grabbed the treats,\nfrom the tiny creatures so neat!";
        _canvasGroup.interactable = _canvasGroup.blocksRaycasts = true;
        StartCoroutine(AnimateAlpha());
    }

    private IEnumerator AnimateAlpha()
    {
        var time = 0f;
        while (time < 0.4f)
        {
            time += Time.unscaledDeltaTime;
            _canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / 0.4f);
            yield return null;
        }
    }
}