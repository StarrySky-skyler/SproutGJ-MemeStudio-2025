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
    [Header("�Ի�����")] public string name;

    [Header("�Ի�����")] [TextArea] public string chatContent; // ��������

    [Header("��ʾ����")] public GameObject character;

    public List<SelectBtn> selectsbtn; // ѡ��ť�б�
    public bool isStop; // �Ƿ�ֹͣ���죨����ͣ�������

    [Header("��ʼ�Ի�ʱ")] public UnityEvent chatStartEvent; // ���쿪ʼ�¼�

    [Header("�ı���ʾ����ʱ")] public UnityEvent chatEndEvent; // ��������¼�
}

[Serializable]
public class SelectBtn
{
    public string btnContent; // ��ť����
    [HideInInspector] public Button selectsbtn; // ��ť�������Inspector�����أ�
    public UnityEvent btnEvent; // ��ť�¼�
}

public class ChatManager : MonoBehaviour
{
    [Header("��ʾ�ַ����ʱ��")]
    public float chatContentTime = 0.1f; // ÿ����ʾһ���ַ���ʱ��������λ���룩

    [Header("�ı�����һ�����Ⱥ��������ʾ")] [Range(0.1f, 1.0f)]
    public float ChatContentRot = 0.5f; // ÿ����ʾ�ַ�����ת�ٶȣ������ַ���ʾ��ƽ����

    [Header("��������")] public GameObject[] AllCharacters;

    public List<ChatInfo> chatInfos = new(); // ������Ϣ�б��洢�����������ݺͰ�ť����Ϣ

    public GameObject chatBackGround, chatRot; // ���챳����������תЧ�������������Ӿ�Ч����

    public TMP_Text chatContent; // ��ʾ�������ݵ��ı���

    public TMP_Text chatName; // ��ʾ�������������

    public Button chatButton; // ���쿪ʼ��ť�����ڴ��������¼���

    public Transform selectBtnLayout; // ѡ��ť�Ĳ���λ��

    public Transform selectBtnPool; // ѡ��ť�أ����ڹ���ť����

    [ReadOnly] public int currentIndex; // ��ǰ��ʾ��������Ϣ������

    [ReadOnly] public bool isChat; // ��ǰ�Ƿ��ڽ������죨��־λ��

    [ReadOnly] public bool isSelected; // ��ǰ�Ƿ�ѡ���˰�ť����־λ��

    [ReadOnly] public List<Button> currentSelectBtn = new(); // ��ǰ���õ�ѡ��ť�б�

    public void Update()
    {
        if (Input.GetMouseButtonDown(0)) // �ж��Ƿ�����������
        {
            if (isSelected) return; // �����ǰѡ���˰�ť���򲻽�����������

            if (currentIndex >= chatInfos.Count) // �����ǰ������������ڵ��������������ݵ�����
            {
                gameObject.SetActive(false); // �رյ�ǰ�������
                return;
            }

            // �����ǰ������������ʾ�������ڽ�������
            if (chatContent.text.Length >=
                chatInfos[currentIndex].chatContent.Length * ChatContentRot &&
                isChat)
            {
                StopAllCoroutines(); // ֹͣ����Э��
                chatContent.text =
                    chatInfos[currentIndex].chatContent; // ֱ����ʾ��������������

                StartCoroutine(CreateChatBtns(currentIndex)); // ��ʼ��������ѡ�ť
                chatInfos[currentIndex].chatEndEvent?.Invoke(); // ִ����������¼�
                isChat = false; // ���Ϊ�����ѽ���
                currentIndex++; // ����������������ʾ��һ����������
                return;
            }

            // �����ǰδ��������
            if (isChat) return;

            // �����ǰ���������Ѿ���ʾ��ɣ��Ҹ���������Ϊֹͣ
            if (chatContent.text == chatInfos[currentIndex - 1].chatContent &&
                chatInfos[currentIndex - 1].isStop)
            {
                gameObject.SetActive(false); // �ر��������
                if (ischatEnd() == false) // �ж��Ƿ�Ϊ�������״̬
                {
                    chatButton.gameObject.SetActive(true); // ��ʾ���쿪ʼ��ť
                    EventSystem.current.SetSelectedGameObject(chatButton
                        .gameObject); // ����ѡ�еİ�ťΪ���쿪ʼ��ť
                }

                chatInfos[currentIndex - 1].chatEndEvent
                    ?.Invoke(); // �������������¼����򴥷�
                return;
            }

            StartCoroutine(LoadChatContent()); // ���������������ݵ�Э��
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
        if (chatInfos[index].selectsbtn.Count > 0) // �����ǰ��������ѡ��ť
        {
            isSelected = true; // �������ѡ��ť
            for (int i = 0;
                 i < chatInfos[index].selectsbtn.Count;
                 i++) // ��������ѡ��ť
            {
                int n = i;
                Button selectBtn =
                    selectBtnPool.GetChild(0)
                        .GetComponent<Button>(); // �Ӱ�ť�ػ�ȡһ����ť
                currentSelectBtn.Add(selectBtn); // ����ť��ӵ���ǰ��ť�б�
                selectBtn.transform.SetParent(selectBtnLayout); // ���ð�ť�ĸ�����
                selectBtn.transform.GetChild(0).GetComponent<TMP_Text>().text =
                    chatInfos[index].selectsbtn[i].btnContent; // ���ð�ť�ı�
                selectBtn.onClick.RemoveAllListeners(); // �Ƴ���ť�ϵ������¼�����
                selectBtn.onClick.AddListener(() =>
                {
                    chatInfos[index].selectsbtn[n].btnEvent
                        ?.Invoke(); // �����ťʱ������Ӧ���¼�
                    for (int j = 0;
                         j < currentSelectBtn.Count;
                         j++) // �Ƴ��������ɵİ�ť
                        currentSelectBtn[j].transform.SetParent(selectBtnPool);
                    currentSelectBtn.Clear(); // ��յ�ǰ��ť�б�
                    if (chatInfos[currentIndex - 1].isStop) // ����������
                    {
                        gameObject.SetActive(false); // �ر��������
                        if (ischatEnd() == false) // �ж��Ƿ�Ϊ�������״̬
                        {
                            chatButton.gameObject.SetActive(true); // ��ʾ���쿪ʼ��ť
                            EventSystem.current.SetSelectedGameObject(chatButton
                                .gameObject); // ����ѡ�еİ�ť
                        }
                    }
                    else
                    {
                        StartCoroutine(LoadChatContent()); // ����������һ���������ݵ�Э��
                    }

                    isSelected = false; // ����ѡ��״̬
                });
                yield return null; // ÿ������һ����ť���ȴ���һ֡
            }
        }
    }


    public IEnumerator LoadChatContent()
    {
        chatRot.SetActive(false); // ������תЧ��

        isChat = true; // ���Ϊ��������

        chatInfos[currentIndex].chatStartEvent?.Invoke(); // ִ�����쿪ʼ�¼�

        chatContent.text = string.Empty; // ��յ�ǰ���������ı���

        chatName.text = chatInfos[currentIndex].name; //�Ի����Ʊ��

        foreach (GameObject o in AllCharacters) o.SetActive(false); //�ر���������

        if (chatInfos[currentIndex].character != null)
            chatInfos[currentIndex].character.SetActive(true);


        for (int i = 0;
             i < chatInfos[currentIndex].chatContent.Length;
             i++) // �������������е�ÿ���ַ�
        {
            chatContent.text +=
                chatInfos[currentIndex].chatContent[i]; // ��ʾһ���ַ�
            if (chatContent.text.Length >=
                chatInfos[currentIndex].chatContent.Length * ChatContentRot)
                chatRot.SetActive(true); // ���ﵽһ���ַ���ʱ����ʾ��תЧ��
            yield return new WaitForSeconds(chatContentTime); // ����ÿ���ַ���ʾ��ʱ��
        }

        StartCoroutine(CreateChatBtns(currentIndex)); // �������������ʾ������ѡ��ť
        chatInfos[currentIndex].chatEndEvent?.Invoke(); // ִ����������¼�
        currentIndex++; // ��������������׼����ʾ��һ����������
        isChat = false; // ��������ѽ���
    }
}
