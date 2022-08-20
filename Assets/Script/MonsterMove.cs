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

    private void Start()
    {
        movingMonsterParent = gameObject.transform.parent.gameObject;

        int childCount = movingMonsterParent.transform.childCount;
        for (int i = 1; i < childCount; i++)
        {
            wayPoints.Add(movingMonsterParent.transform.GetChild(i).gameObject);
        }

        monsterSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
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
    }
}