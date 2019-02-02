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

    public Transform PanelRooms;
    public GameObject RoomDetailsPrefab;

    private List<GameObject> roomsList;

	// Use this for initialization
	void Start () {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        roomsList = new List<GameObject>();
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

        //Maybe update current rooms and add new ones
        //Clear the UI list
        foreach(GameObject roomObject in roomsList)
        {
            Destroy(roomObject);
        }

        //Populate with new infos
        foreach(RoomInfo roomInfo in PhotonNetwork.GetRoomList())
        {
            var detailRoom = Instantiate(RoomDetailsPrefab);
            detailRoom.transform.parent = PanelRooms;
            detailRoom.GetComponent<DetailRoom>().UpdateUI(roomInfo);
            roomsList.Add(detailRoom);
        }
    }
}
