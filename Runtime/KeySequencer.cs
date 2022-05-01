using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Linq;
using rm.Trie;

/** Strictly speaking this class doesn't depend on Unity. But to play nice with
    the serialization system so it can be manipulated by Unity's editor we
    support it.
  */
#if UNITY_2019_4_OR_NEWER
using UnityEngine;
#endif

namespace SeawispHunter.KeySequences {
/** KeySequencer listens to a stream of characters and fires events for key
    sequence matches.

 */
[Serializable]
public class KeySequencer : IKeySequencer
#if UNITY_2019_4_OR_NEWER
  , ISerializationCallbackReceiver
#endif
{
#if UNITY_2019_4_OR_NEWER
  [SerializeField]
  private string[] keySequences;
#endif
  public const int MaxKeySequenceCount = 10;

  [NonSerialized]
  protected Trie trie = new Trie();
  [NonSerialized]
  private StringBuilder keyAccum = new StringBuilder();
  [NonSerialized]
  private char[] buffer = new char[MaxKeySequenceCount];
  [NonSerialized]
  private EnumerableCacher<char> charCollection;

  public event Action<string> accept;
  public event PropertyChangedEventHandler propertyChanged;

  public bool enabled { get; private set; } = false;

  public void Enable() => enabled = true;
  public void Disable() => enabled = false;

  [NonSerialized]
  private string _accumulated;
  public string accumulated {
    get {
      if (_accumulated == null)
        _accumulated = new string(buffer, 0, keyAccum.Length);
      return _accumulated;
    }
    private set {
      if (_accumulated != value) {
        _accumulated = value;
        propertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(accumulated)));
      }
    }
  }
  public int Count => trie.Count();

  // XXX: AddKeys?
  public void Add(string key) {
    if (trie.HasWord(key))
      throw new ArgumentException($"The key sequence {key} is already present. Use index notation to overwrite existing keys.", nameof(key));
    trie.AddWord(key);
  }

  public bool HasKeys(string key) => trie.HasWord(key);

  public void Remove(string keys) => trie.RemoveWord(keys);

  public bool HasPrefix(string key) => trie.HasPrefix(key);

  public bool TryGetWord(IEnumerable<char> key, out bool hasPrefix) {
    hasPrefix = trie.HasPrefix(key);
    return trie.HasWord(key);
  }

  /** I wanted to make this a struct IEnumerable which would work for doing a foreach, but
      it requires boxing if I need to pass it on as an IEnumerable<char>, which I do. So
      they remain classes.

      Instead, I made this enumerable cache one enumerator, which if only one
      enumerator is used at a time, there won't be any allocation. If more than
      one enumerator is used, the code will still behave appropriately but it
      will cause an allocation.

      Really good reference[1] on how to get allocation-less enumeration in a game context.

      [1]: https://hellomister.com/blog/how-to-ienumerable/
  */
  internal class EnumerableCacher<T>: IEnumerable<T> {
    internal T[] buffer;
    internal int start;
    internal int count;
    /** Keep one enumerator cached. */
    Enumerator cache;
    internal EnumerableCacher(T[] buffer, int start, int count) {
      this.buffer = buffer;
      this.start = start;
      this.count = count;
      this.cache = new Enumerator(this);
      this.cache.Dispose();
    }

    /** Return the cached enumerator as long as it's been disposed. */
    public Enumerator GetEnumerator() {
      if (cache.disposed) {
        cache.Reset();
        return cache;
      } else {
        return new Enumerator(this);
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator(); // boxing conversion if used, but required by IEnumerable interface
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    internal class Enumerator : IEnumerator<T> {
      EnumerableCacher<T> collection;
      int i;
      internal bool disposed;
      internal Enumerator(EnumerableCacher<T> collection) {
        this.collection = collection;
        i = collection.start - 1;
        disposed = false;
      }
      public bool MoveNext() {
        i++;
        return i < collection.start + collection.count;
      }
      public T Current => collection.buffer[i];
      object IEnumerator.Current => Current; // required by IEnumerator interface, but doesn't need to be public
      public void Reset() {
        i = collection.start - 1;
        disposed = false;
      }
      public void Dispose() {
        disposed = true;
      }
    }
  }

  public void Input(char c) {
    if (! enabled)
      return;
    keyAccum.Append(c);
    int length = Math.Min(buffer.Length, keyAccum.Length);
    keyAccum.CopyTo(0, buffer, 0, length);

    // var key = this.accumulated = keyAccum.ToString();
    this.accumulated = null;
    // if (charCollection == null)
    if (charCollection == null) {
      charCollection = new EnumerableCacher<char>(buffer, 0, length);
      // boxedBuffer = charCollection;
    }
    charCollection.count = length;
    foreach (char d in charCollection)
      ;
    // if (TryGetWord(buffer.Take(keyAccum.Length), out bool hasPrefix)) {
    // TryGetWord(charCollection, out bool hasPrefix2);
    // return;
    if (TryGetWord(charCollection, out bool hasPrefix)) {
      // We have a key.
      if (accept != null)
        accept(this.accumulated);
    } else if (! hasPrefix) {
      // No key and no prefix.
      // if (reject != null)
      //   reject(key);
      this.accumulated = null;
      // We could be starting a new input.
      if (keyAccum.Length > 1) {
        // Re-evaluate with a clean slate.
        keyAccum.Clear();
        // Recurse. Will only ever be one call deep.
        Input(c);
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
    trie.Clear();
    foreach (var keySequence in keySequences)
      this.Add(keySequence);
  }
#endif
}
}

