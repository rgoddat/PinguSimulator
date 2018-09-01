using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {

    public Text TxtRoom;
    public Text TxtPlayerList;
    public GameObject PlayerPrefab;

	// Use this for initialization
	void Start () {
        TxtRoom.text = PhotonNetwork.player.NickName + " you are in room: " + PhotonNetwork.room.Name;
        DisplayPlayers();
        PhotonNetwork.Instantiate(PlayerPrefab.name, Vector3.zero, Quaternion.identity, 0);
	}

    public void BackToLooby()
    {
        PhotonNetwork.LeaveRoom();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }

    void DisplayPlayers()
    {
        TxtPlayerList.text = null;
        foreach(PhotonPlayer player in PhotonNetwork.playerList)
        {
            TxtPlayerList.text += player.NickName + "\n";
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
