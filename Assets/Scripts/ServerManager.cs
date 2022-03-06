using Colyseus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : ColyseusManager<ServerManager>
{
    public ColyseusClient Client
    {
        get
        {
            return client;
        }
    }
}
