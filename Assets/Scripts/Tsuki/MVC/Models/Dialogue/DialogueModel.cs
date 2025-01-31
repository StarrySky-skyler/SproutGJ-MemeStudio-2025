// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/30 22:01
// @version: 1.0
// @description:
// *****************************************************************************

using System.Collections.Generic;
using UnityEngine;

namespace Tsuki.MVC.Models.Dialogue
{
    [CreateAssetMenu(fileName = "DialogueListConfig",
        menuName = "Tsuki/New Total Dialogue Config", order = 4)]
    public class DialogueModel : ScriptableObject
    {
        [Header("总对话序列")] [SerializeField]
        public List<SingleDialogueModel> dialogueList;
    }
}
