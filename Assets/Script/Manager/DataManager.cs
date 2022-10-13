using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// J : json 파일로 저장할 데이터 클래스
[Serializable]
public class GameData
{
    public bool[] sign = { false, false, false, false, false }; // J : 표지판 읽음 여부

    // J : 지금까지 읽은 지판의 인덱스 리스트 리턴
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
    public bool isBelieving;    // J : 플레이어 입력 (믿는다vs믿지 않는다)

    // J : 표지판/편지 내용 배열 (추후 데이터 파싱으로 변경 예정)
    // yesman: skillmanager; 테이블 데이터에서 직접 가져옴
    
    //편지 스크립트 가져오기
    //string letterStr = TableData.instance.GetScript("Letter", id);

    private string DataFileName = "Data.json";  // J : json 파일 이름

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
                DontDestroyOnLoad(_container);  // J : scene을 이동해도 game object 유지
            }
            return _instance;
        }
    }

    // J : json 파일로 저장할 객체
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
    

    // J : json 파일 불러오기
    private void LoadGameData()
    {
        string filePath = Application.persistentDataPath + "/" + DataFileName;
        Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            Debug.Log("데이터 불러오기");
            string FromJsonData=File.ReadAllText(filePath);
            _data=JsonUtility.FromJson<GameData>(FromJsonData);
        }
        else
        {
            Debug.Log("새로운 데이터 파일 생성");
            _data = new GameData();
        }
    }

    // J : json 파일로 저장
    private void SaveGameData()
    {
        string ToJsonData=JsonUtility.ToJson(data);
        string filePath = Application.persistentDataPath + "/" + DataFileName;
        File.WriteAllText(filePath, ToJsonData);
        Debug.Log("데이터 저장 완료");
    }

    // J : 해당 id의 표지판 내용 리턴
    public string GetSignStr(int id)
    {
        string str = TableData.instance.GetScript("Borad", "Borad0" + (id + 1));
        Debug.Log(str);
        return str;
    }

    // J : 해당 id의 편지 내용 리턴
    public string GetLetterStr(int id)
    {
        string str = TableData.instance.GetScript("Letter", "Letter0" + (id + 1));
        Debug.Log(str);
        return str;
    }

    // J : 프로그램 종료 시 데이터 저장
    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
