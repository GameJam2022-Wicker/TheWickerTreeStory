using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Rigidbody2D rigid;
    BoxCollider2D boxCollider;
    bool canPush;

    private void Start() 
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        //yesman: ����ĳ��Ʈ�� �÷��̾ �����Ͽ� �÷��̾ �ڽ� ���� �ִٸ� canPush = false;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(boxCollider.bounds.center, Vector2.up, boxCollider.bounds.extents.y);
        Color rayColor;
        if (raycastHit2D.collider != null)
        {
            rayColor = Color.green;
            canPush = false;
        }
        else
        {
            rayColor = Color.red;
            canPush = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && SkillManager.instance.maskManager.currentMask != MaskManager.Mask.Pig)
        {
            rigid.bodyType = RigidbodyType2D.Kinematic;
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && SkillManager.instance.maskManager.currentMask == MaskManager.Mask.Pig)
        {
            if (Input.GetKey(KeyCode.F))
            {
                rigid.bodyType = RigidbodyType2D.Dynamic;
                rigid.constraints = RigidbodyConstraints2D.None;
                rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                //yesman: ������ ���� �и��� �ʵ��� velocity 0
                rigid.velocity = new Vector2(0, 0);
                //yesman: �� �� ���� ���¶�� return
                if (!canPush)
                    return;
                //yesman: �÷��̾ ������ ��� ��ó�� ���̵��� �÷��̾��� �ڽ����� ������.
                gameObject.transform.SetParent(collision.transform);
                //yesman: ��ư�� �ôٸ� �ڽĿ��� Ǯ����
                if (Input.GetButtonUp("Horizontal"))
                {
                    gameObject.transform.SetParent(null);
                }
            }
            else
            {
                rigid.bodyType = RigidbodyType2D.Kinematic;
                rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //yesman: �������� ����ٸ� ������ �÷��̾��� �ڽĿ��� Ǯ����.
        if (collision.gameObject.tag == "Player" && SkillManager.instance.maskManager.currentMask == MaskManager.Mask.Pig)
        {
            rigid.velocity = new Vector2(0, 0);
            gameObject.transform.SetParent(null);
        }
    }
}
