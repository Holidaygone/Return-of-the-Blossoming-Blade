using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class End5 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;
    public Dialogue dialogue_3;
    public Dialogue dialogue_4;
    public Dialogue dialogue_5;

    private BGMManager bgmManager;
    public int playMusicTrack1;
    public int playMusicTrack2;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;
    private ChapterManager theChapter;

    public string swordSound;
    private AudioManager theAudio;

    //private bool flag;
    private bool can = false;
    private bool one = true;

    public static bool end = false;

    public GameObject hpBar;
    public GameObject bossName;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
        bgmManager = FindObjectOfType<BGMManager>();
        theAudio = FindObjectOfType<AudioManager>();
        theChapter = FindObjectOfType<ChapterManager>();
    }

    void Update()
    {
        if (one && end)
        {
            one = false;
            TransferMap[] temp = FindObjectsOfType<TransferMap>();
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].gateName.Equals("EndPoint5"))
                {
                    temp[i].move = false;
                    break;
                }
            }
            StartCoroutine(EventCoroutine());
        }
    }

    IEnumerator EventCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        yield return new WaitForSeconds(0.2f);
        bgmManager.Stop();
        bgmManager.Play(playMusicTrack1);

        theOrder.Action("Player", "LAST");

        hpBar.SetActive(false);
        bossName.SetActive(false);

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);

        theDM.ShowDialogue(dialogue_2);
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "UP");
        yield return new WaitUntil(() => !theDM.talking);
        theDM.ShowDialogue(dialogue_3);
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.Action("Player", "AttackH");
        bgmManager.Stop();
        theAudio.Play(swordSound);

        theDM.ShowDialogue(dialogue_4);
        theOrder.Action("Player", "DIE");
        bgmManager.Play(playMusicTrack2);
        yield return new WaitUntil(() => !theDM.talking);


        theOrder.Appear("BlackScreen", true);
        theDM.ShowDialogue(dialogue_5);
        yield return new WaitUntil(() => !theDM.talking);

        theChapter.ShowChapter("�ḻ 5\nõ��õ�� �����Ӻ�");
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("Main");

        theOrder.Move();
    }
}

