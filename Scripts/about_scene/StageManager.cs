using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public int currentStageIndex; // 현재 스테이지 번호

    public void CompleteStage()
    {
        // 다음 스테이지 잠금 해제
        Debug.Log(PlayerPrefs.GetInt("StageUnlocked", 1));
        int unlockedStages = PlayerPrefs.GetInt("StageUnlocked", 1);
        if (currentStageIndex <= unlockedStages)
        {
            PlayerPrefs.SetInt("StageUnlocked", currentStageIndex + 1);
            Debug.Log(PlayerPrefs.GetInt("StageUnlocked", 1));
            PlayerPrefs.Save();
        }

        // 스테이지 선택 화면으로 돌아가기
    }
}
