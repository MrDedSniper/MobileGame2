using UnityEditor;
using UnityEditor.UI;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Tool.Tween.Editor
{
    [CustomEditor(typeof(CustomButtonEditor))]
    internal class CustomButtonEditor : ButtonEditor
    {
        private SerializedProperty m_InteractableProperty;

        protected override void OnEnable()
        {
            m_InteractableProperty = serializedObject.FindProperty("m_Interactable");
        }

        // Новый способ редактирования представления инскпектора
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            var animationType = new PropertyField(serializedObject.FindProperty(CustomButton_Obsolete.AnimationTypeName));
            var curveEase = new PropertyField(serializedObject.FindProperty(CustomButton_Obsolete.CurveEaseName));
            var duration = new PropertyField(serializedObject.FindProperty(CustomButton_Obsolete.DurationName));
            var independentUpdate =
                new PropertyField(serializedObject.FindProperty(CustomButton_Obsolete.IndependentUpdateName));

            var tweenLabel = new Label("Settings Tween");
            var intractableLabel = new Label("Interactable");

            root.Add(tweenLabel);
            root.Add(animationType);
            root.Add(curveEase);
            root.Add(duration);
            root.Add(independentUpdate);

            root.Add(intractableLabel);
            root.Add(new IMGUIContainer(OnInspectorGUI));

            return root;
        }

        // Старый способ представления инскпектора
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_InteractableProperty);

            EditorGUI.BeginChangeCheck();
            EditorGUI.EndChangeCheck();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
