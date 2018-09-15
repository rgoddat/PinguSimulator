using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickFish : MonoBehaviour
{
    public AudioClip SoundFishPick;
    private ScoreManager scoreManager;
    private PhotonView view;
    private Text txtScore;

    [SerializeField]
    private int score = 0;

    private void Start()
    {
        scoreManager = GetComponent<ScoreManager>();
        view = GetComponent<PhotonView>();
        txtScore = GetComponentInChildren<Text>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("collision with" + collision.gameObject.name);
        if(collision.gameObject.tag == "Fish")
        {
            //collision.gameObject.GetComponent<AudioSource>().PlayOneShot(SoundFishPick);
            if (view.isMine)
            {
                view.RPC("DestroyGOMasterClient", PhotonTargets.MasterClient, collision.gameObject.name);
                score++;
                PhotonNetwork.player.SetScore(score);
                txtScore.text = null;
                txtScore.text = PhotonNetwork.player.GetScore().ToString();
                view.RPC("UpdateListScoreForAllPlayers", PhotonTargets.All);
            }
        }
    }

    IEnumerator DestroyGOAfterDelay(float delay, GameObject goToDestroy)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.Destroy(goToDestroy);
    }

    [PunRPC]
    void DestroyGOMasterClient(string obj)
    {
        if (GameObject.Find("Fishes").transform.childCount == 1)
        {
            view.RPC("EndOfGame", PhotonTargets.All);
        }
        Debug.Log("RPC Received, destroy " + obj);
        view.RPC("PlayPickSoundForAll", PhotonTargets.All, obj);
        GameObject goToDestroy = GameObject.Find(obj);
        goToDestroy.tag = "Untagged";
        StartCoroutine(DestroyGOAfterDelay(0.5f, goToDestroy));
        
    }

    [PunRPC]
    void PlayPickSoundForAll(string obj)
    {
        GameObject goPicked = GameObject.Find(obj);
        goPicked.GetComponent<AudioSource>().PlayOneShot(SoundFishPick);
    }

    [PunRPC]
    void UpdateListScoreForAllPlayers()
    {
        Debug.Log("UpdatingList");
        GameObject.Find("GameManager").GetComponent<GameManager>().UpdateListOfPlayers();
    }

    [PunRPC]
    void EndOfGame()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
}
