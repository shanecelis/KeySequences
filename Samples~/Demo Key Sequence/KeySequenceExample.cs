using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using SeawispHunter.KeySequences;
using UnityEngine.InputSystem;
using System.ComponentModel;


public class KeySequenceExample : MonoBehaviour {
  KeySequencerMap<Action<string>> ks = new KeySequencerMap<Action<string>>();
  public string[] keys;
  Label label;

  void OnEnable() {
    foreach (var key in keys)
      ks.Add(key);
    ks.Enable();
    Keyboard.current.onTextInput += ks.OnTextInput;
  }

  void OnDisable() {
    ks.Disable();
    Keyboard.current.onTextInput -= ks.OnTextInput;
  }

  void Start() {
#if ! ENABLE_INPUT_SYSTEM
    Debug.LogError("New input system required.");
#else
    Debug.Log("Using new input system.");
    var root = GetComponent<UIDocument>().rootVisualElement;
    label = root.Q<Label>();
    ks.accept += (_key, subAction) => {
      Debug.Log("action " + _key);
      subAction?.Invoke(_key);
    };
    ks.propertyChanged += OnPropertyChange;
#endif
  }

  void OnPropertyChange(object sender, PropertyChangedEventArgs args) {
    label.text = "here " + ks.accumulated;
  }

}
