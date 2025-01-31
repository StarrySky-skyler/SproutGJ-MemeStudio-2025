// ********************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/30 22:01
// @version: 1.0
// @description:
// ********************************************************************************

using System.Collections.Generic;
using UnityEngine;

namespace Tsuki.MVC.Models.Dialogue
{
    [CreateAssetMenu(fileName = "SingleDialogueConfig", menuName = "Tsuki/New Single Character Dialogue Config", order = 3)]
    public class SingleDialogueModel : ScriptableObject
    {
        [Header("对话角色")]
        [SerializeField]
        public Speaker speaker;
        
        [Header("对话内容")]
        [TextArea]
        [SerializeField]
        public List<string> dialogueList;
    }
}
