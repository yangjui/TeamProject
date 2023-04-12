using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BlackHoleGrenade : MonoBehaviour
{
    [SerializeField] 
    private GameObject obstacle;

    private float destroyTime = 7f;

    private void Start()
    {
        StartCoroutine(BlackholeStart());
    }

    private IEnumerator BlackholeStart()
    {
        yield return null;
        GameObject newOBJ = Instantiate(obstacle, transform.position, Quaternion.Euler(Vector3.zero));

        Destroy(newOBJ, destroyTime);
        Destroy(this.gameObject);
    }

    public void GrenadeRigidbody(Vector3 _direction)
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(_direction, ForceMode.Impulse);
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
    }
}
