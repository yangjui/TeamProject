using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionSetting : MonoBehaviour
{
    [Header("# Sound")]
    [SerializeField] private Slider sfxSound;
    [SerializeField] private Slider bgmSound;

    [Header("# MouseSpeed")]
    [SerializeField] private Slider mouseXSlider;
    [SerializeField] private Slider mouseYSlider;
    [SerializeField] private TMP_InputField mouseXInputField;
    [SerializeField] private TMP_InputField mouseYInputField;

    [Header("# AimMouseSpeed")]
    [SerializeField] private Slider aimMouseXSlider;
    [SerializeField] private Slider aimMouseYSlider;
    [SerializeField] private TMP_InputField aimModeMouseXInputField;
    [SerializeField] private TMP_InputField aimModeMouseYInputField;

    public void Init()
    {
        //if (!PlayerPrefs.HasKey("sfx"))
        //{
        //    SFXSound(1);
        //}
        //if (!PlayerPrefs.HasKey("bgm"))
        //{
        //    BGMSound(1);
        //}
        sfxSound.value = PlayerPrefs.GetFloat("sfx");
        bgmSound.value = PlayerPrefs.GetFloat("bgm");

        mouseXSlider.value = PlayerPrefs.GetFloat("mouseX");
        mouseYSlider.value = PlayerPrefs.GetFloat("mouseY");
        mouseXInputField.text = (PlayerPrefs.GetFloat("mouseX") * 100f).ToString("F1");
        mouseYInputField.text = (PlayerPrefs.GetFloat("mouseY") * 100f).ToString("F1");

        aimMouseXSlider.value = PlayerPrefs.GetFloat("aimModeMouseX");
        aimMouseYSlider.value = PlayerPrefs.GetFloat("aimModeMouseY");
        aimModeMouseXInputField.text = (PlayerPrefs.GetFloat("aimModeMouseX") * 100f).ToString("F1");
        aimModeMouseYInputField.text = (PlayerPrefs.GetFloat("aimModeMouseY") * 100f).ToString("F1");
    }

    public void SFXSound(float _val)
    {
        PlayerPrefs.SetFloat("sfx", _val);
        SoundManager.instance.SFXVolume(_val);
    }

    public void BGMSound(float _val)
    {
        PlayerPrefs.SetFloat("bgm", _val);
        SoundManager.instance.BGMVolume(_val);
    }

    public void MouseX(float _val)
    {
        mouseXInputField.text = (_val * 100).ToString("F1");
        PlayerPrefs.SetFloat("mouseX", _val);
    }

    public void MouseY(float _val)
    {
        mouseYInputField.text = (_val * 100).ToString("F1");
        PlayerPrefs.SetFloat("mouseY", _val);
    }

    public void AimModeMouseX(float _val)
    {
        aimModeMouseXInputField.text = (_val * 100).ToString("F1");
        PlayerPrefs.SetFloat("aimModeMouseX", _val);
    }

    public void AimModeMouseY(float _val)
    {
        aimModeMouseYInputField.text = (_val * 100).ToString("F1");
        PlayerPrefs.SetFloat("aimModeMouseY", _val);
    }

    public void MouseXInputField()
    {
        float text = float.Parse(mouseXInputField.text) * 0.01f;
        if (text > 100) text = 100;
        else if (text < 0) text = 0;
        mouseXInputField.text = text.ToString();
        mouseXSlider.value = text;
        PlayerPrefs.SetFloat("MouseX", mouseXSlider.value);
    }

    public void MouseYInputField()
    {
        float text = float.Parse(mouseYInputField.text) * 0.01f;
        if (text > 100) text = 100;
        else if (text < 0) text = 0;
        mouseYInputField.text = text.ToString();
        mouseYSlider.value = text;
        PlayerPrefs.SetFloat("MouseY", mouseYSlider.value);
    }

    public void AimModeMouseXInputField()
    {
        float text = float.Parse(aimModeMouseXInputField.text) * 0.01f;
        if (text > 100) text = 100;
        else if (text < 0) text = 0;
        aimModeMouseXInputField.text = text.ToString();
        aimMouseXSlider.value = text;
        PlayerPrefs.SetFloat("aimModeMouseX", aimMouseXSlider.value);
    }

    public void AimModeMouseYInputField()
    {
        float text = float.Parse(aimModeMouseYInputField.text) * 0.01f;
        if (text > 100) text = 100;
        else if (text < 0) text = 0;
        aimModeMouseYInputField.text = text.ToString();
        aimMouseYSlider.value = text;
        PlayerPrefs.SetFloat("aimModeMouseY", aimMouseYSlider.value);
    }
}
