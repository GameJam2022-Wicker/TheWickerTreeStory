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

    //yesman: ���� None
    public GameObject signUI;
    public Text signText;

    //yesman: ���� owl
    public bool isOwlSkilling = false;
    private float owlSkillTime = 0.5f;  // ���� �û��� ��ų Ÿ��
    private float owlSkillCoolTime = 0f;  // �û��� ��ų ��Ÿ��

    //yesman: �÷��̾� ����
    public PlayerAction player;
    Rigidbody2D rigid;
    CapsuleCollider2D capsuleCollider2D;
    Animator animator;

    private bool canOwlSkill = true;    // �û��� ��ų ��� ���� ����

    public MaskManager maskManager;

    //���ʿ��� ���� ���
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
                    if (owlSkillCoolTime == 0)  // ��Ÿ�� �������� ��ų ����
                        StartOwlSkill();
                }
                else if (Input.GetKey(KeyCode.F))
                {
                    if (owlSkillTime > 0)
                        owlSkillTime -= Time.deltaTime;
                    else     // ��ų �ð� �������� ���� ����
                        FinishOwlSkill();
                }
                else if (Input.GetKeyUp(KeyCode.F)) // ��ų ����
                    FinishOwlSkill();
                else    // ��Ÿ�� ������Ʈ
                    UpdateCoolTime();
                break;
            case MaskManager.Mask.Pig:
                //obstacle�� ����
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

    //yesman: ������ ���� �ʾ��� �� ǥ���� üũ ����
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

    // �û��� ��ų ����
    private void StartOwlSkill()
    {
        isOwlSkilling = true;
        animator.SetBool("isFlying", isOwlSkilling);
        rigid.gravityScale = 0;

        canOwlSkill = false;
        owlSkillCoolTime = 5.0f;
    }

    // �û��� ��ų ����
    private void FinishOwlSkill()
    {
        isOwlSkilling = false;
        animator.SetBool("isFlying", isOwlSkilling);
        owlSkillTime = 0.5f;
    }
}
