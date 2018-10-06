using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CombatController : MonoBehaviour {

    public float FireRate = 1f, ThrowForce = 500f;
    public GameObject Head;
    public GameObject Eject;
    public GameObject SnowballPrefab;
    public GameObject FishEject;
    public GameObject FishPrefab;
    public AudioClip SoundShot;

    private PhotonView view;
    private AudioSource audioSource;
    private float nextShot;

    // Use this for initialization
    void Start () {
        view = GetComponent<PhotonView>();
        audioSource = GetComponent<AudioSource>();
	}

    private void FixedUpdate()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1") && Time.time > nextShot && view.isMine)
        {
            nextShot = Time.time + FireRate;
            audioSource.PlayOneShot(SoundShot);
            view.RPC("ShootSnowball", PhotonTargets.All, Eject.transform.position, Head.transform.TransformDirection(Vector3.forward));
        }
    }

    public void DropFish(PhotonPlayer owner)
    {
        Debug.Log("DropFish ?");
        if (owner.GetScore() > 0 && owner.ID == view.ownerId)
        {
            Debug.Log("Yes, drop plz" + view.owner.NickName);
            owner.AddScore(-1);
            view.RPC("DropFishMasterClient", PhotonTargets.MasterClient, FishEject.transform.position, transform.TransformDirection(Vector3.back));
            view.RPC("UpdateListScoreForAllPlayers", PhotonTargets.All);
        }
    }

    [PunRPC]
    void ShootSnowball(Vector3 pos, Vector3 dir)
    {
        GameObject snowball;
        snowball = Instantiate(SnowballPrefab, pos, Quaternion.identity);
        snowball.GetComponent<Rigidbody>().AddForce(dir * ThrowForce);
    }

    [PunRPC]
    void DropFishMasterClient(Vector3 pos, Vector3 dir)
    {
        Debug.Log("Instantiating new fish");
        GameObject fish;
        fish = PhotonNetwork.Instantiate(FishPrefab.name, pos, Quaternion.identity, 0);
        fish.GetComponent<Rigidbody>().isKinematic = false;
        fish.GetComponent<Rigidbody>().AddForce(dir * 200f);
        fish.GetComponent<BoxCollider>().isTrigger = false;
    }
}
