using System.Collections.Generic;
using UnityEngine;
public class MsgHandler
{
    public delegate void DelMsgHandler(Msg msg);

    public static Dictionary<string, DelMsgHandler> delDic = new Dictionary<string, DelMsgHandler>();

    /// <summary>
    /// 添加监听
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="msgHandler"></param>
    public static void AddListener(string msg, DelMsgHandler msgHandler)
    {
        if (delDic == null) delDic = new Dictionary<string, DelMsgHandler>();

        if (!delDic.ContainsKey(msg)) delDic.Add(msg, null);

        delDic[msg] += msgHandler;
    }

    /// <summary>
    /// 移除监听
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="msgHandler"></param>
    public static void RemoveListener(string msg, DelMsgHandler msgHandler)
    {
        if (delDic != null && delDic.ContainsKey(msg)) delDic[msg] -= msgHandler;
    }

    public static void RemoveAllListener()
    {
        if (delDic != null) delDic.Clear();
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="msg"></param>
    public static void SendMsg(string type, Msg msg)
    {
        DelMsgHandler delMsgHandler;
        if (delDic != null && delDic.TryGetValue(type, out delMsgHandler))
        {
            if (msg != null) delMsgHandler(msg);
        }
    }

    /// <summary>
    /// 广播消息
    /// </summary>
    /// <param name="msg"></param>
    public static void SendAllMsg(Msg msg)
    {
        foreach (var item in delDic)
        {
            item.Value(msg);
        }
    }
}

public class Msg
{

    public string Key;
    public float Value;
    public Animator Animator;
    public Vector3 Pos;
    public Msg(string key,float value,Animator ani,Vector3 pos)
    {
        Key = key;
        Value = value;
        Animator = ani;
        Pos = pos;
    }
}