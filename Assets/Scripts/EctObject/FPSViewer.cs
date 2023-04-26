using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSViewer : MonoBehaviour
{
    [Range(10, 150)]
    public int fontSize = 30;
    public Color color = new Color(.0f, .0f, .0f, 1.0f);
    public float width, height;

    private void Start()
    {
        Application.targetFrameRate = 300;
    }

    private void OnGUI()
    {
        Rect position = new Rect(width, height, Screen.width, Screen.height);

        float fps = 1.0f / Time.deltaTime;
        string text = "FPS : " + fps.ToString("F0");

        GUIStyle style = new GUIStyle();

        style.fontSize = fontSize;
        style.normal.textColor = color;

        GUI.Label(position, text, style);
    }
}
