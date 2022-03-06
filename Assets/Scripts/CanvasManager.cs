
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    private Button[] handBtns;
    [SerializeField]
    private CanvasGroup buttonCanvasGroup;
    [SerializeField]
    private Sprite[] handSprites;
    [SerializeField]
    private Hand playerHand;
    [SerializeField]
    private Hand opponentHand;
    private Dictionary<int, HandType> submittedHands = new Dictionary<int, HandType>();
    private bool _isOnMatch;
    private Action _onMatchEnded;
    private int winnerIndex = -1;
    private StatData playerStatData = new StatData();
    [SerializeField]
    private StatInfo playerStatInfo;
    private StatData opponentStatData = new StatData();
    [SerializeField]
    private StatInfo opponetStatInfo;
    private Action<HandType> _onHandSelected;
    public void SetPlayerHandSelectedCallback(Action<HandType> callback) 
    {
        _onHandSelected = callback;
    }

    void Start()
    {
        for (int i = 0; i < handBtns.Length; i++)
        {
            int localI = i;
            handBtns[i].onClick.AddListener(() => OnHandBtnClicked(localI));
        }
    }
    private void OnHandBtnClicked(int index)
    {
        buttonCanvasGroup.interactable = false;
        HandType handType = (HandType)index;
        submittedHands.Add(0, handType);
        if(submittedHands.Count==2 && !_isOnMatch)
        {
            StartShowAndCompare();
        }
        if(_onHandSelected!=null)
        {
            _onHandSelected(handType);
        }    
    }
    public void ReceiveOpponentMove(HandType hand)
    {
        submittedHands.Add(1, hand);
        if(submittedHands.Count == 2 && !_isOnMatch)
        {
            StartShowAndCompare();
        }
    }

    private void StartShowAndCompare()
    {
        HandType[] hands = new HandType[2];
        hands[0] = submittedHands[0];
        hands[1] = submittedHands[1];
        winnerIndex = CheckWinnerIndex(hands);
        _isOnMatch = true;
        playerHand.Show(handSprites[(int)submittedHands[0]]);
        opponentHand.Show(handSprites[(int)submittedHands[1]], OnMatchFinished);
    }

    void OnMatchFinished()
    {
        if (winnerIndex==0)
        {
            UpdatePlayerWin();
            playerHand.WinningShake(WinninAnimationEnded);
        }
        else if(winnerIndex==1)
        {
            UpdateOpponentWin();
            opponentHand.WinningShake(WinninAnimationEnded);
        }
        else
        {
            UpdateDraw();
            WinninAnimationEnded();
        }
        UpdateStatInfo();
    }
    void WinninAnimationEnded()
    {
        Invoke("EndMatch", 1);
    }
    void EndMatch()
    {
        _isOnMatch = false;
        playerHand.Reset();
        opponentHand.Reset();
        buttonCanvasGroup.interactable = true;
        winnerIndex = -1;
        submittedHands.Clear();
        if (_onMatchEnded!=null)
        {
            _onMatchEnded();
        }
    }
    public void SetMatchEndedCallback(Action callback)
    {
        _onMatchEnded = callback;
    }
    void UpdatePlayerWin()
    {
        playerStatData.winCount++;
        opponentStatData.loseCount++;
    }
    void UpdateOpponentWin()
    {
        playerStatData.loseCount++;
        opponentStatData.winCount++;
    }
    void UpdateDraw()
    {
        playerStatData.drawCount++;
        opponentStatData.drawCount++;
    }
    void UpdateStatInfo()
    {
        playerStatInfo.UpdateData(playerStatData);
        opponetStatInfo.UpdateData(opponentStatData);
    }
    /***
     * rock, paper, scissor
     * 0   , 1    , 2
     */
    public static int CheckWinnerIndex(HandType[] playerHands)
    {
        if(playerHands.Length == 2)
        {
            int diff = playerHands[0] - playerHands[1];
            switch (diff)
            {
                case 1:
                    return 0;
                case -1:
                    return 1;
                case -2:
                    return 0;
                case 2:
                    return 1;
                default:
                    return -1;
            }
        }
        return -1;
    }    
}
