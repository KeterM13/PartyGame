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
    
    public int playerIndex;
    public PlayerMovement myMov;
    static public bool isPlayerWinning;
    static public bool isPlayerLosing;
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
            
            return;
        }


        base.OnServerAddPlayer(conn);

        var newPlayer = conn.identity.GetComponent<MyNetworkPlayer>();

        if (playerNames.Count==numPlayers)
        {
            newPlayer.SetDisplayName(playerNames[numPlayers - 1]);
        }       
        var instace = Instantiate(players[numPlayers - 1], conn.identity.transform);

        instace.GetComponent<PlayerCharacter>().parentIdentity = conn.identity;

        NetworkServer.Spawn(instace, conn);

        newPlayer.SetChild(instace.GetComponent<NetworkIdentity>());
    }

    
    public void PassTurn()
    {
       
        if (numPlayers.Equals( Stone.playerTurn) )
        {
            Debug.Log("funciono");
            GameManager.move = true;
            
            Debug.Log("canMove es:" +GameManager.move);
            

        }
        if (Stone.playerTurn >= numPlayers)
        {
            Debug.Log("deberia cambiar el turno");
            Stone.playerTurn = 1;
            Debug.Log("Mi turno es:" + Stone.playerTurn);
            GameManager.move = true;
        }
        if(Stone.diceSum>=20)
        {
            CesarController.isDice = true;
            Stone.diceSum = 0;
        }
        if(Stone.diceSum<20)
        {
            CesarController.isDice = false;
        }
    }

    public void PointsCheck()
    {
        int morePoints = 0;
        int lessPoints = 0;
        for (int i = 1; i < numPlayers; i++)
        {
            if (Stone.points > morePoints)
            {
                morePoints = Stone.points;
            }
            else
            {
                lessPoints = Stone.points;
            }

            if (morePoints -20 >= lessPoints)
            {
                isPlayerWinning = true;
                if (lessPoints+40 <= morePoints)
                {
                    isPlayerLosing = true;
                }
            }
            //checar los puntos de cada jugador
            //checar si esos puntos son mas de 20 del ultimo
            //checar si el ultimo es menos 40 del primero
        }
    }
        
    

    public override void ServerChangeScene(string newSceneName)
    {
        base.ServerChangeScene(newSceneName);
        
    }
}
