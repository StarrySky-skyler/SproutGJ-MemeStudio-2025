using UnityEditor;

namespace Febucci.UI.Core.Editors
{
    [CustomEditor(typeof(TypewriterByWord), true)]
    internal class TypewriterByWordDrawer : TypewriterCoreDrawer
    {
        private PropertyWithDifferentLabel disappearanceDelay;
        private SerializedProperty waitForNormalWord;
        private SerializedProperty waitForWordWithPunctuation;

        protected override void OnEnable()
        {
            base.OnEnable();

            waitForNormalWord =
                serializedObject.FindProperty("waitForNormalWord");
            waitForWordWithPunctuation =
                serializedObject.FindProperty("waitForWordWithPunctuation");
            disappearanceDelay = new PropertyWithDifferentLabel(
                serializedObject, "disappearanceDelay", "Disappearances Wait");
        }

        protected override string[] GetPropertiesToExclude()
        {
            string[] newProperties =
            {
                "script",
                "waitForNormalWord",
                "waitForWordWithPunctuation",
                "disappearanceDelay"
            };

            string[] baseProperties = base.GetPropertiesToExclude();

            string[] mergedArray =
                new string[newProperties.Length + baseProperties.Length];

            for (int i = 0; i < baseProperties.Length; i++)
                mergedArray[i] = baseProperties[i];

            for (int i = 0; i < newProperties.Length; i++)
                mergedArray[i + baseProperties.Length] = newProperties[i];

            return mergedArray;
        }

        protected override void OnTypewriterSectionGUI()
        {
            EditorGUILayout.PropertyField(waitForNormalWord);
            EditorGUILayout.PropertyField(waitForWordWithPunctuation);
        }

        protected override void OnDisappearanceSectionGUI()
        {
            disappearanceDelay.PropertyField();
        }
    }
}
