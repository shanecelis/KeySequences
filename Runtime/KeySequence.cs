using System;
using System.Text;
using rm.Trie;

namespace SeawispHunter.KeySequences {
/* KeySequences

 This class was made to interoperate with Unity's InputSystem
 */
public class KeySequence {
  TrieMap<Action<string>> trie = new TrieMap<Action<string>>();
  private StringBuilder keyAccum = new StringBuilder();
  public Action<string> defaultAction = null;
  public Action<string> overrideAction = null;
  public event PropertyChangedEventHandler propertyChanged;

  public bool enabled { get; private set; } = false;

  public void Enable() => enabled = true;
  public void Disable() => enabled = false;

  private string _accumulated;
  public string accumulated {
    get {
      if (_accumulated == null)
        _accumulated = keyAccum.ToString();
      return _accumulated;
    }
    private set {
      if (_accumulated != value) {
        _accumulated = value;
        propertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(accumulated)));
      }
    }
  }

  public Action<string> this[string key] {
    get => trie.ValueBy(key);
    set => trie.Add(key, value);
  }

  public void Add(string key, Action<string> action = null) {
    trie.Add(key, action);
  }

  public bool HasKey(string key) => trie.HasKey(key);

  public bool HasKeyPrefix(string key) => trie.HasKeyPrefix(key);

  public bool TryGetValue(string key, out Action<string> action, out bool hasPrefix) {
    hasPrefix = trie.HasKeyPrefix(key);
    action = trie.ValueBy(key);
    return trie.HasKey(key);
  }

  public void OnTextInput(char c) {
    if (! enabled)
      return;
    keyAccum.Append(c);
    var key = accumulated = keyAccum.ToString();
    if (TryGetValue(key, out var action, out bool hasPrefix)) {
      // We have a key.
      if (overrideAction != null)
        overrideAction(key);
      else if (action != null)
        action(key);
      else if (defaultAction != null)
        defaultAction(key);
    } else if (! hasPrefix) {
      // No key and no prefix. We could be starting a new input.
      if (keyAccum.Length > 1) {
        // Re-evaluate with a clean slate.
        keyAccum.Clear();
        // Recurse. Will only ever be one call deep.
        OnTextInput(c);
        return;
      }
    }
    if (! hasPrefix)
      keyAccum.Clear();
  }
}
}

