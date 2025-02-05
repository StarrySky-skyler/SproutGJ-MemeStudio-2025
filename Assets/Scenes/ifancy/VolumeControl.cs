using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;   // ����������
    public AudioSource audioSource; // ��ƵԴ�����ڿ�������

    void Start()
    {
        // ��ʼ��ʱ���û�������ֵΪ��ǰ��ƵԴ������
        volumeSlider.value = audioSource.volume;

        // ����������Ӽ��������ı�����
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    // ��������ֵ�ı�ʱ����
    void OnVolumeChanged(float value)
    {
        audioSource.volume = value;  // ������ƵԴ������
    }
}
