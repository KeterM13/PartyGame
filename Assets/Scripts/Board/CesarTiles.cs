using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CesarTiles : NetworkBehaviour
{
    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Piece") && Stone.hasStop)
        {
            CesarController.isStart=true;
        }
    }
}
