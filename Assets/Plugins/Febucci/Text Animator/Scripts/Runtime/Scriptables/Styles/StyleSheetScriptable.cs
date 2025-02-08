using System;
using System.Collections.Generic;
using UnityEngine;

namespace Febucci.UI.Styles
{
    /// <summary>
    /// Contains a list of styles that can be used in the text
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "TextAnimator StyleSheet",
        menuName = "Text Animator/StyleSheet", order = 100)]
    public class StyleSheetScriptable : ScriptableObject
    {
        [SerializeField] private Style[] styles = Array.Empty<Style>();

        private bool built;
        private Dictionary<string, Style> dictionary;

        public Style[] Styles
        {
            get => styles;
            set
            {
                styles = value;
                built = false;
            }
        }

        public void BuildOnce()
        {
            if (built) return;
            built = true;

            if (dictionary != null) dictionary.Clear();
            else dictionary = new Dictionary<string, Style>();

            if (styles == null) return;

            foreach (var style in styles)
            {
                var tag = style.styleTag.ToLowerInvariant();
                if (string.IsNullOrEmpty(tag)) continue;

                if (dictionary.ContainsKey(tag))
                {
                    Debug.LogError(
                        $"[TextAnimator] StyleSheetScriptable: duplicated style tag '{tag}",
                        this);
                    continue;
                }

                dictionary.Add(tag, style);
            }
        }

        public void ForceBuildRefresh()
        {
            built = false;
            BuildOnce();
        }

        public virtual bool TryGetStyle(string tag, out Style result)
        {
            BuildOnce();
            return dictionary.TryGetValue(tag, out result);
        }
    }
}
