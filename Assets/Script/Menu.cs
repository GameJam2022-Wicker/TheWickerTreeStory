using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject MenuUI;
    [SerializeField] private GameObject SignList, LetterList;

    void Start()
    {
        int i = 0;
        foreach (Button btn in SignList.GetComponentsInChildren<Button>())
        {
            // J : �ӽ� ������ ���ڷ� ����
            int id = i;
            btn.onClick.AddListener(() => OnClickSignBtn(id));
            i++;
        }

        i = 0;
        foreach (Button btn in LetterList.GetComponentsInChildren<Button>())
        {
            // J : �ӽ� ������ ���ڷ� ����
            int id = i;
            btn.onClick.AddListener(() => OnClickLetterBtn(id));
            i++;
        }
    }

    // J : ���� ����� �޴� ��ư Ŭ��
    public void OnClickMenuBtn()
    {
        if (MenuUI.activeSelf == false)
            MenuUI.SetActive(true);
        else
            MenuUI.SetActive(false);
    }

    // J : ������ �̾߱� ��ư Ŭ�� -> ���� ���
    public void OnClickSignBtn()
    {
        LetterList.SetActive(false);
        SignList.SetActive(true);
    }

    // J : ���� ���� Ŭ��
    public void OnClickSignBtn(int id)
    {
        Debug.Log("���� " + id);
    }

    // J : ���� �̾߱� ��ư Ŭ�� -> ���� ���
    public void OnClickLetterBtn()
    {
        SignList.SetActive(false);
        LetterList.SetActive(true);
    }

    // J : ���� ���� Ŭ��
    public void OnClickLetterBtn(int id)
    {
        Debug.Log("���� " + id);
    }

    // J : ���� ������ ��ư Ŭ��
    public void OnClickExitBtn()
    {
        SceneManager.LoadScene(0);
    }
}
