using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particleFx;
    [SerializeField]
    private RectTransform rectTransform;
    private static ParticleController _instance;
    public static ParticleController Instance 
    {
        get
        {
            return _instance;
        }
    }
    
    void Awake()
    {
        _instance = this;
    }

    public void Play(Vector3 pos)
    {
        rectTransform.anchoredPosition = pos;
        particleFx.Play();
    }    
}
