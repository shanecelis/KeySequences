using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using SeawispHunter.KeySequences;
using UnityEngine.InputSystem;

public class KeySequenceExample : MonoBehaviour {
    KeySequence ks = new KeySequence();
    public string[] keys;
    Label label;
    // Start is called before the first frame update
    void Start() {
#if ! ENABLE_INPUT_SYSTEM
      Debug.LogError("New input system required.");
#else
        var root = GetComponent<UIDocument>().rootVisualElement;
        label = root.Q<Label>();
        foreach (var key in keys)
            ks.Add(key);
        Keyboard.current.onTextInput += OnTextInput;
        ks.defaultAction += key => Debug.Log("action " + key);
        ks.Enable();
#endif
    }

    void OnTextInput(char c) {
        ks.OnTextInput(c);
        label.text = "here " + ks.accumulated;
    }

}
