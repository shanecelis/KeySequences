using System;
using System.ComponentModel;
using System.Text;
using rm.Trie;
#if UNITY_2019_4_OR_NEWER
using UnityEngine;
#endif

namespace SeawispHunter.KeySequences {
/* KeySequencer

 */
[Serializable]
public class KeySequencer : IKeySequencer
#if UNITY_2019_4_OR_NEWER
  , ISerializationCallbackReceiver
#endif
{
#if UNITY_2019_4_OR_NEWER
  [SerializeField]
  private string[] keySequences;
#endif
  [NonSerialized]
  protected Trie trie = new Trie();
  [NonSerialized]
  private StringBuilder keyAccum = new StringBuilder();
  public event Action<string> accept;
  public event Action<string> reject;
  public event PropertyChangedEventHandler propertyChanged;

  public bool enabled { get; private set; } = false;

  public void Enable() => enabled = true;
  public void Disable() => enabled = false;

  [NonSerialized]
  private string _accumulated;
  public string accumulated {
    get {
      return _accumulated ?? "";
    }
    private set {
      if (_accumulated != value) {
        _accumulated = value;
        propertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(accumulated)));
      }
    }
  }
  public int Count => trie.Count();

  // XXX: AddKeys?
  public void Add(string key) => trie.AddWord(key);

  public bool HasKeys(string key) => trie.HasWord(key);

  public bool HasPrefix(string key) => trie.HasPrefix(key);

  public bool TryGetWord(string key, out bool hasPrefix) {
    hasPrefix = trie.HasPrefix(key);
    return trie.HasWord(key);
  }

  public void OnTextInput(char c) {
    if (! enabled)
      return;
    keyAccum.Append(c);
    var key = this.accumulated = keyAccum.ToString();
    if (TryGetWord(key, out bool hasPrefix)) {
      // We have a key.
      if (accept != null)
        accept(key);
    } else if (! hasPrefix) {
      // No key and no prefix.
      if (reject != null)
        reject(key);
      accumulated = null;
      // We could be starting a new input.
      if (keyAccum.Length > 1) {
        // Re-evaluate with a clean slate.
        keyAccum.Clear();
        // Recurse. Will only ever be one call deep.
        OnTextInput(c);
        return;
      }
    }

    if (! hasPrefix) {
      keyAccum.Clear();
      // this.accumulated = "";
    }
  }

#if UNITY_2019_4_OR_NEWER
  public void OnBeforeSerialize() {
  }

  public void OnAfterDeserialize() {
    foreach (var keySequence in keySequences)
      this.Add(keySequence);
  }
#endif
}
}

