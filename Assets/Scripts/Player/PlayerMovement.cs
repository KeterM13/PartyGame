using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector3 playerDir;
    [SerializeField] Rigidbody myRb;
    [SerializeField] float jumpForce;
    [SerializeField] bool canJump;
    [SyncVar] public bool isBoard;
    [SerializeField] int hitwallNumber;
    [SerializeField] int wins;
    [SerializeField] Stone myStone;
    [SyncVar]  public bool canMove;

    #region Server

   

    [Command]
    void CmdMove(Vector3 dir)
    {
        playerDir = dir;
    }
    [Command]
    void CmdJump()
    {
        if(canJump)
        {
            Debug.Log("Salte");
            myRb.velocity = new Vector3(myRb.velocity.x, jumpForce, myRb.velocity.z);
            canJump = false;
        }
       
    }
    [Command]
    void CmdRoll()
    {
        myStone.RollDice();
        Debug.Log("Estoy Rolleando");
        
    }
    

    [ServerCallback]
    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * playerDir);
        
    }

    [Server]
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            canJump = true;
        }

        if (collision.gameObject.CompareTag("Platform"))
        {
            hitwallNumber++;
            AnalyticsEvent.Custom("hit_wall", new Dictionary<string, object>
            {
                {"number_of_walls", hitwallNumber }
            });
            
        }
    }
   


    #endregion

    #region Client

    public void Movement(InputAction.CallbackContext context)
    {
        
        if (!hasAuthority) { return; }

        Vector2 move = context.ReadValue<Vector2>();
        var dir = new Vector3(move.x, 0, move.y);
        CmdMove(dir);
    }
    public void Jump(InputAction.CallbackContext context)
    {
       
        if(!hasAuthority) { return; }
        CmdJump();

    }
    public void Roll(InputAction.CallbackContext context)
    {
        if(!GameManager.move) { return; }
        if(!hasAuthority) { return; }
        CmdRoll();
    }
    #endregion
}
