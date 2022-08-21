using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    bool isBelieving;
    public CutScene cutScene;

    public List<Sprite> realEndingList;
    public List<Sprite> badEndingLIst;

    public AudioSource bgm;
    public AudioClip realBGM, badBGM;

    private void Start()
    {
        isBelieving = DataManager.instance.isBelieving;

        if(isBelieving)
        {
            bgm.clip = realBGM;
            cutScene.imageList = realEndingList;
            cutScene.imageNum = realEndingList.Count;
        }
        else
        {
            bgm.clip = badBGM;
            cutScene.imageList = badEndingLIst;
            cutScene.imageNum = badEndingLIst.Count;
        }

        bgm.Play();
    }
}
