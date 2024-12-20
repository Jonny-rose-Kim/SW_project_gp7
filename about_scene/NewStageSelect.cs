using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class NewStageSelect : MonoBehaviour
{
    public Button[] stageButtons; // 스테이지 버튼 배열
    public Text[] scoreTexts;     // 각 스테이지 점수를 표시할 Text 배열
    public Button MainMenu;
    public Text countdownText;    // 카운트다운 텍스트

    void Start()
    {
        Time.timeScale = 1f;
        ResetStageSelect(); // 초기화
    }

    public void ResetStageSelect()
    {
        Debug.Log("Time.timeScale: " + Time.timeScale);
        Time.timeScale = 1f;
        // UI 초기화
        MainMenu.gameObject.SetActive(true);
        countdownText.gameObject.SetActive(false);

        for (int i = 0; i < stageButtons.Length; i++)
        {
            stageButtons[i].gameObject.SetActive(true);
        }

        UpdateStageButtons(); // 버튼 상태 업데이트
        UpdateStageScores();  // 점수 업데이트
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

    void UpdateStageScores()
    {
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            int stageNumber = i + 1; // 스테이지 번호
            List<int> scores = LoadStageScores(stageNumber);

            if (scores.Count > 0)
            {
                scoreTexts[i].text = $"Stage {stageNumber} Scores:\n1. {scores[0]}\n2. {scores[1]}\n3. {scores[2]}";
            }
            else
            {
                scoreTexts[i].text = $"Stage {stageNumber}:\nNo Scores Yet";
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
        foreach (Text text in scoreTexts)
        {
            text.gameObject.SetActive(false);
        }

        SceneManager.LoadScene($"Stage{stageIndex}"); // 스테이지 로드
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("menu"); // 메인 메뉴로 돌아가기
    }

    List<int> LoadStageScores(int stageNumber)
    {
        List<int> scores = new List<int>();

        for (int i = 1; i <= 3; i++) // 상위 3개의 점수 불러오기
        {
            string key = $"Stage{stageNumber}_Score{i}";
            if (PlayerPrefs.HasKey(key))
            {
                scores.Add(PlayerPrefs.GetInt(key));
            }
            else
            {
                scores.Add(0); // 점수가 없는 경우 0으로 초기화
            }
        }

        return scores;
    }
}

