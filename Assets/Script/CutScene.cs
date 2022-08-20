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
            StartCoroutine("FadeOutCoroutine");
        else if (idx < imageNum)
            image.sprite = imageList[idx];
    }

    IEnumerator FadeOutCoroutine()
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
        SceneManager.LoadScene(++currentSceneNumber);
    }
}
