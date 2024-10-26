using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Padding : MonoBehaviour
{
    public float speed = 5f;               // �����ƶ��ٶ�
    private Vector2 screenBounds;          // ��Ļ�߽�
    private float squareWidth;             // ����Ŀ��

    public Transform ballTransform;        // ׷�ٵ����λ��
    public bool isAutoMode = false;        // �Ƿ������Զ�ģʽ

    public Bricks bricks;
    void Start()
    {
        // ��ȡ��Ļ�߽�
        Camera mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        // ���㲢���õ���Ŀ�Ⱥ͸߶ȱ���
        float targetWidth = screenBounds.x * 0.2f;   // ������ռ��Ļ��ȵ�20%
        float targetHeight = screenBounds.y * 0.05f; // ����߶�ռ��Ļ�߶ȵ�5%

        // ��ȡ��ǰ�����ʵ�ʿ�Ⱥ͸߶�
        float currentWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x;
        float currentHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y;

        // ����ˮƽ�ʹ�ֱ��������ű���
        float scaleFactorX = targetWidth / currentWidth;
        float scaleFactorY = targetHeight / currentHeight;

        // ���õ���Ŀ�Ⱥ͸߶�
        transform.localScale = new Vector3(scaleFactorX * transform.localScale.x, scaleFactorY * transform.localScale.y, transform.localScale.z);

        // ���µ���Ŀ�ȣ����ں����߽�����
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
        Vector3 targetPosition = new Vector3(ballTransform.position.x, transform.position.y, transform.position.z);

        // ��ֵ�ƶ���Ŀ��λ�ã�ȷ��ƽ���ƶ�
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // ���Ƶ�������Ļ���ƶ�
        float clampedX = Mathf.Clamp(transform.position.x, screenBounds.x * -1 + squareWidth, screenBounds.x - squareWidth);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("powerUp"))
        {
            GameManager.instance.life += 1;
            // ��ȡ�� powerUp ������ Bricks �ű�
            Bricks bricks = collider.gameObject.GetComponent<Bricks>();
            if (bricks != null)
            {
                // ���� RemovePowerUp ���������뵱ǰ�� powerUp ����
                bricks.RemovePowerUp(collider.gameObject);
            }

            // �������ѡ��ֱ������ powerUp��������������������߼���Ҫ����
            Destroy(collider.gameObject);
        }
    }
}
