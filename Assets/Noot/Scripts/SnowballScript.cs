using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballScript : MonoBehaviour {

    public float LifeSpan = 1f;
    public AudioClip SoundHit;

    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        audioSource.PlayOneShot(SoundHit);
        Destroy(gameObject, LifeSpan);
    }
}
