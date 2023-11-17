using System;
using System.Collections;
using UnityEngine;
using Modularify.LoadingBars3D;

public class SkillCooldownManager : MonoBehaviour
{
    public LoadingBarSegments QBar;
    public LoadingBarSegments WBar;
    public LoadingBarSegments EBar;
    public LoadingBarSegments SpaceBar;


    public GameObject QSkillObject;
    public GameObject WSkillObject;
    public GameObject ESkillObject;

    public float QCooldownTime = 5f;
    public float WCooldownTime = 10f;
    public float ECooldownTime = 30f;

    private bool isQCooldown = false;
    private bool isWCooldown = false;
    private bool isECooldown = false;

    private bool isQActive = false;
    private bool isWActive = false;
    private bool isEActive = false;

    public float QSkillDuration = 0.85f;
    public float WSkillDuration = 0.9f;
    public float ESkillDuration = 3.5f;
    public float SpaceDuration = 0.2f;

    public GameObject QSkillCol; // Q�� Skill Col ����

    public GameObject WSkillCol;
    public float WSkillDistance = 400f; // W ��ų ���󰡴� �Ÿ�
    private Vector2 lastDirection = Vector2.left;  // �⺻������ ������ �ٶ󺸰� ����
    public GameObject WSkillParticle;

    public GameObject ESkillCol;
    public GameObject ESkillCol2;

    public Animator playerAnimator; // �÷��̾� �ִϸ����� ����
    private bool isAttacking = false; // ���� ������ Ȯ���ϴ� ����

    public Collider2D playerCollider1; // �÷��̾� �ݶ��̴� ����
    public Collider2D playerCollider2; // �÷��̾� �ݶ��̴� ����

    public float dashDistance = 185f; // �뽬 �Ÿ�

    public PlayerStatus playerStatus; // PlayerStatus ����

    public float QSkillMP = 5f;
    public float WSkillMP = 5f;
    public float ESkillMP = 15f;

    public float SpaceCooldownTime = 3f; // �����̽��� ��ų�� ��ٿ� �ð�

    private bool isSpaceCooldown = false; // �����̽��� ��ų ��ٿ� ����

    private AudioManager theAudio;

    private void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        // ��ų�� Ȱ��ȭ ������ üũ
        bool anySkillActive = isQActive || isWActive || isEActive;

        if (Input.GetKeyDown(KeyCode.A) && !isAttacking && !anySkillActive)
        {
            theAudio.Play("A");
            StartCoroutine(Attack());
        }

        // �÷��̾��� ������ ������ �����մϴ�.
        if (Input.GetKeyDown(KeyCode.UpArrow)) lastDirection = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) lastDirection = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) lastDirection = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) lastDirection = Vector2.right;

        // Q ��ų ���
        if (Input.GetKeyDown(KeyCode.Q) && !isQCooldown && !anySkillActive)
        {
           if (playerStatus.currentMP >= QSkillMP) // ����� MP�� �ִ��� Ȯ��
            {
                theAudio.Play("Q");
                playerStatus.UseMP(QSkillMP); // MP �Ҹ�
                StartCoroutine(UseSkill(QCooldownTime, QSkillDuration, QBar, QSkillObject, () => isQActive = true, () => isQActive = false, () => isQCooldown = true, () => isQCooldown = false));
                StartCoroutine(QSkillEffect());
            }

        }

        // W ��ų ���
        if (Input.GetKeyDown(KeyCode.W) && !isWCooldown && !anySkillActive)
        {
            if (playerStatus.currentMP >= WSkillMP)
            {
                theAudio.Play("W");
                playerStatus.UseMP(WSkillMP);
                StartCoroutine(UseSkill(WCooldownTime, WSkillDuration, WBar, WSkillObject, () => isWActive = true, () => isWActive = false, () => isWCooldown = true, () => isWCooldown = false));
                StartCoroutine(WSkillEffect());
            }
        }

        // E ��ų ���
        if (Input.GetKeyDown(KeyCode.E) && !isECooldown && !anySkillActive)
        {
            if (playerStatus.currentMP >= ESkillMP)
            {
                theAudio.Play("E");
                playerStatus.UseMP(ESkillMP);
                StartCoroutine(UseSkill(ECooldownTime, ESkillDuration, EBar, ESkillObject, () => isEActive = true, () => isEActive = false, () => isECooldown = true, () => isECooldown = false));
                StartCoroutine(ESkillEffect());
            }
        }
        // �����̽��� ���
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking && !anySkillActive && !PlayerManager.instance.skillNotMove && !PlayerManager.instance.notMove && !isSpaceCooldown)
        {
            theAudio.Play("Space");
            StartCoroutine(Dash());
            StartCoroutine(SpaceCooldown(SpaceCooldownTime, SpaceBar, () => isSpaceCooldown = true, () => isSpaceCooldown = false));
        }
    }

    private IEnumerator UseSkill(float cooldownTime, float skillDuration, LoadingBarSegments bar, GameObject skillObject, Action onStart, Action onEnd, Action onCooldownStart, Action onCooldownEnd)
    {
        onStart();
        skillObject.SetActive(true);

        PlayerManager.instance.skillNotMove = true; // ��ų�� Ȱ��ȭ�Ǹ� �̵��� �����ϴ�.

        // ��ų ���� �ð� ���� Ȱ��ȭ ���� ����
        yield return new WaitForSeconds(skillDuration);

        PlayerManager.instance.skillNotMove = false; // ��ų ���� �ð��� ������ �̵��� ����մϴ�.

        skillObject.SetActive(false);
        onEnd();

        onCooldownStart();
        float timeElapsed = 0;
        while (timeElapsed < cooldownTime)
        {
            timeElapsed += Time.deltaTime;
            float percentage = timeElapsed / cooldownTime;
            bar.SetPercentage(percentage);
            yield return null;
        }
        bar.SetPercentage(0);
        onCooldownEnd();
    }

    private IEnumerator QSkillEffect()
    {
        playerAnimator.SetBool("SkillQ", true);

        for (int i = 0; i < 3; i++) // 3�� �ݺ�
        {
            QSkillCol.SetActive(true);  // Skill Col Ȱ��ȭ
            yield return new WaitForSeconds(0.2f); // 0.1�� ���. �� ���� �����Ͽ� ON/OFF ������ ������ �� �ֽ��ϴ�.
            QSkillCol.SetActive(false); // Skill Col ��Ȱ��ȭ
            yield return new WaitForSeconds(0.1f); // 0.1�� ���
        }
        playerAnimator.SetBool("SkillQ", false);
    }

    private IEnumerator WSkillEffect()
    {
        WSkillCol.SetActive(true);  // ��ų �ݶ��̴� Ȱ��ȭ

        Vector2 originalPos = WSkillCol.transform.position;
        Vector2 targetPos = originalPos + lastDirection * WSkillDistance;

        if (lastDirection == Vector2.left) // ������ �� z �� -90
        {
            WSkillParticle.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (lastDirection == Vector2.down) // �Ʒ����� �� z �� 0
        {
            WSkillParticle.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (lastDirection == Vector2.right) // �������� �� z �� 90
        {
            WSkillParticle.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (lastDirection == Vector2.up) // ������ �� z �� 180
        {
            WSkillParticle.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        playerAnimator.SetBool("SkillW", true);
        float startTime = Time.time;
        float journeyLength = Vector2.Distance(originalPos, targetPos);

        float fractionOfJourney = 0f; // �ʱ� ������ 0���� ����

        while (fractionOfJourney < 1.0f) // 1�� ������ ������ �ݺ�
        {
            float timeSinceStarted = (Time.time - startTime) / WSkillDuration; // ���� �ð��� ��ų ���� �ð����� ������ ������ ���մϴ�.
            fractionOfJourney = 1 - Mathf.Pow(1 - timeSinceStarted, 3); // ������ ������, ���� �������� ȿ���� ����ϴ�.

            WSkillCol.transform.position = Vector2.Lerp(originalPos, targetPos, fractionOfJourney); // �ε巴�� ������ ��ġ�� �ݶ��̴� �̵�


            yield return null;
        }

        WSkillCol.transform.position = targetPos;  // ���� �ð��� ������ ��ǥ �������� �ݶ��̴� ��ġ ����

        WSkillCol.SetActive(false);  // ��ų ȿ�� ���� �� �ݶ��̴� ��Ȱ��ȭ
        playerAnimator.SetBool("SkillW", false);


        WSkillCol.transform.position = originalPos;  // �ݶ��̴��� ������ ��ġ�� �缳��
    }
    private IEnumerator ESkillEffect()
    {
        playerAnimator.SetBool("SkillE", true);

        ESkillCol.SetActive(true); // ù ��° �ݶ��̴� Ȱ��ȭ
        ESkillCol2.SetActive(true); // �� ��° �ݶ��̴� Ȱ��ȭ

        yield return new WaitForSeconds(ESkillDuration/3); 

        ESkillCol.SetActive(false); // ù ��° �ݶ��̴� ��Ȱ��ȭ
        ESkillCol2.SetActive(false); // �� ��° �ݶ��̴� ��Ȱ��ȭ
        ESkillCol.SetActive(true); // ù ��° �ݶ��̴� Ȱ��ȭ
        ESkillCol2.SetActive(true); // �� ��° �ݶ��̴� Ȱ��ȭ

        yield return new WaitForSeconds(ESkillDuration / 3);

        ESkillCol.SetActive(false); // ù ��° �ݶ��̴� ��Ȱ��ȭ
        ESkillCol2.SetActive(false); // �� ��° �ݶ��̴� ��Ȱ��ȭ
        ESkillCol.SetActive(true); // ù ��° �ݶ��̴� Ȱ��ȭ
        ESkillCol2.SetActive(true); // �� ��° �ݶ��̴� Ȱ��ȭ

        yield return new WaitForSeconds(ESkillDuration / 3); 

        playerAnimator.SetBool("SkillE", false);

    }

    // �⺻ ���� �ڷ�ƾ
    private IEnumerator Attack()
    {
        isAttacking = true; // ���� ���·� ����
        PlayerManager.instance.skillNotMove = true; // �÷��̾��� �̵� ���

        // �������� AttackH �Ǵ� AttackV Ʈ���� ����
        if (UnityEngine.Random.value < 0.5f)
        {
            playerAnimator.SetBool("AttackH", true);
        }
        else
        {
            playerAnimator.SetBool("AttackV", true);
        }

        // ���� �ִϸ��̼� �ð���ŭ ���
        yield return new WaitForSeconds(0.75f);

        // �ִϸ��̼� �Ҹ� ���� �ʱ�ȭ
        playerAnimator.SetBool("AttackH", false);
        playerAnimator.SetBool("AttackV", false);

        PlayerManager.instance.skillNotMove = false; // �÷��̾��� �̵� ��� ����
        isAttacking = false; // ���� ���� ����
    }

    private IEnumerator Dash()
    {

        PlayerManager.instance.skillNotMove = true; // �̵� ���
        playerCollider1.enabled = false;
        playerCollider2.enabled = false;// �ݶ��̴� ��Ȱ��ȭ

        playerAnimator.SetBool("Dash", true); // Dash �ִϸ��̼� Ȱ��ȭ

        Vector2 originalPosition = PlayerManager.instance.transform.position;
        Vector2 targetPosition = originalPosition + lastDirection * dashDistance; // dashDistance�� �̵��� �Ÿ��� �����ؾ� ��

        float startTime = Time.time;
        while (Time.time - startTime < SpaceDuration)
        {
            float fractionOfJourney = (Time.time - startTime) / SpaceDuration;
            // PlayerManager�� MovePlayer �Լ��� ����� �÷��̾� ��ġ ������Ʈ
            PlayerManager.instance.MovePlayer(Vector2.Lerp(originalPosition, targetPosition, fractionOfJourney));
            yield return null;
        }

        // ���� �������� �������� �� �÷��̾� ��ġ ����
        PlayerManager.instance.MovePlayer(targetPosition);

        playerCollider1.enabled = true;
        playerCollider2.enabled = true; // �ݶ��̴� Ȱ��ȭ
        PlayerManager.instance.skillNotMove = false; // �̵� ��� ����
        playerAnimator.SetBool("Dash", false); // Dash �ִϸ��̼� ��Ȱ��ȭ

    }
    private IEnumerator SpaceCooldown(float cooldownTime, LoadingBarSegments bar, Action onStartCooldown, Action onEndCooldown)
    {
        onStartCooldown?.Invoke();
        float timeElapsed = 0;
        while (timeElapsed < cooldownTime)
        {
            timeElapsed += Time.deltaTime;
            float percentage = timeElapsed / cooldownTime;
            bar.SetPercentage(percentage);
            yield return null;
        }
        bar.SetPercentage(0);
        onEndCooldown?.Invoke();
    }
}