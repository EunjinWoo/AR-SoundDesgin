using UnityEngine;

public class TapToPlaceWithDynamicSpatialSound : MonoBehaviour
{
    public GameObject spherePrefab; // 생성할 Sphere 프리팹
    private GameObject currentSphere; // 현재 존재하는 Sphere 인스턴스

    private Camera arCamera;
    private LineRenderer lineRenderer; // Visual Indicator

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
    }

    void Update()
    {
        HandleTouchInput();
        UpdateAudioPosition();
        UpdateSphereScale();
        // UpdateVisualIndicator();
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

                if (currentSphere != null)
                {
                    Destroy(currentSphere);
                }

                currentSphere = Instantiate(spherePrefab, worldPosition, Quaternion.identity);

                AudioSource audioSource = currentSphere.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.spatialBlend = 1.0f; // 3D 사운드 활성화
                    audioSource.Play(); // 소리 재생
                }
            }
        }
    }

    private void UpdateAudioPosition()
{
    if (currentSphere == null) return;

    // 카메라와 Sphere 간 상대 위치 계산
    Vector3 directionToSphere = currentSphere.transform.position - arCamera.transform.position;
    float distance = directionToSphere.magnitude; // 거리 계산

    // 방향 벡터를 정규화
    directionToSphere.Normalize();

    // 오디오 소스 업데이트
    AudioSource audioSource = currentSphere.GetComponent<AudioSource>();
    if (audioSource != null)
    {
        audioSource.minDistance = 0.5f; // 최대 볼륨 거리
        audioSource.maxDistance = 10.0f; // 소리가 들리지 않는 거리
        audioSource.spatialBlend = 1.0f; // 3D 사운드 활성화
        audioSource.dopplerLevel = 2.0f; // Doppler 효과 강화
        audioSource.volume = Mathf.Clamp(1 / distance, 0.1f, 1.0f); // 거리 기반 볼륨 조정
    }

    // **Debug: 소리의 방향과 거리 정보**
    Vector3 cameraForward = arCamera.transform.forward; // 카메라가 향하는 방향
    float angle = Vector3.SignedAngle(cameraForward, directionToSphere, Vector3.up);
    Debug.Log($"Angle between camera and sphere: {angle} degrees");
}

    private void UpdateSphereScale()
    {
        if (currentSphere == null) return;

        float distance = Vector3.Distance(currentSphere.transform.position, arCamera.transform.position);

        float scaleFactor = Mathf.Clamp(1.0f / distance, 0.1f, 1.0f);
        currentSphere.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

    private void UpdateVisualIndicator()
    {
        if (currentSphere == null || lineRenderer == null) return;

        Vector3 spherePosition = currentSphere.transform.position;

        lineRenderer.SetPosition(0, arCamera.transform.position); // 시작점: 카메라
        lineRenderer.SetPosition(1, spherePosition); // 끝점: Sphere
    }
}
