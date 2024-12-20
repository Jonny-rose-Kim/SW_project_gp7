using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class StageSelect : MonoBehaviour
{
    public Button[] stageButtons; // 스테이지 버튼 배열
    public Button MainMenu;
    public Text countdownText;       // 카운트다운 텍스트

    void Start()
    {
        ResetStageSelect(); // 초기화
    }

    public void ResetStageSelect()
    {
        // UI 초기화
        MainMenu.gameObject.SetActive(true);
        countdownText.gameObject.SetActive(false);

        for (int i = 0; i < stageButtons.Length; i++)
        {
            stageButtons[i].gameObject.SetActive(true);
        }

        UpdateStageButtons(); // 버튼 상태 업데이트
    }

    void UpdateStageButtons()
    {
        Debug.Log(PlayerPrefs.GetInt("StageUnlocked", 1));
        int unlockedStages = PlayerPrefs.GetInt("StageUnlocked", 1); // 잠금 해제된 스테이지 확인

        for (int i = 0; i < stageButtons.Length; i++)
        {
            int stageIndex = i + 1; // 스테이지 번호
            if (stageIndex <= unlockedStages)
            {
                stageButtons[i].interactable = true; // 잠금 해제
                stageButtons[i].onClick.RemoveAllListeners(); // 중복 이벤트 제거
                stageButtons[i].onClick.AddListener(() => LoadStage(stageIndex)); // 클릭 이벤트 추가
            }
            else
            {
                stageButtons[i].interactable = false; // 잠금
            }
        }
    }

    void LoadStage(int stageIndex)
    {
        /*StartCoroutine*/
        CountdownAndStart(stageIndex);
    }

    void CountdownAndStart(int stageIndex)
    {
        Debug.Log("Time.timeScale: " + Time.timeScale);
        Time.timeScale = 1f;
        MainMenu.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(true);
        foreach (Button button in stageButtons)
        {
            button.gameObject.SetActive(false);
        }
        

        SceneManager.LoadScene($"Stage{stageIndex}"); // 스테이지 로드
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("menu"); // 메인 메뉴로 돌아가기
    }
}
