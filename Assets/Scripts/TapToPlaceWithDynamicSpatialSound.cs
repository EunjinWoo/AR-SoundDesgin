using UnityEngine;

public class TapToPlaceWithDynamicSpatialSound : MonoBehaviour
{
    public GameObject spherePrefab; // 생성할 Sphere 프리팹
    private GameObject currentSphere; // 현재 존재하는 Sphere 인스턴스

    private Camera arCamera;

    void Start()
    {
        // 메인 카메라를 가져옵니다.
        arCamera = Camera.main;
    }

    void Update()
    {
        HandleTouchInput();
        UpdateAudioPosition();
    }

    private void HandleTouchInput()
    {
        // 터치가 1개 이상일 때만 실행
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // 터치가 시작되었을 때만 처리
            if (touch.phase == TouchPhase.Began)
            {
                // 터치한 화면 좌표를 가져옵니다.
                Vector3 touchPosition = touch.position;
                touchPosition.z = 1.0f; // 카메라에서의 거리 설정 (1미터)

                // 터치 좌표를 월드 좌표로 변환
                Vector3 worldPosition = arCamera.ScreenToWorldPoint(touchPosition);

                // 이전 Sphere가 있다면 삭제
                if (currentSphere != null)
                {
                    Destroy(currentSphere);
                }

                // 새로운 Sphere 생성
                currentSphere = Instantiate(spherePrefab, worldPosition, Quaternion.identity);

                // 오디오 소스 가져와서 초기 소리 재생
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
    // Sphere가 없으면 업데이트 불필요
    if (currentSphere == null) return;

    // Sphere의 위치와 카메라 간의 상대 위치 계산
    Vector3 directionToSphere = currentSphere.transform.position - arCamera.transform.position;
    float distance = directionToSphere.magnitude; // 카메라와 Sphere 간 거리

    // 방향 벡터를 정규화
    directionToSphere.Normalize();

    // Sphere의 위치를 카메라 기준으로 업데이트
    currentSphere.transform.position = arCamera.transform.position + directionToSphere * distance;

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
}

}
