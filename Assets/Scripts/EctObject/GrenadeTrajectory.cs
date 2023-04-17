using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeTrajectory : MonoBehaviour
{

    [SerializeField] private GameObject trajectoryLinePrefab; // �˵��� �׸��� ���� ������
    [SerializeField] private int numDots; // �˵��� �׸� ���� ����
    [SerializeField] private float dotSpacing; // �˵��� �׸� �� ����

    [SerializeField] private float lender_right; // �˵��� �׸� �� ����
    [SerializeField] private float lender_up; // �˵��� �׸� �� ����
    [SerializeField] private float lender_forward; // �˵��� �׸� �� ����

    private GameObject trajectoryLine; // �˵��� �׸��� ���� GameObject
    private LineRenderer lineRenderer; // �˵��� �׸��� ���� LineRenderer
    private Vector3 direction;

    private float angle = 0f; // ���� ����
    private float throwForce; // ���� ��

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
        // �˵��� �׸��� ���� GameObject ����
        if (trajectoryLine == null)
        {
            trajectoryLine = Instantiate(trajectoryLinePrefab, transform.position, Quaternion.identity);
            trajectoryLine.transform.parent = transform;
            lineRenderer = trajectoryLine.GetComponent<LineRenderer>();
            // ���� ������ ����
            lineRenderer.positionCount = numDots;
        }
        else if (trajectoryLine != null) return;
    }

    private void SetDotsPosition()
    {
        if (!trajectoryLine.activeSelf) return;
        // �˵��� �׸��� ���� ��ġ ���
        for (int i = 0; i < numDots; i++)
        {
            Vector3 position = CalculatePosition(i * dotSpacing);
            lineRenderer.SetPosition(i, position + transform.forward * lender_forward - transform.right * lender_right + transform.up * lender_up);
        }
    }

    // �˵��� �׸��� ������ ��ġ ����ϴ� �Լ�
    private Vector3 CalculatePosition(float _t)
    {
        direction = transform.root.forward;
        Vector3 velocity = Quaternion.AngleAxis(angle, Vector3.right) * direction * throwForce;
        Vector3 position = transform.position + velocity * _t + 0.5f * Physics.gravity * _t * _t;
        return position;
    }
}
