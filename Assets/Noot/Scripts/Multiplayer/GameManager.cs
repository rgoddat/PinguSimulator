using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Text TxtRoom;
    public Text TxtPlayerList;
    public Text TxtScore;
    public Text TxtWaitForPlayer;
    public GameObject PlayerPrefab;
    public GameObject MainCamera;
    public Transform SpawnPoint;

    private const int MIN_PLAYER_COUNT = 2;

    // Use this for initialization
    void Start()
    {
        OnJoinedRoom();
        //GameObject connectedPlayer = PhotonNetwork.Instantiate(PlayerPrefab.name, SpawnPoint.position, Quaternion.identity, 0);
        //connectedPlayer.GetComponent<FirstPersonController>().enabled = true;
        //connectedPlayer.GetComponentInChildren<Camera>().enabled = true;

        //connectedPlayer.GetComponentInChildren<Camera>().GetComponent<AudioListener>().enabled = true;
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

    void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        TxtRoom.text = PhotonNetwork.player.NickName + " you are in room: " + PhotonNetwork.room.Name;
        UpdateListOfPlayers();
        
        if (PhotonNetwork.room.PlayerCount < MIN_PLAYER_COUNT)
        {
            TxtWaitForPlayer.text = string.Format("Waiting for {0} more players to start", (MIN_PLAYER_COUNT - PhotonNetwork.room.PlayerCount));// + "/" + PhotonNetwork.room.MaxPlayers);
            return;
        }
        TxtWaitForPlayer.text = "";
        

        //Spawnpoint
        Vector3 sp = new Vector3(SpawnPoint.transform.position.x + Random.Range(-1.0f, 1.0f), SpawnPoint.transform.position.y, SpawnPoint.transform.position.z + Random.Range(-1.0f, 1.0f));

        sp = SpawnPoint.transform.position;

        GameObject MyPlayer;

        MyPlayer = PhotonNetwork.Instantiate(PlayerPrefab.name, sp, Quaternion.identity, 0);
        MyPlayer.GetComponent<FirstPersonController>().enabled = true;
        MyPlayer.GetComponentInChildren<Camera>().enabled = true;

        MyPlayer.GetComponentInChildren<Camera>().GetComponent<AudioListener>().enabled = true;

        UpdateListOfPlayers();
    }

    void OnPhotonPlayerConnected()
    {
        OnJoinedRoom();
        UpdateListOfPlayers();
    }

    void OnPhotonPlayerDisconnected()
    {
        UpdateListOfPlayers();
    }

    public void UpdateListOfPlayers()
    {
        TxtPlayerList.text = null;
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            TxtPlayerList.text += player.NickName + "\t Score: " + player.GetScore() + "\n";
        }
    }
}
