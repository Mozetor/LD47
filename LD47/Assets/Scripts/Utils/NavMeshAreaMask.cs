using UnityEditor;
using UnityEngine;

namespace Utils {

    [CustomPropertyDrawer(typeof(NavMeshMaskAttribute))]
    public class NavMeshMaskDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label) {
            EditorGUI.BeginChangeCheck();

            int mask = serializedProperty.intValue;

            mask = EditorGUI.MaskField(position, label, mask, GetCompleteAreaNames());
            if (EditorGUI.EndChangeCheck()) {
                serializedProperty.intValue = mask;
            }
        }

        private string[] GetCompleteAreaNames() {
            string[] areaNames = GameObjectUtility.GetNavMeshAreaNames();
            string[] completeAreaNames = new string[32];

            foreach (string name in areaNames) {
                completeAreaNames[GameObjectUtility.GetNavMeshAreaFromName(name)] = name;
            }
            return completeAreaNames;
        }
    }

    public class NavMeshMaskAttribute : PropertyAttribute { }

}