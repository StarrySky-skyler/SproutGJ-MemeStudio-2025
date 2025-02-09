using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider; // ����������
    private AudioSource _audioSource; // ��ƵԴ�����ڿ�������

    private void Start()
    {
        if (gameObject.name.Contains("音乐"))
            _audioSource = GameObject.FindWithTag("Audio")
                .GetComponents<AudioSource>()[0];
        else
        {
            _audioSource = GameObject.FindWithTag("Audio")
                .GetComponents<AudioSource>()[1];
        }

        // ��ʼ��ʱ���û�������ֵΪ��ǰ��ƵԴ������
        //volumeSlider.value = audioSource.volume;
        switch (_audioSource.outputAudioMixerGroup.name)
        {
            case "BGM":
                _audioSource.outputAudioMixerGroup.audioMixer.GetFloat(
                    "BGMVolume", out float bgmVolume);
                volumeSlider.value = Mathf.Pow(10, bgmVolume / 20);
                break;
            case "SoundEffect":
                _audioSource.outputAudioMixerGroup.audioMixer.GetFloat(
                    "SFXVolume", out float soundEffectVolume);
                volumeSlider.value = Mathf.Pow(10, soundEffectVolume / 20);
                break;
            default:
                Debug.LogWarning("Unknown audio mixer group: " +
                                 _audioSource.outputAudioMixerGroup.name);
                break;
        }

        // ����������Ӽ��������ı�����
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    // ��������ֵ�ı�ʱ����
    private void OnVolumeChanged(float value)
    {
        switch (_audioSource.outputAudioMixerGroup.name)
        {
            case "BGM":
                _audioSource.outputAudioMixerGroup.audioMixer.SetFloat(
                    "BGMVolume", Mathf.Clamp(Mathf.Log10(value) * 20, -80, 0));
                break;
            case "SoundEffect":
                _audioSource.outputAudioMixerGroup.audioMixer.SetFloat(
                    "SFXVolume", Mathf.Clamp(Mathf.Log10(value) * 20, -80, 0));
                break;
            default:
                Debug.LogWarning("Unknown audio mixer group: " +
                                 _audioSource.outputAudioMixerGroup.name);
                break;
        }
        //audioSource.volume = value;  // ������ƵԴ������
    }
}
