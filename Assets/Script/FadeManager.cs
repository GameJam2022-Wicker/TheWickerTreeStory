using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    [SerializeField]
    private Image fadePanel;

    private void Start()
    {
        StartCoroutine("FadeInCoroutine");
    }

    IEnumerator FadeInCoroutine()
    {
        float fadeCount = 1;    // �ʱ� ���İ�(���� ȭ��)
        while (fadeCount > 0)    // ���İ��� �ּ�(0)�� �� ������ �ݺ�
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f); // 0.01�ʸ��� �������->1�� �� ������ �����
            fadePanel.color = new Color(0, 0, 0, fadeCount);    // ���İ� ����
        }
    }

    public IEnumerator FadeOutCoroutine()
    {
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
        SceneManager.LoadScene(++currentSceneNumber);   // scene ��ȯ
    }
}
