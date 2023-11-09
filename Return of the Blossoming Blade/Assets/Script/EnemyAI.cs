using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public enum EnemyState
{
    Idle,
    Aggro,
    Moving,
    Attacking,
    Hit,
    Dead,
    Dashing
}


public class EnemyAI : MonoBehaviour
{
    public float speed = 140f;
    public float detectionDistance = 55f;  // Raycasting �Ÿ�
    private Transform playerTransform;
    private Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    private Animator animator;

    public float aggroRange = 650f;  // �÷��̾ �����ϴ� �Ÿ�

    public float distanceToPlayer; // �÷��̾���� �Ÿ��� ������ public ����

    public EnemyState currentState = EnemyState.Idle; //�� ���� ����

    private float aggroTimer = 0.5f;

    public float attackRange = 45f;  // �÷��̾ ������ �� �ִ� �Ÿ�

    private GameObject attackCollider;  // ���� �ݶ��̴�

    private bool isAttacking = false;

    public float enemyHP = 100f; // ���� HP

    public float damageToPlayer = 20f; // �÷��̾�� �� ������

    private float knockbackDuration = 0.2f; // �з����� �ð�

    public bool canDash = false;  // ���� ����(����) �� �� �ִ��� ���θ� �����ϴ� ����

    public float dashProbability = 0.3f;  // ����(����)�� Ȯ��

    public float dashSpeed = 500f;  // ����(����) �ӵ�

    public float dashHeight = 250f;  // ����(����) �� Y������ �󸶳� ���� ������ ������

    public float dashDuration = 0.8f;  // ����(����)�� ���ӽð� (��)

    private bool hasDashed = false; // ���� �̹� ����(����)�� �ߴ��� Ȯ���ϴ� ����

    private float dashTime = 0f;  // ����(����)�ϴ� ���� ����� �ð�

    public Collider2D enemyCollider;  // ���� �ݶ��̴��� ������ ����

    public float knockbackForce; // ���ϴ� ���� ũ�⸦ �����մϴ�. (���� ���� ����)

    private PlayerManager playerManager; // �÷��̾� �Ŵ��� ����

    public MonoBehaviour targetScript; // Inspector���� ������ ��� �� ī��Ʈ ������ ��ũ��Ʈ

    private bool isDead = false; // ���� �׾������� ��Ÿ���� �÷���


    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        animator = GetComponent<Animator>();

        attackCollider = transform.Find("Attack Collider").gameObject;

        attackCollider.SetActive(false);  // ó���� ��Ȱ��ȭ..

        enemyCollider = GetComponent<Collider2D>();  // ���� �ݶ��̴��� �����ɴϴ�.

        playerManager = PlayerManager.instance; // PlayerManager�� �ν��Ͻ��� �����ɴϴ�.

    }

    private void Update()
    {
        // PlayerManager�� notMove ������ true�̸� �Ʒ� ������ �������� �ʽ��ϴ�.
        if (playerManager.notMove)
            return;

        distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position); // �÷��̾���� �Ÿ� ���

        switch (currentState)
        {
            case EnemyState.Idle:
                // Idle ������ ����
                if (distanceToPlayer <= aggroRange)
                {
                    ChangeState(EnemyState.Aggro);
                }

                break;
            case EnemyState.Aggro:
                // AggroedIdle ������ ����
                if (distanceToPlayer > aggroRange)
                {
                    ChangeState(EnemyState.Idle);
                }
                else
                {
                    StartCoroutine(MoveAfterDelay());
                }
                break;
            case EnemyState.Moving:
                // �÷��̾ ���� �̵��ϴ� ����
                Vector2 currentDirection = GetDirectionToPlayer();
                MoveInDirection(currentDirection);
                if (distanceToPlayer > aggroRange)
                {
                    ChangeState(EnemyState.Idle);
                }
                if (distanceToPlayer <= attackRange)  // ���� ���� ���� ���� ��
                {
                    ChangeState(EnemyState.Attacking);
                }
                // ����(����) ���� �߰�:
                if (canDash && !hasDashed && Random.value < dashProbability)
                {
                    hasDashed = true;
                    dashTime = 0f;  // ����(����) �ð� �ʱ�ȭ
                    ChangeState(EnemyState.Dashing);
                }
                break;
            case EnemyState.Attacking:
                // �÷��̾ �����ϴ� ����
                if (!isAttacking) // ���� ���� �ƴ϶�� �ڷ�ƾ ȣ��
                {
                    StartCoroutine(AttackSequence());
                }
                break;
            case EnemyState.Hit:
                // ���ظ� �޴� �ִϸ��̼� �� ����
                StartCoroutine(HitSequence());
                break;
            case EnemyState.Dead:
                // ���� �ִϸ��̼� �� ����
                if (!isDead)
                {
                    isDead = true;
                    DecreaseEnemyCount();
                }
                break;
            case EnemyState.Dashing:
                // �÷��̾ ���� ����(����)�ϴ� ����
                DashTowardsPlayer();

                if (dashTime > dashDuration)
                {
                    ChangeState(EnemyState.Moving); // ����(����) ���ӽð��� ������ Moving ���·� ����
                }

                break;

        }
    }

    IEnumerator MoveAfterDelay()
    {
        yield return new WaitForSeconds(aggroTimer);
        if (currentState == EnemyState.Aggro) // Aggro ���� Ȯ��
        {
            if (distanceToPlayer <= attackRange)  // ���� ���� ���� ���� ��
            {
                ChangeState(EnemyState.Attacking);
            }
            else
            {
                ChangeState(EnemyState.Moving);
            }
        }
    }

    IEnumerator AttackSequence()
    {
        isAttacking = true;

        yield return new WaitForSeconds(0.4f);  // �ִϸ��̼� ���� �� 0.4�� ���
        attackCollider.SetActive(true);  // �ݶ��̴� Ȱ��ȭ
        yield return new WaitForSeconds(0.3f);  // 0.3�� ���� ����
        attackCollider.SetActive(false);  // �ٽ� ��Ȱ��ȭ

        isAttacking = false; // �ڷ�ƾ�� �������Ƿ� ���� �� ���¸� ����

        if (distanceToPlayer > attackRange)
        {
            ChangeState(EnemyState.Aggro); // ������ ������ Aggro ���·� ��ȯ
        }
        else
        {
            ChangeState(EnemyState.Idle); // �ƴ϶�� �Ͻ������� Idle ���·� ��ȯ�Ͽ� �ִϸ��̼��� ������ϰ� �մϴ�.
        }
    }

    private Vector2 GetDirectionToPlayer()
    {
        Vector2 directionToPlayer = playerTransform.position - transform.position;

        // Determine primary direction based on the magnitude of x and y difference.
        if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
        {
            // Move horizontally.
            if (directionToPlayer.x > 0)
            {
                // Check for obstacle on the right.
                if (!IsObstacleInDirection(Vector2.right))
                    return Vector2.right;
                else
                    return Vector2.up; // or Vector2.down based on some other logic
            }
            else
            {
                // Check for obstacle on the left.
                if (!IsObstacleInDirection(Vector2.left))
                    return Vector2.left;
                else
                    return Vector2.up; // or Vector2.down based on some other logic
            }
        }
        else
        {
            // Move vertically.
            if (directionToPlayer.y > 0)
            {
                // Check for obstacle above.
                if (!IsObstacleInDirection(Vector2.up))
                    return Vector2.up;
                else
                    return Vector2.right; // or Vector2.left based on some other logic
            }
            else
            {
                // Check for obstacle below.
                if (!IsObstacleInDirection(Vector2.down))
                    return Vector2.down;
                else
                    return Vector2.right; // or Vector2.left based on some other logic
            }
        }
    }

    private bool IsObstacleInDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionDistance);
        if (hit.collider != null
            && hit.collider.tag != "Player"
            && hit.collider.tag != "Weapon"
            && hit.collider.tag != "Enemy"
            && hit.collider.tag != "Bound"
            && hit.collider.tag != "event")  // Enemy �±׸� �����ϴ� ������ �߰��մϴ�.
        {
            return true;
        }
        return false;
    }

    private void MoveInDirection(Vector2 direction)
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    IEnumerator HitSequence()
    {
        // �¾��� ���� �ִϸ��̼� �� ����
        animator.SetInteger("State", 4); // Hit �ִϸ��̼� ����
        attackCollider.SetActive(false);
        yield return new WaitForSeconds(0.2f); // 0.2�� ����
        OnHit(); // �и��� ���� ȣ��
        if (enemyHP <= 0)
        {
            ChangeState(EnemyState.Dead); // ���� ���� HP�� 0 ���϶�� ���� ���·� ��ȯ
        }
        else
        {
            ChangeState(EnemyState.Idle); // �ƴ϶�� Idle ���·� ��ȯ
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon") && currentState != EnemyState.Hit) // Weapon�� �ݶ��̴��� �ε��� ���
        {
            WeaponDamage weapon = collision.GetComponent<WeaponDamage>();
            if (weapon)
            {
                enemyHP -= weapon.damageAmount; // ������ ��������ŭ ü�� ���
                knockbackForce = 5 + Mathf.Sqrt(weapon.damageAmount);
                ChangeState(EnemyState.Hit); // ü���� ���� �����ִٸ�, �ǰ� ���·� ����
            }
        }
    }

    private void OnHit()
    {
        Vector2 knockbackDirection = (transform.position - playerTransform.position).normalized; // �÷��̾���� �ݴ� ���� ���

        GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(StopKnockback()); // �з����� ȿ�� ���߱�
    }

    IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(knockbackDuration);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero; // �ӵ� 0���� ����
    }

    private void DashTowardsPlayer()
    {
        Vector2 dashDirection = GetDirectionToPlayer();

        // Y�� "����" ������ ���
        float yOffset = Mathf.Sin(dashTime * 2 * Mathf.PI) * dashHeight;

        if (dashDirection.x > 0)
        {   
            transform.Translate(new Vector3(dashSpeed * Time.deltaTime, yOffset * Time.deltaTime, 0f));
        }
        else if (dashDirection.x < 0)
        {
            transform.Translate(new Vector3(-dashSpeed * Time.deltaTime, yOffset * Time.deltaTime, 0f));
        }

        dashTime += Time.deltaTime;
    }

    private void ChangeState(EnemyState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case EnemyState.Idle:
                animator.SetInteger("State", 0);
                break;
            case EnemyState.Aggro:
                animator.SetInteger("State", 1);

                // �÷��̾ ������ �����ʿ� �ִ��� üũ
                if (playerTransform.position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);  // �������� ������
                }
                else
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);  // ������ ������
                }
                break;
            case EnemyState.Moving:
                animator.SetInteger("State", 2);
                enemyCollider.isTrigger = false;  // ������ ������ �ݶ��̴��� ������� ����
                // �÷��̾ ������ �����ʿ� �ִ��� üũ
                if (playerTransform.position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);  // �������� ������
                }
                else
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);  // ������ ������
                }
                break;
            case EnemyState.Attacking:
                animator.SetInteger("State", 3);
                break;
            case EnemyState.Hit:
                animator.SetInteger("State", 4);
                break;
            case EnemyState.Dead:
                animator.SetInteger("State", 5);
                GetComponent<BoxCollider2D>().enabled = false; // Box Collider 2D ��Ȱ��ȭ
                break;
            case EnemyState.Dashing:
                animator.SetInteger("State", 6);
                enemyCollider.isTrigger = true;  // ���� �߿��� �ݶ��̴��� ����ϵ��� ����
                break;
        }
    }
    public void CutsceneMove(Vector2 direction, float distance, float duration)
    {
        StartCoroutine(CutsceneMoveCoroutine(direction, distance, duration));
    }

    private IEnumerator CutsceneMoveCoroutine(Vector2 direction, float distance, float duration)
    {
        float time = 0;
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + (direction.normalized * distance);

        animator.SetInteger("State", 2); // �޸��� �ִϸ��̼� Ȱ��ȭ

        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, endPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition; // ���� ��ġ ����
        animator.SetInteger("State", 0); // �޸��� �ִϸ��̼� ��Ȱ��ȭ, Idle ���·� ����
    }

    // �ƾ� ��Ʈ�ѷ� ��ũ��Ʈ ����
    // EnemyAI enemyAI = enemyGameObject.GetComponent<EnemyAI>();
    // enemyAI.PerformCutsceneMove(new Vector2(1, 0), 5f, 1.5f); // ���������� 5 ������ 1.5�� ���� �̵�
    private void DecreaseEnemyCount()
    {
        // targetScript���� "enemyCount"��� ������ ã���ϴ�.
        FieldInfo enemyCountField = targetScript.GetType().GetField("enemyCount", BindingFlags.Public | BindingFlags.Instance);

        if (enemyCountField != null && enemyCountField.FieldType == typeof(int))
        {
            int currentCount = (int)enemyCountField.GetValue(targetScript);
            if (currentCount > 0)
            {
                currentCount--; // ���� ���� ����
                enemyCountField.SetValue(targetScript, currentCount); // ����� ���� �ٽ� ����
            }
        }
        else
        {
            Debug.LogError("The target script does not have a public 'enemyCount' variable, or it is not an integer.");
        }
    }
}