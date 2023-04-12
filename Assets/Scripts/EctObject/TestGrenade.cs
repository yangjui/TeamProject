using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrenade : MonoBehaviour
{

    [SerializeField] private GameObject trajectoryLinePrefab; // �˵��� �׸��� ���� ������
    [SerializeField] private int numDots; // �˵��� �׸� ���� ����
    [SerializeField] private float dotSpacing; // �˵��� �׸� �� ����

    private GameObject trajectoryLine; // �˵��� �׸��� ���� GameObject
    private LineRenderer lineRenderer; // �˵��� �׸��� ���� LineRenderer
    private Vector3 direction;

    private float angle = 0f; // ���� ����
    private float throwForce; // ���� ��

    private void OnEnable()
    {
        TrajectoryLineInit();
    }

    void Update()
    {
        SetDotsPosition();
    }

    public void SetThrowForce(float _throwForce)
    {
        throwForce = _throwForce;
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
        // �˵��� �׸��� ���� ��ġ ���
        for (int i = 0; i < numDots; i++)
        {
            Vector3 position = CalculatePosition(i * dotSpacing);
            lineRenderer.SetPosition(i, position + Vector3.forward * 0.5f + Vector3.right * 0.5f);
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
