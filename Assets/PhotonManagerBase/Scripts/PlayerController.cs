using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float Speed = 0.1f;
    private PhotonView view;

    // Use this for initialization
    void Start () {
        view = GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
	void Update () {
        if (view.isMine)
        {
            Vector3 move = new Vector3();

            if (Input.GetKey(KeyCode.UpArrow))
            {
                move.z += Speed;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                move.z -= Speed;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                move.x -= Speed;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                move.x += Speed;
            }

            transform.position += move;
        }
	}
}
