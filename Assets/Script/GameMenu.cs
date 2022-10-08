using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    //��ư ���� �� ����
    public GameObject buttonGroupObject;

    //startButton ���� �� �Ѱ� ������ ������Ʈ
    public GameObject prologueFirstTextObject;
    public GameObject menu_ImageObject;

    //creditButton ���� �� �Ѱ� ������ ������Ʈ
    public GameObject credit_Object;

    public Button start_Button;
    public Button credit_Button;
    public Button exit_Button;
    public Button tempback;

    private void Start()
    {
        start_Button.onClick.AddListener(OnClickStartButton);
        credit_Button.onClick.AddListener(OnClickCreditButton);
        exit_Button.onClick.AddListener(OnClickExitButton);
        tempback.onClick.AddListener(OnClickBackButton);
    }

    public void OnClickStartButton()
    {
        prologueFirstTextObject.SetActive(true);
        menu_ImageObject.SetActive(false);
        buttonGroupObject.SetActive(false);
    }

    public void OnClickCreditButton()
    {
        buttonGroupObject.SetActive(false);
        credit_Object.SetActive(true);
        //�ӽ�
        tempback.gameObject.SetActive(true);
    }

    //�ӽ�
    public void OnClickBackButton()
    {
        credit_Object.SetActive(false);
        buttonGroupObject.SetActive(true);
        tempback.gameObject.SetActive(false);
    }

    public void OnClickExitButton()
    {
        Application.Quit();
    }
}
