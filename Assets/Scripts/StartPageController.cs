using UnityEngine;
using UnityEngine.UIElements;

public class StartPageController : MonoBehaviour
{
    private VisualElement root;
    private ARUIScreenController arScreenController;

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        // ARUIScreenController 찾기
        arScreenController = FindObjectOfType<ARUIScreenController>();
        if (arScreenController != null)
        {
            arScreenController.SetMenuTransparency(0f); // ARSoundMenu 투명하게 설정
        }

        StartSplashTimer();
    }

    private void StartSplashTimer()
    {
        Invoke(nameof(DisableSplashScreen), 5f);
    }

    private void DisableSplashScreen()
    {
        gameObject.SetActive(false);

        if (arScreenController != null)
        {
            arScreenController.SetMenuTransparency(1f); // ARSoundMenu 투명도 복원
        }

        Debug.Log("Splash screen hidden, AR environment activated.");
    }
}
