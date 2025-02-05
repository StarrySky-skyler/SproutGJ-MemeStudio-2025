using UnityEngine;
using UnityEngine.UI;

public class PopupWindow : MonoBehaviour
{
    public GameObject window;  // 需要控制显示和隐藏的窗口
    public Button closeButton; // 用于关闭窗口的返回按钮
    public Button openButton;  // 用于打开窗口的按钮

    void Start()
    {
        // 初始时设置窗口为隐藏
        window.SetActive(false);

        // 给返回按钮添加点击事件监听
        closeButton.onClick.AddListener(CloseWindow);

        // 给打开按钮添加点击事件监听
        openButton.onClick.AddListener(ShowWindow);
    }

    void Update()
    {
        // 按下 ESC 键时关闭窗口
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseWindow();
        }

        // 如果鼠标点击到窗口外部区域，关闭窗口
        if (Input.GetMouseButtonDown(0) && !RectTransformUtility.RectangleContainsScreenPoint(window.GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
        {
            CloseWindow();
        }
    }

    // 关闭窗口的函数
    void CloseWindow()
    {
        window.SetActive(false);  // 隐藏窗口
    }

    // 显示窗口的函数（点击时调用）
    public void ShowWindow()
    {
        window.SetActive(true);  // 显示窗口
    }
}
