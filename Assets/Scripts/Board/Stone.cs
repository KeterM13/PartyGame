using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class Stone : NetworkBehaviour
{
    public Route currentRoute;
    int routePosition;
    public int steps;
    bool isMoving;
    
    static public int playerTurn=1;
    public PlayerMovement myMovement;
    public MyNetworkManager myNet;

    public void RollDice()
    {
        

        if (!isMoving)
        {
            steps = Random.Range(1, 7);
            Debug.Log("Dice rolled" + steps);
            
           
            StartCoroutine(Move());
        }
    }

    
    
    [ServerCallback]
    IEnumerator Move()
    {
        if(isMoving)
        {
            yield break;
        }
        isMoving = true;

        while(steps>0)
        {
            routePosition++;
            routePosition %= currentRoute.childNodeList.Count;

            Vector3 nextPos = currentRoute.childNodeList[routePosition].position;
            while (MoveToNextNode(nextPos)) { yield return null; }

            yield return new WaitForSeconds(0.1f);
            steps--;
            //routePosition++;
        }
        playerTurn++;
        isMoving = false;
        GameManager.move = false;
        myNet.PassTurn();
        Debug.Log("mi turno es:" + playerTurn);

    }

    
    bool MoveToNextNode(Vector3 goal)
    {
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, 2f * Time.deltaTime));
        

    }
}
