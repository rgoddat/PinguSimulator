using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlaySoundOnKeyPressed : MonoBehaviour {

    public AudioClip NootSound;
    public PhotonView view;

	// Use this for initialization
	void Start () {
        view = GetComponent<PhotonView>();	
	}
	
	// Update is called once per frame
	void Update () {
		if (CrossPlatformInputManager.GetButtonDown("Noot") && view.isMine) {
            GetComponent<AudioSource>().PlayOneShot(NootSound);
            GetComponent<CombatController>().Noot();
		}
	}
}
