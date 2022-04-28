using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeawispHunter.KeySequences;
using UnityEngine.UI;
using System.ComponentModel;

public class CanvasExample : MonoBehaviour {
  public KeySequencer keySequences;
  public Text label;

  void OnEnable() {
    keySequences.Enable();
  }

  void OnDisable() {
    keySequences.Disable();
  }

  void Start() {
#if ! ENABLE_LEGACY_INPUT_MANAGER
    Debug.LogError("Old input system required.");
#else
    Debug.Log("Using old input system.");
    Debug.Log($"There are {keySequences.Count} key sequences.");
    keySequences.accept += _key => Debug.Log("action " + _key);
    keySequences.propertyChanged += OnPropertyChange;
#endif
  }

  /** We have to do this 60 times a second to make sure we don't miss a key.
      Boo. */
  void Update() {
    foreach (var c in Input.inputString) {
      keySequences.Input(c);
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
