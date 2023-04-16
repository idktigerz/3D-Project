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
    public List<Texture2D> crocodilePhotos;
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

    private int start = 0;

    private int end = 4;

    string folderName;

    private void Start()
    {

    }
    private void ShowDiaryPage(List<Texture2D> list)
    {
        GameObject main = FindChildGameObjectByName(gameObject, folderName + "Page");
        Debug.Log(folderName + "Page");

        //FindChildGameObjectByName(main, "image1").GetComponent<Image>().sprite = null;
        //FindChildGameObjectByName(main, "image2").GetComponent<Image>().sprite = null;
        //FindChildGameObjectByName(main, "image3").GetComponent<Image>().sprite = null;
        //FindChildGameObjectByName(main, "image4").GetComponent<Image>().sprite = null;


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
            Debug.LogError("No photos");
        }

        /*FindChildGameObjectByName(main, "image1").GetComponent<Image>().sprite = list[start];
        FindChildGameObjectByName(main, "image2").GetComponent<Image>().sprite = list[start + 1];
        FindChildGameObjectByName(main, "image3").GetComponent<Image>().sprite = list[start + 2];
        FindChildGameObjectByName(main, "image4").GetComponent<Image>().sprite = list[start + 3];*/
    }
    private void ShowFirstDiaryPage(List<Texture2D> list)
    {
        GameObject main = FindChildGameObjectByName(gameObject, folderName + "Page");
        GameObject seccond = FindChildGameObjectByName(main, "FirstPage");
        //Debug.Log(folderName + "Page");
        //FindChildGameObjectByName(seccond, "Image").GetComponent<RawImage>().texture = null;
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
        //ImportPhotos(folderName);
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

            start = 0;
            if (folderName == "Crocodile") ShowDiaryPage(crocodilePhotos);
            else if (folderName == "Owl") ShowDiaryPage(owlPhotos);
            else if (folderName == "Butterfly") ShowDiaryPage(butterflyPhotos);
            else if (folderName == "Bug") ShowDiaryPage(bugPhotos);
            else if (folderName == "Frog") ShowDiaryPage(frogPhotos);
            else if (folderName == "WhiteOrchid") ShowDiaryPage(whiteOrchidPhotos);
            else if (folderName == "PurpleOrchid") ShowDiaryPage(purpleOrchidPhotos);
            else if (folderName == "CocoaTree") ShowDiaryPage(cocoaTreePhotos);
            else if (folderName == "BananaTree") ShowDiaryPage(bananaTreePhotos);

        }
        else
        {
            start = diaryPage * 4 - 3;
            if (folderName == "Crocodile") ShowDiaryPage(crocodilePhotos);
            else if (folderName == "Owl") ShowDiaryPage(owlPhotos);
            else if (folderName == "Butterfly") ShowDiaryPage(butterflyPhotos);
            else if (folderName == "Bug") ShowDiaryPage(bugPhotos);
            else if (folderName == "Frog") ShowDiaryPage(frogPhotos);
            else if (folderName == "WhiteOrchid") ShowDiaryPage(whiteOrchidPhotos);
            else if (folderName == "PurpleOrchid") ShowDiaryPage(purpleOrchidPhotos);
            else if (folderName == "CocoaTree") ShowDiaryPage(cocoaTreePhotos);
            else if (folderName == "BananaTree") ShowDiaryPage(bananaTreePhotos);

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

    public void FirstPage(string folder)
    {
        folderName = folder;
        //ImportFirstPhoto(folderName);
        if (folderName == "Crocodile") ShowFirstDiaryPage(crocodilePhotos);
        else if (folderName == "Owl") ShowFirstDiaryPage(owlPhotos);
        else if (folderName == "Butterfly") ShowFirstDiaryPage(butterflyPhotos);
        else if (folderName == "Bug") ShowFirstDiaryPage(bugPhotos);
        else if (folderName == "Frog") ShowFirstDiaryPage(frogPhotos);
        else if (folderName == "WhiteOrchid") ShowFirstDiaryPage(whiteOrchidPhotos);
        else if (folderName == "PurpleOrchid") ShowFirstDiaryPage(purpleOrchidPhotos);
        else if (folderName == "CocoaTree") ShowFirstDiaryPage(cocoaTreePhotos);
        else if (folderName == "BananaTree") ShowFirstDiaryPage(bananaTreePhotos);

        diaryPage = 1;
    }

    public void Album(string folder)
    {
        folderName = folder;
        Debug.Log($"passei aqui");
        if (folderName == "Crocodile") ShowDiaryPage(crocodilePhotos);
        else if (folderName == "Owl") ShowDiaryPage(owlPhotos);
        else if (folderName == "Butterfly") ShowDiaryPage(butterflyPhotos);
        else if (folderName == "Bug") ShowDiaryPage(bugPhotos);
        else if (folderName == "Frog") ShowDiaryPage(frogPhotos);
        else if (folderName == "WhiteOrchid") ShowDiaryPage(whiteOrchidPhotos);
        else if (folderName == "PurpleOrchid") ShowDiaryPage(purpleOrchidPhotos);
        else if (folderName == "CocoaTree") ShowDiaryPage(cocoaTreePhotos);
        else if (folderName == "BananaTree") ShowDiaryPage(bananaTreePhotos);
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

    public void Back()
    {
        diaryPage = 0;
        start = 0;
    }

}
