using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float maxHP = 100f; // �ִ� HP
    public float currentHP;

    public float maxMP = 50f;  // �ִ� MP
    public float currentMP;

    public int maxPosion = 1;
    public int currentPosion;

    private AudioManager theAudio;
    public string posionSound;

    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // ���� ���۽� �÷��̾��� HP�� MP�� �ִ�ġ�� ����
        if (!PlayerPrefs.HasKey("playerHP"))
        {
            PlayerPrefs.SetFloat("playerHP", maxHP);
            currentHP = maxHP;
        }
        else
        {
            currentHP = PlayerPrefs.GetFloat("playerHP");
        }

        if (!PlayerPrefs.HasKey("playerMP"))
        {
            PlayerPrefs.SetFloat("playerMP", maxMP);
            currentMP = maxMP;
        }
        else
        {
            currentMP = PlayerPrefs.GetFloat("playerMP");
        }

        if (!PlayerPrefs.HasKey("havePosion"))
        {
            PlayerPrefs.SetInt("havePosion", 0);
            currentPosion = 0;
        }
        else
        {
            currentPosion = PlayerPrefs.GetInt("havePosion");
        }

        if (!PlayerPrefs.HasKey("maxPosion"))
        {
            PlayerPrefs.SetInt("maxPosion", 1);
            maxPosion = 1;
        }
        else
        {
            maxPosion = PlayerPrefs.GetInt("maxPosion");
        }
    }

    // �������� �Ծ��� �� ȣ��Ǵ� �Լ�
    public void TakeDamage(float damage)
    {

        // HP�� 0 �̸��̸� 0���� ����
        if (PlayerPrefs.GetFloat("playerHP") - damage < 0f)
        {
            PlayerPrefs.SetFloat("playerHP", 0);
            currentHP = 0;
            // TODO: �÷��̾� ��� ���� ����
        }
        currentHP = PlayerPrefs.GetFloat("playerHP") - damage;
        PlayerPrefs.SetFloat("playerHP", currentHP);

        StartCoroutine(FlashCoroutine()); // ���� ���� ȿ��
    }
    private IEnumerator FlashCoroutine()
    {
        float flashDuration = 0.1f;

        spriteRenderer.color = Color.red; // ������ ���������� ����
        yield return new WaitForSeconds(flashDuration); // ������ �ð� ���� ��ٸ�
        spriteRenderer.color = Color.white; // ���� �������� �ǵ���
    }

    // MP�� ������� �� ȣ��Ǵ� �Լ�
    public void UseMP(float amount)
    {
        if (PlayerPrefs.GetFloat("playerMP") - amount < 0f)
        {
            PlayerPrefs.SetFloat("playerMP", 0);
            currentMP = 0;
            // TODO: �÷��̾� ��� ���� ����
        }
        currentMP = PlayerPrefs.GetFloat("playerMP") - amount;
        PlayerPrefs.SetFloat("playerMP", currentMP);
    }

    public void GetPosion(int n)
    {
        if (PlayerPrefs.HasKey("havePosion") && PlayerPrefs.HasKey("maxPosion"))
        {
            int maxPosion = PlayerPrefs.GetInt("maxPosion");
            if (PlayerPrefs.GetInt("havePosion") + n > maxPosion)
            {
                PlayerPrefs.SetInt("havePosion", maxPosion);
                currentPosion = maxPosion;
            }
            else
            {
                currentPosion = PlayerPrefs.GetInt("havePosion") + n;
                PlayerPrefs.SetInt("havePosion", currentPosion);
            }
        }
    }

    public void UpgradeMaxPosion()
    {
        maxPosion += 1;
        PlayerPrefs.SetInt("maxPosion", maxPosion);
        currentPosion = maxPosion;
        PlayerPrefs.SetInt("havePosion", currentPosion);
    }

    public void UsePosion()
    {
        if (PlayerPrefs.HasKey("havePosion"))
        {
            int havePosion = PlayerPrefs.GetInt("havePosion");
            if (havePosion > 0)
            {
                theAudio.Play(posionSound);
                PlayerPrefs.SetInt("havePosion", havePosion - 1);

                float playerHP = PlayerPrefs.GetFloat("playerHP");
                PlayerPrefs.SetFloat("playerHP", playerHP + 30f);
                currentHP = playerHP;

                float playerMP = PlayerPrefs.GetFloat("playerMP");
                PlayerPrefs.SetFloat("playerMP", playerMP + 10f);
                currentMP = playerMP;
            }
        }
    }
}