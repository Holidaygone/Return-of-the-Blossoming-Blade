using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SavePoint1 : MonoBehaviour
{
    private SaveContinueDialogueManager save;

    // Start is called before the first frame update
    void Start()
    {
        save = FindObjectOfType<SaveContinueDialogueManager>();
    }

    public void OnBtnClick()
    {
        if (!PlayerPrefs.HasKey("chapter"))
        {
            PlayerPrefs.SetInt("chapter", 1);
            PlayerPrefs.SetString("save1", "��1��");
            save.saveChapter[0].text = "��1��";
        }
        else
        {
            string chapter = PlayerPrefs.GetInt("chapter").ToString();
            PlayerPrefs.SetString("save1", "��" + chapter + "��");
            save.saveChapter[0].text = "��" + chapter + "��";
        }
        string chapterName = GetChapterName();
        PlayerPrefs.SetString("save1Name", chapterName);
        save.saveChapterName[0].text = chapterName;
        string today = DateTime.Now.ToString("yyyy��MM��dd��");
        PlayerPrefs.SetString("save1Date", today);
        save.saveDate[0].text = today;

        save.saveButton[0].SetActive(false);
        save.goSavePoint[0].SetActive(true);

        save.delete[0].SetActive(true);
        save.deleteText[0].text = "X";

        //�߰� ����
        PlayerPrefs.SetString("save1SceneName", GetSceneName());
        PlayerPrefs.SetFloat("save1PlayerHP", PlayerPrefs.GetFloat("playerHP"));
        PlayerPrefs.SetFloat("save1PlayerMP", PlayerPrefs.GetFloat("playerMP"));
        PlayerPrefs.SetFloat("save1HavePosion", PlayerPrefs.GetFloat("havePosion"));
        PlayerPrefs.SetFloat("save1MaxPosion", PlayerPrefs.GetFloat("maxPosion"));

        PlayerPrefs.SetString("save1MapName", PlayerPrefs.GetString("playerMapName"));
        PlayerPrefs.SetString("save1playerGateName", PlayerPrefs.GetString("playerGateName"));
        PlayerPrefs.SetInt("save1CJEvent2One", PlayerPrefs.GetInt("CJEvent2One"));
        PlayerPrefs.SetInt("save1Choice1", PlayerPrefs.GetInt("choice1"));
        PlayerPrefs.SetInt("save1Choice2", PlayerPrefs.GetInt("choice2"));
        PlayerPrefs.SetInt("save1Choice3", PlayerPrefs.GetInt("choice3"));
        PlayerPrefs.SetFloat("save1JeokCheonPlayTime", PlayerPrefs.GetFloat("JeokCheonPlayTime"));
        PlayerPrefs.SetFloat("save1GwanghonPlayTime", PlayerPrefs.GetFloat("GwanghonPlayTime"));
        PlayerPrefs.SetFloat("save1ChunsalPlayTime", PlayerPrefs.GetFloat("ChunsalPlayTime"));
    }

    public string GetSceneName()
    {
        switch (PlayerPrefs.GetInt("chapter"))
        {
            case 1:
                return "BlossomingBlade";
                break;
            case 2:
                return "OutOfMainland";
                break;
            case 3:
                return "Mudang";
                break;
            case 4:
                return "Jongnam";
                break;
            case 5:
                return "DangHouse";
                break;
            case 6:
                return "CheongJin";
                break;
            case 7:
                return "Chunma";
                break;
        }
        return "";
    }

    public string GetChapterName()
    {
        switch (PlayerPrefs.GetInt("chapter"))
        {
            case 1:
                return "����";
                break;
            case 2:
                return "���ܹ湮";
                break;
            case 3:
                return "����湮";
                break;
            case 4:
                return "�����湮";
                break;
            case 5:
                return "�簡�湮";
                break;
            case 6:
                return "�������� ����";
                break;
            case 7:
                return "�������";
                break;
        }
        return "";
    }
}
