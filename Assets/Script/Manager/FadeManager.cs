using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    [SerializeField]
    private Image fadePanel;
    [SerializeField]
    private TextMeshProUGUI text;

    private void Start()
    {
        StartCoroutine("FadeInCoroutine");
    }

    IEnumerator FadeInCoroutine()
    {
        fadePanel.gameObject.SetActive(true);
        float fadeCount = 1;    // 초기 알파값(검은 화면)

        while (fadeCount > 0)    // 알파값이 최소(0)가 될 때까지 반복
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f); // 0.01초마다 밝아지게->1초 후 완전히 밝아짐
            fadePanel.color = new Color(0, 0, 0, fadeCount);    // 알파값 조정
        }

        fadePanel.gameObject.SetActive(false);
    }

    public IEnumerator FadeOutCoroutine()
    {
        if(AudioManager.instance != null)
            StartCoroutine(AudioManager.instance.AnimateMusicCrossfadeOut(2));
        fadePanel.gameObject.SetActive(true);

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

        // scene 전환
        if (currentSceneNumber == 10)
            SceneManager.LoadScene(0);
        else
            SceneManager.LoadScene(++currentSceneNumber);
    }

    public IEnumerator GameOverCoroutine()
    {
        fadePanel.GetComponent<Image>().color = Color.white;
        fadePanel.gameObject.SetActive(true);
        fadePanel.gameObject.GetComponent<Animator>().enabled = true;
        fadePanel.gameObject.GetComponent<Animator>().Play("GameoverAnimClip");

        float fadeCount = 0;    // 초기 알파값(검은 화면)
        while (true)    // 알파값이 최대(1)가 될 때까지 반복
        {
            if (fadeCount >= 1) // 페이드아웃 종료
                break;

            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f); // 0.01초마다 어두워지게->1초 후 완전히 어두워짐
            //fadePanel.color = new Color(0, 0, 0, fadeCount);    // 알파값 조정
            text.color = new Color(text.color.r, text.color.g, text.color.b, fadeCount);
        }

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   // 현재 스테이지 재시작
    }
}
