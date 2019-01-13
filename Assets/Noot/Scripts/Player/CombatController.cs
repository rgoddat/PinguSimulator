using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField]
    private List<GameObject> ennemiesInNootRange;

    // Use this for initialization
    void Start () {
        view = GetComponent<PhotonView>();
        audioSource = GetComponent<AudioSource>();
        ennemiesInNootRange = new List<GameObject>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (view.isMine)
        {
            if (other.gameObject.tag == "Player" && other.gameObject != this.gameObject)
            {
                if (!ennemiesInNootRange.Contains(other.gameObject))
                {
                    ennemiesInNootRange.Add(other.gameObject);
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player" && view.isMine)
        {
            Debug.Log("Removing target");
            ennemiesInNootRange.Remove(other.gameObject);
        }
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
        if (owner.GetScore() > 0 && owner.ID == view.ownerId)
        {
            owner.AddScore(-1);
            view.RPC("DropFishMasterClient", PhotonTargets.MasterClient, FishEject.transform.position, transform.TransformDirection(Vector3.back));
            view.RPC("UpdateListScoreForAllPlayers", PhotonTargets.All);
        }
    }

    public void Noot()
    {
        if (ennemiesInNootRange.Any() && view.isMine)
        {
            var listPlayerToStun = new List<int>();
            foreach (GameObject stunnedPlayer in ennemiesInNootRange)
            {
                listPlayerToStun.Add(stunnedPlayer.GetComponent<PhotonView>().ownerId);
                stunnedPlayer.GetPhotonView().RPC("StunForAMoment", PhotonTargets.Others, listPlayerToStun.ToArray());
            }
            //Debug.Log("I (" + view.owner.NickName + ") send the RPC");
            //view.RPC("StunForAMoment", PhotonTargets.Others, listPlayerToStun.ToArray());
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

    [PunRPC]
    void StunForAMoment(params int[] listStunnedId)
    {
        foreach (int id in listStunnedId)
        {
            Debug.Log(id + "==?" + view.ownerId);
            if (id == view.ownerId)
            {
                //TODO find the correct player locally
                StartCoroutine(StunCoroutine(id));
            }
        }
    }

    IEnumerator StunCoroutine(int id)
    {

        Debug.Log("I (" + view.owner.NickName + ") am stunned");
        GetComponent<FirstPersonController>().Stunned = true;
        Debug.Log(GetComponent<FirstPersonController>().Stunned);
        yield return new WaitForSeconds(20);
        GetComponent<FirstPersonController>().Stunned = false;
        Debug.Log(GetComponent<FirstPersonController>().Stunned);
    }
}
