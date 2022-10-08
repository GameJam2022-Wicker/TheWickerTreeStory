using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempOwl : MonoBehaviour
{
    Rigidbody2D rigid;
    public bool isOwlSkilling = false;
    private float owlSkillTime = 5.0f;  // 남은 올빼미 스킬 타임
    private float owlSkillCoolTime = 0f;  // 올빼미 스킬 쿨타임

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
