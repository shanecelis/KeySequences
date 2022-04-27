using System;
using System.ComponentModel;
using System.Text;
using rm.Trie;

#if UNITY_2019_4_OR_NEWER
using UnityEngine;
#endif

namespace SeawispHunter.KeySequences {

[Serializable]
public class KeySequencerMapInt : KeySequencerMap<int> { }
/** KeySequencerMap is similar to KeySequencer but each key sequence accepts
    a value that will be passed to the accept method.

    Because of the generic type, this is not as easy to embed in a Unity
    MonoBehaviour script. Only concrete types will be shown in the inspector, so
    one will have to do something like this:

    ```
    [Serializable] public class KeySequencerMapFloat : KeySequencerMap<float> { }
    ```

    In order to not get double nesting in the inspector, add the new class to
    the KeySequencerDrawer class like so:

    ```
    [CustomPropertyDrawer(typeof(KeySequencerMapFloat))]
    [CustomPropertyDrawer(typeof(KeySequencer))]
    public class KeySequencerDrawer : PropertyDrawer {
    ...
    ```
 */
[Serializable]
public class KeySequencerMap<T> : IKeySequencer<T>
#if UNITY_2019_4_OR_NEWER
  , ISerializationCallbackReceiver
#endif
{

#if UNITY_2019_4_OR_NEWER
  [Serializable]
  internal struct Entry {
    [SerializeField]
    internal string keys;
    [SerializeField]
    internal T value;
  }
  [SerializeField]
  private Entry[] keySequences;
#endif
  [NonSerialized]
  TrieMap<T> trie = new TrieMap<T>();
  [NonSerialized]
  private StringBuilder keyAccum = new StringBuilder();

  [NonSerialized]
  private Action<string> _accept;
  event Action<string> IKeySequencer.accept {
    add {
      this._accept += value;
    }
    remove {
      this._accept -= value;
    }
  }
  public event Action<string, T> accept;
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

  public T this[string key] {
    get => trie.ValueBy(key);
    set => trie.Add(key, value);
  }

  public void Add(string key) {
    trie.Add(key, default(T));
  }

  public void Add(string key, T value) {
    if (trie.HasKey(key))
      throw new ArgumentException($"The key sequence {key} is already present. Use index notation to overwrite existing keys.", nameof(key));
    trie.Add(key, value);
  }

  public void Remove(string keys) => trie.Remove(keys);

  public bool HasKeys(string key) => trie.HasKey(key);

  public bool HasPrefix(string key) => trie.HasKeyPrefix(key);

  public bool TryGetValue(string key, out T value, out bool hasPrefix) {
    hasPrefix = trie.HasKeyPrefix(key);
    value = trie.ValueBy(key);
    return trie.HasKey(key);
  }

  public void OnTextInput(char c) {
    if (! enabled)
      return;
    keyAccum.Append(c);
    var key = this.accumulated = keyAccum.ToString();
    if (TryGetValue(key, out T value, out bool hasPrefix)) {
      // We have a key.
      // Q: Should the action override the accept or just do something too?
      // Q: Should it return a bool to say it was handled?
      if (accept != null)
        accept(key, value);
      if (_accept != null)
        _accept(key);
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
      this.Add(keySequence.keys, keySequence.value);
  }
#endif
}
}

