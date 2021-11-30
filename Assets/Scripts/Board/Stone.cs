using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;
using Mirror;
using TMPro;

public class Stone : NetworkBehaviour
{
    public Route currentRoute;
    int routePosition;
    public int steps;
    public bool isMoving;
    static public bool hasStop;
    static public int diceSum;
    static public int playerTurn=0;
    public PlayerMovement myMovement;
    public MyNetworkManager myNet;
    static public int points;
    public TextMeshPro pointsText;
    public void RollDice()
    {
        

        if (!isMoving)
        {
            
            steps = Random.Range(1, 7);
            AnalyticsEvent.Custom("RollDice", new Dictionary<string, object>
            {
                {"roll_dice_numbers", steps }
            });
            if(CesarController.canAdd)
            {
                steps += CesarController.addSteps;
            }
            diceSum += steps;
            Debug.Log("Dice rolled" + steps);
            CesarController.canAdd = false;
           
            StartCoroutine(Move());
        }
    }


    IEnumerator Move()
    {
        if(isMoving)
        {
            yield break;
        }
        isMoving = true;
        hasStop = false;
        while(steps>0)
        {
            routePosition++;
            routePosition %= currentRoute.childNodeList.Count;

            Vector3 nextPos = currentRoute.childNodeList[routePosition].position;
            while (MoveToNextNode(nextPos)) { yield return null; }

            yield return new WaitForSeconds(0.1f);
            steps--;

            if(steps==1)
            {
                hasStop = true;
            }
            //routePosition++;
        }
        playerTurn++;
        isMoving = false;
        GameManager.move = false;
        Debug.Log("esto debe de ser falso:" + GameManager.move);
        myNet.PassTurn();
        Debug.Log("mi turno es:" + playerTurn);

    }

    
    bool MoveToNextNode(Vector3 goal)
    {
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, 2f * Time.deltaTime));
        

    }
}
