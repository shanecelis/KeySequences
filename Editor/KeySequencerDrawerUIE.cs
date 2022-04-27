// https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace SeawispHunter.KeySequences {

  // [CustomPropertyDrawer(typeof(KeySequencer))]
  // public class KeySequencerDrawerUIE : PropertyDrawer {
  //   public override VisualElement CreatePropertyGUI(SerializedProperty property) {
  //     // Create property container element.
  //     var container = new VisualElement();

  //     // Create property fields.
  //     var keySequences = new PropertyField(property.FindPropertyRelative("keySequences"));
  //     // var unitField = new PropertyField(property.FindPropertyRelative("unit"));
  //     // var nameField = new PropertyField(property.FindPropertyRelative("name"), "Fancy Name");

  //     // Add fields to the container.
  //     container.Add(keySequences);
  //     // container.Add(unitField);
  //     // container.Add(nameField);

  //     return container;
  //   }

  // }

}
