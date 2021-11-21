using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class TableTopManager : NetworkBehaviour
{
    [SerializeField] Button botonCambioEscenas;
    [SerializeField] Button botonMinijuego;
    
    [ServerCallback]
    private void Start()
    {
        botonCambioEscenas.onClick.AddListener(ChangeScene);
        botonMinijuego.onClick.AddListener(GoToMiniGame);
    }

    [Server]
    public void ChangeScene()
    {
        NetworkManager.singleton.ServerChangeScene("Board");
        
    }

    [Server]
    public void GoToMiniGame()
    {
        NetworkManager.singleton.ServerChangeScene("MiniGame");
    }
}
