using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Ending : MonoBehaviour
{
    bool isBelieving;

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
            playableDirector.playableAsset = happyTimeline;
        }
        else
        {
            playableDirector.playableAsset = badTimeline;
        }

        playableDirector.Play();
    }
}
