using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PUN : MonoBehaviour {

    public Text TxtPhotonInfos;
    public Text TxtRoomList;
    public string SceneToLoad;

    public InputField IfPseudo;
    public InputField IfRoom;

	// Use this for initialization
	void Start () {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (!PhotonNetwork.connected)
        {
            IfPseudo.text = "Player" + Random.Range(1, 400);
            IfRoom.text = "MyRoom";
            PhotonNetwork.ConnectUsingSettings("24ad5231c96d08eb3ac4c7a2977e96f4");
        }
	}
	
	// Update is called once per frame
	void Update () {
        TxtPhotonInfos.text = PhotonNetwork.connectionStateDetailed.ToString();

    }

    public void JoinRoom()
    {
        PhotonNetwork.playerName = IfPseudo.text;
        RoomOptions myRoomOptions = new RoomOptions();
        myRoomOptions.MaxPlayers = 10;
        myRoomOptions.IsVisible = true;

        PhotonNetwork.JoinOrCreateRoom(IfRoom.text, myRoomOptions, TypedLobby.Default);
    }

    public void JoinMainRoom()
    {
        PhotonNetwork.playerName = IfPseudo.text;
        RoomOptions myRoomOptions = new RoomOptions();
        myRoomOptions.MaxPlayers = 10;
        myRoomOptions.IsVisible = true;

        PhotonNetwork.JoinOrCreateRoom("Main", myRoomOptions, TypedLobby.Default);
    }

    void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(SceneToLoad);
        Debug.Log("Entering room");
    }

    void OnPhotonJoinRoomFailed()
    {
        Debug.LogError("Room join failed for reasons");
    }

    void OnReceivedRoomListUpdate()
    {
        Debug.Log("Updating room list");
        TxtRoomList.text = null;
        foreach(RoomInfo roomInfo in PhotonNetwork.GetRoomList())
        {
            TxtRoomList.text += roomInfo.Name + "[" + roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers + "]\n";
        }
    }
}
