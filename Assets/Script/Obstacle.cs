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
        {
            canPush = false;
        }
        else
        {
            canPush = true;
        }

        if (raycastHit2DDown.collider == null)
        {
            rigid.bodyType = RigidbodyType2D.Dynamic;
            rigid.constraints = RigidbodyConstraints2D.None;
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            gameObject.transform.SetParent(null);
        }

        Debug.DrawRay(boxCollider.bounds.center, Vector2.down * (boxCollider.bounds.extents.y), Color.red);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && SkillManager.instance.maskManager.currentMask != MaskManager.Mask.Pig)
        {
            //kinematic으로 하여 움직이지 않도록 함.
            rigid.bodyType = RigidbodyType2D.Kinematic;
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && SkillManager.instance.maskManager.currentMask == MaskManager.Mask.Pig)
        {
            if (Input.GetKey(KeyCode.F) && canPush) // 밀 수 있는 상태
            {
                rigid.bodyType = RigidbodyType2D.Dynamic;
                rigid.constraints = RigidbodyConstraints2D.None;
                rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                //yesman: 바위가 많이 밀리지 않도록 velocity 0, 무겁게 밀리도록 스피드 줄임
                collision.gameObject.GetComponent<PlayerAction>().maxSpeed = 3f;
                rigid.velocity = new Vector2(0, 0);

                animator.SetBool("isPushing", true);    // 바위 밀기 애니메이션 재생
                if (!pushSound.isPlaying)   // 바위 미는 소리 재생
                    pushSound.Play();
            }
            else
            {
                rigid.bodyType = RigidbodyType2D.Kinematic;
                rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
                collision.gameObject.GetComponent<PlayerAction>().maxSpeed = 6f;

                animator.SetBool("isPushing", false);   // 바위 밀기 애니메이션 중지
                pushSound.Stop();   // 바위 미는 소리 중지
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //yesman: 바위에서 벗어난다면 바위가 플레이어의 자식에서 풀려남.
        if (collision.gameObject.tag == "Player" && SkillManager.instance.maskManager.currentMask == MaskManager.Mask.Pig)
        {
            rigid.velocity = new Vector2(0, 0);
        }
    }
}
