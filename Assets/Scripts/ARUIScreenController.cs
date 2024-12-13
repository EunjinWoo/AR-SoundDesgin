using UnityEngine;
using UnityEngine.UIElements;

public class ARUIScreenController : MonoBehaviour
{
    public AudioClip[] audioClips; // Inspector에서 AudioClip 배열로 설정
    private AudioClip selectedClip; // 현재 선택된 AudioClip
    private string selectedButtonName; // 현재 선택된 버튼 이름
    private UIDocument uiDocument; // UIDocument 참조
    private VisualElement root; // 루트 VisualElement 참조

    void OnEnable()
    {
        // UIDocument에서 VisualElement 가져오기
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        // 버튼 연결
        var buttonMusic = root.Q<Button>("Button_Music");
        var buttonApplause = root.Q<Button>("Button_Applause");
        var buttonLaugh = root.Q<Button>("Button_Laugh");
        var buttonBark = root.Q<Button>("Button_Bark");
        var buttonMeow = root.Q<Button>("Button_Meow");
        var buttonBirds = root.Q<Button>("Button_Birds");

        // 각 버튼의 클릭 이벤트에 메서드 연결
        buttonMusic?.RegisterCallback<ClickEvent>(evt => OnButtonClicked(0, "Button_Music"));
        buttonApplause?.RegisterCallback<ClickEvent>(evt => OnButtonClicked(1, "Button_Applause"));
        buttonLaugh?.RegisterCallback<ClickEvent>(evt => OnButtonClicked(2, "Button_Laugh"));
        buttonBark?.RegisterCallback<ClickEvent>(evt => OnButtonClicked(3, "Button_Bark"));
        buttonMeow?.RegisterCallback<ClickEvent>(evt => OnButtonClicked(4, "Button_Meow"));
        buttonBirds?.RegisterCallback<ClickEvent>(evt => OnButtonClicked(5, "Button_Birds"));
    }

    private void OnButtonClicked(int clipIndex, string buttonName)
    {
        if (clipIndex >= 0 && clipIndex < audioClips.Length)
        {
            selectedClip = audioClips[clipIndex];
            selectedButtonName = buttonName;
            Debug.Log($"AudioClip Selected: {selectedClip.name}, Button: {selectedButtonName}");
        }
        else
        {
            Debug.LogError("Invalid clip index!");
        }
    }

    public AudioClip GetSelectedClip()
    {
        return selectedClip;
    }

    public string GetSelectedButtonName()
    {
        return selectedButtonName;
    }

    public void SetMenuTransparency(float alpha)
    {
        if (root != null)
        {
            root.style.opacity = alpha;
        }
    }
}
