using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeleteSavePoint3 : MonoBehaviour
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
        save.delete[2].SetActive(false);//X��ư ��Ȱ��ȭ
        save.deleteText[2].text = "";
        save.goSavePoint[2].SetActive(false);//����ҷ� ���� ��ư ��Ȱ��ȭ
        save.saveButton[2].SetActive(true);//���� ��ư Ȱ��ȭ
        save.saveChapterName[2].text = "����";//���� �����
        save.saveChapter[2].text = "";//�Էµ� �� ����
        save.saveDate[2].text = "";
        PlayerPrefs.DeleteKey("save3");
        PlayerPrefs.DeleteKey("save3Name");
        PlayerPrefs.DeleteKey("save3Date");
    }
}
