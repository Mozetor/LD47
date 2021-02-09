using UnityEditor;
using UnityEngine;

namespace Utils {

    [CustomPropertyDrawer(typeof(NavMeshSelectorAttribute))]
    public class NavMeshSelectorDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label) {
            EditorGUI.BeginChangeCheck();
            string[] navMeshAreaNames = GameObjectUtility.GetNavMeshAreaNames();
            int navMeshArea = serializedProperty.intValue;
            int selectedIndex = -1;
            for (int i = 0; i < navMeshAreaNames.Length; i++) {
                if (GameObjectUtility.GetNavMeshAreaFromName(navMeshAreaNames[i]) == navMeshArea) {
                    selectedIndex = i;
                    break;
                }
            }
            int num = EditorGUI.Popup(position, "Navigation Area", selectedIndex, navMeshAreaNames);
            if (EditorGUI.EndChangeCheck()) {
                int navMeshAreaFromName = GameObjectUtility.GetNavMeshAreaFromName(navMeshAreaNames[num]);
                serializedProperty.intValue = navMeshAreaFromName;
            }
        }
    }

    public class NavMeshSelectorAttribute : PropertyAttribute { }

}