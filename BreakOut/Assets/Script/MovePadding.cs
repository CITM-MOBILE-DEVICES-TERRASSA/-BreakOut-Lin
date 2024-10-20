using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Padding : MonoBehaviour
{
    public float speed = 5f;               // 控制移动速度
    private Vector2 screenBounds;          // 屏幕边界
    private float squareWidth;             // 挡板的宽度

    public Transform ballTransform;        // 追踪的球的位置
    public bool isAutoMode = false;        // 是否启用自动模式

    void Start()
    {
        // 获取屏幕边界
        Camera mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        squareWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
    }

    void Update()
    {
        // 如果启用了自动模式，自动追踪球的位置
        if (isAutoMode && ballTransform != null)
        {
            AutoMoveToBall();
        }
        else
        {
            // 否则，使用玩家的输入来移动
            PlayerControl();
        }
    }

    void PlayerControl()
    {
        float m_horizontal = Input.GetAxis("Horizontal");
        Vector3 newPosition = transform.position + new Vector3(m_horizontal, 0, 0) * speed * Time.deltaTime;

        // 限制挡板在屏幕内移动
        newPosition.x = Mathf.Clamp(newPosition.x, screenBounds.x * -1 + squareWidth, screenBounds.x - squareWidth);
        transform.position = newPosition;
    }

    void AutoMoveToBall()
    {
        float randomOffsetX = Random.Range(-0.5f, 0.5f);

        // 自动追踪球的位置，只在水平轴上移动
        Vector3 targetPosition = new Vector3(ballTransform.position.x + randomOffsetX, transform.position.y, transform.position.z);

        // 插值移动到目标位置，确保平滑移动
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // 限制挡板在屏幕内移动
        float clampedX = Mathf.Clamp(transform.position.x, screenBounds.x * -1 + squareWidth, screenBounds.x - squareWidth);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}
