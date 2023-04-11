using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float force = 0f;
    [SerializeField] private Vector3 offset = Vector3.zero;
    private Quaternion originRot;

    private void Start()
    {
        originRot = transform.rotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            StartCoroutine(ShakeCameraCoroutine());
        if (Input.GetKeyDown(KeyCode.P))
            StartCoroutine(StopShakeCameraCoroutine());
    }

    private IEnumerator ShakeCameraCoroutine()
    {
        Vector3 originEuler = transform.eulerAngles;

        while (true)
        {
            float rotX = Random.Range(-offset.x, offset.x);
            float rotY = Random.Range(-offset.y, offset.y);
            float rotZ = Random.Range(-offset.z, offset.z);

            Vector3 randomRot = originEuler + new Vector3(rotX, rotY, rotZ);
            Quaternion rot = Quaternion.Euler(randomRot);
            while (Quaternion.Angle(transform.rotation, rot) > 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, force * Time.deltaTime);
                yield return null;
            }
            yield return null;
        }
    }

    private IEnumerator StopShakeCameraCoroutine()
    {
        while (Quaternion.Angle(transform.rotation, originRot) > 0f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, originRot, force * Time.deltaTime);
            yield return null;
        }
    }



































}
