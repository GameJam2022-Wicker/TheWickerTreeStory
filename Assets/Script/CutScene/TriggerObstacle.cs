using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerObstacle : MonoBehaviour
{
    public PlayableDirector timeline_cutscene1_2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Obstacle")
        {
            timeline_cutscene1_2.Play();
        }
    }
}
