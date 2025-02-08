using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tsuki.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class ChatInfo
{
    [Header("对话名称")] public string name;

    [Header("对话内容")] [TextArea] public string chatContent; // 聊天内容

    [Header("显示立绘")] public GameObject character;

    public List<SelectBtn> selectsbtn; // 选择按钮列表
    public bool isStop; // 是否停止聊天（如暂停或结束）

    [Header("开始对话时")] public UnityEvent chatStartEvent; // 聊天开始事件

    [Header("文本显示完整时")] public UnityEvent chatEndEvent; // 聊天结束事件
}

[Serializable]
public class SelectBtn
{
    public string btnContent; // 按钮内容
    [HideInInspector] public Button selectsbtn; // 按钮组件（在Inspector中隐藏）
    public UnityEvent btnEvent; // 按钮事件
}

public class ChatManager : MonoBehaviour
{
    [Header("显示字符间隔时间")]
    public float chatContentTime = 0.1f; // 每次显示一个字符的时间间隔（单位：秒）

    [Header("文本到达一定进度后可完整显示")] [Range(0.1f, 1.0f)]
    public float ChatContentRot = 0.5f; // 每次显示字符的旋转速度，控制字符显示的平滑度

    [Header("所有立绘")] public GameObject[] AllCharacters;

    public List<ChatInfo> chatInfos = new(); // 聊天信息列表，存储多个聊天的内容和按钮等信息

    public GameObject chatBackGround, chatRot; // 聊天背景和聊天旋转效果（可能用于视觉效果）

    public TMP_Text chatContent; // 显示聊天内容的文本框

    public TMP_Text chatName; // 显示聊天人物的名字

    public Button chatButton; // 聊天开始按钮（用于触发聊天事件）

    public Transform selectBtnLayout; // 选择按钮的布局位置

    public Transform selectBtnPool; // 选择按钮池，用于管理按钮对象

    [ReadOnly] public int currentIndex; // 当前显示的聊天信息的索引

    [ReadOnly] public bool isChat; // 当前是否在进行聊天（标志位）

    [ReadOnly] public bool isSelected; // 当前是否选择了按钮（标志位）

    [ReadOnly] public List<Button> currentSelectBtn = new(); // 当前可用的选择按钮列表

    public void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 判断是否按下了鼠标左键
        {
            if (isSelected) return; // 如果当前选择了按钮，则不进行其他操作

            if (currentIndex >= chatInfos.Count) // 如果当前聊天的索引大于等于所有聊天内容的数量
            {
                gameObject.SetActive(false); // 关闭当前聊天界面
                return;
            }

            // 如果当前聊天内容已显示完且正在进行聊天
            if (chatContent.text.Length >=
                chatInfos[currentIndex].chatContent.Length * ChatContentRot &&
                isChat)
            {
                StopAllCoroutines(); // 停止所有协程
                chatContent.text =
                    chatInfos[currentIndex].chatContent; // 直接显示完整的聊天内容

                StartCoroutine(CreateChatBtns(currentIndex)); // 开始生成聊天选项按钮
                chatInfos[currentIndex].chatEndEvent?.Invoke(); // 执行聊天结束事件
                isChat = false; // 标记为聊天已结束
                currentIndex++; // 增加聊天索引，显示下一个聊天内容
                return;
            }

            // 如果当前未进行聊天
            if (isChat) return;

            // 如果当前聊天内容已经显示完成，且该聊天项标记为停止
            if (chatContent.text == chatInfos[currentIndex - 1].chatContent &&
                chatInfos[currentIndex - 1].isStop)
            {
                gameObject.SetActive(false); // 关闭聊天界面
                if (ischatEnd() == false) // 判断是否为聊天结束状态
                {
                    chatButton.gameObject.SetActive(true); // 显示聊天开始按钮
                    EventSystem.current.SetSelectedGameObject(chatButton
                        .gameObject); // 设置选中的按钮为聊天开始按钮
                }

                chatInfos[currentIndex - 1].chatEndEvent
                    ?.Invoke(); // 如果有聊天结束事件，则触发
                return;
            }

            StartCoroutine(LoadChatContent()); // 启动加载聊天内容的协程
        }
    }

    private void OnEnable()
    {
        StartCoroutine(LoadChatContent());
        chatInfos[chatInfos.Count - 1].chatEndEvent.RemoveAllListeners();

        chatInfos[chatInfos.Count - 1].chatEndEvent.AddListener(LoadGame);
    }

    private void LoadGame()
    {
        GameManager.Instance.AllowLoadGame = true;
        transform.root.gameObject.SetActive(false);
    }

    public void ExitChat()
    {
        chatBackGround.SetActive(false);
        currentIndex--;
        for (int j = 0; j < currentSelectBtn.Count; j++)
            currentSelectBtn[j].transform.SetParent(selectBtnPool);
        currentSelectBtn.Clear();
    }

    public bool ischatEnd()
    {
        return currentIndex >= chatInfos.Count;
    }

    public void Minus()
    {
        currentIndex--;
    }

    public void Add()
    {
        currentIndex++;
    }


    public void SetCurrentIndex(int n)
    {
        currentIndex = n - 1;
    }

    public IEnumerator CreateChatBtns(int index)
    {
        if (chatInfos[index].selectsbtn.Count > 0) // 如果当前聊天项有选择按钮
        {
            isSelected = true; // 标记正在选择按钮
            for (int i = 0;
                 i < chatInfos[index].selectsbtn.Count;
                 i++) // 遍历所有选择按钮
            {
                int n = i;
                Button selectBtn =
                    selectBtnPool.GetChild(0)
                        .GetComponent<Button>(); // 从按钮池获取一个按钮
                currentSelectBtn.Add(selectBtn); // 将按钮添加到当前按钮列表
                selectBtn.transform.SetParent(selectBtnLayout); // 设置按钮的父物体
                selectBtn.transform.GetChild(0).GetComponent<TMP_Text>().text =
                    chatInfos[index].selectsbtn[i].btnContent; // 设置按钮文本
                selectBtn.onClick.RemoveAllListeners(); // 移除按钮上的所有事件监听
                selectBtn.onClick.AddListener(() =>
                {
                    chatInfos[index].selectsbtn[n].btnEvent
                        ?.Invoke(); // 点击按钮时触发对应的事件
                    for (int j = 0;
                         j < currentSelectBtn.Count;
                         j++) // 移除所有生成的按钮
                        currentSelectBtn[j].transform.SetParent(selectBtnPool);
                    currentSelectBtn.Clear(); // 清空当前按钮列表
                    if (chatInfos[currentIndex - 1].isStop) // 如果聊天结束
                    {
                        gameObject.SetActive(false); // 关闭聊天界面
                        if (ischatEnd() == false) // 判断是否为聊天结束状态
                        {
                            chatButton.gameObject.SetActive(true); // 显示聊天开始按钮
                            EventSystem.current.SetSelectedGameObject(chatButton
                                .gameObject); // 设置选中的按钮
                        }
                    }
                    else
                    {
                        StartCoroutine(LoadChatContent()); // 启动加载下一个聊天内容的协程
                    }

                    isSelected = false; // 重置选择状态
                });
                yield return null; // 每次生成一个按钮，等待下一帧
            }
        }
    }


    public IEnumerator LoadChatContent()
    {
        chatRot.SetActive(false); // 隐藏旋转效果

        isChat = true; // 标记为正在聊天

        chatInfos[currentIndex].chatStartEvent?.Invoke(); // 执行聊天开始事件

        chatContent.text = string.Empty; // 清空当前聊天内容文本框

        chatName.text = chatInfos[currentIndex].name; //对话名称变更

        foreach (GameObject o in AllCharacters) o.SetActive(false); //关闭所有立绘

        if (chatInfos[currentIndex].character != null)
            chatInfos[currentIndex].character.SetActive(true);


        for (int i = 0;
             i < chatInfos[currentIndex].chatContent.Length;
             i++) // 遍历聊天内容中的每个字符
        {
            chatContent.text +=
                chatInfos[currentIndex].chatContent[i]; // 显示一个字符
            if (chatContent.text.Length >=
                chatInfos[currentIndex].chatContent.Length * ChatContentRot)
                chatRot.SetActive(true); // 当达到一定字符数时，显示旋转效果
            yield return new WaitForSeconds(chatContentTime); // 控制每个字符显示的时间
        }

        StartCoroutine(CreateChatBtns(currentIndex)); // 完成聊天内容显示后，生成选择按钮
        chatInfos[currentIndex].chatEndEvent?.Invoke(); // 执行聊天结束事件
        currentIndex++; // 更新聊天索引，准备显示下一个聊天内容
        isChat = false; // 标记聊天已结束
    }
}
