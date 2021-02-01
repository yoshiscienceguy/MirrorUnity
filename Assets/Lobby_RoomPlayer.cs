using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;

public class Lobby_RoomPlayer : NetworkRoomPlayer
{
    static readonly ILogger logger = LogFactory.GetLogger(typeof(Lobby_RoomPlayer));
    

    public override void OnStartClient()
    {
        if (logger.LogEnabled()) logger.LogFormat(LogType.Log, "OnStartClient {0}", SceneManager.GetActiveScene().path);

        base.OnStartClient();
    }

    public override void OnClientEnterRoom()
    {
        if (logger.LogEnabled()) logger.LogFormat(LogType.Log, "OnClientEnterRoom {0}", SceneManager.GetActiveScene().path);
        Debug.Log("hi");
        Transform playerList = GameObject.Find("Players").transform;
        playerList.GetChild(index).gameObject.SetActive(true);
        playerList.GetChild(index).GetComponentInChildren<Button>().onClick.AddListener(readyUp);
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
