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
    
    [SyncVar]
    public string playerName;
    public static event Action<Lobby_RoomPlayer, string> OnMessage;
    [Command]
    public void CmdSend(string message)
    {
        if (message.Trim() != "")
            RpcReceive(message.Trim());
    }

    [ClientRpc]
    public void RpcReceive(string message)
    {
        OnMessage?.Invoke(this, message);
    }

    public override void OnStartClient()
    {
        if (logger.LogEnabled()) logger.LogFormat(LogType.Log, "OnStartClient {0}", SceneManager.GetActiveScene().path);
        
        base.OnStartClient();
    }

    public override void OnClientEnterRoom()
    {
        if (logger.LogEnabled()) logger.LogFormat(LogType.Log, "OnClientEnterRoom {0}", SceneManager.GetActiveScene().path);
       
        Transform playerList = GameObject.Find("Players").transform;
        playerList.GetChild((int)netId).gameObject.SetActive(true);
        playerList.GetChild((int)netId).GetComponentInChildren<Text>().text = playerName + netId.ToString();
        RefreshView((int)netId);
        

  
    }

    public void RefreshView(int Cid) {
        if (!isLocalPlayer)
            return;
        Transform playerList = GameObject.Find("Players").transform;
        playerList.GetChild(Cid).GetChild(1).gameObject.SetActive(true);
        playerList.GetComponentInChildren<Button>().onClick.AddListener(readyUp);
    }

    public override void OnClientExitRoom()
    {
        if (logger.LogEnabled()) logger.LogFormat(LogType.Log, "OnClientExitRoom {0}", SceneManager.GetActiveScene().path);
    }

    public override void ReadyStateChanged(bool _, bool newReadyState)
    {
        if (logger.LogEnabled()) logger.LogFormat(LogType.Log, "ReadyStateChanged {0}", newReadyState);
    }

    public void readyUp() {
        CmdChangeReadyState(true);
    }
}
