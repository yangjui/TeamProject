using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [SerializeField]
    private Camera maincam;
    [SerializeField]
    private Camera subcam;

    private Camera activeCam;

    [SerializeField]
    private float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        activeCam = maincam;
        activeCam.enabled = true;

        subcam.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Vector3 targetPosition = activeCam == maincam ? subcam.transform.position : maincam.transform.position;
            activeCam.enabled = !activeCam.enabled;
            subcam.enabled = !subcam.enabled;

            if (activeCam.enabled)
            {
                activeCam = maincam;
            }
            else
            {
                activeCam = subcam;
            }

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }

        if (Input.GetMouseButtonDown(0))
        {
            activeCam.enabled = true;
            subcam.enabled = false;
            activeCam = maincam;
        }
    }

    private void LateUpdate()
    {
        // 카메라의 로컬 위치를 플레이어 객체와 같게 설정
        transform.localPosition = Vector3.zero;
    }
}

