using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    [SerializeField]
    private Image view;
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private Vector2 startPosition;
    [SerializeField]
    private Vector2 endPosition;
    private bool _isAnimatingShow;
    private Action _onAnimationFinishCallback = null;
    private Vector3 scaleSize = new Vector3(1.5f, 1.5f, 1);
    public void Show(Sprite sprite, Action onAnimationFinishCallback=null)
    {
        if (_isAnimatingShow)
            return;
        _isAnimatingShow = true;
        _onAnimationFinishCallback = onAnimationFinishCallback;
        rectTransform.anchoredPosition = startPosition;
        view.sprite = sprite;
        gameObject.SetActive(true);
        LeanTween.move(rectTransform, endPosition, 0.5f)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(OnAnimationComplete);
    }

    public void Reset()
    {
        if (_isAnimatingShow)
            return;
        gameObject.SetActive(false);
        rectTransform.anchoredPosition = startPosition;
        rectTransform.localScale = Vector3.one;
    }

    private void OnAnimationComplete()
    {
        _isAnimatingShow = false;
        if(_onAnimationFinishCallback!=null)
        {
            _onAnimationFinishCallback();
        }
    }

    public void WinningShake(Action onFinished)
    {
        LeanTween.scale(rectTransform, scaleSize, 1f)
            .setEase(LeanTweenType.easeInBounce)
            .setOnComplete(onFinished);
    }
}
