using System;
using System.Text;
using rm.Trie;

namespace SeawispHunter.KeySequences {
/* KeySequences

 This class was made to interoperate with Unity's InputSystem
 */
public class KeySequence {
  protected TrieMap<Action<string>> trie = new TrieMap<Action<string>>();
  private StringBuilder keyAccum = new StringBuilder();
  public Action<string> defaultAction = null;
  public Action<string> overrideAction = null;
  public bool enabled { get; private set; } = false;

  public void Enable() => enabled = true;
  public void Disable() => enabled = false;

  public Action<string> this[string key] {
    get => trie.ValueBy(key);
    set => trie.Add(key, value);
  }

  public void Add(string key, Action<string> action = default) {
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
    var key = keyAccum.ToString();
    if (TryGetValue(key, out var action, out bool hasPrefix)) {
      // We have a key.
      if (overrideAction is not null)
        overrideAction(key);
      else if (action is not null)
        action(key);
      else if (defaultAction is not null)
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

