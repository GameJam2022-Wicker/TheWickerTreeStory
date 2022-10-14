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
        // J : Timeline 오브젝트의 PlayableDirector 컴포넌트 가져오기
        playableDirector=GameObject.Find("Timeline").GetComponent<PlayableDirector>();
    }

    private void Start()
    {
        isBelieving = DataManager.instance.isBelieving; // 믿는다vs믿지않는다

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
