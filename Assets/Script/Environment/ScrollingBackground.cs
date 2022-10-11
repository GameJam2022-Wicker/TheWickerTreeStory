using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public bool scrolling, parallax, automatically;

    public float backgroundSize;

    [SerializeField] float multiplier = 0.0f;
    [SerializeField] float autoSpeed = 0.0f;
    [SerializeField] bool horizontalOnly = true;
    [SerializeField] bool verticalOnly = false;

    private Transform cameraTransform;

    private Vector3 startCameraPos;
    private Vector3 startPos;

    private Transform[] layers;
    private float viewZone = 10;
    private int leftIndex;
    private int rightIndex;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        startCameraPos = cameraTransform.position;
        startPos = transform.position;

        layers = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
            layers[i] = transform.GetChild(i);

        leftIndex = 0;
        rightIndex = layers.Length - 1;
    }

    private void Update()
    {
        if (scrolling)
        {
            if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
                ScrollLeft();
            if (cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone))
                ScrollRight();
        }

    }

    private void LateUpdate()
    {
        if (parallax)
        {
            var position = startPos;
            if (horizontalOnly)
                position.x += multiplier * (cameraTransform.position.x - startCameraPos.x);
            else if(verticalOnly)
                position.y += multiplier * (cameraTransform.position.y - startCameraPos.y);
            else
                position += multiplier * (cameraTransform.position - startCameraPos);
            transform.position = position;
        }
        // yesman: monstermove에서 가져옴. 자신의 가로사이즈만큼 더한 위치까지 움직임.
        // 그 위치가 되면 ScrollLeft, ScrollRight를 통해 스크롤됨
        if(automatically)
            transform.position = Vector2.MoveTowards(transform.position, 
                new Vector2(transform.position.x + backgroundSize/2, transform.position.y), Time.deltaTime * autoSpeed);

    }

    private void ScrollLeft()
    {
        int lastRight = rightIndex;
        Vector3 newPos = new Vector3(layers[leftIndex].position.x - backgroundSize, transform.position.y, 0);
        layers[rightIndex].position = newPos;
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
            rightIndex = layers.Length - 1;
    }

    private void ScrollRight()
    {
        int lastRight = leftIndex;
        Vector3 newPos = new Vector3(layers[rightIndex].position.x + backgroundSize, transform.position.y, 0);
        layers[leftIndex].position = newPos;
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layers.Length)
            leftIndex = 0;
    }
}
