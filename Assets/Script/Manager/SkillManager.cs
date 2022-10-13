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

    public bool isOwlSkilling, isPigSkilling = false;   // J : �ɷ� ��� �� ����
    private float owlSkillTime = 0.5f;  // J : ���� �û��� ��ų Ÿ��
    private float owlSkillCoolTime = 0f;  // J : �û��� ��ų ��Ÿ��

    //yesman: �÷��̾� ����
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
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (owlSkillCoolTime == 0)  // J : ��Ÿ�� �������� ��ų ����
                        StartOwlSkill();
                }
                else if (Input.GetKey(KeyCode.F))
                {
                    if (isOwlSkilling)
                    {
                        if (owlSkillTime > 0)
                            owlSkillTime -= Time.deltaTime;
                        else     // J : ��ų �ð� �������� ���� ����
                            FinishOwlSkill();
                    }
                }
                else if (Input.GetKeyUp(KeyCode.F)) // J : ��ų ����
                    FinishOwlSkill();
                break;
            case MaskManager.Mask.Pig:
                //obstacle�� ����
                break;
        }

        if (maskManager.canChangeMask) UpdateCoolTime();   // J : �ξ��� ��ų ��Ÿ�� ������Ʈ
    }

    // J : �ξ��� ��ų ��Ÿ�� ������Ʈ
    private void UpdateCoolTime()
    {
        owlSkillCoolTime -= Time.deltaTime;
        if (owlSkillCoolTime < 0)
            owlSkillCoolTime = 0;

        maskManager.fillImage.fillAmount = owlSkillCoolTime / 5;
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

            string id = signCollider.GetComponent<Sign>().id;  // J : ǥ���� id Ȯ��
            int orderNum = signCollider.GetComponent<Sign>().orderNum;
            signTextPro.text = TableData.instance.GetScript("Borad", id);   // J : �ش� id�� ǥ���� ���� ����
            DataManager.instance.data.sign[orderNum] = true; ; // J : ǥ���� ���� ����

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
    }

    // J : �û��� ��ų ����
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
