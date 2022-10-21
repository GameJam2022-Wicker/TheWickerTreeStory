using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject mapSceneButtons;
    private FadeManager fadeManager;

    private void Awake()
    {
        // J : 컷씬 끝나면 페이드아웃 하기 위해 미리 페이드매니저 컴포넌트 가져옴
        fadeManager = GameObject.Find("FadeManager").GetComponent<FadeManager>();
    }

    public void OnClickSkipButton()
    {
        StartCoroutine(fadeManager.FadeOutCoroutine());
    }

    void OnEnable()
    {
        director.stopped += OnPlayableDirectorStopped;
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
        {
            if(SceneManager.GetActiveScene().buildIndex == 8)
            {
                mapSceneButtons.SetActive(true);
            }
            else
            {
                fadeManager.StartCoroutine("FadeOutCoroutine");
            }
            Debug.Log("PlayableDirector named " + aDirector.name + " is now stopped.");
        }
    }

    void OnDisable()
    {
        director.stopped -= OnPlayableDirectorStopped;
    }
}
