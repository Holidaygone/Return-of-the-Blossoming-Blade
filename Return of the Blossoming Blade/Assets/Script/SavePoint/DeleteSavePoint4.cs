using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeleteSavePoint4 : MonoBehaviour
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
        save.delete[3].SetActive(false);//X��ư ��Ȱ��ȭ
        save.deleteText[3].text = "";
        save.goSavePoint[3].SetActive(false);//����ҷ� ���� ��ư ��Ȱ��ȭ
        save.saveButton[3].SetActive(true);//���� ��ư Ȱ��ȭ
        save.saveChapterName[3].text = "����";//���� �����
        save.saveChapter[3].text = "";//�Էµ� �� ����
        save.saveDate[3].text = "";
        PlayerPrefs.DeleteKey("save4");
        PlayerPrefs.DeleteKey("save4Name");
        PlayerPrefs.DeleteKey("save4Date");
    }
}
