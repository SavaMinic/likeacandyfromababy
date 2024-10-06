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
    private Color _initialColor;

    private void Awake()
    {
        _meshRenderer = GetComponent<SkinnedMeshRenderer>();
        _initialColor = _meshRenderer.material.color;
    }

    public void DoHitAnimation()
    {
        if (_hitAnimation != null) StopCoroutine(_hitAnimation);
        StartCoroutine(_hitAnimation = DoHit());
    }

    public void ToggleCrying(bool isCrying)
    {
        if (_hitAnimation != null) StopCoroutine(_hitAnimation);
        _meshRenderer.material.color = isCrying ? cryColor : _initialColor;
    }

    private IEnumerator DoHit()
    {
        var time = 0f;
        while (time <= hitDuration)
        {
            var colorIntensity = hitColorAnimationCurve.Evaluate(time / hitDuration);
            _meshRenderer.material.color = Color.Lerp(_initialColor, hitColor, colorIntensity);
            time += Time.deltaTime;
            yield return null;
        }
    }
    
}