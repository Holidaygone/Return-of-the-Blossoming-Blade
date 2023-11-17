using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeleteSavePoint2 : MonoBehaviour
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
        save.delete[1].SetActive(false);//X��ư ��Ȱ��ȭ
        save.deleteText[1].text = "";
        save.goSavePoint[1].SetActive(false);//����ҷ� ���� ��ư ��Ȱ��ȭ
        save.saveButton[1].SetActive(true);//���� ��ư Ȱ��ȭ
        save.saveChapterName[1].text = "����";//���� �����
        save.saveChapter[1].text = "";//�Էµ� �� ����
        save.saveDate[1].text = "";
        PlayerPrefs.DeleteKey("save2");
        PlayerPrefs.DeleteKey("save2Name");
        PlayerPrefs.DeleteKey("save2Date");
    }
}
