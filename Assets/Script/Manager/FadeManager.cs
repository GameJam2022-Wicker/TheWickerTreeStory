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
        float fadeCount = 1;    // �ʱ� ���İ�(���� ȭ��)

        while (fadeCount > 0)    // ���İ��� �ּ�(0)�� �� ������ �ݺ�
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f); // 0.01�ʸ��� �������->1�� �� ������ �����
            fadePanel.color = new Color(0, 0, 0, fadeCount);    // ���İ� ����
        }

        fadePanel.gameObject.SetActive(false);
    }

    public IEnumerator FadeOutCoroutine()
    {
        if(AudioManager.instance != null)
            StartCoroutine(AudioManager.instance.AnimateMusicCrossfadeOut(2));
        fadePanel.gameObject.SetActive(true);

        float fadeCount = 0;    // �ʱ� ���İ�(���� ȭ��)
        while (true)    // ���İ��� �ִ�(1)�� �� ������ �ݺ�
        {
            if (fadeCount >= 1) // ���̵�ƿ� ����
                break;

            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f); // 0.01�ʸ��� ��ο�����->1�� �� ������ ��ο���
            fadePanel.color = new Color(0, 0, 0, fadeCount);    // ���İ� ����
        }

        int currentSceneNumber = SceneManager.GetActiveScene().buildIndex;  // ���� scene number

        // scene ��ȯ
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

        float fadeCount = 0;    // �ʱ� ���İ�(���� ȭ��)
        while (true)    // ���İ��� �ִ�(1)�� �� ������ �ݺ�
        {
            if (fadeCount >= 1) // ���̵�ƿ� ����
                break;

            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f); // 0.01�ʸ��� ��ο�����->1�� �� ������ ��ο���
            //fadePanel.color = new Color(0, 0, 0, fadeCount);    // ���İ� ����
            text.color = new Color(text.color.r, text.color.g, text.color.b, fadeCount);
        }

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   // ���� �������� �����
    }
}
