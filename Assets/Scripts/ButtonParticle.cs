using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonParticle : Button
{
    public RectTransform rt;
    private Vector2 localPos;
    override public void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, eventData.position, Camera.main, out localPos);
        ParticleController.Instance.Play(localPos);
       // Debug.Log("Screen Point: " + localPos);
    }
}