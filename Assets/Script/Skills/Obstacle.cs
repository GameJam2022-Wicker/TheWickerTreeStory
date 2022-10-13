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
        //yesman: ����ĳ��Ʈ�� �÷��̾ �����Ͽ� �÷��̾ �ڽ� ���� �ִٸ� �б� �Ұ���
        RaycastHit2D raycastHit2D = Physics2D.Raycast(boxCollider.bounds.center, Vector2.up, boxCollider.bounds.extents.y + 0.2f, playerLayerMask);
        // J : �̹� Ÿ�� ���� ��� boxCollider.bounds.extents.y�δ� Ÿ�� ������ ���ϹǷ� offset �߰�
        RaycastHit2D raycastHit2DDown = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + 0.2f, tileLayerMask);
        // J : ������ �����ؾ� Ÿ�� ���� ����
        RaycastHit2D raycastHit2DDrop = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y, tileLayerMask);

        
        if (Input.GetKey(KeyCode.F) && raycastHit2D.collider == null)
            canPush = true;
        else
            canPush = false;

        // J : �������� ��
        if (raycastHit2DDown.collider == null)
            Move();
        // J : ������ ������ Ÿ�� ���� �����ϸ� �ٽ� �б� �Ұ��� (Kinematic���� ����)
        else if (!canPush && rigid.bodyType == RigidbodyType2D.Dynamic && raycastHit2DDrop.collider != null)
            DontMove();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" 
            && SkillManager.instance.maskManager.currentMask == MaskManager.Mask.Pig
            && canPush)
        {
            Push();
        }
    }

    //���� �̴� �ִϸ��̼� ��� ����Ǵ� �� ����
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && SkillManager.instance.maskManager.currentMask == MaskManager.Mask.Pig)
            StopPush();
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

        SkillManager.instance.isPigSkilling = true;
    }

    // ���� �б� ����
    private void StopPush()
    {
        DontMove();

        animator.SetBool("isPushing", false);   // ���� �б� �ִϸ��̼� ����
        pushSound.Stop();   // ���� �̴� �Ҹ� ����

        SkillManager.instance.isPigSkilling = false;
    }
}