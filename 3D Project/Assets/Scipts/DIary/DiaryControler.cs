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

    }
    private void ShowDiaryPage(List<Sprite> list)
    {
        for (int i = 0; i < diaryPhotoPlace.Count; i++)
        {
            diaryPhotos[i].SetActive(false);
        }
        diaryPhotoPlace.Add(new Vector3(427, 0, 0));
        diaryPhotoPlace.Add(new Vector3(169, 0, 0));
        diaryPhotoPlace.Add(new Vector3(-96, 0, 0));
        diaryPhotoPlace.Add(new Vector3(-355, 0, 0));
        for (int i = 0; i < diaryPhotoPlace.Count; i++)
        {
            diaryPhotos[i].GetComponent<Image>().sprite = list[i];

        }
    }

    private void SwitchDiaryPage()
    {
        
    }
}
