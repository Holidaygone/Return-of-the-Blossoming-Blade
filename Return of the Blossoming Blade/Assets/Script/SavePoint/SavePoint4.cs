using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SavePoint4 : MonoBehaviour
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
            PlayerPrefs.SetString("save4", "��1��");
            save.saveChapter[3].text = "��1��";
        }
        else
        {
            string chapter = PlayerPrefs.GetInt("chapter").ToString();
            PlayerPrefs.SetString("save4", "��" + chapter + "��");
            save.saveChapter[3].text = "��" + chapter + "��";
        }
        string chapterName = GetChapterName();
        PlayerPrefs.SetString("save4Name", chapterName);
        save.saveChapterName[3].text = chapterName;
        string today = DateTime.Now.ToString("yyyy��MM��dd��");
        PlayerPrefs.SetString("save4Date", today);
        save.saveDate[3].text = today;

        save.saveButton[3].SetActive(false);
        save.goSavePoint[3].SetActive(true);

        save.delete[3].SetActive(true);
        save.deleteText[3].text = "X";

        //�߰� ����
        PlayerPrefs.SetString("save4SceneName", GetSceneName());
        PlayerPrefs.SetFloat("save4PlayerHP", PlayerPrefs.GetFloat("playerHP"));
        PlayerPrefs.SetFloat("save4PlayerMP", PlayerPrefs.GetFloat("playerMP"));
        PlayerPrefs.SetFloat("save4HavePosion", PlayerPrefs.GetFloat("havePosion"));
        PlayerPrefs.SetFloat("save4MaxPosion", PlayerPrefs.GetFloat("maxPosion"));

        PlayerPrefs.SetString("save4MapName", PlayerPrefs.GetString("playerMapName"));
        PlayerPrefs.SetString("save4playerGateName", PlayerPrefs.GetString("playerGateName"));
        PlayerPrefs.SetInt("save4CJEvent2One", PlayerPrefs.GetInt("CJEvent2One"));
        PlayerPrefs.SetInt("save4Choice1", PlayerPrefs.GetInt("choice1"));
        PlayerPrefs.SetInt("save4Choice2", PlayerPrefs.GetInt("choice2"));
        PlayerPrefs.SetInt("save4Choice3", PlayerPrefs.GetInt("choice3"));
        PlayerPrefs.SetFloat("save4JeokCheonPlayTime", PlayerPrefs.GetFloat("JeokCheonPlayTime"));
        PlayerPrefs.SetFloat("save4GwanghonPlayTime", PlayerPrefs.GetFloat("GwanghonPlayTime"));
        PlayerPrefs.SetFloat("save4ChunsalPlayTime", PlayerPrefs.GetFloat("ChunsalPlayTime"));
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
