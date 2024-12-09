using UnityEngine;
using UnityEngine.UIElements;

public class StartPageController : MonoBehaviour
{
    private VisualElement root;

    void OnEnable()
    {
        // UI Document의 루트 VisualElement 가져오기
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        // Splash 화면 자동 비활성화 타이머 시작
        StartSplashTimer();
    }

    private void StartSplashTimer()
    {
        // 5초 후 UI 비활성화
        Invoke(nameof(DisableSplashScreen), 5f);
    }

    private void DisableSplashScreen()
    {
        // UI 비활성화
        gameObject.SetActive(false);

        // 추가: 필요한 다른 동작(예: AR 환경 활성화)
        Debug.Log("스플래쉬 화면 종료 및 AR 환경 시작");
    }

    void OnDisable()
    {
        // 이벤트 리스너 해제
        if (root != null)
        {
            root.UnregisterCallback<ClickEvent>(OnScreenClicked);
        }
    }

    private void OnScreenClicked(ClickEvent evt)
    {
        // 유저가 탭해도 Splash는 유지되며 Debug 표시
        Debug.Log("스플래쉬 화면 표시 중");
    }
}
