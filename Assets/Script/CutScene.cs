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
            StartCoroutine("FadeOutCoroutine");
        else if (idx < imageNum)
            image.sprite = imageList[idx];
    }

    IEnumerator FadeOutCoroutine()
    {
        float fadeCount = 0;    // 초기 알파값(검은 화면)
        while (true)    // 알파값이 최대(1)가 될 때까지 반복
        {
            if (fadeCount >= 1) // 페이드아웃 종료
                break;

            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f); // 0.01초마다 어두워지게->1초 후 완전히 어두워짐
            fadePanel.color = new Color(0, 0, 0, fadeCount);    // 알파값 조정
        }

        int currentSceneNumber = SceneManager.GetActiveScene().buildIndex;  // 현재 scene number
        SceneManager.LoadScene(++currentSceneNumber);
    }
}
