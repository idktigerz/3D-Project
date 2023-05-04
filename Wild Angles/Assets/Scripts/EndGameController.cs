using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameController : MonoBehaviour
{
    public TextMeshProUGUI scoreText; 
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        int score = PlayerPrefs.GetInt("points");
        if(score < 10000)
        {
            scoreText.text = "Bad job! \nYou scored: " + score;
        }else if(score > 10000 && score < 20000)
        {
            scoreText.text = "OK job! \nYou scored: " + score;
        }else if (score > 20000)
        {
            scoreText.text = "Good job! \nYou scored: " + score;
        }
    }
}
