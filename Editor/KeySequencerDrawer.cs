using UnityEditor;
using UnityEngine;

namespace SeawispHunter.KeySequences {

  /** Only draw the `keySequences` element from the KeySequencer object. */
[CustomPropertyDrawer(typeof(KeySequencerMapInt))]
[CustomPropertyDrawer(typeof(KeySequencer))]
public class KeySequencerDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.PropertyField(position, property.FindPropertyRelative("keySequences"), label, true);
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
      return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("keySequences"), label, true);
    }
}
}
