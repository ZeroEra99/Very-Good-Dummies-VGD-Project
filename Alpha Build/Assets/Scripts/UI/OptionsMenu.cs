using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    CinemachineFreeLook cam;
    float _sensitivity;
    float xSpeed;
    float ySpeed;
    Slider volumeSlider;
    Slider cameraSlider;
    private void Start()
    {
        cam = FindObjectOfType<CinemachineFreeLook>();
        volumeSlider = transform.Find("Volume/VolumeSlider").GetComponent<Slider>();
        cameraSlider = transform.Find("CameraSensitivity/CameraSlider").GetComponent<Slider>();
        xSpeed = cam.m_XAxis.m_MaxSpeed;
        ySpeed = cam.m_YAxis.m_MaxSpeed;
        _sensitivity=PlayerPrefs.GetFloat("CameraSensitivity");
        if (_sensitivity != 0)
        {
            changeSensitivity(_sensitivity);
            cameraSlider.value = _sensitivity;
        }
        if (PlayerPrefs.GetFloat("Volume")!=0)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        }
        DisableOptionsMenuUI();
    }
    private void OnDestroy()
    {

    }

    public void changeSensitivity(float sensitivity)
    {
        cam.m_XAxis.m_MaxSpeed = xSpeed*sensitivity;
        cam.m_YAxis.m_MaxSpeed = ySpeed*sensitivity;
    }

    public void changeVolume(float volume)
    {
        GameManager.audioManager.setVolume(volume);
    }


    private void EnableOptionsMenuUI() => gameObject.SetActive(true);
    private void DisableOptionsMenuUI() => gameObject.SetActive(false);
}
