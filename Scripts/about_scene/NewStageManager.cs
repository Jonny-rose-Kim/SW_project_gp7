using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class NewStageManager : MonoBehaviour
{
    public int currentStageIndex = 1; // 기본 스테이지 번호
    public int playerScore; // 현재 스테이지에서의 플레이어 점수
    public int maxScoresToSave = 3; // 저장할 최대 점수 개수

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject); // 씬 전환 시 삭제되지 않도록 설정
    }

    // 스테이지 번호 설정
    public void SetStageIndex(int stageIndex)
{
    currentStageIndex = stageIndex;
    Debug.Log($"Stage index set to {currentStageIndex}");
}


    // 스테이지 완료 처리
    public void CompleteStage()
    {
        Debug.Log($"Completing Stage {currentStageIndex} with Score {playerScore}");

        // 점수 저장
        SaveStageScore(currentStageIndex, playerScore);

        // 다음 스테이지 잠금 해제
        UnlockNextStage();

        // 스테이지 선택 화면으로 이동
        SceneManager.LoadScene("Stage_select");
    }

    // 점수 저장
    public void SaveStageScore(int stageNumber, int newScore)
    {
        // 점수 로드 및 정렬
        List<int> scores = LoadStageScores(stageNumber);
        scores.Add(newScore);
        scores.Sort((a, b) => b.CompareTo(a)); // 내림차순 정렬

        // 상위 점수 유지
        while (scores.Count > maxScoresToSave)
        {
            scores.RemoveAt(scores.Count - 1);
        }

        // 저장
        for (int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.SetInt($"Stage{stageNumber}_Score{i + 1}", scores[i]);
        }

        PlayerPrefs.Save();
        Debug.Log($"Stage {stageNumber} scores updated: {string.Join(", ", scores)}");
    }

    // 점수 로드
    public List<int> LoadStageScores(int stageNumber)
    {
        List<int> scores = new List<int>();

        for (int i = 1; i <= maxScoresToSave; i++)
        {
            string key = $"Stage{stageNumber}_Score{i}";
            if (PlayerPrefs.HasKey(key))
            {
                scores.Add(PlayerPrefs.GetInt(key));
            }
        }

        return scores;
    }

    // 다음 스테이지 잠금 해제
    private void UnlockNextStage()
    {
        int unlockedStages = PlayerPrefs.GetInt("StageUnlocked", 1); // 잠금 해제된 스테이지 확인

        if (currentStageIndex >= unlockedStages)
        {
            int nextStage = currentStageIndex + 1;
            PlayerPrefs.SetInt("StageUnlocked", nextStage);
            PlayerPrefs.Save();
            Debug.Log($"Stage {nextStage} unlocked!");
        }
    }
}


