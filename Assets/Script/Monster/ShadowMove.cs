using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMove : MonoBehaviour
{
    public GameObject endPoint;
    public GameObject wall;
    public float speed;
    bool canChase = false;
    Animator animator;


    private void Start()
    {
        StartCoroutine(CoChasePlayer());
        animator = gameObject.GetComponent<Animator>();
        
    }

    private void Update()
    {
        if(canChase)
            transform.position = Vector2.MoveTowards(transform.position,
                    endPoint.transform.position, Time.deltaTime * speed);
    }

    IEnumerator CoChasePlayer()
    {
        yield return new WaitForSeconds(3f);
        canChase = true;
        wall.SetActive(false);
        animator.SetTrigger("isWalking");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.layer = LayerMask.NameToLayer("Dead");
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }
            
    }
}
