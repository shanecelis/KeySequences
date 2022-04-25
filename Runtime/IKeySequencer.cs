using System;

public interface IKeySequencer {

  /** Call this event when a key sequence has been input. */
  event Action<string> accept;
  /** Call this event when a key sequence has been rejected.
    
      Note: This is likely to be called many more times than accept. Consider leaving it null if the rejected candidates are not important.
   */
  event Action<string> reject;

  /** Return any accumulated prefix or key sequence. Will return a zero-length string when no prefixes or key sequences are present. */
  string accumulated { get; }

  /** Return true if this key sequencer is enabled. */
  bool enabled { get; }

  /** Enable the sequencer. By default it is disabled. */
  void Enable();
  /** Disable the sequencer. */
  void Disable();

  /** Add a key sequence. */
  void Add(string keys);

  /** Return true if this object has the `keys` key sequence. */
  bool HasKeys(string keys);

  /** Return true if this object has a prefix `keysPrefix` in any key sequence. */
  bool HasPrefix(string keysPrefix);

  /** Provide an input to the sequencer. */
  void OnTextInput(char c);
}
