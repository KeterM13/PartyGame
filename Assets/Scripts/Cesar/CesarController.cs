using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class CesarController : MonoBehaviour
{
 
    
    
    
    static public bool canAdd;


    static public int addSteps;
    [Task]
    static public bool isDice;

    [Task]
    static public bool isStart;

    [Task]
    static public bool isGiveAndTake;

    [Task]
    void LosePoints()
    {
        Stone.points -= 5;
        isStart = false;
        isDice = false;
        Task.current.Succeed();
    }

    [Task]
    void GainPoints()
    {
        Stone.points += 5;
        isStart = false;
        isDice = false;
        Task.current.Succeed();
         
    }

    [Task]
    void AddSteps()
    {
        canAdd = true;
        addSteps = Random.Range(1, 7);
        isStart = false;
        isDice = false;
        Task.current.Succeed();
    }

    [Task]
    void GiveAndTake()
    {
        //aqui quitaria puntos al primer jugador
        //aqui daria puntos al ultimo jugador
        Task.current.Succeed();
    }

    [Task]
    void DiceCheck()
    {
        if(Stone.diceSum>=20)
        {
            isDice = true;
            
        }
    }

    [Task]
    void Succed()
    {
        Task.current.Succeed();
    }
   
}
