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
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    [SerializeField] LayerMask signLayerMask;

    //yesman: 가면 None
    public GameObject signUI;

    //yesman: 가면 owl
    public bool isOwlSkilling = false;
    private float owlSkillTime = 5.0f;  // 남은 올빼미 스킬 타임
    private float owlSkillCoolTime = 0f;  // 올빼미 스킬 쿨타임

    //yesman: 플레이어 관련
    public PlayerAction player;
    Rigidbody2D rigid;
    CapsuleCollider2D capsuleCollider2D;

    public MaskManager maskManager;

    //최초에는 벗은 모습
    MaskManager.Mask mask;

    private void Start()
    {
        maskManager = gameObject.GetComponent<MaskManager>();
        capsuleCollider2D = player.gameObject.GetComponent<CapsuleCollider2D>();
        rigid = player.gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        switch(maskManager.currentMask)
        {
            case MaskManager.Mask.None:
                if(Input.GetKeyDown(KeyCode.F))
                    UseSkillNoneMask();
                break;
            case MaskManager.Mask.Owl:
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
                {
                    FinishOwlSkill();
                }
                else    // 쿨타임 업데이트
                {
                    owlSkillCoolTime -= Time.deltaTime;
                    if (owlSkillCoolTime < 0)
                        owlSkillCoolTime = 0;
                }
                break;
            case MaskManager.Mask.Pig:
                //obstacle에 구현
                break;
        }
    }

    //yesman: 가면을 쓰지 않았을 때 표지판 체크 가능
    void UseSkillNoneMask()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(capsuleCollider2D.bounds.center, Vector2.right, 5f, signLayerMask);
        Color rayColor;
        if(raycastHit2D.collider!= null && raycastHit2D.collider.gameObject.tag == "Sign")
        {
            signUI.SetActive(true);
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
        rigid.gravityScale = 0;
        owlSkillCoolTime = 5.0f;
    }

    // 올빼미 스킬 종료
    private void FinishOwlSkill()
    {
        isOwlSkilling = false;
        owlSkillTime = 5.0f;
    }
}
