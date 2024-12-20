using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // 일시정지 메뉴 UI
    public Button resetButton; // Reset 버튼
    public Button ReturnMenu; // End 버튼

    private bool isPaused = false; // 게임이 정지 상태인지 확인

    void Start()
    {
        // 버튼에 클릭 이벤트 연결
        resetButton.onClick.AddListener(ResetGame);
        ReturnMenu.onClick.AddListener(Return_to_Menu);

        // 게임 시작 시 메뉴 숨기기
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        // ESC 키 입력으로 일시정지 메뉴 토글
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // 게임을 정지하고 메뉴를 표시
    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // 게임 일시정지
        isPaused = true;
    }

    // 게임을 재개하고 메뉴를 숨김
    void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // 게임 재개
        isPaused = false;
    }

    // Reset 버튼 클릭 시 호출
    void ResetGame()
    {
        Time.timeScale = 1f; // 게임 속도 정상화
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬 다시 로드
    }

    // End 버튼 클릭 시 호출
    void Return_to_Menu()
    {
        Time.timeScale = 1f; // 게임 속도 정상화
        SceneManager.LoadScene("menu"); // "menu" 씬으로 이동
        Debug.Log("Game Ended"); // 에디터에서 실행 시 종료 확인
    }
}
