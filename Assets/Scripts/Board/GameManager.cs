using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public MyNetworkManager networkManager;
    
    [SerializeField] static public int playersTurn =1;
    // Start is called before the first frame update
    void Start()
    {
        networkManager.GetComponent<MyNetworkManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(networkManager.numPlayers==1)
        {
            
            if(playersTurn>networkManager.numPlayers)
            {
                playersTurn = 1;
            }
        }
    }
}
