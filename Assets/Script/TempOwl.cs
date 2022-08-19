using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempOwl : MonoBehaviour
{
    Rigidbody2D rigid;
    public bool isOwlSkilling = false;
    private float owlSkillTime = 5.0f;  // ���� �û��� ��ų Ÿ��
    private float owlSkillCoolTime = 0f;  // �û��� ��ų ��Ÿ��

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
