using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CombatController : MonoBehaviour {

    public float FireRate = 1f, ThrowForce = 500f;
    public GameObject Eject;
    public GameObject SnowballPrefab;
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
        if (CrossPlatformInputManager.GetButtonDown("Fire1") && Time.time > nextShot)
        {
            nextShot = Time.time + FireRate;
            audioSource.PlayOneShot(SoundShot);
            view.RPC("ShootSnowball", PhotonTargets.All, Eject.transform.position, transform.TransformDirection(Vector3.forward));
        }
    }

    [PunRPC]
    void ShootSnowball(Vector3 pos, Vector3 dir)
    {
        GameObject snowball;
        snowball = Instantiate(SnowballPrefab, pos, Quaternion.identity);
        snowball.GetComponent<Rigidbody>().AddForce(dir * ThrowForce);
    }
}
