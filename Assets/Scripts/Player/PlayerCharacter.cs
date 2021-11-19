using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCharacter : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandleUpdateParent))]
    public NetworkIdentity parentIdentity;

    public override void OnStartClient()
    {
        base.OnStartClient();
        transform.SetParent(parentIdentity.transform);
    }

    void HandleUpdateParent(NetworkIdentity oldParentId, NetworkIdentity newParentId)
    {
        transform.SetParent(newParentId.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
