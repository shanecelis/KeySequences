using UnityEditor;
using UnityEngine;

namespace SeawispHunter.KeySequences {

// IngredientDrawer
[CustomPropertyDrawer(typeof(KeySequencer))]
public class KeySequencerDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        // position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        // var indent = EditorGUI.indentLevel;
        // EditorGUI.indentLevel = 0;

        // Calculate rects
        // var amountRect = new Rect(position.x, position.y, 30, position.height);
        // var keySequencesRect = new Rect(position.x, position.y, position.width, position.height);
        // var unitRect = new Rect(position.x + 35, position.y, 50, position.height);
        // var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);

        // // Draw fields - pass GUIContent.none to each so they are drawn without labels
        // EditorGUI.PropertyField(keySequencesRect, property.FindPropertyRelative("keySequences"), GUIContent.none, true);
        // EditorGUI.PropertyField(keySequencesRect, property.FindPropertyRelative("keySequences"), label, true);
        EditorGUI.PropertyField(position, property.FindPropertyRelative("keySequences"), label, true);
        // EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"), GUIContent.none);
        // EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("unit"), GUIContent.none);
        // EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);

        // Set indent back to what it was
        // EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
      return //EditorGUI.GetPropertyHeight(property, label, true);
          EditorGUI.GetPropertyHeight(property.FindPropertyRelative("keySequences"), label, true);
      // return label != GUIContent.none && Screen.width < 333 ? (16f + 18f) : 16f;
    }
}
}
