using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public TextMeshProUGUI signTextPro;

    public bool isOwlSkilling, isPigSkilling = false;   // J : 능력 사용 중 여부
    private float owlSkillTime = 0.5f;  // J : 남은 올빼미 스킬 타임
    private float owlSkillCoolTime = 0f;  // J : 올빼미 스킬 쿨타임

    //yesman: 플레이어 관련
    public PlayerAction player;
    Rigidbody2D rigid;
    CapsuleCollider2D capsuleCollider2D;
    Animator animator;

    public MaskManager maskManager;

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
                if (Input.GetKeyDown(KeyCode.F) && signUI != null)  // J : 표지판이 있는 맵에서 F키 누름
                {
                    if (signUI.activeSelf == true)  // J : 이미 표지판을 보고 있는 경우
                    {
                        // J : 표지판 닫음
                        signUI.SetActive(false);
                        Time.timeScale = 1;
                    }
                    else
                        UseSkillNoneMask();
                }
                break;
            case MaskManager.Mask.Owl:
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (owlSkillCoolTime == 0)  // J : 쿨타임 지났으면 스킬 시작
                        StartOwlSkill();
                }
                else if (Input.GetKey(KeyCode.F))
                {
                    if (isOwlSkilling)
                    {
                        if (owlSkillTime > 0)
                            owlSkillTime -= Time.deltaTime;
                        else     // J : 스킬 시간 끝났으면 강제 종료
                            FinishOwlSkill();
                    }
                }
                else if (Input.GetKeyUp(KeyCode.F)) // J : 스킬 종료
                    FinishOwlSkill();
                break;
            case MaskManager.Mask.Pig:
                //obstacle에 구현
                break;
        }

        if (maskManager.canChangeMask) UpdateCoolTime();   // J : 부엉이 스킬 쿨타임 업데이트
    }

    // J : 부엉이 스킬 쿨타임 업데이트
    private void UpdateCoolTime()
    {
        owlSkillCoolTime -= Time.deltaTime;
        if (owlSkillCoolTime < 0)
            owlSkillCoolTime = 0;

        maskManager.fillImage.fillAmount = owlSkillCoolTime / 5;
    }

    //yesman: 가면을 쓰지 않았을 때 표지판 체크 가능
    void UseSkillNoneMask()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(capsuleCollider2D.bounds.center, Vector2.right, 5f, signLayerMask);
        RaycastHit2D raycastLeftHit2D = Physics2D.Raycast(capsuleCollider2D.bounds.center, Vector2.left, 5f, signLayerMask);
        Color rayColor;

        Collider2D signCollider = null;  // J : 표지판 콜라이더
        if (raycastHit2D.collider != null && raycastHit2D.collider.gameObject.tag == "Sign")
            signCollider = raycastHit2D.collider;
        else if (raycastLeftHit2D.collider != null && raycastLeftHit2D.collider.gameObject.tag == "Sign")
            signCollider = raycastLeftHit2D.collider;

        if (signCollider != null)    // J : 표지판 체크된 경우
        {
            signUI.SetActive(true);

            string id = signCollider.GetComponent<Sign>().id;  // J : 표지판 id 확인
            int orderNum = signCollider.GetComponent<Sign>().orderNum;
            signTextPro.text = TableData.instance.GetScript("Borad", id);   // J : 해당 id의 표지판 내용 세팅
            DataManager.instance.data.sign[orderNum] = true; ; // J : 표지판 읽음 저장

            Time.timeScale = 0;
            rayColor = Color.green;
        }
        else
            rayColor = Color.red;

        Debug.DrawRay(capsuleCollider2D.bounds.center, Vector2.right * 5f, rayColor);
    }

    // J : 올빼미 스킬 시작
    private void StartOwlSkill()
    {
        isOwlSkilling = true;
        animator.SetBool("isFlying", isOwlSkilling);
        rigid.gravityScale = 0;
    }

    // J : 올빼미 스킬 종료
    private void FinishOwlSkill()
    {
        if (isOwlSkilling)
        {
            isOwlSkilling = false;
            animator.SetBool("isFlying", isOwlSkilling);
            owlSkillTime = 0.5f;
            owlSkillCoolTime = 5.0f;
        }
    }
}
