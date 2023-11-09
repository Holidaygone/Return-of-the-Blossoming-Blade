using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyAttack : MonoBehaviour
{
    private EnemyAI enemyAI;  // �θ� ������Ʈ�� EnemyAI ��ũ��Ʈ�� ���� ����

    private void Start()
    {
        // �θ� ������Ʈ���� EnemyAI ������Ʈ�� ã�� �����մϴ�.
        enemyAI = GetComponentInParent<EnemyAI>();

        if (enemyAI == null)
        {
            Debug.LogError("EnemyAI component not found on parent object.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name + " with tag: " + collision.tag);

        // Weapon �±׿��� �浹�� ����
        if (collision.gameObject.CompareTag("Weapon"))
        {
            return;
        }

        // ���� �ε��� ������Ʈ�� PlayerCombatCol �±׸� ������ �ִٸ�
        if (collision.CompareTag("PlayerCombatCol"))
        {
            // PlayerStatus ��ũ��Ʈ�� �����ɴϴ�. (�̶� PlayerCombatCol�� �θ��� Player ������Ʈ�κ��� �����ɴϴ�.)
            PlayerStatus playerStatus = collision.transform.parent.GetComponent<PlayerStatus>();

            if (playerStatus)
            {
                // PlayerStatus�� TakeDamage �Լ��� ȣ���Ͽ� damageToPlayer ����ŭ�� �������� �ݴϴ�.
                playerStatus.TakeDamage(enemyAI.damageToPlayer);
            }
        }
    }
}