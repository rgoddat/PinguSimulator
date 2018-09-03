using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public Text TxtScore;

    private float score = 0;

    private void Start()
    {
        TxtScore.text = score.ToString();
    }

    public void AddScore()
    {
        score++;
        TxtScore.text = score.ToString();

    }

}
