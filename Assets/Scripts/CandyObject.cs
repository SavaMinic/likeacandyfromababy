using System;
using System.Collections;
using UnityEngine;

public class CandyObject : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private float timeToEat = 5f;
    [SerializeField] private Color finishColor;
    [SerializeField] private AnimationCurve eatingScale;

    private IEnumerator _eatingAnimation;
    private Color _fullColor;

    private void Awake()
    {
        _fullColor = meshRenderer.material.color;
    }

    public void GrabByPlayer(Action<GameObject> candyEaten)
    {
        if (_eatingAnimation != null) StopCoroutine(_eatingAnimation);
        StartCoroutine(_eatingAnimation = StartEatingIt(candyEaten));
    }

    private IEnumerator StartEatingIt(Action<GameObject> candyEaten)
    {
        var time = 0f;
        var material = meshRenderer.material;
        while (time <= timeToEat)
        {
            material.color = Color.Lerp(_fullColor, finishColor, time / timeToEat);
            transform.localScale = Vector3.one * eatingScale.Evaluate(time / timeToEat);
            time += Time.deltaTime;
            yield return null;
        }
        candyEaten?.Invoke(gameObject);
    }

    public void ReleaseIt()
    {
        if (_eatingAnimation != null) StopCoroutine(_eatingAnimation);
        meshRenderer.material.color = _fullColor;
        transform.localScale = Vector3.one;
    }
}