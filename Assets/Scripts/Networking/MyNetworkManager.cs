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
    [SerializeField] GameObject boardPrefab;
    [SerializeField] bool canMove = true;
    public int playerIndex;
    public PlayerMovement myMov;
    
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
        
       
        if (networkSceneName == "Board")
        {
           
            Transform startPos = GetStartPosition();
            GameObject boardPlayer = startPos != null
                ? Instantiate(boardPrefab, startPos.position, startPos.rotation)
                : Instantiate(boardPrefab);

            NetworkServer.AddPlayerForConnection(conn, boardPlayer);
            Debug.Log("Mi jugador es:" +numPlayers);
            canMove = true;
            conn.identity.GetComponent<PlayerMovement>().SetCanMove(canMove);
            return;
        }


        base.OnServerAddPlayer(conn);

        var newPlayer = conn.identity.GetComponent<MyNetworkPlayer>();

        

        

        if (playerNames.Count==numPlayers)
        {
            newPlayer.SetDisplayName(playerNames[numPlayers - 1]);
        }
        conn.identity.GetComponent<PlayerMovement>().isBoard = false;


        var instace = Instantiate(players[numPlayers - 1], conn.identity.transform);

        instace.GetComponent<PlayerCharacter>().parentIdentity = conn.identity;

        NetworkServer.Spawn(instace, conn);

        newPlayer.SetChild(instace.GetComponent<NetworkIdentity>());
    }

    [Server]
    public void PassTurn()
    {
        Debug.Log("funciono");
        if (numPlayers == Stone.playerTurn)
        {
            canMove = true;
            myMov.SetCanMove(canMove);
            Debug.Log("canMove es:" +canMove);
            if (Stone.playerTurn > numPlayers)
            {
                Stone.playerTurn = 1;
            }
        }
    }

    
        
    

    public override void ServerChangeScene(string newSceneName)
    {
        base.ServerChangeScene(newSceneName);
        
    }
}
