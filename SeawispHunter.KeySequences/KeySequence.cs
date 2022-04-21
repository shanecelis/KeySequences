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
  TrieMap<Action<string>> trie = new TrieMap<Action<string>>();
  private StringBuilder keyAccum = new StringBuilder();

  public Action<string> this[string key] {
    get => trie.ValueBy(key);
    set => trie.Add(key, value);
  }

  public bool TryGetValue(string key, out Action<string> action, out bool hasPrefix) {
    hasPrefix = trie.HasKeyPrefix(key);
    action = trie.ValueBy(key);
    return trie.HasKey(key);
  }

  public void OnTextInput(char c) {
    keyAccum.Append(c);
    var key = keyAccum.ToString();
    if (TryGetValue(key, out var action, out bool hasPrefix)) {
      // We have a key.
      action(key);
      if (! hasPrefix)
        keyAccum.Clear();
    } else {
      if (! hasPrefix) {
        if (keyAccum.Length > 1) {
          keyAccum.Clear();
          // We could be starting a new input. 
          // Recurse. Will only ever be one call deep.
          OnTextInput(c);
        } else {
          keyAccum.Clear();
        }
      }
    }
  }
}

}
