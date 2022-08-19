using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskManager : MonoBehaviour
{
    public enum Mask
    {
        None,
        Pig,
        Owl,
    }
    public Mask currentMask;

    [SerializeField] private Image maskImage; 
    [SerializeField] private List<Sprite> maskSpriteList;
    private int maskNum;
    

    private void Awake()
    {
        maskNum = System.Enum.GetValues(typeof(Mask)).Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentMask--;
            if (currentMask < 0)
                currentMask = (Mask)(maskNum - 1);
            SetMaskImage();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            currentMask++;
            if (currentMask == (Mask)maskNum)
                currentMask = 0;
            SetMaskImage();
        }
    }

    private void SetMaskImage()
    {
        maskImage.sprite = maskSpriteList[(int)currentMask];
    }
}