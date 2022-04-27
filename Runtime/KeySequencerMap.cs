using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using rm.Trie;

namespace SeawispHunter.KeySequences {
/* KeySequencerMap

 */
public class KeySequencerMap<T> : IKeySequencer<T> {
  TrieMap<T> trie = new TrieMap<T>();
  private StringBuilder keyAccum = new StringBuilder();

  private Dictionary<Action<string>, Action<string, T>> acceptors;
  event Action<string> IKeySequencer.accept {
    add {
      if (acceptors == null)
        acceptors = new Dictionary<Action<string>, Action<string, T>>();

      this.accept += acceptors[value] = (s, _) => value(s);
    }
    remove {
      if (acceptors != null && acceptors.TryGetValue(value, out var acceptDelegate))
        this.accept -= acceptDelegate;
      else {
        /* XXX: This is not guaranteed to work since the lambdas may in fact be
           different, which is why we try to keep track of the delegates we
           make. */
        this.accept -= (s, _) => value(s);
      }
    }
  }
  public event Action<string, T> accept;
  public event Action<string> reject;
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

  public T this[string key] {
    get => trie.ValueBy(key);
    set => trie.Add(key, value);
  }

  public void Add(string key) {
    trie.Add(key, default(T));
  }

  public void Add(string key, T value) {
    trie.Add(key, value);
  }

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

