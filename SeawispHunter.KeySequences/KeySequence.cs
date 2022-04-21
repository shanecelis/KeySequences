using System.Collections;
using System.Collections.Generic;
using rm.Trie;
using System;
using System.Text;

namespace SeawispHunter.KeySequences {
/* This handles a key sequence.

  - Q? How to handle ctrl and alt?

 */
public class KeySequence {
  TrieMap<Action> trie = new TrieMap<Action>();
  private StringBuilder keyAccum = new StringBuilder();

  public Action this[string key] {
    get => trie.ValueBy(key);
    set => trie.Add(key, value);
  }

  public bool TryGetValue(string key, out Action action, out bool hasPrefix) {
    hasPrefix = trie.HasKeyPrefix(key);
    action = trie.ValueBy(key);
    return trie.HasKey(key);
  }

  public void OnTextInput(char c) {
    keyAccum.Append(c);
    var key = keyAccum.ToString();
    if (TryGetValue(key, out var action, out bool hasPrefix)) {
      // We have a key.
      defaultAction();
      if (! hasPrefix)
        keyAccum.Clear();
    } else if (! hasPrefix) {
      keyAccum.Clear();
      if (trie.HasKeyPrefix(c.ToString()))
        keyAccum.Append(c);
    }
  }
}

}
