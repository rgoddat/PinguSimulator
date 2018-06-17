using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickFish : MonoBehaviour
{
    public AudioClip SoundFishPick;
    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = GameObject.Find("FPSController").GetComponent<ScoreManager>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("collision with" + collision.gameObject.name);
        if(collision.gameObject.tag == "Fish")
        {
            StartCoroutine(DestroyFish(collision.gameObject));
        }
    }

    private IEnumerator DestroyFish(GameObject fish)
    {
        fish.GetComponent<AudioSource>().PlayOneShot(SoundFishPick);
        scoreManager.AddScore();
        yield return new WaitForSeconds(0.5f);
        Destroy(fish);
    }
}
