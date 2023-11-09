using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class ResizeCanvas : MonoBehaviour
{
    private Canvas canvas;
    public Camera mainCamera;
    public Vector3 offset = Vector3.zero; // ���ϴ� ��� ī�޶�κ����� �������� ������ �� �ֽ��ϴ�.


    private void Start()
    {
        canvas = GetComponent<Canvas>();
        if (mainCamera == null)
            mainCamera = Camera.main;

        Resize();
    }

    private void LateUpdate()
    {
        FollowCamera();
    }

    void FollowCamera()
    {
        // ī�޶� ��ġ�� �������� ���� ĵ���� ��ġ�� ������Ʈ�մϴ�.
        transform.position = mainCamera.transform.position + offset;
    }


    void Resize()
    {
        // Orthographic ī�޶� �����Ͽ����ϴ�.
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(cameraWidth, cameraHeight);
    }
}