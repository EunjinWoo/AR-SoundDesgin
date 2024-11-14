using UnityEngine;

public class TapToPlaceWithSpatialSound : MonoBehaviour
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
        // 터치가 1개 이상일 때만 실행
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // 터치가 시작되었을 때만 처리
            if (touch.phase == TouchPhase.Began)
            {
                // 터치한 화면 좌표를 3D 공간의 좌표로 변환
                Vector3 touchPosition = touch.position;
                touchPosition.z = 1.0f; // 카메라에서의 거리 설정 (필요에 따라 조절 가능)

                // 터치 위치를 기준으로 월드 좌표 변환
                Vector3 worldPosition = arCamera.ScreenToWorldPoint(touchPosition);

                // 이전 Sphere가 있다면 삭제
                if (currentSphere != null)
                {
                    Destroy(currentSphere);
                }

                // 새로운 Sphere 생성 및 위치 설정
                currentSphere = Instantiate(spherePrefab, worldPosition, Quaternion.identity);

                // 오디오 소스 가져와서 소리 재생
                AudioSource audioSource = currentSphere.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.spatialBlend = 1.0f; // 3D 사운드 적용
                    audioSource.Play(); // 오브젝트가 생성될 때 소리 재생
                }
            }
        }
    }
}
