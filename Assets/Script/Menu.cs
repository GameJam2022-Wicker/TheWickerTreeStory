using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject MenuUI;

    // J : 왼쪽 상단의 메뉴 버튼 클릭
    public void OnClickMenuBtn()
    {
        if (MenuUI.activeSelf == false)
            MenuUI.SetActive(true);
        else
            MenuUI.SetActive(false);
    }

    // J : 게임 나가기 버튼 클릭
    public void OnClickExitBtn()
    {
        SceneManager.LoadScene(0);
    }
}
