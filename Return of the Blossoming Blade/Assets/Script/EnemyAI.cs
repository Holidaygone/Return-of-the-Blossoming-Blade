using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState
{
    Idle,
    Aggro,
    Moving,
    Attacking,
    Hit,
    Dead
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




    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        animator = GetComponent<Animator>();

        attackCollider = transform.Find("Attack Collider").gameObject;

        attackCollider.SetActive(false);  // ó���� ��Ȱ��ȭ
    }

    private void Update()
    {

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
                break;
            case EnemyState.Attacking:
                // �÷��̾ �����ϴ� ����
                StartCoroutine(AttackSequence());
                break;
            case EnemyState.Hit:
                // ���ظ� �޴� �ִϸ��̼� �� ����
                break;
            case EnemyState.Dead:
                // ���� �ִϸ��̼� �� ����
                break;
        }
    }

    IEnumerator MoveAfterDelay()
    {
        yield return new WaitForSeconds(aggroTimer);
        if (currentState == EnemyState.Aggro) // Aggro ���� Ȯ��
        {
            ChangeState(EnemyState.Moving);
        }
    }

    IEnumerator AttackSequence()
    {
        yield return new WaitForSeconds(0.4f);  // �ִϸ��̼� ���� �� 0.4�� ���
        attackCollider.SetActive(true);  // �ݶ��̴� Ȱ��ȭ
        yield return new WaitForSeconds(0.3f);  // 0.3�� ���� ����
        attackCollider.SetActive(false);  // �ٽ� ��Ȱ��ȭ
        if (distanceToPlayer <= attackRange)  // ���� ���� ���� ���� ��
        {
            ChangeState(EnemyState.Aggro);  // �ٽ� Aggro ���·�
        }
        else
        {
            ChangeState(EnemyState.Moving);  // �׷��� ������ �̵� ���·�
        }
    }

    private Vector2 GetDirectionToPlayer()
    {
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

        // �켱������ �÷��̾ ���� �������� ������ ������ Ȯ���մϴ�.
        if (!IsObstacleInDirection(directionToPlayer))
        {
            return directionToPlayer;
        }

        // �÷��̾ ���� ���⿡ ��ֹ��� �ִٸ�, �ٸ� ������ �������� �����Դϴ�.
        foreach (Vector2 dir in directions)
        {
            if (!IsObstacleInDirection(dir))
            {
                return dir;
            }
        }

        return Vector2.zero;  // ���� ��� ���⿡ ��ֹ��� �ִٸ� �������� �ʽ��ϴ�.
    }

    private bool IsObstacleInDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionDistance);
        if (hit.collider != null && hit.collider.tag != "Player")
        {
            return true;
        }
        return false;
    }

    private void MoveInDirection(Vector2 direction)
    {
        transform.Translate(direction * speed * Time.deltaTime);
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
                break;
            case EnemyState.Moving:
                animator.SetInteger("State", 2);
                break;
            case EnemyState.Attacking:
                animator.SetInteger("State", 3);
                break;
            case EnemyState.Hit:
                animator.SetInteger("State", 4);
                break;
            case EnemyState.Dead:
                animator.SetInteger("State", 5);
                break;
        }
    }
}