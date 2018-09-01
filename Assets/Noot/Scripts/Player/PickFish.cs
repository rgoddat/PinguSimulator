using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickFish : MonoBehaviour
{
    public AudioClip SoundFishPick;
    private ScoreManager scoreManager;
    private PhotonView view;

    private void Start()
    {
        scoreManager = GetComponent<ScoreManager>();
        view = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("collision with" + collision.gameObject.name);
        if(collision.gameObject.tag == "Fish")
        {
            collision.GetComponent<AudioSource>().PlayOneShot(SoundFishPick);
            if (view.isMine)
            {
                view.RPC("DestroyGOMasterClient", PhotonTargets.MasterClient, collision.gameObject.name);
                scoreManager.AddScore();
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
        GameObject goToDestroy = GameObject.Find(obj);
        goToDestroy.tag = "Untagged";
        StartCoroutine(DestroyGOAfterDelay(0.5f, goToDestroy));
        
    }

    [PunRPC]
    void UpdateListScoreForAllPlayers()
    {
        //GameObject.Find("NetworkManager").GetComponent<NetworkScript>().UpdateListOfPlayers();
    }

    [PunRPC]
    void EndOfGame()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
}
