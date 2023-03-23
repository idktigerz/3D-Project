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

    string folderName;

    private void Start()
    {
        
    }
    private void ShowDiaryPage(List<Sprite> list)
    {
        GameObject main =FindChildGameObjectByName(gameObject, folderName+"Page");
        FindChildGameObjectByName(main, "image1").GetComponent<Image>().sprite = list[start];
        FindChildGameObjectByName(main, "image2").GetComponent<Image>().sprite = list[start + 1];
        FindChildGameObjectByName(main, "image3").GetComponent<Image>().sprite = list[start + 2];
        FindChildGameObjectByName(main, "image4").GetComponent<Image>().sprite = list[start + 3];
    }

    public void SwitchDiaryPage(int num)
    {
        Debug.Log(PhotosPage1.Count);
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
    private void ImportPhotos(string folder)
    {
        folderName = folder;
        Object[] sprite = Resources.LoadAll("gamepics/" + folder, typeof(Sprite));
        foreach (Sprite t in sprite)
        {
            PhotosPage1.Add(t);
        }
       
    }

    private void FirstPage(string folderName)
    {
        ImportPhotos(folderName);
        ShowDiaryPage(PhotosPage1);
        diaryPage = 1;
    }

    private GameObject FindChildGameObjectByName(GameObject topParentObject, string gameObjectName)
    {
        for (int i = 0; i < topParentObject.transform.childCount; i++)
        {
            if (topParentObject.transform.GetChild(i).name == gameObjectName)
            {
                return topParentObject.transform.GetChild(i).gameObject;
            }

            GameObject tmp = FindChildGameObjectByName(topParentObject.transform.GetChild(i).gameObject, gameObjectName);

            if (tmp != null)
            {
                return tmp;
            }
        }
        return null;
    }

}
