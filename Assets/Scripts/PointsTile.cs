using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PointsTile : NetworkBehaviour
{
    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Piece") && Stone.hasStop)
        {
            Stone.points += 3;
        }
    }
}
