using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GoSavePoint2 : MonoBehaviour
{
    private SaveContinueDialogueManager save;
    private PlayerManager player;
    private CameraManager theCamera;
    private DialogueManager theDialogue;

    // Start is called before the first frame update
    void Start()
    {
        save = FindObjectOfType<SaveContinueDialogueManager>();
        player = FindObjectOfType<PlayerManager>();
        theCamera = FindObjectOfType<CameraManager>();
        theDialogue = FindObjectOfType<DialogueManager>();
    }

    public void OnBtnClick()
    {
        theDialogue.ShowLoading();

        //â �ݱ�
        save.CloseSaveDialogue();

        PlayerPrefs.SetInt("onLoad", 2);
        //�� �̵�
        SceneManager.LoadScene(PlayerPrefs.GetString("save2SceneName"));

        theDialogue.UnShowLoading();
    }
}
