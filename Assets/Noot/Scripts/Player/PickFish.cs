using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickFish : MonoBehaviour
{
    public AudioClip SoundFishPick;
    private PhotonView view;
    private Text txtScore;

    [SerializeField]
    private int score = 0;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        txtScore = GetComponentInChildren<Text>();
    }

    public void PickAFish(GameObject fish)
    {
        if (view.isMine)
        {
            view.RPC("DestroyGOMasterClient", PhotonTargets.MasterClient, fish.name);
            score++;
            PhotonNetwork.player.SetScore(score);
            txtScore.text = PhotonNetwork.player.GetScore().ToString();
            view.RPC("UpdateListScoreForAllPlayers", PhotonTargets.All);
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
        GameObject.Find("GameManager").GetComponent<GameManager>().UpdateListOfPlayers();
    }
    
}
