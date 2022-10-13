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
    public float colliderSizeX;
    public bool isHurting = false;

    Rigidbody2D rigid;
    bool facingRight = true;
    bool isJumping = false;
    bool isLadder;
    bool isClimbing;
    bool isGameOver;

    //�ڿ��� Ÿ��
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private GameObject ladder;  // ���� �÷��̾ Ÿ�� �ִ� ��ٸ�

    private SkillManager skillManager;
    private CapsuleCollider2D capsuleCollider2D;
    private Animator animator;
    private LayerMask ladderLayer, tileLayer, playerLayer, portalLayer, obstacleLayer, gameOverLayer;

    [SerializeField] private AudioSource walkSound, jumpSound, flySound, ladderSound, portalSound;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();

        ladderLayer = LayerMask.NameToLayer("Ladder");
        tileLayer = LayerMask.NameToLayer("Tile");
        playerLayer = LayerMask.NameToLayer("Player");
        portalLayer = LayerMask.NameToLayer("Portal");
        obstacleLayer = LayerMask.NameToLayer("Obstacle");
        gameOverLayer = LayerMask.NameToLayer("GameOver");

        if (GameObject.Find("SkillManager") != null)
            skillManager = GameObject.Find("SkillManager").GetComponent<SkillManager>();
    }

    void Start()
    {
        colliderSizeX = capsuleCollider2D.size.x;   // ���� �ݶ��̴� ������ ����
    }


    void Update()
    {
        animator.speed = 1;

        if (skillManager != null && !skillManager.isOwlSkilling)    // �ξ��� ��ų ��� X
        {
            if (IsGrounded())   // ���� ������
            {
                Physics2D.IgnoreLayerCollision(playerLayer, tileLayer, false);  // Ÿ�ϰ��� �浹 ó�� O
                rigid.gravityScale = gravityScale;
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
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

                if (!ladderSound.isPlaying) // ��ٸ� Ÿ�� �Ҹ� ���
                    ladderSound.Play();
            }
            else if (isClimbing)    // ��ٸ��� Ÿ�� ������ �������� �ʴ� ���
            {
                rigid.gravityScale = 0; // �߷� X
                animator.speed = 0; // �ִϸ��̼� ����
                ladderSound.Stop();
            }

            if (isClimbing)
            {
                rigid.velocity = Vector3.zero;   // ��ٸ� �������� ����
                capsuleCollider2D.size = new Vector2(colliderSizeX / 2, capsuleCollider2D.size.y);  // �÷��̾� �ݶ��̴��� �ٿ��� ��ٸ� Ÿ�� ����
            }
        }

        // Jump
        // J : ��ٸ� Ÿ�� �� or �û��� ��ų ��� �߿��� ���� �Ұ���
        // yesman: �ڿ���Ÿ�� ����
        if (Input.GetButtonDown("Jump") && !isClimbing && skillManager != null && !skillManager.isOwlSkilling)
        {
            if(coyoteTimeCounter > 0f)
            {
                isJumping = true;
                animator.SetBool("isJumping", true);
                rigid.velocity = Vector2.up * jumpPower;

                // �Ҹ� ���
                jumpSound.Play();
            }
            
        }

        if (Input.GetButtonUp("Jump") && rigid.velocity.y > 0f)
        {
            coyoteTimeCounter = 0f;
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
        // ��ٸ��� Ÿ�� �����鼭 ���� ���� ���� ��� ���� �̵� �Ұ���
        if (!isClimbing || isClimbing && IsGrounded())
        {
            float moveInput = Input.GetAxisRaw("Horizontal");

            bool isMoving;
            if (moveInput != 0)
                isMoving = true;
            else
                isMoving = false;
            animator.SetBool("isMoving", isMoving);

            // �ȴ� �Ҹ� ���
            if (isMoving && IsGrounded() && skillManager != null && !skillManager.isOwlSkilling)
            {
                if (!walkSound.isPlaying)
                    walkSound.Play();
            }
                
            else if (skillManager != null && skillManager.isOwlSkilling && !flySound.isPlaying)
                flySound.Play();
            else
                walkSound.Stop();

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
        }
        if (collision.gameObject.layer == portalLayer)  // ��Ż
        {
            portalSound.Play();
            GameObject.Find("FadeManager").GetComponent<FadeManager>().StartCoroutine("FadeOutCoroutine");
        }
    }

    // ��ٸ� ������
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == ladderLayer)  // ��ٸ�
        {
            isLadder = isClimbing = false;
            animator.SetBool("isClimbing", isClimbing);
            capsuleCollider2D.size = new Vector2(colliderSizeX, capsuleCollider2D.size.y);  // �÷��̾� �ݶ��̴� �����·�
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

        int layerMask = (1 << tileLayer) + (1 << obstacleLayer);    // tile, obstacle ���̾ �浹 üũ
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
        // J : ���� ���̾��ٸ� ���� ����
        if (IsGrounded() && isJumping)
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }
        if (collision.gameObject.layer == gameOverLayer)    // ���� ����
        {
            Debug.Log("���� ����");
            if (!isGameOver)
            {
                GameObject.Find("FadeManager").GetComponent<FadeManager>().StartCoroutine("GameOverCoroutine");
                isGameOver = !isGameOver;
                maxSpeed = 0f;
            }
        }
    }
}