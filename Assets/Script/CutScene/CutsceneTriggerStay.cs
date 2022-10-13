using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTriggerStay : MonoBehaviour
{
    private bool isPlayerStay;
    public PlayableDirector timeline_cutscene;

    private void Update()
    {
        if(isPlayerStay)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                timeline_cutscene.Play();
                Debug.Log("f버튼 누름");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            isPlayerStay = true;
            
        }
    }
}
