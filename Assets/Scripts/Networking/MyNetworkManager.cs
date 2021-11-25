using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    [Header("My Variables")]
    [SerializeField] List<GameObject> players;
    [SerializeField] TMP_InputField field;
    [SerializeField] List<string> playerNames = new List<string>();

    public struct NameMessage : NetworkMessage
    {
        public string userName;
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        NetworkServer.RegisterHandler<NameMessage>(OnGetMessage, false);
    }

    public void OnGetMessage(NetworkConnection conn, NameMessage msg)
    {
        playerNames.Add(msg.userName);

        if (!conn.isReady) return;

        if(!conn.identity.TryGetComponent<MyNetworkPlayer>(out MyNetworkPlayer player))
        {
            return;
        }

        player.SetDisplayName(playerNames[numPlayers-1]);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("Comienza el cliente");
        Debug.Log(field);

    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        var msg = new NameMessage()
        {
            userName = field.text
        };

        NetworkClient.Send(msg);
        
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        var newPlayer = conn.identity.GetComponent<MyNetworkPlayer>();

        if(playerNames.Count==numPlayers)
        {
            newPlayer.SetDisplayName(playerNames[numPlayers - 1]);
        }

        var instace = Instantiate(players[numPlayers - 1], conn.identity.transform);

        instace.GetComponent<PlayerCharacter>().parentIdentity = conn.identity;

        NetworkServer.Spawn(instace, conn);

        newPlayer.SetChild(instace.GetComponent<NetworkIdentity>());
    }

    public override void ServerChangeScene(string newSceneName)
    {
        base.ServerChangeScene(newSceneName);
        
    }
}
