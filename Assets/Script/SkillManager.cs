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

    //yesman: ���� None
    public GameObject signUI;

    //yesman: ���� owl
    public bool isOwlSkilling = false;
    private float owlSkillTime = 5.0f;  // ���� �û��� ��ų Ÿ��
    private float owlSkillCoolTime = 0f;  // �û��� ��ų ��Ÿ��

    //yesman: �÷��̾� ����
    public PlayerAction player;
    Rigidbody2D rigid;
    CapsuleCollider2D capsuleCollider2D;

    public MaskManager maskManager;

    //���ʿ��� ���� ���
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
                {
                    FinishOwlSkill();
                }
                else    // ��Ÿ�� ������Ʈ
                {
                    owlSkillCoolTime -= Time.deltaTime;
                    if (owlSkillCoolTime < 0)
                        owlSkillCoolTime = 0;
                }
                break;
            case MaskManager.Mask.Pig:
                //obstacle�� ����
                break;
        }
    }

    //yesman: ������ ���� �ʾ��� �� ǥ���� üũ ����
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

    // �û��� ��ų ����
    private void StartOwlSkill()
    {
        isOwlSkilling = true;
        rigid.gravityScale = 0;
        owlSkillCoolTime = 5.0f;
    }

    // �û��� ��ų ����
    private void FinishOwlSkill()
    {
        isOwlSkilling = false;
        owlSkillTime = 5.0f;
    }
}
