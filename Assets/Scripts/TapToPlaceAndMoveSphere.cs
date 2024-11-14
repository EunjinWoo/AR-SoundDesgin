using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class TapToPlaceAndMoveSphere : MonoBehaviour
{
    public GameObject spherePrefab; // 생성할 Sphere 프리팹
    private GameObject currentSphere; // 현재 존재하는 Sphere 인스턴스

    private ARRaycastManager arRaycastManager;

    void Start()
    {
        // ARRaycastManager 컴포넌트를 가져옵니다.
        arRaycastManager = GetComponent<ARRaycastManager>();
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
                // 터치 위치에서 평면에 레이캐스트 실행
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose; // 첫 번째 히트 포인트 가져오기

                    // 이전 Sphere가 있다면 삭제
                    if (currentSphere != null)
                    {
                        Destroy(currentSphere);
                    }

                    // 새로운 Sphere 생성 및 위치 설정
                    currentSphere = Instantiate(spherePrefab, hitPose.position, hitPose.rotation);

                     // 오디오 소스 가져와서 소리 재생
                    AudioSource audioSource = currentSphere.GetComponent<AudioSource>();
                    if (audioSource != null)
                    {
                        audioSource.Play(); // 오브젝트가 생성될 때 소리 재생
                    }
                }
            }
        }
    }
}
