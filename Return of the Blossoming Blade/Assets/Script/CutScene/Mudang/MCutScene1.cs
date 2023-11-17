using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCutScene1 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;

    public CameraManager theCamera; // ī�޶� �Ŵ��� ���� �߰�


    //private bool flag;
    private bool can = false;
    private bool one = true;
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
        if (one)
        {
            one = false;
            StartCoroutine(EventCoroutine());
        }
    }

    IEnumerator EventCoroutine()
    {
        theCamera.SetCutsceneMode(true, new Vector3(-1384f, -276f, theCamera.transform.position.z)); // �ƽ� ��ġ�� �̵�

        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        yield return new WaitForSeconds(0.2f);

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.Action("Student1", "DIE");

        theDM.ShowDialogue(dialogue_2);
        yield return new WaitUntil(() => !theDM.talking);

        theOrder.Move();
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");

        theCamera.SetCutsceneMode(false, Vector3.zero); // �ƽ� ���� �� ���� ���� ���ư���
    }
}
