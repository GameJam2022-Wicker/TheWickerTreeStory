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
    
    //���� ��ũ��Ʈ ��������
    //string letterStr = TableData.instance.GetScript("Letter", id);

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
        string str = TableData.instance.GetScript("Borad", "Borad0" + (id + 1));
        Debug.Log(str);
        return str;
    }

    // J : �ش� id�� ���� ���� ����
    public string GetLetterStr(int id)
    {
        string str = TableData.instance.GetScript("Letter", "Letter0" + (id + 1));
        Debug.Log(str);
        return str;
    }

    // J : ���α׷� ���� �� ������ ����
    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
