using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SavePoint2 : MonoBehaviour
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
            PlayerPrefs.SetString("save2", "��1��");
            save.saveChapter[1].text = "��1��";
        }
        else
        {
            string chapter = PlayerPrefs.GetInt("chapter").ToString();
            PlayerPrefs.SetString("save2", "��" + chapter + "��");
            save.saveChapter[1].text = "��" + chapter + "��";
        }
        string chapterName = GetChapterName();
        PlayerPrefs.SetString("save2Name", chapterName);
        save.saveChapterName[1].text = chapterName;
        string today = DateTime.Now.ToString("yyyy��MM��dd��");
        PlayerPrefs.SetString("save2Date", today);
        save.saveDate[1].text = today;

        save.saveButton[1].SetActive(false);
        save.goSavePoint[1].SetActive(true);

        save.delete[1].SetActive(true);
        save.deleteText[1].text = "X";

        //�߰� ����
        PlayerPrefs.SetString("save2SceneName", GetSceneName());
        PlayerPrefs.SetFloat("save2PlayerHP", PlayerPrefs.GetFloat("playerHP"));
        PlayerPrefs.SetFloat("save2PlayerMP", PlayerPrefs.GetFloat("playerMP"));
        PlayerPrefs.SetFloat("save2HavePosion", PlayerPrefs.GetFloat("havePosion"));
        PlayerPrefs.SetFloat("save2MaxPosion", PlayerPrefs.GetFloat("maxPosion"));

        PlayerPrefs.SetString("save2MapName", PlayerPrefs.GetString("playerMapName"));
        PlayerPrefs.SetString("save2playerGateName", PlayerPrefs.GetString("playerGateName"));
        PlayerPrefs.SetInt("save2CJEvent2One", PlayerPrefs.GetInt("CJEvent2One"));
        PlayerPrefs.SetInt("save2Choice1", PlayerPrefs.GetInt("choice1"));
        PlayerPrefs.SetInt("save2Choice2", PlayerPrefs.GetInt("choice2"));
        PlayerPrefs.SetInt("save2Choice3", PlayerPrefs.GetInt("choice3"));
        PlayerPrefs.SetFloat("save2JeokCheonPlayTime", PlayerPrefs.GetFloat("JeokCheonPlayTime"));
        PlayerPrefs.SetFloat("save2GwanghonPlayTime", PlayerPrefs.GetFloat("GwanghonPlayTime"));
        PlayerPrefs.SetFloat("save2ChunsalPlayTime", PlayerPrefs.GetFloat("ChunsalPlayTime"));
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
