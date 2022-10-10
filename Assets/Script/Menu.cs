using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI signNum, letterNum;
    [SerializeField] private GameObject MenuUI;
    [SerializeField] private GameObject SignList, LetterList;

    void Start()
    {
        int i = 0;
        foreach (Button btn in SignList.GetComponentsInChildren<Button>())
        {
            // J : 임시 변수를 인자로 전달
            int id = i;
            btn.onClick.AddListener(() => OnClickSignBtn(id));
            i++;
        }

        i = 0;
        foreach (Button btn in LetterList.GetComponentsInChildren<Button>())
        {
            // J : 임시 변수를 인자로 전달
            int id = i;
            btn.onClick.AddListener(() => OnClickLetterBtn(id));
            i++;
        }
    }

    // J : 왼쪽 상단의 메뉴 버튼 클릭
    public void OnClickMenuBtn()
    {
        if (MenuUI.activeSelf == false)
        {
            SetMenuUI();
            MenuUI.SetActive(true);
        }
        else
            MenuUI.SetActive(false);
    }

    // J : 현재까지 읽은 지판에 따라 MenuUI 세팅
    private void SetMenuUI()
    {
        List<int> readIdxList=DataManager.instance.data.GetReadSignList();

        // J : 볼 수 있는 지판/편지 개수 표시
        string str = readIdxList.Count.ToString() + " / 5";
        signNum.text = letterNum.text = str;
    }

    // J : 지판의 이야기 버튼 클릭 -> 지판 목록
    public void OnClickSignBtn()
    {
        LetterList.SetActive(false);
        SignList.SetActive(true);
    }

    // J : 지판 보기 클릭
    public void OnClickSignBtn(int id)
    {
        Debug.Log("지판 " + id);
    }

    // J : 편지 이야기 버튼 클릭 -> 편지 목록
    public void OnClickLetterBtn()
    {
        SignList.SetActive(false);
        LetterList.SetActive(true);
    }

    // J : 편지 보기 클릭
    public void OnClickLetterBtn(int id)
    {
        Debug.Log("편지 " + id);
    }

    // J : 게임 나가기 버튼 클릭
    public void OnClickExitBtn()
    {
        SceneManager.LoadScene(0);
    }
}
