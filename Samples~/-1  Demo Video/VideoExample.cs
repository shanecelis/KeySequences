using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeawispHunter.KeySequences;
using UnityEngine.UI;
using System.ComponentModel;

public class VideoExample : MonoBehaviour {
  public KeySequencer keySequences;
  public Text label;
  public Text console;

  void OnEnable() {
    label.text = "";
    console.text = "";
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
    keySequences.accept += _key => {
      var msg = "ACCEPT " + _key;
      Debug.Log(msg);
      label.text = msg;
    };
    keySequences.propertyChanged += OnPropertyChange;
#endif
  }

  void Log(string s) {
    Debug.Log(s);
    // console.text = s;
    StartCoroutine(LogConsole(s));
  }

  IEnumerator LogConsole(string s) {
    console.text = "";
    yield return null;
    yield return null;
    console.text = s;
  }

  /** We have to do this 60 times a second to make sure we don't miss a key.
      Boo. */
  void Update() {
    // foreach (var c in Input.inputString) {
    //   // keySequences.Input(c);
    //   if (! char.IsControl(c))
    //     Log($"Input char '{c}' int {(int) c}");
    //   else
    //     Log($"Input char non-printable int {(int) c}");
    // }
  }

  void OnPropertyChange(object sender, PropertyChangedEventArgs args) {
    label.text = keySequences.accumulated;
  }

}
