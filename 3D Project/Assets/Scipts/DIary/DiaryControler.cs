using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryControler : MonoBehaviour
{
    public List<Vector3> diaryPhotoPlace;
    public int diaryPage;
    public List<GameObject> diaryPhotos;

    public List<Sprite> PhotosPage1;
    public List<Sprite> PhotosPage2;



    private void Start()
    {
        ShowDiaryPage(PhotosPage1);
        diaryPage = 1;
        diaryPhotoPlace.Add(new Vector3(427, 0, 0));
        diaryPhotoPlace.Add(new Vector3(169, 0, 0));
        diaryPhotoPlace.Add(new Vector3(-96, 0, 0));
        diaryPhotoPlace.Add(new Vector3(-355, 0, 0));
        for (int i = 0; i < diaryPhotoPlace.Count; i++)
        {
            diaryPhotos[i].GetComponent<Image>().sprite = PhotosPage1[i];

        }
    }
    private void ShowDiaryPage(List<Sprite> list)
    {

        for (int i = 0; i < diaryPhotoPlace.Count; i++)
        {
            diaryPhotos[i].GetComponent<Image>().sprite = list[i];

        }
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
            ShowDiaryPage(PhotosPage1);
        }
        else
        {
            ShowDiaryPage(PhotosPage2);
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


}
