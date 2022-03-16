using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameInputReceiver : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private Button backBtn;
    [SerializeField]
    private Button okBtn;
    private string playerName;
    private const int MIN_NAME_LENGTH = 1;
    private Action _onClosedCallback;
    void Start()
    {
        backBtn.onClick.AddListener(OnBackBtnClicked);
        okBtn.onClick.AddListener(OnOkBtnClicked);
        inputField.text = PlayerPrefs.GetString(GameManager.nameKey);
    }
    private void OnBackBtnClicked()
    {
        playerName = inputField.text;
        if (playerName.Length >= MIN_NAME_LENGTH)
        {
            PlayerPrefs.SetString(GameManager.nameKey, playerName);
            if (_onClosedCallback != null)
            {
                _onClosedCallback();
            }
        }
    }
    private void OnOkBtnClicked()
    {
        playerName = inputField.text;
        if(playerName.Length>=MIN_NAME_LENGTH)
        {
            PlayerPrefs.SetString(GameManager.nameKey, playerName);
            if(_onClosedCallback!=null)
            {
                _onClosedCallback();
            }
        }
    }
    public void SetOnCloseCallback(Action callback)
    {
        _onClosedCallback = callback;
    }
}
