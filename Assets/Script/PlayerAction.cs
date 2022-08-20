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

    private GameObject ladder;  // ���� �÷��̾ Ÿ�� �ִ� ��ٸ�

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

        if (isLadder)   // ��ٸ� �տ� �ִ� ���
        {
            // ���� ����Ű �Է� ����
            Vector3 horizontalMove = Vector3.zero;
            if (Input.GetKey(KeyCode.UpArrow))
                horizontalMove = Vector3.up;
            else if (Input.GetKey(KeyCode.DownArrow))
                horizontalMove = Vector3.down;

            if (horizontalMove!= Vector3.zero)  // ���� ����Ű �Է��� �ִٸ�
            {
                rigid.gravityScale = 0; // �߷� X
                isClimbing = true;
                animator.SetBool("isClimbing", isClimbing);
                transform.position = new Vector3(ladder.transform.position.x, transform.position.y, transform.position.z);  // �÷��̾ ��ٸ� ����� ����
                transform.position += horizontalMove * ladderSpeed * Time.deltaTime;    // �÷��̾� �̵�
                Physics2D.IgnoreLayerCollision(playerLayer, tileLayer, true); // Ÿ�ϰ��� �浹 ó�� X
            }
            else if (isClimbing)    // ��ٸ��� Ÿ�� ������ �������� �ʴ� ���
            {
                rigid.gravityScale = 0; // �߷� X
                animator.speed = 0; // �ִϸ��̼� ����
            }
        }

        //Jump
        if (Input.GetButtonDown("Jump") && IsGrounded() && !isClimbing && !skillManager.isOwlSkilling) // ��ٸ��� Ÿ�� ��, �û��� ��ų ��� �߿��� ���� �Ұ���
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

        // ��ٸ��� Ÿ�� �����鼭 ���� ���� ���� ��� ���� �̵� �Ұ���
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

    // ��ٸ� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == ladderLayer)  // ��ٸ�
        {
            ladder = collision.gameObject;
            isLadder = true;
            capsuleCollider2D.size = new Vector2(capsuleCollider2D.size.x / 2, capsuleCollider2D.size.y);
        }
        if (collision.gameObject.layer == portalLayer)  // ��Ż
        {
            int currentSceneNumber = SceneManager.GetActiveScene().buildIndex;  // ���� scene number ��������
            SceneManager.LoadScene(++currentSceneNumber);   // ���� scene���� �̵�
        }
    }

    // ��ٸ� ������
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == ladderLayer)  // ��ٸ�
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

        int layerMask = (1 << tileLayer) + (1 << obstacleLayerMask);    // Player �� MyTeammate ���̾ �浹üũ��
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