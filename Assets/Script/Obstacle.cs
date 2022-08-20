using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Rigidbody2D rigid;
    BoxCollider2D boxCollider;
    bool canPush;
    public LayerMask playerLayerMask;
    public LayerMask tileLayerMask;

    private void Start() 
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        //yesman: ����ĳ��Ʈ�� �÷��̾ �����Ͽ� �÷��̾ �ڽ� ���� �ִٸ� canPush = false;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(boxCollider.bounds.center, Vector2.up, boxCollider.bounds.extents.y, playerLayerMask);
        RaycastHit2D raycastHit2DDown = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y, tileLayerMask);

        if (raycastHit2D.collider != null)
        {
            canPush = false;
        }
        else
        {
            canPush = true;
        }

        if(raycastHit2DDown.collider == null)
        {
            rigid.bodyType = RigidbodyType2D.Dynamic;
            rigid.constraints = RigidbodyConstraints2D.None;
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            gameObject.transform.SetParent(null);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && SkillManager.instance.maskManager.currentMask != MaskManager.Mask.Pig)
        {
            //kinematic���� �Ͽ� �������� �ʵ��� ��.
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
                //yesman: ������ ���� �и��� �ʵ��� velocity 0, ���̰� �и����� ���ǵ� ����
                collision.gameObject.GetComponent<PlayerAction>().maxSpeed = 3f;
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
                gameObject.transform.SetParent(null);
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
