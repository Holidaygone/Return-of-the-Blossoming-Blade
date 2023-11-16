using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class MainGoSavePoint1 : MonoBehaviour
{
    private ContinueDialogueManager save;
    private PlayerManager player;
    private CameraManager theCamera;
    private DialogueManager theDialogue;

    // Start is called before the first frame update
    void Start()
    {
        save = FindObjectOfType<ContinueDialogueManager>();
        player = FindObjectOfType<PlayerManager>();
        theCamera = FindObjectOfType<CameraManager>();
        theDialogue = FindObjectOfType<DialogueManager>();
    }

    public void OnBtnClick()
    {
        theDialogue.ShowLoading();

        //â �ݱ�
        save.CloseDialogue();

        PlayerPrefs.SetInt("onLoad", 1);
        //�� �̵�
        SceneManager.LoadScene(PlayerPrefs.GetString("save1SceneName"));

        theDialogue.UnShowLoading();
    }
}
