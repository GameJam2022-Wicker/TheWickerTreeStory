using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Prologue : MonoBehaviour
{
    public Text lineText;
    public Image blackpanel;
    public GameObject StartBackground;
    public GameObject BackgroundObject;

    public List<string> lineTextList;

    int count = 0;
    public void Start()
    {
        lineTextList.Add("��Ӱ� ��ο� �������� ���峪���� ��Ⱑ �����ϰ� �帣�� �־����ϴ�. \n\n" +
                               "�Ͼ� �Һ��� ���� �ҳడ ���� ���� ��, �׳��� ���� �Կ��� �� ��¦������ ���� �����־�����.");
        lineTextList.Add("�ҳ�� ���� �󱼷� ������ ���� �������� �ٶ󺸾ҽ��ϴ�. \n\n" +
                                "���� ����� ? �Ƹ� �׷� ������ �ϰ� ���� �ʾ������?");
        lineTextList.Add("���� ������ ���� ������ ���๰, �ڽ��� ���ΰ� �ִ� �۰� ���� ����.. \n\n" +
                                "�ҳ�� ������, ���� �� ������ ������ �⹦�ϰ� ���������ϴ�. \n\n" +
                                "���㿡 ���� ���� �ִ� ����..�ҳ�� �ٽ� ������ �����ϴ�.");
        lineTextList.Add("�̷�, �ҳ��� �Ӹ����� �˰� ����� ���ȳ׿�. \n\n" +
                               "�׳�� �ƹ��� ����� ���ø� �� �������ϴ�. \n" +
                               "���̵�, ���⵵, ������, ������ �ڽ��� �̸������� ���ø��� ���� �ҳ�� ȥ�������� ������ ���� �� �������ϴ�.");
        lineTextList.Add("ħ���� ���� ������ ��Ҹ��� �ҳ��� �Ϳ� �ʶ��ʶ� ��Ƚ��ϴ�. \n\n" +
                                "�ҳ�� �Ҹ��� �� ���� ���� �ִ� �� �˾Ҿ��. \n\n" +
                                "�Ҹ��� �鸰 ���� ���� ������ ���� ������ ����Ű�� ��, �װ��� ���� Ż�� �� �ҳ��� ���־����ϴ�.");
        lineTextList.Add("���� �ҳ��� ������ �ҳฦ �Ƹ����ϰ� ��������ϴ�. \n\n" +
                                "�ҳ�� �ҳ��� �콺�ν����� Ż�� ����, ���� �Ǿ󱼵� ���� ���մϴ�. \n\n" +
                                "�׳��� ����� ��� ��ο� �� �ȿ� ���� �־����ϱ��.");
        lineTextList.Add("�ҳ⵵ �׳��� ����� ���� ��ġæ �� ���ҽ��ϴ�. \n\n" +
                            "��ó���ó�� ������ ��Ҹ���, �ҳ��� �������ϴ�.");
        lineTextList.Add("���� Ż �ʸ��� ���� ��ο������ϴ�.\n\n" +
                            "�̰� ����ü ���� ������! \n\n" +
                            "�ҳ�� �̷����� �������� ���� ä, �ҳ��� �ٶ󺸾ҽ��ϴ�.");
        lineTextList.Add("�ҳ��� ����� ��ó�� �Ҹ��ƽ��ϴ�.");
        lineTextList.Add("���� ������ ��������, �ҳ�� ������ ��ã�ҽ��ϴ�. \n\n" +
                             "�ҳ��� �� �տ��� �ҳ��� ���� ���� ���о��� �־����ϴ�. \n\n" +
                            "�� �۰� ��ó�ο� ����, �ҳ�� �ܸ����� ���߽��ϴ�.");
        lineText.text = lineTextList[count];
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartBackground.SetActive(false);
            blackpanel.gameObject.SetActive(true);
            if(blackpanel.gameObject.activeSelf)
                Destroy(blackpanel);
        }
    }

    public void OnClickBackgroundImg()
    {
        lineText.text = lineTextList[count];
        count++;
        if(count == lineTextList.Count)
            SceneManager.LoadScene(1);
    }
}
