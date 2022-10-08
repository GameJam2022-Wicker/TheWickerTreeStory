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
        lineTextList.Add("어둡고 어두운 감옥에는 버드나무의 향기가 은은하게 흐르고 있었습니다. \n\n" +
                               "하얀 소복을 입은 소녀가 눈을 떴을 때, 그녀의 작은 뿔에도 그 달짝지근한 향이 뭍어있었지요.");
        lineTextList.Add("소녀는 멍한 얼굴로 나무로 만든 구조물을 바라보았습니다. \n\n" +
                                "여긴 어디지 ? 아마 그런 생각을 하고 있지 않았을까요?");
        lineTextList.Add("버드 나무로 만든 기이한 건축물, 자신을 가두고 있는 작고 각진 감옥.. \n\n" +
                                "소녀는 낯설고, 낯선 이 공간이 참으로 기묘하게 느껴졌습니다. \n\n" +
                                "간밤에 무슨 일이 있던 건지..소녀는 다시 생각에 잠겼습니다.");
        lineTextList.Add("이런, 소녀의 머릿속이 검게 물들어 버렸네요. \n\n" +
                               "그녀는 아무런 기억을 떠올릴 수 없었습니다. \n" +
                               "나이도, 고향도, 가족도, 심지어 자신의 이름마저도 떠올리지 못한 소녀는 혼란스러운 마음을 감출 수 없었습니다.");
        lineTextList.Add("침묵을 깨는 낭랑한 목소리가 소녀의 귀에 꽈랑꽈랑 울렸습니다. \n\n" +
                                "소녀는 소리꾼 한 명이 옆에 있는 줄 알았어요. \n\n" +
                                "소리가 들린 곳은 나무 감옥의 썩은 가지가 가리키는 곳, 그곳에 광대 탈을 쓴 소년이 서있었습니다.");
        lineTextList.Add("광대 소년의 걱정은 소녀를 아리송하게 만들었습니다. \n\n" +
                                "소녀는 소년의 우스꽝스러운 탈은 물론, 그의 맨얼굴도 알지 못합니다. \n\n" +
                                "그녀의 기억은 깊고 어두운 늪 안에 갇혀 있었으니까요.");
        lineTextList.Add("소년도 그녀의 모습에 무언갈 눈치챈 것 같았습니다. \n\n" +
                            "사시나무처럼 떨리는 목소리로, 소년이 물었습니다.");
        lineTextList.Add("광대 탈 너머의 눈이 어두워졌습니다.\n\n" +
                            "이게 도대체 무슨 일인지! \n\n" +
                            "소녀는 이러지도 저러지도 못한 채, 소년을 바라보았습니다.");
        lineTextList.Add("소년은 결심한 것처럼 소리쳤습니다.");
        lineTextList.Add("낡은 감옥이 무너지고, 소녀는 자유를 되찾았습니다. \n\n" +
                             "소녀의 눈 앞에는 소년의 작은 손이 내밀어져 있었습니다. \n\n" +
                            "그 작고 애처로운 손을, 소녀는 외면하지 못했습니다.");
        lineText.text = lineTextList[count];
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartBackground.SetActive(false);
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
