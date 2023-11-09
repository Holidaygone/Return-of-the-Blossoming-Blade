using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PosionManager : MonoBehaviour
{
    public TextMeshProUGUI count;

    private DialogueManager dialogueManager;
    private PlayerStatus playerStatus;

    private int havePosion = 0;

    // Start is called before the first frame update
    void Start()
    {
        havePosion = PlayerPrefs.GetInt("havePosion");
        Debug.Log(havePosion);
        count.text = havePosion.ToString();
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
    }

    // Update is called once per frame
    void Update()
    {
            count.text = PlayerPrefs.GetInt("havePosion").ToString();
    }
}
