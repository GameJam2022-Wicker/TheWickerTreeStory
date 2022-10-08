using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // J : 해당 id의 표지판 내용 리턴
    public string GetSingStr(int id)
    {
        Debug.Log(signStr[id]);
        return signStr[id];
    }
}
