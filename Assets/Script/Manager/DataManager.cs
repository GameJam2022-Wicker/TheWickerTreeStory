using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public bool isBelieving;    // J : �÷��̾� �Է� (�ϴ´�vs���� �ʴ´�)
    // J : ǥ���� ���� �迭 (���� ������ �Ľ����� ���� ����)
    private string[] signStr = {"������ �� �ʰ� �ٱ��� �� ũ�Ⱑ �ٸ���. \n" +
                 "����� �̸���, �̴� \"�츮�� ���� �ٸ��� �����̴�.\" �Ͽ���. \n" +
                 "���� ���� С���� ������ �׸��ڿ��� ����� �ʴ´�. \n",
                "��С�� �ڪ�� �ִ� �ڸ����� �¾��, \n" +
                "������ ��ġ �����Ʊ�͵� ���Ƽ�, ���θ� ���� ���� ��� � �̵��� \n" +
                "�� �Ҹ��� Ȧ���� ������⵵ �Ѵ�. \n",
                "�� �̻� ���� ������ ���� �ʾҴ�. ��� ������ Ž��������. \n" +
                "�̴� �ʽ� 񫰡 �츮�� 𮸦 ��� ����� �����̶�. \n" +
                "�׸��ڿ� ��ģ �׵��� ���� ���� �Ƹ��ٿ���. \n",
                "���� ���̰� �¾��. ���ο� ������ �����ΰ��Ͽ�, \n" +
                "����� �̴� ������ 񫰡 �츮���� ������ ��������, ���̴϶� �ϴ�, \n" +
               "�����δ� ���� ���̿� �Բ� ���ο� �ڪ�� �غ��϶� �Ͽ���.",
                "�����μ� ���� ��Ÿ������ ��, �� �� ���̴�. \n" +
                "���� ����̰� �Ǿ�, �� ���Ⱑ �ϴÿ� ������, 񫲲�� ǳ���� ����ϽŴ�. \n" +
                "�׷��� �� �� �ϳ���, ����� �ٶ����� ����� ���̴�. \n"};

    public static DataManager instance = null;
    private void Awake()
    {
        if(null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // J : �ش� id�� ǥ���� ���� ����
    public string GetSingStr(int id)
    {
        Debug.Log(signStr[id]);
        return signStr[id];
    }
}
