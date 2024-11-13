using UnityEngine;

public class PlaceObjectRelativeToCamera : MonoBehaviour
{
    public GameObject sphere; // 왼쪽에 놓을 오브젝트
    public GameObject cube;   // 오른쪽에 놓을 오브젝트
    public float distance = 1.5f; // 카메라로부터의 거리

    private Transform cameraTransform;

    void Start()
    {
        // 카메라의 Transform을 가져옵니다.
        cameraTransform = Camera.main.transform;

        // 왼쪽과 오른쪽에 배치
        PlaceObjects();
    }

    void PlaceObjects()
    {
        // 왼쪽 위치: 카메라 기준 왼쪽(distance만큼 떨어진 위치)
        sphere.transform.position = cameraTransform.position + (-cameraTransform.right * distance);

        // 오른쪽 위치: 카메라 기준 오른쪽(distance만큼 떨어진 위치)
        cube.transform.position = cameraTransform.position + (cameraTransform.right * distance);
    }
}
