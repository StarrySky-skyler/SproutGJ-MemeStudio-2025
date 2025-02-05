using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;   // 音量滑动条
    public AudioSource audioSource; // 音频源，用于控制音量

    void Start()
    {
        // 初始化时设置滑动条的值为当前音频源的音量
        volumeSlider.value = audioSource.volume;

        // 给滑动条添加监听器，改变音量
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    // 当滑动条值改变时调用
    void OnVolumeChanged(float value)
    {
        audioSource.volume = value;  // 设置音频源的音量
    }
}
