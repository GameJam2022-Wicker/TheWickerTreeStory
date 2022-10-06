using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public PlayableDirector director;
    private FadeManager fadeManager;

    private void Awake()
    {
        // J : 컷씬 끝나면 페이드아웃 하기 위해 미리 페이드매니저 컴포넌트 가져옴
        fadeManager = GameObject.Find("FadeManager").GetComponent<FadeManager>();
    }

    public void OnClickSkipButton()
    {
        int idx = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(idx);
    }

    void OnEnable()
    {
        director.stopped += OnPlayableDirectorStopped;
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
        {
            Debug.Log("PlayableDirector named " + aDirector.name + " is now stopped.");
            fadeManager.StartCoroutine("FadeOutCoroutine");
        }
    }

    void OnDisable()
    {
        director.stopped -= OnPlayableDirectorStopped;
    }
}
