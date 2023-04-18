using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("# CameraShaking")]
    [SerializeField] private float force = 0f;
    [SerializeField] private Vector3 offset = Vector3.zero;
    
    [Header("# DeadCamera")]
    [SerializeField] private Transform deadCameraPos1 = null;
    [SerializeField] private Transform deadCameraPos2 = null;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float waitTime = 0.3f;
    [SerializeField] private float rotSpeed = 0.3f;



    public static CameraController instance;

    private Quaternion originRot;
    private float timer;
    private bool isMoving = false;

    private void Start()
    {
        originRot = transform.rotation;
        instance = this;
    }

    public void StartShakeCamera()
    {
        StopCoroutine("ShakeCameraCoroutine");
        if (isMoving) return;
        StartCoroutine("ShakeCameraCoroutine");
    }

    public void StopShakeCamera()
    {
        StopCoroutine("ShakeCameraCoroutine");
        StartCoroutine(StopShakeCameraCoroutine());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            DeadCameraMove();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            StopAllCoroutines();
            transform.localPosition = new Vector3(0f, 0.05f, 0.1f);
            transform.rotation = originRot;
        }
    }

        public void DeadCameraMove()
    {
        StartCoroutine(DeadCameraMoveCoroutine());
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

    private IEnumerator DeadCameraMoveCoroutine()
    {
        // Move to target A
        isMoving = true;
        GetComponent<Camera>().cullingMask = ~(1 << LayerMask.NameToLayer("Player"));
        while (Vector3.Distance(transform.position, deadCameraPos1.position) > 0.3f)
        {
            transform.position = Vector3.Lerp(transform.position, deadCameraPos1.position, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, deadCameraPos1.rotation, rotSpeed * Time.deltaTime);
            yield return null;
        }

        // Move to target B
        while (Vector3.Distance(transform.position, deadCameraPos2.position) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, deadCameraPos2.position, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, deadCameraPos2.rotation, rotSpeed * Time.deltaTime);
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
