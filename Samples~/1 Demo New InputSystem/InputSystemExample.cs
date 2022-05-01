using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeawispHunter.KeySequences;
using UnityEngine.UI;
using System.ComponentModel;
using UnityEngine.InputSystem;

public class InputSystemExample : MonoBehaviour {
  public KeySequencer keySequences;
  [Header("Turn off logging (causes allocations)")]
  public bool enableLogging = true;
  public Text label;

  void OnEnable() {
    keySequences.Enable();
    /* Normally, we'd just send the input stream directly to the keySequences.
       However, in this case, we want to output something about it so we'll
       redirect to our own Input() method.
     */
    // Keyboard.current.onTextInput += keySequences.Input;
    Keyboard.current.onTextInput += Input;
    keySequences.propertyChanged += OnPropertyChange;
  }

  void OnDisable() {
    keySequences.Disable();
    // Keyboard.current.onTextInput -= keySequences.Input;
    Keyboard.current.onTextInput -= Input;
    keySequences.propertyChanged -= OnPropertyChange;
  }

  void Start() {
#if ! ENABLE_INPUT_SYSTEM
    Debug.LogError("New input system required.");
#else
    Debug.Log("Using new input system.");
    // Debug.Log($"There are {keySequences.Count} key sequences.");
    keySequences.accept += (_key) => {
      var msg = $"ACCEPT {_key}";
      Debug.Log(msg);
      label.text = msg;
    };
#endif
  }

  void Input(char c) {
    keySequences.Input(c);
    if (enableLogging) {
      if (! char.IsControl(c))
        Debug.Log($"Input char '{c}' int {(int) c}");
      else
        Debug.Log($"Input char non-printable int {(int) c}");
    }
  }

  void OnPropertyChange(object sender, PropertyChangedEventArgs args) {
    label.text = keySequences.accumulated;
  }

}
