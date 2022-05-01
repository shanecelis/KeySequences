using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeawispHunter.KeySequences;
using UnityEngine.UI;
using System.ComponentModel;

public class InputManagerExample : MonoBehaviour {
  public KeySequencer keySequences;
  [Header("Turn off logging (causes allocations)")]
  public bool enableLogging = true;
  public Text label;

  void OnEnable() {
    keySequences.Enable();
    keySequences.propertyChanged += OnPropertyChange;
  }

  void OnDisable() {
    keySequences.Disable();
    keySequences.propertyChanged -= OnPropertyChange;
  }

  void Start() {
#if ! ENABLE_LEGACY_INPUT_MANAGER
    Debug.LogError("Old input system required.");
#else
    Debug.Log("Using old input system.");
    Debug.Log($"There are {keySequences.Count} key sequences.");
    keySequences.accept += _key => {
      var msg = $"ACCEPT {_key}";
      Debug.Log(msg);
      label.text = msg;
    };
#endif
  }

  /** You can use keySequences with the legacy InputManager. However, there are
      a few caveats.

      You have to poll 60 times a second to make sure we don't miss a key. Boo.
      Another disadvantage of this method is that calling `Input.inputString`
      causes Unity to allocate a string. So there are three strikes against
      this:

      - It allocates unavoidably for every keypress that contains key presses.
      - It runs every frame.
      - It's a pull rather than a push.

      Each of these are mitigated by the new InputSystem shown in demo 1. But the code
      works and it probably won't be an issue if you're already deeply invested
      in the legacy InputManager.
  */
  void Update() {
    foreach (var c in Input.inputString) {
      keySequences.Input(c);
      if (enableLogging) {
        if (! char.IsControl(c))
          Debug.Log($"Input char '{c}' int {(int) c}");
        else
          Debug.Log($"Input char non-printable int {(int) c}");
      }
    }
  }

  void OnPropertyChange(object sender, PropertyChangedEventArgs args) {
    label.text = keySequences.accumulated;
  }

}
