using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Tile : NetworkBehaviour
{
    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Piece") && Stone.hasStop)
        {
            // NetworkManager.singleton.ServerChangeScene("MiniGame");
            Debug.Log("Juego");
        }
    }
}
