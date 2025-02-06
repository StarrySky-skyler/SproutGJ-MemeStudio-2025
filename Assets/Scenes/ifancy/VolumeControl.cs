using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider; // ����������
    public AudioSource audioSource; // ��ƵԴ�����ڿ�������

    void Start()
    {
        // ��ʼ��ʱ���û�������ֵΪ��ǰ��ƵԴ������
        //volumeSlider.value = audioSource.volume;
        switch (audioSource.outputAudioMixerGroup.name)
        {
            case "BGM":
                audioSource.outputAudioMixerGroup.audioMixer.GetFloat(
                    "BGMVolume", out float bgmVolume);
                volumeSlider.value = Mathf.Pow(10, bgmVolume / 20);
                break;
            case "SoundEffect":
                audioSource.outputAudioMixerGroup.audioMixer.GetFloat(
                    "SFXVolume", out float soundEffectVolume);
                volumeSlider.value = Mathf.Pow(10, soundEffectVolume / 20);
                break;
            default:
                Debug.LogWarning("Unknown audio mixer group: " +
                                 audioSource.outputAudioMixerGroup.name);
                break;
        }

        // ����������Ӽ��������ı�����
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    // ��������ֵ�ı�ʱ����
    void OnVolumeChanged(float value)
    {
        switch (audioSource.outputAudioMixerGroup.name)
        {
            case "BGM":
                audioSource.outputAudioMixerGroup.audioMixer.SetFloat(
                    "BGMVolume", Mathf.Clamp(Mathf.Log10(value) * 20, -80, 0));
                break;
            case "SoundEffect":
                audioSource.outputAudioMixerGroup.audioMixer.SetFloat(
                    "SFXVolume", Mathf.Clamp(Mathf.Log10(value) * 20, -80, 0));
                break;
            default:
                Debug.LogWarning("Unknown audio mixer group: " +
                                 audioSource.outputAudioMixerGroup.name);
                break;
        }
        //audioSource.volume = value;  // ������ƵԴ������
    }
}
