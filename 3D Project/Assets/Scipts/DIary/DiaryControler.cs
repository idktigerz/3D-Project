using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class DiaryControler : MonoBehaviour
{
    public List<Vector3> diaryPhotoPlace;
    public int diaryPage;
    public List<Sprite> MainPagePhotos;
    public List<GameObject> diaryPhotos;


    public List<Sprite> PhotosPage1;


    private int start = 0;

    private int end = 4;



    private void Start()
    {
        ImportPhotos();
        ShowDiaryPage(PhotosPage1);
        diaryPage = 1;


        diaryPhotos[0].GetComponent<Image>().sprite = PhotosPage1[start];
        diaryPhotos[1].GetComponent<Image>().sprite = PhotosPage1[start + 1];
        diaryPhotos[2].GetComponent<Image>().sprite = PhotosPage1[start + 2];
        diaryPhotos[3].GetComponent<Image>().sprite = PhotosPage1[start + 3];


    }
    private void ShowDiaryPage(List<Sprite> list)
    {

        diaryPhotos[0].GetComponent<Image>().sprite = list[start];
        diaryPhotos[1].GetComponent<Image>().sprite = list[start + 1];
        diaryPhotos[2].GetComponent<Image>().sprite = list[start + 2];
        diaryPhotos[3].GetComponent<Image>().sprite = list[start + 3];
    }

    public void SwitchDiaryPage(int num)
    {
        if (num == 0)
        {
            diaryPage++;
        }
        else
        {
            diaryPage--;
        }
        if (diaryPage == 1)
        {
            start = 0;
            end = 4;
            ShowDiaryPage(PhotosPage1);
        }
        else
        {
            start = diaryPage * 4 - 4;
            ShowDiaryPage(PhotosPage1);
            Debug.Log(diaryPage);
            Debug.Log(start);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SwitchDiaryPage(1);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            SwitchDiaryPage(0);
        }
    }

    private void ImportPhotos()
    {
        Object[] sprite = Resources.LoadAll("gamepics", typeof(Sprite));
        foreach (Sprite t in sprite)
        {
            PhotosPage1.Add(t);
        }

    }

}
