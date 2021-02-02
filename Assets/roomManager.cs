using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
public class roomManager : NetworkRoomManager
{
    /// <summary>
    /// Called just after GamePlayer object is instantiated and just before it replaces RoomPlayer object.
    /// This is the ideal point to pass any data like player name, credentials, tokens, colors, etc.
    /// into the GamePlayer object as it is about to enter the Online scene.
    /// </summary>
    /// <param name="roomPlayer"></param>
    /// <param name="gamePlayer"></param>
    /// <returns>true unless some code in here decides it needs to abort the replacement</returns>
    /// 
    public GameObject StartButton;

    public void SetHostname(string hostname)
    {
        networkAddress = hostname;
    }
  

    public override GameObject OnServerAddPlayer(NetworkConnection conn)
    {
        GameObject player = base.OnServerAddPlayer(conn);
        player.GetComponent<Lobby_RoomPlayer>().playerName = "lalalalala";

        return null;
    }


    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        //PlayerScore playerScore = gamePlayer.GetComponent<PlayerScore>();
        //playerScore.index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
        return true;
    }

    public override void OnRoomStopClient()
    {
        // Demonstrates how to get the Network Manager out of DontDestroyOnLoad when
        // going to the offline scene to avoid collision with the one that lives there.
        if (gameObject.scene.name == "DontDestroyOnLoad" && !string.IsNullOrEmpty(offlineScene) && SceneManager.GetActiveScene().path != offlineScene)
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

        base.OnRoomStopClient();
    }

    public override void OnRoomStopServer()
    {
        // Demonstrates how to get the Network Manager out of DontDestroyOnLoad when
        // going to the offline scene to avoid collision with the one that lives there.
        if (gameObject.scene.name == "DontDestroyOnLoad" && !string.IsNullOrEmpty(offlineScene) && SceneManager.GetActiveScene().path != offlineScene)
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

        base.OnRoomStopServer();
    }

    /*
        This code below is to demonstrate how to do a Start button that only appears for the Host player
        showStartButton is a local bool that's needed because OnRoomServerPlayersReady is only fired when
        all players are ready, but if a player cancels their ready state there's no callback to set it back to false
        Therefore, allPlayersReady is used in combination with showStartButton to show/hide the Start button correctly.
        Setting showStartButton false when the button is pressed hides it in the game scene since NetworkRoomManager
        is set as DontDestroyOnLoad = true.
    */

    bool showStartButton;

    public override void OnRoomServerPlayersReady()
    {
        // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
        if (isHeadless)
            base.OnRoomServerPlayersReady();
        else
        {
            showStartButton = true;
            StartButton.SetActive(true);
        }
            

    }

    public override void OnGUI()
    {
        base.OnGUI();

        if (allPlayersReady && showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
        {
            // set to false to hide it in the game scene
            showStartButton = false;
            StartButton.SetActive(false);
            ServerChangeScene(GameplayScene);
        }
    }

    public void HostGame() {
        StartHost();
    
    }
    
    public void StartGame()
    {
        if (allPlayersReady && showStartButton)
        {
            ServerChangeScene(GameplayScene);
        }
    }

    public void ClientConnect() {
        networkAddress = "localhost";
        StartClient();
    }
}

