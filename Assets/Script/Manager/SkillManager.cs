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

    //yesman: ���� None
    public GameObject signUI;
    public TextMeshProUGUI signTextPro;

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
                if (Input.GetKeyDown(KeyCode.F) && signUI != null)  // J : ǥ������ �ִ� �ʿ��� FŰ ����
                {
                    if (signUI.activeSelf == true)  // J : �̹� ǥ������ ���� �ִ� ���
                    {
                        // J : ǥ���� ����
                        signUI.SetActive(false);
                        Time.timeScale = 1;
                    }
                    else
                        UseSkillNoneMask();
                }
                break;
            case MaskManager.Mask.Owl:
                if (canOwlSkill)
                    maskManager.maskImage.color = new Color(maskManager.maskImage.color.r, maskManager.maskImage.color.g, maskManager.maskImage.color.b, 1);
                else
                    maskManager.maskImage.color = new Color(maskManager.maskImage.color.r, maskManager.maskImage.color.g, maskManager.maskImage.color.b, 0.5f);

                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (owlSkillCoolTime == 0)  // J : ��Ÿ�� �������� ��ų ����
                        StartOwlSkill();
                }
                else if (Input.GetKey(KeyCode.F))
                {
                    if (owlSkillTime > 0)
                        owlSkillTime -= Time.deltaTime;
                    else     // J : ��ų �ð� �������� ���� ����
                        FinishOwlSkill();
                }
                else if (Input.GetKeyUp(KeyCode.F)) // J : ��ų ����
                    FinishOwlSkill();
                break;
            case MaskManager.Mask.Pig:
                //obstacle�� ����
                break;
        }

        UpdateCoolTime();   // J : �ξ��� ��ų ��Ÿ�� ������Ʈ
    }

    // J : �ξ��� ��ų ��Ÿ�� ������Ʈ
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

        Collider2D signCollider = null;  // J : ǥ���� �ݶ��̴�
        if (raycastHit2D.collider != null && raycastHit2D.collider.gameObject.tag == "Sign")
            signCollider = raycastHit2D.collider;
        else if (raycastLeftHit2D.collider != null && raycastLeftHit2D.collider.gameObject.tag == "Sign")
            signCollider = raycastLeftHit2D.collider;

        if (signCollider != null)    // J : ǥ���� üũ�� ���
        {
            signUI.SetActive(true);
            Debug.Log(signCollider.GetComponent<Sign>().id);
            signTextPro.text = DataManager.instance.GetSingStr(signCollider.GetComponent<Sign>().id);   // J : �ش� ǥ���� id�� ���� ����
            Time.timeScale = 0;
            rayColor = Color.green;
        }
        else
            rayColor = Color.red;

        Debug.DrawRay(capsuleCollider2D.bounds.center, Vector2.right * 5f, rayColor);
    }

    // J : �û��� ��ų ����
    private void StartOwlSkill()
    {
        isOwlSkilling = true;
        animator.SetBool("isFlying", isOwlSkilling);
        rigid.gravityScale = 0;

        canOwlSkill = false;
        owlSkillCoolTime = 5.0f;
    }

    // J : �û��� ��ų ����
    private void FinishOwlSkill()
    {
        isOwlSkilling = false;
        animator.SetBool("isFlying", isOwlSkilling);
        owlSkillTime = 0.5f;
    }
}
