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

    [SerializeField] private GameObject signItem, noSignItem;
    [SerializeField] private GameObject letterItem, noLetterItem;
    
    private string[] numStrList = { "첫", "두", "세", "네", "다섯" };
    private string signStr = " 번째 지판", letterStr = " 번째 편지";


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
        foreach (Transform child in SignList.GetComponentInChildren<Transform>())
            Destroy(child.gameObject);
        foreach (Transform child in LetterList.GetComponentInChildren<Transform>())
            Destroy(child.gameObject);

        int readCount = DataManager.instance.data.GetReadCount();

        // J : 볼 수 있는 지판/편지 개수 표시
        string str = readCount.ToString() + " / 5";
        signNum.text = letterNum.text = str;

        // J : 지판/편지 목록 표시
        bool[] sign = DataManager.instance.data.sign;
        GameObject signItemObj, letterItemObj;
        for (int i = 0; i < sign.Length; i++)
        {
            if (sign[i])
            {
                signItemObj = Instantiate(signItem, Vector2.zero, Quaternion.identity);
                signItemObj.transform.parent = SignList.transform;

                letterItemObj = Instantiate(letterItem, Vector2.zero, Quaternion.identity);
                letterItemObj.transform.parent = LetterList.transform;
            }
            else
            {
                signItemObj = Instantiate(noSignItem, Vector2.zero, Quaternion.identity);
                signItemObj.transform.parent = SignList.transform;

                letterItemObj = Instantiate(noLetterItem, Vector2.zero, Quaternion.identity);
                letterItemObj.transform.parent = LetterList.transform;
            }

            // J : 버튼 리스너 설정
            int id = i;
            signItemObj.GetComponentInChildren<Button>().onClick.AddListener(() => OnClickSignBtn(id));
            letterItemObj.GetComponentInChildren<Button>().onClick.AddListener(() => OnClickLetterBtn(id));

            // J : 텍스트 설정 (i번째 지판/편지)
            signItemObj.GetComponentInChildren<TextMeshProUGUI>().text = numStrList[i] + signStr;
            letterItemObj.GetComponentInChildren<TextMeshProUGUI>().text = numStrList[i] + letterStr;
        }
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
