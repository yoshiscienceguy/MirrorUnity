using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;
using System;

public class Lobby_RoomPlayer : NetworkRoomPlayer
{
    static readonly ILogger logger = LogFactory.GetLogger(typeof(Lobby_RoomPlayer));
    private roomManager manager;
    [SyncVar]
    public string playerName;

    public override void OnStartClient()
    {
        if (logger.LogEnabled()) logger.LogFormat(LogType.Log, "OnStartClient {0}", SceneManager.GetActiveScene().path);

        base.OnStartClient();
    }

    public void updateView() {
        RpcSetPlayerName();
    }
    public override void OnClientEnterRoom()
    {
        if (logger.LogEnabled()) logger.LogFormat(LogType.Log, "OnClientEnterRoom {0}", SceneManager.GetActiveScene().path);
        SetPlayerName(); 
        CmdSendNameToServer();
        
    }
    
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        SetPlayerName();
        
    }
    [Client]
    void SetPlayerName()
    {
        if(isLocalPlayer)
            playerName = GameObject.Find("Canvas").transform.GetChild(0).GetChild(0).GetComponentInChildren<InputField>().text;
    }

    [Command]
    void CmdSendNameToServer()
    {
        
        RpcSetPlayerName();
    }
    [ClientRpc]
    void RpcSetPlayerName()
    {
        if (manager == null)
        {
            manager = GameObject.Find("Room Manager").GetComponent<roomManager>();
        }
        Transform playerList = GameObject.Find("Players").transform;
        for (int i = 0; i < manager.roomSlots.Count; i++) {
            Lobby_RoomPlayer p = manager.roomSlots[i].GetComponent<Lobby_RoomPlayer>();
            playerList.GetChild(i).gameObject.SetActive(true);
            playerList.GetChild(i).GetChild(1).gameObject.SetActive(true);
            playerList.GetChild(i).GetComponentInChildren<Button>().onClick.AddListener(readyUp);
            playerList.GetChild(i).GetComponentInChildren<Text>().text = p.playerName;
        }
    }



    public override void OnClientExitRoom()
    {
        if (logger.LogEnabled()) logger.LogFormat(LogType.Log, "OnClientExitRoom {0}", SceneManager.GetActiveScene().path);
    }

    public override void ReadyStateChanged(bool _, bool newReadyState)
    {
        if (logger.LogEnabled()) logger.LogFormat(LogType.Log, "ReadyStateChanged {0}", newReadyState);
    }

    public void readyUp()
    {
        CmdChangeReadyState(true);
    }
}
