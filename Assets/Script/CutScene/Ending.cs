using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Ending : MonoBehaviour
{
    bool isBelieving;

    public AudioSource bgm;
    public AudioClip happyBGM, badBGM;

    private PlayableDirector playableDirector;
    [SerializeField] private TimelineAsset happyTimeline, badTimeline;

    private void Awake()
    {
        // J : Timeline ������Ʈ�� PlayableDirector ������Ʈ ��������
        playableDirector=GameObject.Find("Timeline").GetComponent<PlayableDirector>();
    }

    private void Start()
    {
        isBelieving = DataManager.instance.isBelieving; // �ϴ´�vs�����ʴ´�

        if(isBelieving)
        {
            bgm.clip = happyBGM;
            playableDirector.playableAsset = happyTimeline;
        }
        else
        {
            bgm.clip = badBGM;
            playableDirector.playableAsset = badTimeline;
        }

        playableDirector.Play();
        bgm.Play();
    }
}
