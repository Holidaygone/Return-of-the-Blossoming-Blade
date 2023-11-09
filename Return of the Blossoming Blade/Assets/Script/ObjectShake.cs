using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShake : MonoBehaviour
{
    public float amplitude = 2f; // ��鸲�� ����
    public float frequency = 4f; // ��鸲�� �ֱ�

    public bool shakeVertical = true; // ��鸲 ������ �������� ���θ� �����ϴ� �÷���

    private Vector3 originalPosition;
    private bool isShaking = false;

    private void Start()
    {
        originalPosition = transform.position; // ���� ��ġ�� �����մϴ�.
    }

    private void Update()
    {
        if (!isShaking)
        {
            StartShaking();
        }
    }

    private void StartShaking()
    {
        isShaking = true;
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        while (isShaking)
        {
            // Mathf.Sin �Լ��� ����Ͽ� ��鸲 ȿ���� ����ϴ�.
            float delta = Mathf.Sin(Time.time * frequency) * amplitude;
            Vector3 newPosition = originalPosition;

            // ���� ��鸲�� ���õ� ��� y���� �����ϰ�, �׷��� ������ x���� �����մϴ�.
            if (shakeVertical)
            {
                newPosition.y += delta;
            }
            else
            {
                newPosition.x += delta;
            }

            transform.position = newPosition;
            yield return null;
        }
    }

    private void OnDisable()
    {
        isShaking = false; // ������Ʈ�� ��Ȱ��ȭ�� �� ��鸲�� �����մϴ�.
        transform.position = originalPosition; // ��ġ�� �ʱ�ȭ�մϴ�.
    }
}
