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
                "그러니 둘 중 하나는, 昇天의 바람으로 사라질 것이다."};
    //편지 스크립트 가져오기
    //string letterStr = TableData.instance.GetScript("Letter", id);
    private string[] letterStr = {"주인 없는 종이를 줍자, 아버님의 존재가 떠올랐습니다.\n"+
                "아직 제 기억에 아무 확신이 없지만.. 그래도 앞으로를 위해 편지를 씁니다.\n"+
                "저는 어린 탈을 쓴 선생의 도움으로 바깥으로 가고 있습니다.\n"+
                "지판에 적힌 말을 빌리면, 우리를 이 꼭두에 가둔 자는 하원(夏嫄)이라는 자 같습니다.\n"+
                "저로서는 이해하기 어렵지만, 이곳과 바깥은 무언가 다른 모양입니다.\n"+
                "이 큰 꼭두는 어떻게 만든 것일까요? 기이한 일 투성이입니다.",

                "아버님, 이곳엔 검은 탈과 뿔을 가진 귀신이 돌아다니고 있습니다.\n"+
                "바깥의 사람들은 이 귀신이 사람을 홀린다고 생각하는 모양입니다.\n"+
                "하지만 제가 그들의 소리를 듣고 있자면.. 마치 저잣거리에 내몰린 아이들의비명처럼 들립니다.\n"+ 
                "어째서 그들은 이 소리를 어린아이의 눈물로 알고 있는 것일까요?\n"+
                "탈 선생께서는 '눈 뜬 장님들의 변명'이라고 하십니다.\n"+
                "…아직 아무 것도 알 수 없습니다. 아버님, 제가 밖으로 나가면 모든 걸 알게 될까요?\n",

                "주(主)라고 하는 존재의 글귀를 읽었습니다.\n"+
                "도대체 무엇을 소유한 주인인 것일까요? 지판 구석에 해를 그린 그림을 보았습니다.\n"+
                "만약 해의 주를 섬긴다고 한다면, 앞 뒤가 맞지 않는다고 생각합니다.\n"+
                "검은 각귀들은 아무리 봐도 그림자에 빠진 어린이들 같으니까요.\n"+
                "…저와 함께 나무를 넘어가는 탈 선생은 그저 모든 게 기쁜 모양입니다.\n"+
                "무엇 때문에 저를 데리고 가려는 것일까요? 점점 불안해집니다.",

                "…전 바깥의 사람들과 동향인 걸까요? 이젠 잿가루처럼 남은 기억도 믿을 수 없습니다.\n"+
                "아버님.. 정말로 제 옆에 있어주셨나요?\n"+
                "아니면 그저 저의 미친 상상이 만든 존재신가요?\n"+
                "이 끔찍한 꼭두 속에 저 스스로 걸어들어온 건가요?\n"+
                "탈 선생은 아무렇지 않은 거 같습니다. 그저 자신을 믿으라는 말씀을…\n"+
                "…............전 어찌하면 되는 걸까요",

                "머리에 맴돌던 짙은 안개가 사라졌습니다.\n"+
                "적어도 버드나무로 만든 꼭두 바깥에서 미소 지으시던 아버님의 모습은 떠올렸습니다.\n"+
                "탈 선생께서 이젠 저의 안색을 살피며, 눈을 흘리고 계십니다.\n"+
                "아마 밖으로 나가도 아버님께 돌아가는 일은 없을 것입니다.\n"+
                "그러니 전……"
    };


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
        Debug.Log(signStr[id]);
        return signStr[id];
    }

    // J : 해당 id의 편지 내용 리턴
    public string GetLetterStr(int id)
    {
        Debug.Log(letterStr[id]);
        return letterStr[id];
    }

    // J : 프로그램 종료 시 데이터 저장
    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
