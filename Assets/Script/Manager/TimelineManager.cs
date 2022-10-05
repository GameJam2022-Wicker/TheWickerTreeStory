using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public PlayableDirector director;

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
            int idx = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(idx);
        }
    }

    void OnDisable()
    {
        director.stopped -= OnPlayableDirectorStopped;
    }
}
