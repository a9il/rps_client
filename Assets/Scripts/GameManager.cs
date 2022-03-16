using Colyseus;
using Colyseus.Schema;
using GameDevWare.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    ColyseusRoom<MyRoomState> gameRoom = null;
    ColyseusRoom<LeaderboardRoomState> leaderboardRoom = null;
    [SerializeField]
    private CanvasManager canvasManager;
    private GameState state = GameState.mainMenu;
    private const int MAX_MATCH_COUNT = 3;
    private int matchCount = 0;
    public const string nameKey = "k";
    private LeaderboardRoomState emptyState = new LeaderboardRoomState();
    async void Start()
    {
        canvasManager.SetMatchEndedCallback(OnMatchEnded);
        canvasManager.SetPlayerHandSelectedCallback(OnPlayerHandSelected);
        canvasManager.SetOnGameStartedCallback(OnGameStarted);
        ServerManager.Instance.InitializeClient();
        ColyseusClient client = ServerManager.Instance.Client;
        try
        {
            leaderboardRoom = await client.JoinOrCreate<LeaderboardRoomState>("l_room");
        }
        catch (System.Exception)
        {
            //fail to join leaderboard room
        }
        if (leaderboardRoom != null)
        {
            leaderboardRoom.OnMessage<object>(1, OnLeaderboardUpdated);
            await leaderboardRoom.Send(1, emptyState);
        }
        string savedName = PlayerPrefs.GetString(nameKey);
        if(string.IsNullOrEmpty(savedName))
        {
            canvasManager.GoToChangeName();
        }
        else
        {
            canvasManager.GoToMainMenu();
        }    
    }

    private void OnPlayerHandSelected(HandType hand)
    {
        if(gameRoom!=null)
        {
            MyRoomState state = new MyRoomState();
            state.hand = (int)hand;
            gameRoom.Send(0, state);
        }
    }

    private void OnMatchEnded()
    {
        matchCount++;
        if (matchCount >= MAX_MATCH_COUNT)
        {
            state = GameState.leaderboard;
            canvasManager.GoToLeaderboard();
            SendScore();
        }
        if (gameRoom==null)
        {
            StartOfflineAIMove();
        }
    }

    private void SendScore()
    {
        if(gameRoom!=null)
        {
            gameRoom.Leave();
        }
        int[] playerAndOpponentScore = canvasManager.GetPlayerAndOpponentScore();
        if (leaderboardRoom != null)
        {
            LeaderboardRoomState roomState = new LeaderboardRoomState();
            roomState.playerData = new LeaderboardData();
            roomState.playerData.name = PlayerPrefs.GetString(nameKey);
            roomState.playerData.score = playerAndOpponentScore[0];
            leaderboardRoom.Send(1, roomState);
        }
        else
        {
            canvasManager.DisplayOfflineLeaderboard(playerAndOpponentScore);
        }
        canvasManager.ResetScore();
    }

    private void OnMoveUpdated(MyRoomState message)
    {
        //server will call this function (if there are two player server will send another player move, if this is the only player on the room, server will randomize opponent move)
        //Debug.Log("OnRoomMessage: " + message.hand);
        canvasManager.ReceiveOpponentMove((HandType)message.hand);
    }

    private void OnLeaderboardUpdated(object message)
    {
        canvasManager.DisplayOnlineLeaderboard(message);
    }

    private void StartOfflineAIMove()
    {
        float randomMoveDelay = Random.Range(0.5f, 2.5f);
        Invoke("SendHand", randomMoveDelay);
    }

    private void SendHand()
    {
        int randomHand = Random.Range(0, 3);
        canvasManager.ReceiveOpponentMove((HandType)randomHand);
    }

    private void OnDestroy()
    {
        if(gameRoom!=null)
        {
            gameRoom.Leave();
        }
        if(leaderboardRoom!=null)
        {
            leaderboardRoom.Leave();
        }
    }

    async void OnGameStarted()
    {
        ColyseusClient client = ServerManager.Instance.Client;
        if (gameRoom==null)
        {
            try
            {
                gameRoom = await client.JoinOrCreate<MyRoomState>("my_room");
            }
            catch (System.Exception)
            {
                StartOfflineAIMove();
            }
        }
        else
        {
            gameRoom = await client.JoinOrCreate<MyRoomState>("my_room");
        }
        if (gameRoom != null)
        {
            gameRoom.OnMessage<MyRoomState>(0, OnMoveUpdated);
        }
        state = GameState.playing;
        matchCount = 0;
    }

    //string FormatOfflineLeaderboard(string playerName, int[] playerAndOpponentScore)
    //{
    //    string result = "1. "
    //}

}

public enum GameState
{
    mainMenu=0,
    playing=1,
    leaderboard=2
}
