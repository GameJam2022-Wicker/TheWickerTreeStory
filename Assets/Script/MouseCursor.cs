using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// J : 클릭 여부에 따른 마우스 커서 이미지 변경 스크립트
public class MouseCursor : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D clickCursor;
    [SerializeField] private Vector2 hotSpot;

    public static MouseCursor Instance;

    // J : 씬 변경되어도 유지, 중복 생성 방지
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
        if (Input.GetMouseButtonDown(0))    // J : 클릭
            Cursor.SetCursor(clickCursor, hotSpot, CursorMode.ForceSoftware);
        if (Input.GetMouseButtonUp(0))      // J : 클릭 해제
            Cursor.SetCursor(defaultCursor, hotSpot, CursorMode.ForceSoftware);
    }
}
