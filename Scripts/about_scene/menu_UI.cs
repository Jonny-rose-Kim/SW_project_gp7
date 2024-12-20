#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu_UI : MonoBehaviour
{
    public Button startButton;       // "게임하기" 버튼
    public Button infoButton;        // "설명보기" 버튼
    public Button closeButton;       // "설명창 닫기" 버튼
    public Button quitButton;        // "종료" 버튼
    public Button resetDataButton;   // "데이터 초기화" 버튼 (새로 추가)
    public GameObject infoPanel;     // 설명 패널

    void Start()
    {
        Time.timeScale = 1f;
        // Debug로 버튼 연결 여부 확인
        if (startButton == null || infoButton == null || quitButton == null || infoPanel == null || resetDataButton == null)
        {
            Debug.LogError("Buttons or InfoPanel are not assigned in the Inspector!");
            return;
        }

        startButton.onClick.AddListener(StartGame);
        infoButton.onClick.AddListener(ShowInfo);
        quitButton.onClick.AddListener(QuitGame);
        closeButton.onClick.AddListener(CloseInfoPanel);
        resetDataButton.onClick.AddListener(ResetPlayerPrefs); // Reset 버튼 클릭 이벤트 추가

        infoPanel.SetActive(false); // 설명 패널 초기 비활성화
    }

    void StartGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Start Button Clicked");
        SceneManager.LoadScene("Stage_select");
    }

    void ShowInfo()
    {
        Debug.Log("Info Button Clicked");
        infoPanel.SetActive(true);
    }

    void CloseInfoPanel()
    {
        infoPanel.SetActive(false);   // 설명 화면 비활성화
    }

    void QuitGame()
    {
        Debug.Log("Quit Button Clicked");
        #if UNITY_EDITOR
    // 에디터에서 게임 모드 종료
    EditorApplication.isPlaying = false;
#else
    // 빌드된 게임 종료
    Application.Quit();
#endif
    }

    void ResetPlayerPrefs()
    {
        Debug.Log("Resetting PlayerPrefs...");
        PlayerPrefs.DeleteAll(); // PlayerPrefs 데이터 초기화
        PlayerPrefs.Save();      // 변경된 데이터 저장
        Debug.Log("All PlayerPrefs data has been reset.");
    }
}