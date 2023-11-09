using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CJCutScene4 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;

    //private bool flag;
    private bool can = false;
    private bool one = true;

    public bool enable = false;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (one && enable)
        {
            one = false;
            StartCoroutine(EventCoroutine());
        }
    }

    IEnumerator EventCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        yield return new WaitForSeconds(0.2f);

        theOrder.Appear("BlackScreen", true);
        if (PlayerPrefs.HasKey("choice2"))
        {
            if (PlayerPrefs.GetInt("choice2") == 1)
            {
                theDM.ShowDialogue(dialogue_1);
                yield return new WaitUntil(() => !theDM.talking);
            }
        }
        if (PlayerPrefs.HasKey("choice3"))
        {
            if (PlayerPrefs.GetInt("choice3") == 1)
            {
                theDM.ShowDialogue(dialogue_2);
                yield return new WaitUntil(() => !theDM.talking);
            }
        }
        theOrder.Appear("BlackScreen", false);

        TransferScene[] temp = FindObjectsOfType<TransferScene>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].gateName.Equals("GoToChunma"))
            {
                temp[i].move = true;
                break;
            }
        }

        theOrder.Move();
    }
}

