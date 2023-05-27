using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;



public class DiaryController : MonoBehaviour
{
    public List<Vector3> diaryPhotoPlace;
    public int diaryPage;
    public List<GameObject> diaryPhotos;
    public List<Sprite> PhotosPage1;
    public List<Sprite> MainPagePhotos;
    public PlayerController playerController;
    public List<DiaryData> animalList = new List<DiaryData>();
    private int start = 0;
    private int end = 4;
    string folderName;
    public class DiaryData
    {
        public string type;
        public Texture2D texture;
        public bool saved;
        public int score;

        public DiaryData(string t, Texture2D tex, bool s, int sc)
        {
            type = t;
            texture = tex;
            saved = s;
            score = sc;
        }
    }
    private void Start()
    {

    }
    private void ShowDiaryPage(List<Texture2D> list)
    {
        GameObject main = FindChildGameObjectByName(gameObject, folderName + "Page");

        if (list.Count == 1)
        {
            FindChildGameObjectByName(main, "image1").GetComponent<RawImage>().texture = list[start];

        }
        else if (list.Count == 2)
        {
            FindChildGameObjectByName(main, "image1").GetComponent<RawImage>().texture = list[start];
            FindChildGameObjectByName(main, "image2").GetComponent<RawImage>().texture = list[start + 1];
        }
        else if (list.Count == 3)
        {
            FindChildGameObjectByName(main, "image1").GetComponent<RawImage>().texture = list[start];
            FindChildGameObjectByName(main, "image2").GetComponent<RawImage>().texture = list[start + 1];
            FindChildGameObjectByName(main, "image3").GetComponent<RawImage>().texture = list[start + 2];
        }
        else if (list.Count >= 4)
        {
            FindChildGameObjectByName(main, "image1").GetComponent<RawImage>().texture = list[start];
            FindChildGameObjectByName(main, "image2").GetComponent<RawImage>().texture = list[start + 1];
            FindChildGameObjectByName(main, "image3").GetComponent<RawImage>().texture = list[start + 2];
            FindChildGameObjectByName(main, "image4").GetComponent<RawImage>().texture = list[start + 3];
        }
        else
        {
            FindChildGameObjectByName(main, "image1").GetComponent<RawImage>().texture = null;
            FindChildGameObjectByName(main, "image2").GetComponent<RawImage>().texture = null;
            FindChildGameObjectByName(main, "image3").GetComponent<RawImage>().texture = null;
            FindChildGameObjectByName(main, "image4").GetComponent<RawImage>().texture = null;
            Debug.LogError("No photos");
        }
    }
    private void ShowFirstDiaryPage(List<Texture2D> list, string name)
    {
        GameObject main = FindChildGameObjectByName(gameObject, folderName + "Page");
        GameObject seccond = FindChildGameObjectByName(main, "FirstPage");
        if (list.Count > 0)
        {
            Texture2D chosen = null;
            List<DiaryData> temp = new List<DiaryData>();
            int score = 4;
            foreach (var item in animalList)
            {
                if (item.type == name)
                {
                    temp.Add(item);
                }
            }
            foreach (var item in temp)
            {
                if (item.score < score)
                {
                    chosen = item.texture;
                }
            }
            FindChildGameObjectByName(seccond, "Image").GetComponent<RawImage>().texture = chosen;
        }
        else
        {
            FindChildGameObjectByName(seccond, "Image").GetComponent<RawImage>().texture = null;
        }
    }
    public void ResetStart()
    {
        start = 0;
        diaryPage = 1;
    }

    public void SwitchDiaryPage(int num)
    {
        if (num == 0)
        {
            diaryPage++;
            if (diaryPage > end)
            {
                diaryPage = 1;
            }
        }
        else
        {
            diaryPage--;
            if (diaryPage < 1)
            {
                diaryPage = 1;
            }
        }
        if (diaryPage == 1)
        {
            List<Texture2D> tempList = ReturnList(folderName);


            start = 0;
            ShowDiaryPage(tempList);
        }
        else
        {
            List<Texture2D> tempList = ReturnList(folderName);

            start = diaryPage * 4 - 3;
            ShowDiaryPage(tempList);
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
    public void FirstPage(string folder)
    {
        List<Texture2D> tempList = ReturnList(folder);

        folderName = folder;

        ShowFirstDiaryPage(tempList, folder);

        diaryPage = 1;
    }
    public void Album(string folder)
    {
        List<Texture2D> tempList = ReturnList(folder);


        folderName = folder;
        ShowDiaryPage(tempList);


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
    private List<Texture2D> ReturnList(string folder)
    {
        List<Texture2D> tempList = new List<Texture2D>();
        foreach (DiaryData item in animalList)
        {
            if (item.type == folder)
            {
                tempList.Add(item.texture);
            }
        }
        return tempList;
    }
    public void DeleteUnsavedPhotos()
    {

        for (int i = 0; i < animalList.Count; i++)
        {
            if (!animalList[i].saved)
            {
                animalList.Remove(animalList[i]);
                i--;
            }
        }
    }
    public void SavePhotos()
    {
        foreach (DiaryData item in animalList)
        {
            if (!item.saved)
            {
                item.saved = true;
            }
        }
    }
    public void Back()
    {
        diaryPage = 0;
        start = 0;
    }
}
