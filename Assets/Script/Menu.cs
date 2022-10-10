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
    
    private string[] numStrList = { "ù", "��", "��", "��", "�ټ�" };
    private string signStr = " ��° ����", letterStr = " ��° ����";


    // J : ���� ����� �޴� ��ư Ŭ��
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

    // J : ������� ���� ���ǿ� ���� MenuUI ����
    private void SetMenuUI()
    {
        foreach (Transform child in SignList.GetComponentInChildren<Transform>())
            Destroy(child.gameObject);
        foreach (Transform child in LetterList.GetComponentInChildren<Transform>())
            Destroy(child.gameObject);

        int readCount = DataManager.instance.data.GetReadCount();

        // J : �� �� �ִ� ����/���� ���� ǥ��
        string str = readCount.ToString() + " / 5";
        signNum.text = letterNum.text = str;

        // J : ����/���� ��� ǥ��
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

            // J : ��ư ������ ����
            int id = i;
            signItemObj.GetComponentInChildren<Button>().onClick.AddListener(() => OnClickSignBtn(id));
            letterItemObj.GetComponentInChildren<Button>().onClick.AddListener(() => OnClickLetterBtn(id));

            // J : �ؽ�Ʈ ���� (i��° ����/����)
            signItemObj.GetComponentInChildren<TextMeshProUGUI>().text = numStrList[i] + signStr;
            letterItemObj.GetComponentInChildren<TextMeshProUGUI>().text = numStrList[i] + letterStr;
        }
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
