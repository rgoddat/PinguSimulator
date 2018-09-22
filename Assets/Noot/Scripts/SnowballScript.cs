using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballScript : MonoBehaviour {

    public float LifeSpan = 1f;
    public AudioClip SoundHit;

    private AudioSource audioSource;
    private bool isActive = true;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        audioSource.PlayOneShot(SoundHit);
        
        if(collision.gameObject.tag == "Player" && isActive)
        {
            Debug.Log("Collision on player");
            collision.gameObject.GetComponent<CombatController>().DropFish(collision.gameObject.GetComponent<PhotonView>().owner);
            isActive = false;
        }
        Destroy(gameObject, LifeSpan);
        
    }
}
