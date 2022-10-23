using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// J : Ŭ�� ���ο� ���� ���콺 Ŀ�� �̹��� ���� ��ũ��Ʈ
public class MouseCursor : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D clickCursor;
    [SerializeField] private Vector2 hotSpot;

    public static MouseCursor Instance;

    // J : �� ����Ǿ ����, �ߺ� ���� ����
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Cursor.SetCursor(defaultCursor, hotSpot, CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))    // J : Ŭ��
            Cursor.SetCursor(clickCursor, hotSpot, CursorMode.ForceSoftware);
        if (Input.GetMouseButtonUp(0))      // J : Ŭ�� ����
            Cursor.SetCursor(defaultCursor, hotSpot, CursorMode.ForceSoftware);
    }
}
