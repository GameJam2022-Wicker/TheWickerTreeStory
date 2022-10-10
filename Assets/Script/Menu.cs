using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject MenuUI;

    // J : ���� ����� �޴� ��ư Ŭ��
    public void OnClickMenuBtn()
    {
        if (MenuUI.activeSelf == false)
            MenuUI.SetActive(true);
        else
            MenuUI.SetActive(false);
    }

    // J : ���� ������ ��ư Ŭ��
    public void OnClickExitBtn()
    {
        SceneManager.LoadScene(0);
    }
}
