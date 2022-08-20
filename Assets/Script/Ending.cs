using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    bool isBelieving;
    public CutScene cutScene;

    public List<Sprite> realEndingList;
    public List<Sprite> badEndingLIst;

    private void Start()
    {
        isBelieving = DataManager.instance.isBelieving;

        if(isBelieving)
        {
            cutScene.imageList = realEndingList;
            cutScene.imageNum = realEndingList.Count;
        }
        else
        {
            cutScene.imageList = badEndingLIst;
            cutScene.imageNum = badEndingLIst.Count;
        }

    }
}
