using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using SeawispHunter.KeySequences;
using UnityEngine.InputSystem;
using System.ComponentModel;


public class KeySequenceExample : MonoBehaviour {
  public KeySequencerMapInt keySequencer;
  Label label;

  void OnEnable() {
    keySequencer.Enable();
    Keyboard.current.onTextInput += keySequencer.OnTextInput;
  }

  void OnDisable() {
    keySequencer.Disable();
    Keyboard.current.onTextInput -= keySequencer.OnTextInput;
  }

  void Start() {
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
  }

  void OnPropertyChange(object sender, PropertyChangedEventArgs args) {
    label.text = "here " + keySequencer.accumulated;
  }

}
