using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.WSA;
using UnityEditor.TerrainTools;

public class DiaryControler : MonoBehaviour
{
    public List<Vector3> diaryPhotoPlace;
    public int diaryPage;
    public List<GameObject> diaryPhotos;

    public List<Sprite> PhotosPage1;
    public List<Sprite> MainPagePhotos;


    private int start = 0;

    private int end = 4;

    string folderName;

    private void Start()
    {
        
    }
    private void ShowDiaryPage(List<Sprite> list)
    {
        GameObject main = FindChildGameObjectByName(gameObject, folderName+"Page");
        Debug.Log(folderName + "Page");
        FindChildGameObjectByName(main, "image1").GetComponent<Image>().sprite = list[start];
        FindChildGameObjectByName(main, "image2").GetComponent<Image>().sprite = list[start + 1];
        FindChildGameObjectByName(main, "image3").GetComponent<Image>().sprite = list[start + 2];
        FindChildGameObjectByName(main, "image4").GetComponent<Image>().sprite = list[start + 3];
    }
    private void ShowFirstDiaryPage(List<Sprite> list)
    {
        GameObject main = FindChildGameObjectByName(gameObject, "FirstPage");
        Debug.Log(folderName + "Page");
        FindChildGameObjectByName(main, "Image").GetComponent<Image>().sprite = list[0];
    }

    public void SwitchDiaryPage(int num)
    {
        ImportPhotos(folderName);
        Debug.Log(PhotosPage1.Count);
        Debug.Log("Page number: " + diaryPage);
        if (num == 0)
        {
            diaryPage++;
            if(diaryPage > 4)
            {
                diaryPage = 1;
            }
        }
        else
        {
            diaryPage--;
            if(diaryPage < 1)
            {
                diaryPage = 1;
            }
        }
        if (diaryPage == 1)
        {
            start = 0;
            ShowDiaryPage(PhotosPage1);
        }
        else
        {
            start = diaryPage * 4 - 3;
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
    public void ImportPhotos(string folder)
    {
        folderName = folder;
        Object[] sprite = Resources.LoadAll("gamepics/" + folder, typeof(Sprite));
        foreach (Sprite t in sprite)
        {
            PhotosPage1.Add(t);
        }
       
    }

    private void ImportFirstPhoto(string folder)
    {
        folderName = folder;
        Object[] sprite = Resources.LoadAll("gamepics/" + folder, typeof(Sprite));
       
        if(sprite.Length >= 1)
        {
            MainPagePhotos.Add((Sprite)sprite[0]);
        }
        else
        {
            Debug.LogError("No photos yet");
        }

    }

    public void FirstPage(string folderName)
    {
        //UnimportPhotos();
        ImportFirstPhoto(folderName);
        ShowFirstDiaryPage(MainPagePhotos);
        diaryPage = 1;
    }

    public void Album(string folderName)
    {
        //UnimportPhotos();
        ImportPhotos(folderName);
        ShowDiaryPage(PhotosPage1);
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


    private void UnimportPhotos()
    {
        Object[] sprite = Resources.LoadAll("gamepics/" + folderName, typeof(Sprite));
        foreach (Sprite t in PhotosPage1)
        {
            PhotosPage1.Remove(t);
        }
    }
}
