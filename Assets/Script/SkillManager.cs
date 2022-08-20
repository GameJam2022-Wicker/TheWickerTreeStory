using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    #region Singleton
    public static SkillManager instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
        }
    }
    #endregion

    [SerializeField] LayerMask signLayerMask;

    //yesman: 가면 None
    public GameObject signUI;
    public Text signText;

    //yesman: 가면 owl
    public bool isOwlSkilling = false;
    private float owlSkillTime = 0.5f;  // 남은 올빼미 스킬 타임
    private float owlSkillCoolTime = 0f;  // 올빼미 스킬 쿨타임

    //yesman: 플레이어 관련
    public PlayerAction player;
    Rigidbody2D rigid;
    CapsuleCollider2D capsuleCollider2D;
    Animator animator;

    private bool canOwlSkill = true;    // 올빼미 스킬 사용 가능 여부

    public MaskManager maskManager;

    //최초에는 벗은 모습
    MaskManager.Mask mask;

    private void Start()
    {
        maskManager = gameObject.GetComponent<MaskManager>();
        capsuleCollider2D = player.gameObject.GetComponent<CapsuleCollider2D>();
        rigid = player.gameObject.GetComponent<Rigidbody2D>();
        animator = player.GetComponent<Animator>();
    }

    private void Update()
    {
        switch(maskManager.currentMask)
        {
            case MaskManager.Mask.None:
                if(signUI != null && signUI.activeSelf == true)
                {
                    if(Input.GetKeyDown(KeyCode.F))
                    {
                        signUI.SetActive(false);
                        Time.timeScale = 1;
                    }                        
                }
                else if (Input.GetKeyDown(KeyCode.F))
                    UseSkillNoneMask();
                break;
            case MaskManager.Mask.Owl:
                if (canOwlSkill)
                    maskManager.maskImage.color = new Color(maskManager.maskImage.color.r, maskManager.maskImage.color.g, maskManager.maskImage.color.b, 1);
                else
                    maskManager.maskImage.color = new Color(maskManager.maskImage.color.r, maskManager.maskImage.color.g, maskManager.maskImage.color.b, 0.5f);

                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (owlSkillCoolTime == 0)  // 쿨타임 지났으면 스킬 시작
                        StartOwlSkill();
                }
                else if (Input.GetKey(KeyCode.F))
                {
                    if (owlSkillTime > 0)
                        owlSkillTime -= Time.deltaTime;
                    else     // 스킬 시간 끝났으면 강제 종료
                        FinishOwlSkill();
                }
                else if (Input.GetKeyUp(KeyCode.F)) // 스킬 종료
                    FinishOwlSkill();
                else    // 쿨타임 업데이트
                    UpdateCoolTime();
                break;
            case MaskManager.Mask.Pig:
                //obstacle에 구현
                break;
        }
    }

    private void UpdateCoolTime()
    {
        owlSkillCoolTime -= Time.deltaTime;
        if (owlSkillCoolTime < 0)
        {
            owlSkillCoolTime = 0;
            canOwlSkill = true;
        }
    }

    //yesman: 가면을 쓰지 않았을 때 표지판 체크 가능
    void UseSkillNoneMask()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(capsuleCollider2D.bounds.center, Vector2.right, 5f, signLayerMask);
        RaycastHit2D raycastLeftHit2D = Physics2D.Raycast(capsuleCollider2D.bounds.center, Vector2.left, 5f, signLayerMask);
        Color rayColor;
        if(raycastHit2D.collider!= null && raycastHit2D.collider.gameObject.tag == "Sign" ||
            raycastLeftHit2D.collider != null && raycastLeftHit2D.collider.gameObject.tag == "Sign")
        {
            signUI.SetActive(true);
            if(raycastHit2D.collider != null)
                signText.text = raycastHit2D.collider.GetComponent<Sign>().signStr;
            if (raycastLeftHit2D.collider != null)
                signText.text = raycastLeftHit2D.collider.GetComponent<Sign>().signStr;
            Time.timeScale = 0;
            rayColor = Color.green;
        }
        else
            rayColor = Color.red;
        Debug.DrawRay(capsuleCollider2D.bounds.center, Vector2.right * 5f, rayColor);
    }

    // 올빼미 스킬 시작
    private void StartOwlSkill()
    {
        isOwlSkilling = true;
        animator.SetBool("isFlying", isOwlSkilling);
        rigid.gravityScale = 0;

        canOwlSkill = false;
        owlSkillCoolTime = 5.0f;
    }

    // 올빼미 스킬 종료
    private void FinishOwlSkill()
    {
        isOwlSkilling = false;
        animator.SetBool("isFlying", isOwlSkilling);
        owlSkillTime = 0.5f;
    }
}
