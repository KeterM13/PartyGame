using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField] GameObject prefabProjectile;
    [SerializeField] Transform spawnPos;

    #region Server
    [Command]
    void CmdShoot()
    {
        //Instanciar a nuestro objeto
        var instance = Instantiate(prefabProjectile, spawnPos.position, spawnPos.rotation);

        //Avisar a la red que se agrego uin nuevo objeto
        NetworkServer.Spawn(instance, connectionToClient);
    }

    #endregion

    #region Client
    public void ShootNow(InputAction.CallbackContext context)
    {
        if (!hasAuthority) return;

        CmdShoot();
    }
    #endregion
}
