using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandleDisplayText))]
    [SerializeField]
    string displayName = "Missing Name";

    [SerializeField] TextMeshPro textMesh;

    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    private void HandleDisplayText(string oldString, string newString)
    {
        textMesh.text = newString;
    }

    [ClientRpc]
    public void SetChild(NetworkIdentity identity)
    {
        identity.transform.SetParent(transform);
    }
}
