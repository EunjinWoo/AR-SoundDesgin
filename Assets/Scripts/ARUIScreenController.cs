using UnityEngine;
using UnityEngine.UIElements;

public class ARUIScreenController : MonoBehaviour
{
    public AudioClip[] audioClips; // Inspector에서 AudioClip 배열로 설정
    private AudioClip selectedClip; // 현재 선택된 AudioClip
    private UIDocument uiDocument; // UIDocument 참조

    void OnEnable()
    {
        // UIDocument에서 VisualElement 가져오기
        uiDocument = GetComponent<UIDocument>();
        VisualElement root = uiDocument.rootVisualElement;

        // 버튼 연결
        var buttonMusic = root.Q<Button>("Button_Music");
        var buttonApplause = root.Q<Button>("Button_Applause");
        var buttonLaugh = root.Q<Button>("Button_Laugh");
        var buttonBark = root.Q<Button>("Button_Bark");
        var buttonMeow = root.Q<Button>("Button_Meow");
        var buttonBirds = root.Q<Button>("Button_Birds");

        // 각 버튼의 클릭 이벤트에 메서드 연결
        buttonMusic?.RegisterCallback<ClickEvent>(evt => OnButtonClicked(0));
        buttonApplause?.RegisterCallback<ClickEvent>(evt => OnButtonClicked(1));
        buttonLaugh?.RegisterCallback<ClickEvent>(evt => OnButtonClicked(2));
        buttonBark?.RegisterCallback<ClickEvent>(evt => OnButtonClicked(3));
        buttonMeow?.RegisterCallback<ClickEvent>(evt => OnButtonClicked(4));
        buttonBirds?.RegisterCallback<ClickEvent>(evt => OnButtonClicked(5));
    }

    private void OnButtonClicked(int clipIndex)
    {
        // 인덱스 범위 검사
        if (clipIndex >= 0 && clipIndex < audioClips.Length)
        {
            selectedClip = audioClips[clipIndex]; // 선택된 AudioClip 저장
            Debug.Log($"AudioClip Selected: {selectedClip.name}");
        }
        else
        {
            Debug.LogError("Invalid clip index!");
        }
    }

    public AudioClip GetSelectedClip()
    {
        return selectedClip; // 현재 선택된 Clip 반환
    }
}
