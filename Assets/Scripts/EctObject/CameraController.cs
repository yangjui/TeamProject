using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float force = 0f;
    [SerializeField] private Vector3 offset = Vector3.zero;
    
    public static CameraController instance;

    private Quaternion originRot;
    private float timer;

    private void Start()
    {
        originRot = transform.rotation;
        instance = this;
    }

    public void StartShakeCamera()
    {
        StopCoroutine("ShakeCameraCoroutine");
        StartCoroutine("ShakeCameraCoroutine");
    }

    public void StopShakeCamera()
    {
        StopCoroutine("ShakeCameraCoroutine");
        StartCoroutine(StopShakeCameraCoroutine());
    }

    private IEnumerator ShakeCameraCoroutine()
    {
        Vector3 originEuler = transform.localEulerAngles;
        timer = 1f;

        while (true)
        {
            float rotX = Random.Range(-offset.x, offset.x);
            float rotY = Random.Range(-offset.y, offset.y);
            float rotZ = Random.Range(-offset.z, offset.z);

            Vector3 randomRot = originEuler + new Vector3(rotX, rotY, rotZ);
            Quaternion rot = Quaternion.Euler(randomRot);
            while (Quaternion.Angle(transform.localRotation, rot) > 0.1f)
            {
                // A로테이션 -> B로테이션 
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, rot, force * Time.deltaTime);
                yield return null;
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    StopShakeCamera();
                    break;
                }
            }
            yield return null;
        }
    }

    private IEnumerator StopShakeCameraCoroutine()
    {
        while (Quaternion.Angle(transform.localRotation, originRot) > 0f)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, originRot, force * Time.deltaTime);
            yield return null;
        }
    }
}
