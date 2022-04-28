using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using SeawispHunter.KeySequences;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using System.ComponentModel;

public class KeySequenceExample : MonoBehaviour {
  public KeySequencerMapInt keySequencer;
  Label label;

  void OnEnable() {
    keySequencer.Enable();
    Keyboard.current.onTextInput += keySequencer.Input;
  }

  void OnDisable() {
    keySequencer.Disable();
    Keyboard.current.onTextInput -= keySequencer.Input;
  }

  void Start() {
    foreach (var device in InputSystem.devices)
      Debug.Log("device " + device);
#if ! ENABLE_INPUT_SYSTEM
    Debug.LogError("New input system required.");
#else
    Debug.Log("Using new input system.");
    var root = GetComponent<UIDocument>().rootVisualElement;
    label = root.Q<Label>();
    keySequencer.accept += (_key, value) => {
      Debug.Log($"action {_key} value {value}");
    };
    keySequencer.propertyChanged += OnPropertyChange;
#endif
    // Response to the first button press. Calls our delegate
// and then immediately stops listening.
    // InputSystem.onAnyButtonPress
    //            .CallOnce(ctrl => Debug.Log($"Button {ctrl} was pressed"));
    InputSystem.onEvent
               .Where(e => e.HasButtonPress())
               .CallOnce(eventPtr =>
                         {
                           foreach (var button in eventPtr.GetAllButtonPresses())
                             Debug.Log($"Button {button} was pressed");
                         });
  }

  void OnPropertyChange(object sender, PropertyChangedEventArgs args) {
    label.text = "here " + keySequencer.accumulated;
  }

}
