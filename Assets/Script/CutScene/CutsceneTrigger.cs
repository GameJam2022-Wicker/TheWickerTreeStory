using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private string tag;
    public PlayableDirector timeline_cutscene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == tag)
        {
            timeline_cutscene.Play();
        }
    }
}
