using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeleteSavePoint1 : MonoBehaviour
{
    private SaveContinueDialogueManager save;

    // Start is called before the first frame update
    void Start()
    {
        save = FindObjectOfType<SaveContinueDialogueManager>();
    }

    public void OnBtnClick()
    {

        //1 ��� ����
        save.delete[0].SetActive(false);//X��ư ��Ȱ��ȭ
        save.deleteText[0].text = "";
        save.goSavePoint[0].SetActive(false);//����ҷ� ���� ��ư ��Ȱ��ȭ
        save.saveButton[0].SetActive(true);//���� ��ư Ȱ��ȭ
        save.saveChapterName[0].text = "����";//���� �����
        save.saveChapter[0].text = "";//�Էµ� �� ����
        save.saveDate[0].text = "";
        PlayerPrefs.DeleteKey("save1");
        PlayerPrefs.DeleteKey("save1Name");
        PlayerPrefs.DeleteKey("save1Date");
    }
}
