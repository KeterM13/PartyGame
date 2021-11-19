using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class TableTopManager : NetworkBehaviour
{
    [SerializeField] Button botonCambioEscenas;
    
    [ServerCallback]
    private void Start()
    {
        botonCambioEscenas.onClick.AddListener(ChangeScene);
    }

    [Server]
    public void ChangeScene()
    {
        NetworkManager.singleton.ServerChangeScene("NextScene");
        
    }
}
