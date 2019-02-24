using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerNetworkManager : NetworkManager
{
    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
    }

    public override void OnStopHost()
    {
        base.OnStopHost();
    }

    private void Update()
    {

    }
}
