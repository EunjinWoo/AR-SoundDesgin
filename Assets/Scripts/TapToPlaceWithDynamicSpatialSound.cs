using UnityEngine;
using System.Collections.Generic;

public class TapToPlaceWithDynamicSpatialSound : MonoBehaviour
{
    public GameObject spherePrefab; // 생성할 Sphere 프리팹
    private Dictionary<string, GameObject> spheres = new Dictionary<string, GameObject>(); // 버튼별 Sphere 관리

    private Camera arCamera;
    private LineRenderer lineRenderer; // Visual Indicator
    private ARUIScreenController uiScreenController; // ARUIScreenController 참조

    void Start()
    {
        // 메인 카메라를 가져옵니다.
        arCamera = Camera.main;

        // Line Renderer 초기화
        GameObject lineObject = new GameObject("DirectionLine");
        lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.red;

        // ARUIScreenController 찾기
        uiScreenController = FindObjectOfType<ARUIScreenController>();
        if (uiScreenController == null)
        {
            Debug.LogError("ARUIScreenController not found in the scene!");
        }
    }

    void Update()
    {
        HandleTouchInput();
        UpdateSpheres();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector3 touchPosition = touch.position;
                touchPosition.z = 1.0f; // 카메라에서의 거리 설정 (1미터)

                Vector3 worldPosition = arCamera.ScreenToWorldPoint(touchPosition);

                // ARUIScreenController에서 선택된 버튼 이름 가져오기
                string selectedButton = uiScreenController?.GetSelectedButtonName();
                if (string.IsNullOrEmpty(selectedButton))
                {
                    Debug.LogWarning("No button selected!");
                    return;
                }

                // 기존 Sphere 삭제 (같은 버튼의 Sphere만 삭제)
                if (spheres.ContainsKey(selectedButton))
                {
                    Destroy(spheres[selectedButton]);
                    spheres.Remove(selectedButton);
                }

                // 새로운 Sphere 생성
                GameObject newSphere = Instantiate(spherePrefab, worldPosition, Quaternion.identity);
                spheres[selectedButton] = newSphere; // Sphere를 Dictionary에 저장

                // AudioSource 설정
                AudioSource audioSource = newSphere.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    // ARUIScreenController에서 선택된 Clip 가져오기
                    AudioClip selectedClip = uiScreenController?.GetSelectedClip();
                    if (selectedClip != null)
                    {
                        audioSource.clip = selectedClip;
                        audioSource.spatialBlend = 1.0f; // 3D 사운드 활성화
                        audioSource.Play();
                    }
                    else
                    {
                        Debug.LogWarning("No AudioClip selected in ARUIScreenController!");
                    }
                }
            }
        }
    }

    private void UpdateSpheres()
    {
        foreach (var sphere in spheres.Values)
        {
            if (sphere == null) continue;

            // 카메라와 Sphere 간 상대 위치 계산
            Vector3 directionToSphere = sphere.transform.position - arCamera.transform.position;
            float distance = directionToSphere.magnitude;

            // 오디오 소스 업데이트
            AudioSource audioSource = sphere.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.minDistance = 0.5f;
                audioSource.maxDistance = 10.0f;
                audioSource.volume = Mathf.Clamp(1 / distance, 0.1f, 1.0f);
            }

            // 크기 조정
            float scaleFactor = Mathf.Clamp(1.0f / distance, 0.1f, 1.0f);
            sphere.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }
    }
}
