using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Rigidbody2D rigid;
    BoxCollider2D boxCollider;
    Animator animator;  // �÷��̾� �ִϸ�����
    AudioSource pushSound;  // ���� �̴� �Ҹ�
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
        //yesman: ����ĳ��Ʈ�� �÷��̾ �����Ͽ� �÷��̾ �ڽ� ���� �ִٸ� canPush = false;
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
            if (Input.GetKey(KeyCode.F) && canPush) // �� �� �ִ� ����
                Push();
            else
                StopPush();
        }
    }

    //���� �̴� �ִϸ��̼� ��� ����Ǵ� �� ����
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && SkillManager.instance.maskManager.currentMask == MaskManager.Mask.Pig)
        {
            StopPush();
        }
    }

    // ���� ������ ����
    private void Move()
    {
        rigid.bodyType = RigidbodyType2D.Dynamic;
        rigid.constraints = RigidbodyConstraints2D.None;
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // ���� ������ �Ұ���
    private void DontMove()
    {
        rigid.bodyType = RigidbodyType2D.Kinematic;
        rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    // ���� �б�
    private void Push()
    {
        Move();

        animator.SetBool("isPushing", true);    // ���� �б� �ִϸ��̼� ���
        if (!pushSound.isPlaying)   // ���� �̴� �Ҹ� ���
            pushSound.Play();
    }

    // ���� �б� ����
    private void StopPush()
    {
        DontMove();

        animator.SetBool("isPushing", false);   // ���� �б� �ִϸ��̼� ����
        pushSound.Stop();   // ���� �̴� �Ҹ� ����
    }
}