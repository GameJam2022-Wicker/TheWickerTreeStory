using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicSources;

    public AudioClip mainmenu;
    public AudioClip cutscene1;
    public AudioClip cutscene2;
    public AudioClip cutscene3;
    public AudioClip map1;
    public AudioClip map1_5;
    public AudioClip map2;
    public AudioClip map2_5;
    public AudioClip map3;
    public AudioClip happyEnding;
    public AudioClip badEnding;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        musicSources.clip = clip;
        musicSources.loop = true;
        musicSources.Play();

        StartCoroutine(AnimateMusicCrossfadeIn(fadeDuration));
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                PlayMusic(mainmenu, 2);
                break;
            case 1:
                PlayMusic(cutscene1, 2);
                break;
            case 2:
            case 3:
                PlayMusic(map1, 2);
                break;
            case 4:
                PlayMusic(map1_5, 2);
                break;
            case 5:
                PlayMusic(cutscene2, 2);
                break;
            case 6:
                PlayMusic(map2, 2);
                break;
            case 7:
                PlayMusic(map2_5, 2);
                break;
            case 8:
                PlayMusic(cutscene3, 2);
                break;
            case 9:
                PlayMusic(map3, 2);
                break;                
            case 10:
                if (DataManager.instance.isBelieving)
                    PlayMusic(happyEnding, 2);
                else
                    PlayMusic(badEnding, 2);
                break;
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    IEnumerator AnimateMusicCrossfadeIn(float duration)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSources.volume = Mathf.Lerp(0, 1, percent);
            yield return null;
        }
    }

    public IEnumerator AnimateMusicCrossfadeOut(float duration)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSources.volume = Mathf.Lerp(1, 0, percent);
            yield return null;
        }
    }
}
