using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Text TxtRoom;
    public Text TxtPlayerList;
    public Text TxtScore;
    public GameObject PlayerPrefab;
    public Transform SpawnPoint;

    // Use this for initialization
    void Start()
    {
        TxtRoom.text = PhotonNetwork.player.NickName + " you are in room: " + PhotonNetwork.room.Name;
        DisplayPlayers();
        GameObject connectedPlayer = PhotonNetwork.Instantiate(PlayerPrefab.name, SpawnPoint.position, Quaternion.identity, 0);
        connectedPlayer.GetComponent<FirstPersonController>().enabled = true;
        connectedPlayer.GetComponentInChildren<Camera>().enabled = true;
        Debug.Log(connectedPlayer.GetComponentInChildren<Camera>());

        connectedPlayer.GetComponentInChildren<Camera>().GetComponent<AudioListener>().enabled = true;
    }

    public void BackToLooby()
    {
        PhotonNetwork.LeaveRoom();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }

    void DisplayPlayers()
    {
        TxtPlayerList.text = null;
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            TxtPlayerList.text += player.NickName + "\n";
        }
    }

    public void UpdateListOfPlayers()
    {
        TxtPlayerList.text = null;
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            TxtPlayerList.text += player.NickName + "\t Score: " + player.GetScore() + "\n";
        }
    }

    void OnPhotonPlayerConnected()
    {
        DisplayPlayers();
    }

    void OnPhotonPlayerDisconnected()
    {
        DisplayPlayers();
    }
}
