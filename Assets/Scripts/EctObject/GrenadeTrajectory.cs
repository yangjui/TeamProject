using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeTrajectory : MonoBehaviour
{

    [SerializeField] private GameObject trajectoryLinePrefab; // 궤도를 그리는 선의 프리팹
    [SerializeField] private int numDots; // 궤도를 그릴 점의 개수
    [SerializeField] private float dotSpacing; // 궤도를 그릴 점 간격

    [SerializeField] private float lender_right; // 궤도를 그릴 점 간격
    [SerializeField] private float lender_up; // 궤도를 그릴 점 간격
    [SerializeField] private float lender_forward; // 궤도를 그릴 점 간격

    private GameObject trajectoryLine; // 궤도를 그리는 선의 GameObject
    private LineRenderer lineRenderer; // 궤도를 그리는 선의 LineRenderer
    private Vector3 direction;

    private float angle = 0f; // 던질 각도
    private float throwForce; // 던질 힘

    private void OnEnable()
    {
        TrajectoryLineInit();
    }

    private void Update()
    {
        SetDotsPosition();
    }

    public void SetThrowForce(float _throwForce)
    {
        throwForce = _throwForce;
    }

    public void OnOffTrajectory(bool _on)
    {
        trajectoryLine.SetActive(_on);
    }

    private void TrajectoryLineInit()
    {
        // 궤도를 그리는 선의 GameObject 생성
        if (trajectoryLine == null)
        {
            trajectoryLine = Instantiate(trajectoryLinePrefab, transform.position, Quaternion.identity);
            trajectoryLine.transform.parent = transform;
            lineRenderer = trajectoryLine.GetComponent<LineRenderer>();
            // 라인 렌더러 설정
            lineRenderer.positionCount = numDots;
        }
        else if (trajectoryLine != null) return;
    }

    private void SetDotsPosition()
    {
        if (!trajectoryLine.activeSelf) return;
        // 궤도를 그리는 선의 위치 계산
        for (int i = 0; i < numDots; i++)
        {
            Vector3 position = CalculatePosition(i * dotSpacing);
            lineRenderer.SetPosition(i, position + transform.forward * lender_forward - transform.right * lender_right + transform.up * lender_up);
        }
    }

    // 궤도를 그리는 점들의 위치 계산하는 함수
    private Vector3 CalculatePosition(float _t)
    {
        direction = transform.root.forward;
        Vector3 velocity = Quaternion.AngleAxis(angle, Vector3.right) * direction * throwForce;
        Vector3 position = transform.position + velocity * _t + 0.5f * Physics.gravity * _t * _t;
        return position;
    }
}
