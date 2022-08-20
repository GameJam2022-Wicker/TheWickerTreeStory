using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMove : MonoBehaviour
{
    public GameObject endPoint;
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
        yield return new WaitForSeconds(5f);
        canChase = true;
        animator.SetTrigger("isWalking");
    }
}
