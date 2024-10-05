using System.Collections;
using UnityEngine;

public class HitFeedback : MonoBehaviour
{
    [SerializeField] private Color hitColor;
    [SerializeField] private Color cryColor;
    [SerializeField] private float hitDuration = 1f;
    [SerializeField] private AnimationCurve hitColorAnimationCurve;
    private IEnumerator _hitAnimation;

    private SkinnedMeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    public void DoHitAnimation()
    {
        if (_hitAnimation != null) StopCoroutine(_hitAnimation);
        StartCoroutine(_hitAnimation = DoHit());
    }

    public void ToggleCrying(bool isCrying)
    {
        if (_hitAnimation != null) StopCoroutine(_hitAnimation);
        _meshRenderer.material.color = isCrying ? cryColor : Color.white;
    }

    private IEnumerator DoHit()
    {
        var time = 0f;
        while (time <= hitDuration)
        {
            var colorIntensity = hitColorAnimationCurve.Evaluate(time / hitDuration);
            _meshRenderer.material.color = Color.Lerp(Color.white, hitColor, colorIntensity);
            time += Time.deltaTime;
            yield return null;
        }
    }
    
}