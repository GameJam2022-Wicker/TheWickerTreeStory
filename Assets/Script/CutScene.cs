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
    private List<Sprite> imageList;
    [SerializeField]
    private GameObject boy;

    private int imageNum;   // �̹��� ����
    private int idx;      // ���� �̹���

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
        {
            image.sprite = imageList[idx];


            // Ư�� ���ǿ� �߻��ϴ� �̺�Ʈ üũ
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                if (idx == 1)
                    boy.SetActive(true);
                else if (idx == 2)
                    boy.SetActive(false);
            }
        }
    }
}
