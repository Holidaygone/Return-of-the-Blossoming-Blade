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
        //1 기록 삭제
        save.delete[4].SetActive(false);//X버튼 비활성화
        save.deleteText[4].text = "";
        save.goSavePoint[4].SetActive(false);//저장소로 가는 버튼 비활성화
        save.saveButton[4].SetActive(true);//저장 버튼 활성화
        save.saveChapterName[4].text = "저장";//저장 만들기
        save.saveChapter[4].text = "";//입력된 값 삭제
        save.saveDate[4].text = "";
        PlayerPrefs.DeleteKey("save5");
        PlayerPrefs.DeleteKey("save5Name");
        PlayerPrefs.DeleteKey("save5Date");
    }
}
