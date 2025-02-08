using UnityEngine;
using UnityEngine.UI;

public class PopupWindow : MonoBehaviour
{
    public GameObject window; // ��Ҫ������ʾ�����صĴ���
    public Button closeButton; // ���ڹرմ��ڵķ��ذ�ť
    public Button openButton; // ���ڴ򿪴��ڵİ�ť

    private void Start()
    {
        // ��ʼʱ���ô���Ϊ����
        window.SetActive(false);

        // �����ذ�ť���ӵ���¼�����
        closeButton.onClick.AddListener(CloseWindow);

        // ���򿪰�ť���ӵ���¼�����
        openButton.onClick.AddListener(ShowWindow);
    }

    private void Update()
    {
        // ���� ESC ��ʱ�رմ���
        if (Input.GetKeyDown(KeyCode.Escape)) CloseWindow();

        //// ���������������ⲿ���򣬹رմ���
        //if (Input.GetMouseButtonDown(0) && !RectTransformUtility.RectangleContainsScreenPoint(window.GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
        //{
        //    CloseWindow();
        //}
    }

    // �رմ��ڵĺ���
    private void CloseWindow()
    {
        window.SetActive(false); // ���ش���
    }

    // ��ʾ���ڵĺ��������ʱ���ã�
    public void ShowWindow()
    {
        window.SetActive(true); // ��ʾ����
    }
}
