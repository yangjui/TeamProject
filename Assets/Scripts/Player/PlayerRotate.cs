using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerRotate : MonoBehaviour
{
    private float rotateXSpeed;
    private float rotateYSpeed;
    private float aimModeRotateXSpeed;
    private float aimModeRotateYSpeed;
    private float finalRotateXSpeed;
    private float finalRotateYSpeed;

    private float limitMinX = -80f;
    private float limitMaxX = 50f;
    private float eulerAngleX;
    private float eulerAngleY;
    private bool isAimMode;

    private void Update()
    {
        rotateXSpeed = PlayerPrefs.GetFloat("mouseX") * 20f;
        rotateYSpeed = PlayerPrefs.GetFloat("mouseY") * 20f;
        aimModeRotateXSpeed = PlayerPrefs.GetFloat("aimModeMouseX") * 20f;
        aimModeRotateYSpeed = PlayerPrefs.GetFloat("aimModeMouseY") * 20f;
    }

    public void Rotate(float _mouseX, float _mouseY)
    {
        finalRotateXSpeed = isAimMode ? aimModeRotateXSpeed : rotateXSpeed;
        finalRotateYSpeed = isAimMode ? aimModeRotateYSpeed : rotateYSpeed;

        eulerAngleX -= _mouseY * finalRotateXSpeed;
        eulerAngleY += _mouseX * finalRotateYSpeed;

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

    public void ChangeAimMode(bool _bool)
    {
        isAimMode = _bool;
    }
}
