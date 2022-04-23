using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using SeawispHunter.KeySequences;
using UnityEngine.InputSystem;
using System.ComponentModel;


public class KeySequenceExample : MonoBehaviour {
  KeySequencerMap ks = new KeySequencerMap();
  public string[] keys;
  Label label;
  // Start is called before the first frame update
  void OnEnable() {
    foreach (var key in keys)
      ks.Add(key);
    ks.Enable();
    Keyboard.current.onTextInput += ks.OnTextInput;
  }
  void OnDisable() {
    Keyboard.current.onTextInput -= ks.OnTextInput;
  }
  void Start() {
#if ! ENABLE_INPUT_SYSTEM
    Debug.LogError("New input system required.");
#else
    Debug.Log("Using new input system.");
    var root = GetComponent<UIDocument>().rootVisualElement;
    label = root.Q<Label>();
    ks.defaultAction += _key => Debug.Log("action " + _key);
    ks.propertyChanged += OnPropertyChange;
#endif
  }

  void OnPropertyChange(object sender, PropertyChangedEventArgs args) {
    label.text = "here " + ks.accumulated;
  }

}
