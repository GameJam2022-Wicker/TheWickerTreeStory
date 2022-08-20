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
    [SerializeField] private List<AudioSource> maskChangeAudioList;
    private Animator animator;
    private int maskNum;
    

    private void Awake()
    {
        animator=GameObject.Find("Player").GetComponent<Animator>();
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
            ChangeMask();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            currentMask++;
            if (currentMask == (Mask)maskNum)
                currentMask = 0;
            ChangeMask();
        }
    }

    private void ChangeMask()
    {
        maskImage.sprite = maskSpriteList[(int)currentMask];

        animator.SetTrigger(currentMask.ToString());
        animator.SetInteger("maskType", (int)currentMask);

        if (maskChangeAudioList[(int)currentMask] != null)
            maskChangeAudioList[(int)currentMask].Play();
    }
}
