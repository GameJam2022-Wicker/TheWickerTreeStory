using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutScene : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    public List<Sprite> imageList;
    [SerializeField]
    private GameObject boy;
    [SerializeField]
    private GameObject buttons;
    public GameObject StartBackground;

    public int imageNum;   // 이미지 개수
    private int idx;      // 현재 이미지

    // Start is called before the first frame update
    void Start()
    {
        imageNum=imageList.Count;
        image.sprite = imageList[idx];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.F))
            ChangeImage();
    }

    public void ChangeImage()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (StartBackground.activeSelf)
            {
                StartBackground.SetActive(false);
                return;
            }
        }
            
        idx++;
        if (idx == imageNum)
        {
            if (SceneManager.GetActiveScene().buildIndex == 5)
                buttons.SetActive(true);
            else
                GameObject.Find("FadeManager").GetComponent<FadeManager>().StartCoroutine("FadeOutCoroutine");
        }
            
        else if (idx < imageNum)
        {
            image.sprite = imageList[idx];


            // 특정 조건에 발생하는 이벤트 체크
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                if (idx == 1)
                    boy.SetActive(true);
                else if (idx == 2)
                    boy.SetActive(false);
            }
            else if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                if(idx == imageList.Count)
                    SceneManager.LoadScene(1);
            }
        }
    }

    public void ClickBelieve()
    {
        EndCutScene();
        DataManager.instance.isBelieving = true;
    }

    public void ClickNonBelieve()
    {
        EndCutScene();
        DataManager.instance.isBelieving = false;
    }

    private void EndCutScene()
    {
        //buttons.SetActive(false);
        GameObject.Find("FadeManager").GetComponent<FadeManager>().StartCoroutine("FadeOutCoroutine");
    }
}
