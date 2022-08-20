using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutScene : MonoBehaviour
{
    [SerializeField]
    private Image fadePanel;
    [SerializeField]
    private Image image;
    [SerializeField]
    private List<Sprite> imageList;
    private int imageNum;   // 이미지 개수
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

    private void ChangeImage()
    {
        idx++;
        if (idx == imageNum)
            GameObject.Find("FadeManager").GetComponent<FadeManager>().StartCoroutine("FadeOutCoroutine");
        else if (idx < imageNum)
            image.sprite = imageList[idx];
    }
}
