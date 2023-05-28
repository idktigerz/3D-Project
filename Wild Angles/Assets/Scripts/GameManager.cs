using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public PlayerController playerController;
    public TimeController timeController;
    public GameObject player;
    public GameObject playerUI;
    public GameObject tent;
    public DiaryController diaryController;


    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
        if (timeController != null)
        {
            if (timeController.dayCounter == 7)
            {
                PlayerPrefs.SetInt("points", playerController.points);
                Endgame();
            }
        }
    }

    private void ProcessInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf == true)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
                playerUI.SetActive(true);
                
            }
            else
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                playerUI.SetActive(false);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                playerController.cameraON = false;
                
            }
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }

    public void ContinueGame()
    {
        Debug.Log("Continue game");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Endgame()
    {
        SceneManager.LoadScene("EndGame");
    }

    public void RespawnPlayer()
    {
        playerController.health = 100;
        timeController.currentTime += timeController.midDayTime;
        timeController.dayCounter = playerController.lastDaySaved;
        player.transform.position = tent.transform.position;
        diaryController.DeleteUnsavedPhotos();
    }
}
