using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SubtitleClip : PlayableAsset
{
    public string subtitleText;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SubtitleBehaviour>.Create(graph);
        /*var playableDirector = owner.GetComponent<PlayableDirector>();
        if (playableDirector != null)
        {
            var timelineAsset = playableDirector.playableAsset as TimelineAsset;
            if (timelineAsset != null)
                Debug.Log(playable.GetGraph().GetRootPlayable(0).GetTime());
        }*/

        SubtitleBehaviour subtitleBehaviour = playable.GetBehaviour();
        subtitleBehaviour.subtitleText = subtitleText;

        return playable;
    }
}