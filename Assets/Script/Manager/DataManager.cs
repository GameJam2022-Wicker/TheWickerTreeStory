using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// J : json ���Ϸ� ������ ������ Ŭ����
[Serializable]
public class GameData
{
    public bool[] sign = { false, false, false, false, false }; // J : ǥ���� ���� ����

    // J : ���ݱ��� ���� ������ �ε��� ����Ʈ ����
    public int GetReadCount()
    {
        List<int> list = new List<int>();

        for (int i = 0; i < sign.Length; i++)
            if (sign[i] == true) list.Add(i);

        return list.Count;
    }
}

public class DataManager : MonoBehaviour
{
    public bool isBelieving;    // J : �÷��̾� �Է� (�ϴ´�vs���� �ʴ´�)

    // J : ǥ����/���� ���� �迭 (���� ������ �Ľ����� ���� ����)
    // yesman: skillmanager; ���̺� �����Ϳ��� ���� ������
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
                "�׷��� �� �� �ϳ���, ����� �ٶ����� ����� ���̴�."};
    //���� ��ũ��Ʈ ��������
    //string letterStr = TableData.instance.GetScript("Letter", id);
    private string[] letterStr = {"���� ���� ���̸� ����, �ƹ����� ���簡 ���ö����ϴ�.\n"+
                "���� �� ��￡ �ƹ� Ȯ���� ������.. �׷��� �����θ� ���� ������ ���ϴ�.\n"+
                "���� � Ż�� �� ������ �������� �ٱ����� ���� �ֽ��ϴ�.\n"+
                "���ǿ� ���� ���� ������, �츮�� �� ���ο� ���� �ڴ� �Ͽ�(���)�̶�� �� �����ϴ�.\n"+
                "���μ��� �����ϱ� �������, �̰��� �ٱ��� ���� �ٸ� ����Դϴ�.\n"+
                "�� ū ���δ� ��� ���� ���ϱ��? ������ �� �������Դϴ�.",

                "�ƹ���, �̰��� ���� Ż�� ���� ���� �ͽ��� ���ƴٴϰ� �ֽ��ϴ�.\n"+
                "�ٱ��� ������� �� �ͽ��� ����� Ȧ���ٰ� �����ϴ� ����Դϴ�.\n"+
                "������ ���� �׵��� �Ҹ��� ��� ���ڸ�.. ��ġ ����Ÿ��� ������ ���̵��Ǻ��ó�� �鸳�ϴ�.\n"+ 
                "��°�� �׵��� �� �Ҹ��� ������� ������ �˰� �ִ� ���ϱ��?\n"+
                "Ż ���������� '�� �� ��Ե��� ����'�̶�� �Ͻʴϴ�.\n"+
                "������ �ƹ� �͵� �� �� �����ϴ�. �ƹ���, ���� ������ ������ ��� �� �˰� �ɱ��?\n",

                "��(�)��� �ϴ� ������ �۱͸� �о����ϴ�.\n"+
                "����ü ������ ������ ������ ���ϱ��? ���� ������ �ظ� �׸� �׸��� ���ҽ��ϴ�.\n"+
                "���� ���� �ָ� ����ٰ� �Ѵٸ�, �� �ڰ� ���� �ʴ´ٰ� �����մϴ�.\n"+
                "���� ���͵��� �ƹ��� ���� �׸��ڿ� ���� ��̵� �����ϱ��.\n"+
                "������ �Բ� ������ �Ѿ�� Ż ������ ���� ��� �� ��� ����Դϴ�.\n"+
                "���� ������ ���� ������ ������ ���ϱ��? ���� �Ҿ������ϴ�.",

                "���� �ٱ��� ������ ������ �ɱ��? ���� ����ó�� ���� ��ﵵ ���� �� �����ϴ�.\n"+
                "�ƹ���.. ������ �� ���� �־��ּ̳���?\n"+
                "�ƴϸ� ���� ���� ��ģ ����� ���� ����Ű���?\n"+
                "�� ������ ���� �ӿ� �� ������ �ɾ���� �ǰ���?\n"+
                "Ż ������ �ƹ����� ���� �� �����ϴ�. ���� �ڽ��� ������� ��������\n"+
                "��............�� �����ϸ� �Ǵ� �ɱ��",

                "�Ӹ��� �ɵ��� £�� �Ȱ��� ��������ϴ�.\n"+
                "��� ���峪���� ���� ���� �ٱ����� �̼� �����ô� �ƹ����� ����� ���÷Ƚ��ϴ�.\n"+
                "Ż �������� ���� ���� �Ȼ��� ���Ǹ�, ���� �긮�� ��ʴϴ�.\n"+
                "�Ƹ� ������ ������ �ƹ��Բ� ���ư��� ���� ���� ���Դϴ�.\n"+
                "�׷��� ������"
    };


    private string DataFileName = "Data.json";  // J : json ���� �̸�

    static GameObject _container;
    static GameObject container
    {
        get
        {
            return _container;
        }
    }

    static DataManager _instance;
    public static DataManager instance
    {
        get
        {
            if (!_instance)
            {
                _container = new GameObject();
                _container.name = "DataManager";
                _instance = _container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(_container);  // J : scene�� �̵��ص� game object ����
            }
            return _instance;
        }
    }

    // J : json ���Ϸ� ������ ��ü
    private GameData _data;
    public GameData data
    {
        get
        {
            if (_data == null)
            {
                LoadGameData();
                SaveGameData();
            }
            return _data;
        }
    }
    

    // J : json ���� �ҷ�����
    private void LoadGameData()
    {
        string filePath = Application.persistentDataPath + "/" + DataFileName;
        Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            Debug.Log("������ �ҷ�����");
            string FromJsonData=File.ReadAllText(filePath);
            _data=JsonUtility.FromJson<GameData>(FromJsonData);
        }
        else
        {
            Debug.Log("���ο� ������ ���� ����");
            _data = new GameData();
        }
    }

    // J : json ���Ϸ� ����
    private void SaveGameData()
    {
        string ToJsonData=JsonUtility.ToJson(data);
        string filePath = Application.persistentDataPath + "/" + DataFileName;
        File.WriteAllText(filePath, ToJsonData);
        Debug.Log("������ ���� �Ϸ�");
    }

    // J : �ش� id�� ǥ���� ���� ����
    public string GetSignStr(int id)
    {
        Debug.Log(signStr[id]);
        return signStr[id];
    }

    // J : �ش� id�� ���� ���� ����
    public string GetLetterStr(int id)
    {
        Debug.Log(letterStr[id]);
        return letterStr[id];
    }

    // J : ���α׷� ���� �� ������ ����
    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
