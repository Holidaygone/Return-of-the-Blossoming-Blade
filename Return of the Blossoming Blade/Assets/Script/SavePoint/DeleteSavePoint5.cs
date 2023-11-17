using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeleteSavePoint5 : MonoBehaviour
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
        save.delete[4].SetActive(false);//X��ư ��Ȱ��ȭ
        save.deleteText[4].text = "";
        save.goSavePoint[4].SetActive(false);//����ҷ� ���� ��ư ��Ȱ��ȭ
        save.saveButton[4].SetActive(true);//���� ��ư Ȱ��ȭ
        save.saveChapterName[4].text = "����";//���� �����
        save.saveChapter[4].text = "";//�Էµ� �� ����
        save.saveDate[4].text = "";
        PlayerPrefs.DeleteKey("save5");
        PlayerPrefs.DeleteKey("save5Name");
        PlayerPrefs.DeleteKey("save5Date");
    }
}
