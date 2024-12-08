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

        // 클릭 이벤트 등록
        root.RegisterCallback<ClickEvent>(OnScreenClicked);
    }

    private void OnScreenClicked(ClickEvent evt)
    {
        // UI를 비활성화
        gameObject.SetActive(false);

        // 추가: 필요한 다른 동작(예: AR 환경 활성화)
        Debug.Log("UI 비활성화 및 AR 환경 시작");
    }

    void OnDisable()
    {
        // 이벤트 리스너 해제
        if (root != null)
        {
            root.UnregisterCallback<ClickEvent>(OnScreenClicked);
        }
    }
}
