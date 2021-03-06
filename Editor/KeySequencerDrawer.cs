/* Original code[1] Copyright (c) 2022 Shane Celis[2]
   Licensed under the MIT License[3]

   This comment generated by code-cite[4].

   [1]: https://github.com/shanecelis/KeySequences.git
   [2]: https://twitter.com/shanecelis
   [3]: https://opensource.org/licenses/MIT
   [4]: https://github.com/shanecelis/code-cite
*/
using UnityEditor;
using UnityEngine;
// https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
namespace SeawispHunter.KeySequences {

/** Only draw the `keySequences` element from the KeySequencer object. */
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
