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

    public Image fillImage;
    public bool canChangeMask;    // 가면 변경 가능 여부 (Map_03 -> false)
    [SerializeField] private Image maskImage;
    [SerializeField] private List<Sprite> maskSpriteList;
    [SerializeField] private List<AudioSource> maskChangeAudioList;
    [SerializeField] private int maskNum;           // 가면 개수
    private Animator animator;
    

    private void Awake()
    {
        animator=GameObject.Find("Player").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canChangeMask & CanChangeMask())
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
    }

    // J : 가면 능력 사용 중이면 가면 변경 불가능
    private bool CanChangeMask()
    {
        return (!SkillManager.instance.isPigSkilling && !SkillManager.instance.isOwlSkilling);
    }

    private void ChangeMask()
    {
        maskImage.sprite = maskSpriteList[(int)currentMask];

        animator.SetTrigger(currentMask.ToString());
        animator.SetInteger("maskType", (int)currentMask);

        if (maskChangeAudioList[(int)currentMask] != null)
            maskChangeAudioList[(int)currentMask].Play();

        if (currentMask == Mask.Owl)
            fillImage.gameObject.SetActive(true);
        else
            fillImage.gameObject.SetActive(false);
    }
}
