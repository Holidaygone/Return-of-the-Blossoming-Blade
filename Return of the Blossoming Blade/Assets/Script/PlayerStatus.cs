using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    public float maxHP = 100f; // �ִ� HP
    public float currentHP;

    public float maxMP = 50f;  // �ִ� MP
    public float currentMP;

    public int maxPosion = 3;
    public int currentPosion;

    private AudioManager theAudio;
    private ChapterManager theChapter;
    public string posionSound;
    private OrderManager theOrder;

    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        theOrder = FindObjectOfType<OrderManager>();
        theChapter = FindObjectOfType<ChapterManager>();
        // ���� ���۽� �÷��̾��� HP�� MP�� �ִ�ġ�� ����
        PlayerPrefs.SetFloat("playerHP", maxHP);
        currentHP = maxHP;

        PlayerPrefs.SetFloat("playerMP", maxMP);
        currentMP = maxMP;

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
        // ü�� ����
        currentHP -= damage;

        // ü���� 0 ���ϰ� �Ǿ����� Ȯ��
        if (currentHP <= 0)
        {
            currentHP = 0; // ü���� 0���� ����
            PlayerPrefs.SetFloat("playerHP", currentHP);

            // ���� �� �̸� Ȯ��
            string currentSceneName = SceneManager.GetActiveScene().name;

            // 'Chunma' ���� ��� DieInCheonma ȣ��
            if (currentSceneName == "Chunma")
            {
                DieInCheonma();
            }

            StartCoroutine(FlashCoroutine()); // ���� ���� ȿ��
            StartCoroutine(DieAfterGo()); // �װ��� ���� �������� �̵�
        }
        else
        {
            // ü���� 0���� ū ��쿡�� PlayerPrefs�� ����
            PlayerPrefs.SetFloat("playerHP", currentHP);
            StartCoroutine(FlashCoroutine()); // ���� ���� ȿ��
        }
    }

    private IEnumerator DieAfterGo()
    {
        theOrder.Appear("BlackScreen", true);
        theChapter.ShowChapter("���");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Main");
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
                if (playerHP + 30f > maxHP)
                {
                    PlayerPrefs.SetFloat("playerHP", maxHP);
                }
                else
                {
                    PlayerPrefs.SetFloat("playerHP", playerHP + 30f);
                }

                playerHP = PlayerPrefs.GetFloat("playerHP");

                currentHP = playerHP;

                float playerMP = PlayerPrefs.GetFloat("playerMP");
                if (playerMP + 10f > maxMP)
                {
                    PlayerPrefs.SetFloat("playerMP", maxMP);
                }
                else
                {
                    PlayerPrefs.SetFloat("playerMP", playerMP + 10f);
                }

                playerMP = PlayerPrefs.GetFloat("playerMP");

                currentMP = playerMP;
            }
        }
    }

    void DieInCheonma()
    {
        End5.end = true;
        TransferMap[] temp = FindObjectsOfType<TransferMap>();

        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].gateName.Equals("EndPoint5"))
            {
                temp[i].GoToPoint();
                break;
            }
        }
        PlayerPrefs.SetFloat("End5", 1);

        GameObject Cheonma = GameObject.Find("Cheonma Bon In");

        DisableCheonmaBehaviors(Cheonma);
        ResetCheonmaAnimator(Cheonma);
        DisableAllClonedSpriteRenderers();
    }

    void DisableCheonmaBehaviors(GameObject cheonma)
    {
        foreach (var behavior in cheonma.GetComponents<Behaviour>())
        {
            behavior.enabled = false;
        }

        Animator animator = cheonma.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }
    }

    void ResetCheonmaAnimator(GameObject cheonma)
    {
        Animator animator = cheonma.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Rebind();
            animator.enabled = false;
        }
    }

    void DisableAllClonedSpriteRenderers()
    {
        SpriteRenderer[] allSpriteRenderers = FindObjectsOfType<SpriteRenderer>();

        foreach (SpriteRenderer renderer in allSpriteRenderers)
        {
            if (renderer.gameObject.name.Contains("(Clone)"))
            {
                renderer.enabled = false;
            }
        }
    }
}