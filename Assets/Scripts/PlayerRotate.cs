using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField] private float rotateXSpeed;
    [SerializeField] private float rotateYSpeed;

    private float limitMinX = -80f;
    private float limitMaxX = 50f;
    private float eulerAngleX;
    private float eulerAngleY;

    public void Rotate(float _mouseX, float _mouseY)
    {
        eulerAngleX -= _mouseY * rotateXSpeed;
        eulerAngleY += _mouseX * rotateYSpeed;

        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }

    private float ClampAngle(float _angle, float _min, float _max)
    {
        if (_angle < -360f) _angle += 360f;
        if (_angle > 360f) _angle -= 360f;

        return Mathf.Clamp(_angle, _min, _max);
        // Clamp = 최소/ 최대값을 설정하여 float값이 범위 이외의 값을 넘지 않도록 한다.
    }
}
