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
    public List<int> GetReadSignList()
    {
        List<int> list = new List<int>();

        for (int i = 0; i < sign.Length; i++)
            if (sign[i] == true) list.Add(i);

        return list;
    }
}

public class DataManager : MonoBehaviour
{
    public bool isBelieving;    // J : 플레이어 입력 (믿는다vs믿지 않는다)
    // J : 표지판 내용 배열 (추후 데이터 파싱으로 변경 예정)
    private string[] signStr = {"꼭두의 안 쪽과 바깥은 그 크기가 다르다. \n" +
                 "夏嫄이 이르길, 이는 \"우리와 軌가 다르기 때문이다.\" 하였다. \n" +
                 "검은 角의 鬼神은 꼭두의 그림자에서 벗어나지 않는다. \n",
                "角鬼는 祭物이 있던 자리에서 태어난다, \n" +
                "울음은 마치 갓난아기와도 같아서, 꼭두를 만들 때면 몇몇 어린 이들이 \n" +
                "그 소리에 홀려서 사라지기도 한다. \n",
                "더 이상 썩은 과실이 나지 않았다. 모든 과실이 탐스러웠다. \n" +
                "이는 필시 主가 우리의 祭를 어여삐 여겼기 때문이라. \n" +
                "그림자에 비친 그들의 角이 보다 아름다웠다. \n",
                "角의 아이가 태어났다. 새로운 飢饉의 전조인가하여, \n" +
                "夏嫄은 이는 오히려 主가 우리에게 보내는 선물이자, 복이니라 하니, \n" +
               "앞으로는 角의 아이와 함께 새로운 祭物을 준비하라 하였다.",
                "번제로서 몸이 불타오르는 건, 한 명 뿐이다. \n" +
                "角이 잿더미가 되어, 그 연기가 하늘에 오르면, 主께서 풍년을 기약하신다. \n" +
                "그러니 둘 중 하나는, 昇天의 바람으로 사라질 것이다. \n"};

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
    public string GetSingStr(int id)
    {
        Debug.Log(signStr[id]);
        return signStr[id];
    }

    // J : 프로그램 종료 시 데이터 저장
    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
