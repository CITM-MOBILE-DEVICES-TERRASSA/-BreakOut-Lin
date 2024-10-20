using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Padding : MonoBehaviour
{
    public float speed = 5f;               // �����ƶ��ٶ�
    private Vector2 screenBounds;          // ��Ļ�߽�
    private float squareWidth;             // ����Ŀ��

    public Transform ballTransform;        // ׷�ٵ����λ��
    public bool isAutoMode = false;        // �Ƿ������Զ�ģʽ

    void Start()
    {
        // ��ȡ��Ļ�߽�
        Camera mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        squareWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
    }

    void Update()
    {
        // ����������Զ�ģʽ���Զ�׷�����λ��
        if (isAutoMode && ballTransform != null)
        {
            AutoMoveToBall();
        }
        else
        {
            // ����ʹ����ҵ��������ƶ�
            PlayerControl();
        }
    }

    void PlayerControl()
    {
        float m_horizontal = Input.GetAxis("Horizontal");
        Vector3 newPosition = transform.position + new Vector3(m_horizontal, 0, 0) * speed * Time.deltaTime;

        // ���Ƶ�������Ļ���ƶ�
        newPosition.x = Mathf.Clamp(newPosition.x, screenBounds.x * -1 + squareWidth, screenBounds.x - squareWidth);
        transform.position = newPosition;
    }

    void AutoMoveToBall()
    {
        float randomOffsetX = Random.Range(-0.5f, 0.5f);

        // �Զ�׷�����λ�ã�ֻ��ˮƽ�����ƶ�
        Vector3 targetPosition = new Vector3(ballTransform.position.x + randomOffsetX, transform.position.y, transform.position.z);

        // ��ֵ�ƶ���Ŀ��λ�ã�ȷ��ƽ���ƶ�
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // ���Ƶ�������Ļ���ƶ�
        float clampedX = Mathf.Clamp(transform.position.x, screenBounds.x * -1 + squareWidth, screenBounds.x - squareWidth);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}
