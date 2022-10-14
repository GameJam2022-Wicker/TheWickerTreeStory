using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    [SerializeField] private GameObject movingMonsterParent;
    [SerializeField] private List<GameObject> wayPoints;
    [SerializeField] private float speed = 2f;
    public int currentWayPointIndex = 0;

    SpriteRenderer monsterSprite;
    BoxCollider2D boxCollider2D;

    [SerializeField] LayerMask obstacleLayerMask;

    private void Start()
    {
        movingMonsterParent = gameObject.transform.parent.gameObject;

        int childCount = movingMonsterParent.transform.childCount;
        for (int i = 1; i < childCount; i++)
        {
            wayPoints.Add(movingMonsterParent.transform.GetChild(i).gameObject);
        }

        monsterSprite = gameObject.GetComponent<SpriteRenderer>();
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if(wayPoints.Count != 0)
        {
            if (Vector2.Distance(wayPoints[currentWayPointIndex].transform.position, transform.position) < .1f)
            {
                currentWayPointIndex++;

                if (currentWayPointIndex >= wayPoints.Count)
                {
                    currentWayPointIndex = 0;
                }
                monsterSprite.flipX = !monsterSprite.flipX;
            }
            transform.position = Vector2.MoveTowards(transform.position,
                        wayPoints[currentWayPointIndex].transform.position, Time.deltaTime * speed);

            Vector2 direction = (wayPoints[currentWayPointIndex].transform.position - transform.position).normalized;
            RaycastHit2D[] raycastHit2D = Physics2D.RaycastAll(boxCollider2D.bounds.center - new Vector3(0f, 0.5f, 0f), direction, 1.2f, obstacleLayerMask);

            Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(0f, 0.5f, 0f), direction * 1.2f, Color.red);

            for (int i = 0; i < raycastHit2D.Length; i++)
            {
                RaycastHit2D ray = raycastHit2D[i];
                Debug.Log(ray.collider.gameObject.name);
                if (ray.collider.gameObject.tag == "Obstacle")
                {
                    currentWayPointIndex++;
                    monsterSprite.flipX = !monsterSprite.flipX;
                }
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }

    }
}