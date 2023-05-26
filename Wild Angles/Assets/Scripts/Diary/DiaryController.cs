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
    /*public List<Texture2D> crocodilePhotos;
    public List<Texture2D> owlPhotos;
    public List<Texture2D> butterflyPhotos;
    public List<Texture2D> bugPhotos;
    public List<Texture2D> frogPhotos;
    public List<Texture2D> babyTigerPhotos;
    public List<Texture2D> snakePhotos;
    public List<Texture2D> whiteOrchidPhotos;
    public List<Texture2D> purpleOrchidPhotos;
    public List<Texture2D> cocoaTreePhotos;
    public List<Texture2D> bananaTreePhotos;
    public List<Texture2D> helconiaPhotos;
    public List<Texture2D> tigerPhotos;*/
    [Header("Temporary Lists")]
    /*public List<Texture2D> temptigerPhotos;
    public List<Texture2D> tempcrocodilePhotos;
    public List<Texture2D> tempowlPhotos;
    public List<Texture2D> tempbutterflyPhotos;
    public List<Texture2D> tempbugPhotos;
    public List<Texture2D> tempfrogPhotos;
    public List<Texture2D> tempTigerPhotos;
    public List<Texture2D> tempsnakePhotos;
    public List<Texture2D> tempwhiteOrchidPhotos;
    public List<Texture2D> temppurpleOrchidPhotos;
    public List<Texture2D> tempcocoaTreePhotos;
    public List<Texture2D> tempbananaTreePhotos;
    public List<Texture2D> temphelconiaPhotos;
    */

    public List<DiaryData> animalList = new List<DiaryData>();


    private int start = 0;

    private int end = 4;

    string folderName;

    public class DiaryData
    {
        public string type;
        public Texture2D texture;
        public bool saved;

        public DiaryData(string t, Texture2D tex, bool s)
        {
            type = t;
            texture = tex;
            saved = s;
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
    private void ShowFirstDiaryPage(List<Texture2D> list)
    {
        GameObject main = FindChildGameObjectByName(gameObject, folderName + "Page");
        GameObject seccond = FindChildGameObjectByName(main, "FirstPage");
        if (list.Count > 0)
        {
            FindChildGameObjectByName(seccond, "Image").GetComponent<RawImage>().texture = list[0];
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
            /*
            List<Texture2D> tempList = new List<Texture2D>();
            foreach (DiaryData item in animalList)
            {
                if (item.type == folderName)
                {
                    tempList.Add(item.texture);
                }
            }*/

            start = 0;
            ShowDiaryPage(tempList);
            /*if (folderName == "Crocodile") ShowDiaryPage(crocodilePhotos);
            else if (folderName == "Owl") ShowDiaryPage(owlPhotos);
            else if (folderName == "Butterfly") ShowDiaryPage(butterflyPhotos);
            else if (folderName == "Bug") ShowDiaryPage(bugPhotos);
            else if (folderName == "Frog") ShowDiaryPage(frogPhotos);
            else if (folderName == "Snake") ShowDiaryPage(snakePhotos);
            else if (folderName == "Tiger") ShowDiaryPage(tigerPhotos);
            else if (folderName == "WhiteOrchid") ShowDiaryPage(whiteOrchidPhotos);
            else if (folderName == "PurpleOrchid") ShowDiaryPage(purpleOrchidPhotos);
            else if (folderName == "CocoaTree") ShowDiaryPage(cocoaTreePhotos);
            else if (folderName == "BananaTree") ShowDiaryPage(bananaTreePhotos);
            else if (folderName == "Helconia") ShowDiaryPage(helconiaPhotos);
            */


        }
        else
        {
            List<Texture2D> tempList = ReturnList(folderName);
            /*List<Texture2D> tempList = new List<Texture2D>();
            foreach (DiaryData item in animalList)
            {
                if (item.type == folderName)
                {
                    tempList.Add(item.texture);
                }
            }*/
            start = diaryPage * 4 - 3;
            ShowDiaryPage(tempList);
            /*
            if (folderName == "Crocodile") ShowDiaryPage(crocodilePhotos);
            else if (folderName == "Owl") ShowDiaryPage(owlPhotos);
            else if (folderName == "Butterfly") ShowDiaryPage(butterflyPhotos);
            else if (folderName == "Bug") ShowDiaryPage(bugPhotos);
            else if (folderName == "Frog") ShowDiaryPage(frogPhotos);
            else if (folderName == "Snake") ShowDiaryPage(snakePhotos);
            else if (folderName == "Tiger") ShowDiaryPage(tigerPhotos);
            else if (folderName == "WhiteOrchid") ShowDiaryPage(whiteOrchidPhotos);
            else if (folderName == "PurpleOrchid") ShowDiaryPage(purpleOrchidPhotos);
            else if (folderName == "CocoaTree") ShowDiaryPage(cocoaTreePhotos);
            else if (folderName == "BananaTree") ShowDiaryPage(bananaTreePhotos);
            else if (folderName == "Helconia") ShowDiaryPage(helconiaPhotos);
            */
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
        /*List<Texture2D> tempList = new List<Texture2D>();
        foreach (DiaryData item in animalList)
        {
            if (item.type == folder)
            {
                tempList.Add(item.texture);
            }
        }*/
        folderName = folder;
        /*
        if (folderName == "Crocodile") ShowFirstDiaryPage(crocodilePhotos);
        else if (folderName == "Owl") ShowFirstDiaryPage(owlPhotos);
        else if (folderName == "Butterfly") ShowFirstDiaryPage(butterflyPhotos);
        else if (folderName == "Bug") ShowFirstDiaryPage(bugPhotos);
        else if (folderName == "Frog") ShowFirstDiaryPage(frogPhotos);
        else if (folderName == "Snake") ShowFirstDiaryPage(snakePhotos);
        else if (folderName == "Tiger") ShowFirstDiaryPage(tigerPhotos);
        else if (folderName == "WhiteOrchid") ShowFirstDiaryPage(whiteOrchidPhotos);
        else if (folderName == "PurpleOrchid") ShowFirstDiaryPage(purpleOrchidPhotos);
        else if (folderName == "CocoaTree") ShowFirstDiaryPage(cocoaTreePhotos);
        else if (folderName == "BananaTree") ShowFirstDiaryPage(bananaTreePhotos);
        else if (folderName == "Helconia") ShowFirstDiaryPage(helconiaPhotos);
        */
        ShowFirstDiaryPage(tempList);

        diaryPage = 1;
    }

    public void Album(string folder)
    {
        List<Texture2D> tempList = ReturnList(folder);
        /*List<Texture2D> tempList = new List<Texture2D>();
        foreach (DiaryData item in animalList)
        {
            if (item.type == folder)
            {
                tempList.Add(item.texture);
            }
        }*/

        folderName = folder;
        ShowDiaryPage(tempList);
        /*if (folderName == "Crocodile") ShowDiaryPage(crocodilePhotos);
        else if (folderName == "Owl") ShowDiaryPage(owlPhotos);
        else if (folderName == "Butterfly") ShowDiaryPage(butterflyPhotos);
        else if (folderName == "Bug") ShowDiaryPage(bugPhotos);
        else if (folderName == "Frog") ShowDiaryPage(frogPhotos);
        else if (folderName == "Snake") ShowDiaryPage(snakePhotos);
        else if (folderName == "Tiger") ShowDiaryPage(tigerPhotos);
        else if (folderName == "WhiteOrchid") ShowDiaryPage(whiteOrchidPhotos);
        else if (folderName == "PurpleOrchid") ShowDiaryPage(purpleOrchidPhotos);
        else if (folderName == "CocoaTree") ShowDiaryPage(cocoaTreePhotos);
        else if (folderName == "BananaTree") ShowDiaryPage(bananaTreePhotos);
        else if (folderName == "Helconia") ShowDiaryPage(helconiaPhotos);
        */

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
        /*foreach (DiaryData item in animalList)
        {
            if (!item.saved)
            {
                animalList.Remove(item);
            }
        }*/
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
