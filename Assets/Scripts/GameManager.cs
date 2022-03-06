using Colyseus;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    ColyseusRoom<MyRoomState> room = null;
    [SerializeField]
    private CanvasManager canvasManager;
    async void Start()
    {
        canvasManager.SetMatchEndedCallback(OnMatchEnded);
        canvasManager.SetPlayerHandSelectedCallback(OnPlayerHandSelected);
        ServerManager.Instance.InitializeClient();
        ColyseusClient client = ServerManager.Instance.Client;
        try
        {
            room = await client.JoinOrCreate<MyRoomState>("my_room");
        }
        catch (System.Exception)
        {
            StartOfflineAIMove();
        }
        if(room!=null)
        {
            room.OnMessage<MyRoomState>(0, OnRoomMessage);
        }
    }

    private void OnPlayerHandSelected(HandType hand)
    {
        if(room!=null)
        {
            MyRoomState state = new MyRoomState();
            state.hand = (int)hand;
            room.Send(0, state);
        }
    }

    private void OnMatchEnded()
    {
        if(room==null)
        {
            StartOfflineAIMove();
        }
    }

    private void OnRoomMessage(MyRoomState message)
    {
        Debug.Log("OnRoomMessage: " + message.hand);
        canvasManager.ReceiveOpponentMove((HandType)message.hand);
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
        if(room!=null)
        {
            room.Leave();
        }
    }

}
