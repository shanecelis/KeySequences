using System;
using System.ComponentModel;
using System.Text;
using rm.Trie;

namespace SeawispHunter.KeySequences {
/* KeySequencerMap

 */
public class KeySequencerMap : IKeySequencer {
  TrieMap<Action<string>> trie = new TrieMap<Action<string>>();
  private StringBuilder keyAccum = new StringBuilder();
  public event Action<string> accept = null;
  public event Action<string> reject = null;
  public event PropertyChangedEventHandler propertyChanged;

  public bool enabled { get; private set; } = false;

  public void Enable() => enabled = true;
  public void Disable() => enabled = false;

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

  public Action<string> this[string key] {
    get => trie.ValueBy(key);
    set => trie.Add(key, value);
  }

  public void Add(string key) {
    trie.Add(key, null);
  }

  public void Add(string key, Action<string> action) {
    trie.Add(key, action);
  }

  public bool HasKeys(string key) => trie.HasKey(key);

  public bool HasPrefix(string key) => trie.HasKeyPrefix(key);

  public bool TryGetValue(string key, out Action<string> action, out bool hasPrefix) {
    hasPrefix = trie.HasKeyPrefix(key);
    action = trie.ValueBy(key);
    return trie.HasKey(key);
  }

  public void OnTextInput(char c) {
    if (! enabled)
      return;
    keyAccum.Append(c);
    var key = this.accumulated = keyAccum.ToString();
    if (TryGetValue(key, out var action, out bool hasPrefix)) {
      // We have a key.
      // Q: Should the action override the accept or just do something too?
      // Q: Should it return a bool to say it was handled?
      if (action != null)
        action(key);
      else if (accept != null)
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
}
}

