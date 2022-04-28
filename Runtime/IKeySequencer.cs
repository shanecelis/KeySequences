using System;
using System.ComponentModel;

namespace SeawispHunter.KeySequences {

/** Given a collection of key sequences and a stream of key sequences
    represented by char, fire accept event on matching key sequences. Can also
    fire reject event on key sequences with no match.

    Note: Implementation suggested to use a trie for performance benefits.
  */
public interface IKeySequencer {

  /** Return true if this key sequencer is enabled. */
  bool enabled { get; }

  /** Enable the sequencer. By default it is disabled. */
  void Enable();
  /** Disable the sequencer. */
  void Disable();

  /** Add a key sequence. Throws an exception if the key sequence is already present. */
  void Add(string keys);

  /** Remove a key sequence. Throws an exception if the key sequence is not present. */
  void Remove(string keys);

  /** Return true if this object has the `keys` key sequence. */
  bool HasKeys(string keys);

  /** Return true if this object has a prefix `keysPrefix` in any key
      sequence. */
  bool HasPrefix(string keysPrefix);

  /** Return any accumulated prefix or key sequence. Will return a zero-length
      string when no prefixes or key sequences are present. */
  string accumulated { get; }

  /** Provide an input to the sequencer. */
  void Input(char c);

  /** Call this event when a key sequence has been input. */
  event Action<string> accept;

  /** Call this event when a key sequence has been rejected.
    
      Note: This is likely to be called many more times than accept. Consider
      leaving it null if the rejected candidates are not important.
   */
  // event Action<string> reject;

  /** Called whenever `accumulated` is updated. */
  event PropertyChangedEventHandler propertyChanged;

}

/** A key sequencer with a little more information. */
public interface IKeySequencer<T> : IKeySequencer {

  /** Add a key sequence and value. Throws an exception if the key sequence is already present. */
  void Add(string keys, T value);

  /** Call this event when a key sequence has been input. */
  new event Action<string, T> accept;

  /** Set a key sequence to a value. */
  T this[string key] { get; set; }

}
}
