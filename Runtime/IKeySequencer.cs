using System;

public interface IKeySequencer {

  event Action<string> defaultAction;
  event Action<string> rejectAction;

  string accumulated { get; }

  bool enabled { get; }

  void Enable();
  void Disable();

  void Add(string key);

  bool HasKey(string key);

  bool HasKeyPrefix(string key);

  void OnTextInput(char c);
}
