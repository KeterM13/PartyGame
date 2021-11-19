using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector3 playerDir;

    #region Server

    [Command]
    void CmdMove(Vector3 dir)
    {
        playerDir = dir;
    }

    [ServerCallback]
    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * playerDir);
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

    #endregion
}
