using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using Colyseus.Schema;
using GameDevWare.Serialization;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI label;
    [SerializeField]
    private Button backBtn;
    private Action _onCloseCallback;
    void Start()
    {
        backBtn.onClick.AddListener(OnBackBtnClicked);
    }
    public void SetOnCloseCallback(Action callback)
    {
        _onCloseCallback = callback;
    }
    void OnBackBtnClicked()
    {
        gameObject.SetActive(false);
        if(_onCloseCallback!=null)
        {
            _onCloseCallback();
        }
    }
    public void DisplayOfflineLeaderboard(int[] playerOpponentScore, string playerName)
    {
        LeaderboardData[] datas = new LeaderboardData[2];
        datas[0] = new LeaderboardData { name = playerName, score = playerOpponentScore[0] };
        datas[1] = new LeaderboardData { name = "Opponent", score = playerOpponentScore[1] };
        Display(datas);
    }

    public void DisplayLeaderboard(object data)
    {
        List<object> list = data as List<object>;
        LeaderboardData[] datas = new LeaderboardData[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            LeaderboardData l = new LeaderboardData();
            IndexedDictionary<string, object> d = list[i] as IndexedDictionary<string, object>;
            l.name = d["name"].ToString();
            int score = 0;
            int.TryParse(d["score"].ToString(), out score);
            l.score = score;
            datas[i] = l;
        }
        Display(datas);
    }

    void Display(LeaderboardData[] datas)
    {
        Array.Sort(datas, delegate (LeaderboardData a, LeaderboardData b) { return b.score.CompareTo(a.score); });
        string result = "";
        for (int i = 0; i < datas.Length; i++)
        {
            result += i + 1 + ". " + datas[i].name + " : " + datas[i].score + "\n";
        }
        label.text = result;
        
    }
}
