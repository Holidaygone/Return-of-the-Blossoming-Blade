using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerStatus playerStatus;  // PlayerStatus ����
    public Image hpBar;  // HP �� �̹���
    public Image mpBar;  // MP �� �̹���

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject)
        {
            playerStatus = playerObject.GetComponent<PlayerStatus>();
            if (!playerStatus)
            {
                Debug.LogWarning("Player ������Ʈ�� PlayerStatus ��ũ��Ʈ�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("Player �±׸� ���� ������Ʈ�� ã�� �� �����ϴ�.");
        }

        // "HP_Gauge1" ������Ʈ ã�� �� hpBar�� �Ҵ�
        GameObject hpGaugeObject = GameObject.Find("HP_Gauge1");
        if (hpGaugeObject)
        {
            hpBar = hpGaugeObject.GetComponent<Image>();
        }
        else
        {
            Debug.LogWarning("HP_Gauge1 ������Ʈ�� ã�� �� �����ϴ�.");
        }

        // "MP_Gauge1" ������Ʈ ã�� �� mpBar�� �Ҵ�
        GameObject mpGaugeObject = GameObject.Find("MP_Gauge1");
        if (mpGaugeObject)
        {
            mpBar = mpGaugeObject.GetComponent<Image>();
        }
        else
        {
            Debug.LogWarning("MP_Gauge1 ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    private void Update()
    {
        if (playerStatus)
        {
            // HP, MP ���� ���
            float hpRatio = PlayerPrefs.GetFloat("playerHP") / playerStatus.maxHP;
            float mpRatio = PlayerPrefs.GetFloat("playerMP") / playerStatus.maxMP;

            // UI ������Ʈ
            hpBar.fillAmount = hpRatio;
            mpBar.fillAmount = mpRatio;
        }
    }
}