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
        // J : �ƾ� ������ ���̵�ƿ� �ϱ� ���� �̸� ���̵�Ŵ��� ������Ʈ ������
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
