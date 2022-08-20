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
        //yesman: 레이캐스트로 플레이어를 감지하여 플레이어가 박스 위에 있다면 canPush = false;
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
                //yesman: 바위가 많이 밀리지 않도록 velocity 0
                rigid.velocity = new Vector2(0, 0);
                //yesman: 밀 수 없는 상태라면 return
                if (!canPush)
                    return;
                //yesman: 플레이어가 바위를 잡는 것처럼 보이도록 플레이어의 자식으로 설정함.
                gameObject.transform.SetParent(collision.transform);
                //yesman: 버튼을 뗐다면 자식에서 풀려남
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
        //yesman: 바위에서 벗어난다면 바위가 플레이어의 자식에서 풀려남.
        if (collision.gameObject.tag == "Player" && SkillManager.instance.maskManager.currentMask == MaskManager.Mask.Pig)
        {
            rigid.velocity = new Vector2(0, 0);
            gameObject.transform.SetParent(null);
        }
    }
}
