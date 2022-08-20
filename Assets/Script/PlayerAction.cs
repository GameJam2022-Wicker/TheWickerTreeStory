using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAction : MonoBehaviour
{
    public float gravityScale;
    public float fallGravityMultiflier;

    public float maxSpeed;
    public float jumpPower;
    public float ladderSpeed;
    public bool isHurting = false;

    Rigidbody2D rigid;
    bool facingRight = true;
    bool isJumping = false;
    bool isLadder;
    bool isClimbing;

    private GameObject ladder;  // 현재 플레이어가 타고 있는 사다리

    private SkillManager skillManager;
    private CapsuleCollider2D capsuleCollider2D;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Color materialTintColor;
    private LayerMask ladderLayer, tileLayer, playerLayer, portalLayer, obstacleLayerMask;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        ladderLayer = LayerMask.NameToLayer("Ladder");
        tileLayer = LayerMask.NameToLayer("Tile");
        playerLayer = LayerMask.NameToLayer("Player");
        portalLayer = LayerMask.NameToLayer("Portal");
        obstacleLayerMask = LayerMask.NameToLayer("Obstacle");

        skillManager = GameObject.Find("SkillManager").GetComponent<SkillManager>();
    }


    void Update()
    {
        animator.speed = 1;

        if (!skillManager.isOwlSkilling)
        {
            if (IsGrounded())
            {
                Physics2D.IgnoreLayerCollision(playerLayer, tileLayer, false);
                rigid.gravityScale = gravityScale;
            }
            else
            {
                rigid.gravityScale = gravityScale * fallGravityMultiflier;
            }
        }

        if (isLadder)   // 사다리 앞에 있는 경우
        {
            // 수직 방향키 입력 감지
            Vector3 horizontalMove = Vector3.zero;
            if (Input.GetKey(KeyCode.UpArrow))
                horizontalMove = Vector3.up;
            else if (Input.GetKey(KeyCode.DownArrow))
                horizontalMove = Vector3.down;

            if (horizontalMove!= Vector3.zero)  // 수직 방향키 입력이 있다면
            {
                rigid.gravityScale = 0; // 중력 X
                isClimbing = true;
                animator.SetBool("isClimbing", isClimbing);
                transform.position = new Vector3(ladder.transform.position.x, transform.position.y, transform.position.z);  // 플레이어를 사다리 가운데로 정렬
                transform.position += horizontalMove * ladderSpeed * Time.deltaTime;    // 플레이어 이동
                Physics2D.IgnoreLayerCollision(playerLayer, tileLayer, true); // 타일과의 충돌 처리 X
            }
            else if (isClimbing)    // 사다리를 타고 있지만 움직이지 않는 경우
            {
                rigid.gravityScale = 0; // 중력 X
                animator.speed = 0; // 애니메이션 정지
            }
        }

        //Jump
        if (Input.GetButtonDown("Jump") && IsGrounded() && !isClimbing && !skillManager.isOwlSkilling) // 사다리를 타는 중, 올빼미 스킬 사용 중에는 점프 불가능
        {
            isJumping = true;
            animator.SetBool("isJumping", true);
            rigid.velocity = Vector2.up * jumpPower;
        }
        /*
        if (Input.GetButtonUp("Jump") && rigid.velocity.y > 0f)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * 0.5f);
        }
        */
        //Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
    }

    void FixedUpdate()
    {
        //Move By Key Control
        //float moveInput = Input.GetAxisRaw("Horizontal");

        /*rigid.AddForce(Vector2.right * moveInput, ForceMode2D.Impulse);
        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1))
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);*/

        // 사다리를 타고 있으면서 땅에 있지 않은 경우 수평 이동 불가능
        if (!isClimbing || isClimbing && IsGrounded())
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            if (moveInput != 0)
                animator.SetBool("isMoving", true);
            else
                animator.SetBool("isMoving", false);

            rigid.velocity = new Vector2(moveInput * maxSpeed, rigid.velocity.y);

            if (moveInput > 0 && !facingRight)
            {
                Flip();
            }
            else if (moveInput < 0 && facingRight)
            {
                Flip();
            }
        }
    }

    // 사다리 진입
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == ladderLayer)  // 사다리
        {
            ladder = collision.gameObject;
            isLadder = true;
            capsuleCollider2D.size = new Vector2(capsuleCollider2D.size.x / 2, capsuleCollider2D.size.y);
        }
        if (collision.gameObject.layer == portalLayer)  // 포탈
        {
            int currentSceneNumber = SceneManager.GetActiveScene().buildIndex;  // 현재 scene number 가져오기
            SceneManager.LoadScene(++currentSceneNumber);   // 다음 scene으로 이동
        }
    }

    // 사다리 나가기
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == ladderLayer)  // 사다리
        {
            isLadder = isClimbing = false;
            animator.SetBool("isClimbing", isClimbing);
            capsuleCollider2D.size = new Vector2(capsuleCollider2D.size.x * 2, capsuleCollider2D.size.y);
        }
    }


    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }

    private bool IsGrounded()
    {
        float extraHeightText = .3f;

        int layerMask = (1 << tileLayer) + (1 << obstacleLayerMask);    // Player 와 MyTeammate 레이어만 충돌체크함
        RaycastHit2D raycastHit = Physics2D.Raycast(capsuleCollider2D.bounds.center, Vector2.down, capsuleCollider2D.bounds.extents.y + extraHeightText, layerMask);
        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(capsuleCollider2D.bounds.center, Vector2.down * (capsuleCollider2D.bounds.extents.y + extraHeightText), rayColor);

        return raycastHit.collider != null; 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == tileLayer && isJumping)
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }
    }

    public void DamageFlash()
    {
        materialTintColor = new Color(1, 1, 1, 0.5f);
        spriteRenderer.material.SetColor("_Color", materialTintColor);

        isHurting = true;
    }

    public void DamageKnockBack(Vector3 targetPos, int damageAmount)
    {
        int dir = transform.position.x - targetPos.x > 0 ? 1 : -1;
        Vector2 knockBack = new Vector2(dir, 1) * 7;
        rigid.AddForce(knockBack, ForceMode2D.Impulse);
        DamageFlash();


        StartCoroutine(CoEnableDamage(0.5f, 1.5f));
    }

    public IEnumerator CoEnableDamage(float waitTime1, float waitTime2)
    {
        if (isHurting)
        {
            yield return new WaitForSeconds(waitTime1);
            yield return new WaitForSeconds(waitTime2);
            materialTintColor = new Color(1, 1, 1, 1f);
            spriteRenderer.material.SetColor("_Color", materialTintColor);
            isHurting = false;
        }
    }
}