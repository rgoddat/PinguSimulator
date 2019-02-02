using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Text TxtRoom;
    public Text TxtPlayerList;
    public Text TxtScore;
    public Text TxtWaitForPlayer;
    public Text TxtTimer;
    public GameObject PlayerPrefab;
    public GameObject MainCamera;
    public Transform SpawnPoint;

    private PhotonView view;
    private float elapsedTime = 0;
    private bool playing = false;

    private const int MIN_PLAYER_COUNT = 1;

    //Time limit in seconds
    private const float TIME_LIMIT = 10*60.0f;

    

    // Use this for initialization
    void Start()
    {
        view = GetComponent<PhotonView>();
        OnJoinedRoom();
    }

    public void BackToLooby()
    {
        PhotonNetwork.LeaveRoom();
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            elapsedTime += Time.deltaTime;

            //Update time counter on screen
            TxtTimer.text = TimeToStringMinSec(TIME_LIMIT - elapsedTime);

            if(elapsedTime > TIME_LIMIT)
            {
                //End the game session
                PhotonNetwork.LoadLevel("Lobby");
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
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

        if (PhotonNetwork.isMasterClient)
        {
            view.RPC("SynchronizeTime",PhotonTargets.All ,elapsedTime);
        } else
        {
            view.RPC("AskTimeSynchronization", PhotonTargets.MasterClient);
        }
        TxtWaitForPlayer.text = "";

        if (!playing)
        {
            //Spawnpoint
            Vector3 sp = new Vector3(SpawnPoint.transform.position.x + Random.Range(-150.0f, 150.0f), SpawnPoint.transform.position.y, SpawnPoint.transform.position.z + Random.Range(-75.0f, 75.0f));

            GameObject MyPlayer;

            MyPlayer = PhotonNetwork.Instantiate(PlayerPrefab.name, sp, Quaternion.identity, 0);
            MyPlayer.GetComponent<FirstPersonController>().enabled = true;
            MyPlayer.GetComponentInChildren<Camera>().enabled = true;

            MyPlayer.GetComponentInChildren<Camera>().GetComponent<AudioListener>().enabled = true;

            UpdateListOfPlayers();
            playing = true;

        }

        
    }

    [PunRPC]
    void SynchronizeTime (float time)
    {
        this.elapsedTime = time;
    }

    [PunRPC]
    void AskTimeSynchronization()
    {
        view.RPC("SynchronizeTime", PhotonTargets.All, elapsedTime);
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

    private string TimeToStringMinSec(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);

        int seconds = Mathf.FloorToInt(time % 60);

        if(seconds == 60)
        {
            Debug.LogError("seconds");
            seconds = 0;
        }

        return string.Format("{0:00}:{1:00}",minutes, seconds);
    }
}
