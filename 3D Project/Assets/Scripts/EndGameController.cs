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
        if(playerController.points < 10000)
        {
            scoreText.text = "Bad job! \nYou scored: " + playerController.points;
        }else if(playerController.points > 10000 && playerController.points < 20000)
        {
            scoreText.text = "OK job! \nYou scored: " + playerController.points;
        }else if (playerController.points > 20000)
        {
            scoreText.text = "Good job! \nYou scored: " + playerController.points;
        }
    }

    
}
