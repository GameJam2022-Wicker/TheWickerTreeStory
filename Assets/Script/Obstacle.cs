using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Rigidbody2D rigid;
    BoxCollider2D boxCollider;
    Animator animator;  // 플레이어 애니메이터
    AudioSource pushSound;  // 바위 미는 소리
    bool canPush;
    public LayerMask playerLayerMask;
    public LayerMask tileLayerMask;

    private void Start() 
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        animator = GameObject.Find("Player").GetComponent<Animator>();
        pushSound = GameObject.Find("Player").GetComponent<AudioSource>();
    }

    private void Update()
    {
        //yesman: 레이캐스트로 플레이어를 감지하여 플레이어가 박스 위에 있다면 canPush = false;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(boxCollider.bounds.center, Vector2.up, boxCollider.bounds.extents.y, playerLayerMask);
        RaycastHit2D raycastHit2DDown = Physics2D.Raycast(gameObject.transform.position, Vector2.down, 1f, tileLayerMask);

        if (raycastHit2D.collider != null)
            canPush = false;
        else
            canPush = true;

        if (raycastHit2DDown.collider == null)
            Move();

        //Debug.DrawRay(boxCollider.bounds.center, Vector2.down * (boxCollider.bounds.extents.y), Color.red);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && SkillManager.instance.maskManager.currentMask == MaskManager.Mask.Pig)
        {
            if (Input.GetKey(KeyCode.F) && canPush) // 밀 수 있는 상태
                Push();
            else
                StopPush();
        }
    }

    //바위 미는 애니메이션 계속 재생되는 것 방지
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && SkillManager.instance.maskManager.currentMask == MaskManager.Mask.Pig)
        {
            StopPush();
        }
    }

    // 바위 움직임 가능
    private void Move()
    {
        rigid.bodyType = RigidbodyType2D.Dynamic;
        rigid.constraints = RigidbodyConstraints2D.None;
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // 바위 움직임 불가능
    private void DontMove()
    {
        rigid.bodyType = RigidbodyType2D.Kinematic;
        rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    // 바위 밀기
    private void Push()
    {
        Move();

        animator.SetBool("isPushing", true);    // 바위 밀기 애니메이션 재생
        if (!pushSound.isPlaying)   // 바위 미는 소리 재생
            pushSound.Play();
    }

    // 바위 밀기 중지
    private void StopPush()
    {
        DontMove();

        animator.SetBool("isPushing", false);   // 바위 밀기 애니메이션 중지
        pushSound.Stop();   // 바위 미는 소리 중지
    }
}