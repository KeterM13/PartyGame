using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector3 playerDir;
    [SerializeField] Rigidbody myRb;
    [SerializeField] float jumpForce;
    [SerializeField] bool canJump;
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
    [ServerCallback]
    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * playerDir);
    }

    [Server]

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Floor")
        {
            canJump = true;
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

    #endregion
}
