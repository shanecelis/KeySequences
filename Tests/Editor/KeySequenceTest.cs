using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SeawispHunter.KeySequences;

namespace SeawispHunter.KeySequences.Tests {
  public class KeySequenceTest {
    private KeySequencerMap ks;
    private Dictionary<string, int> counts;
    [SetUp]
    public void Setup() {
      ks = new KeySequencerMap();
      counts = new Dictionary<string, int>();
    }

    void SetupAbc() {
      AddCounter("a");
      AddCounter("ab");
      AddCounter("abc");
      ks.Enable();
    }

    int GetCount(string key) {
      if (counts.TryGetValue(key, out int count))
        return count;
      else
        return 0;
      // counts.GetValueOrDefault(key, 0);
    }

    private void AddCounter(string key) {
      ks[key] = IncrCounter;
    }

    private void IncrCounter(string key) {
      if (! counts.TryGetValue(key, out int count))
        count = 0;
      counts[key] = ++count;
    }

    [Test]
    public void TestOtherKey() {
      SetupAbc();
      Assert.AreEqual(0, GetCount("a"));
      Assert.AreEqual(0, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('a');
      Assert.AreEqual(1, GetCount("a"));
      Assert.AreEqual(0, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('z');
      Assert.AreEqual(1, GetCount("a"));
      Assert.AreEqual(0, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('b');
      Assert.AreEqual(1, GetCount("a"));
      Assert.AreEqual(0, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
    }

    [Test]
    public void TestSetAction() {
      ks["a"] = IncrCounter;
      ks.Enable();
      Assert.AreEqual(0, GetCount("a"));
      ks.OnTextInput('b');
      Assert.AreEqual(0, GetCount("a"));
      ks.OnTextInput('a');
      Assert.AreEqual(1, GetCount("a"));
    }

    [Test]
    public void TestAddAction() {
      ks["a"] += IncrCounter;
      ks.Enable();
      Assert.AreEqual(0, GetCount("a"));
      ks.OnTextInput('b');
      Assert.AreEqual(0, GetCount("a"));
      ks.OnTextInput('a');
      Assert.AreEqual(1, GetCount("a"));
    }

    [Test]
    public void TestAddMultipleAction() {
      ks["a"] += IncrCounter;
      ks["a"] += IncrCounter;
      ks.Enable();
      Assert.AreEqual(0, GetCount("a"));
      ks.OnTextInput('b');
      Assert.AreEqual(0, GetCount("a"));
      ks.OnTextInput('a');
      Assert.AreEqual(2, GetCount("a"));
    }

    [Test]
    public void TestRemoveAction() {
      ks["a"] += IncrCounter;
      ks["a"] += IncrCounter;
      ks.Enable();
      Assert.AreEqual(0, GetCount("a"));
      ks.OnTextInput('b');
      Assert.AreEqual(0, GetCount("a"));
      ks.OnTextInput('a');
      Assert.AreEqual(2, GetCount("a"));
      ks["a"] -= IncrCounter;
      ks.OnTextInput('a');
      Assert.AreEqual(3, GetCount("a"));
      ks["a"] -= IncrCounter;
      ks.OnTextInput('a');
      Assert.AreEqual(3, GetCount("a"));
    }

    [Test]
    public void TestRepeated() {
      SetupAbc();
      Assert.AreEqual(0, GetCount("a"));
      Assert.AreEqual(0, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('a');
      Assert.AreEqual(1, GetCount("a"));
      Assert.AreEqual(0, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('a');
      Assert.AreEqual(2, GetCount("a"));
      Assert.AreEqual(0, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('b');
      Assert.AreEqual(2, GetCount("a"));
      Assert.AreEqual(1, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
    }
    [Test]
    public void TestSequential() {
      SetupAbc();
      Assert.AreEqual(0, GetCount("a"));
      Assert.AreEqual(0, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('a');
      Assert.AreEqual(1, GetCount("a"));
      Assert.AreEqual(0, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('b');
      Assert.AreEqual(1, GetCount("a"));
      Assert.AreEqual(1, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
    }

    [Test]
    public void TestNewStart() {
      SetupAbc();
      Assert.AreEqual(0, GetCount("a"));
      Assert.AreEqual(0, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('a');
      Assert.AreEqual(1, GetCount("a"));
      Assert.AreEqual(0, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('b');
      Assert.AreEqual(1, GetCount("a"));
      Assert.AreEqual(1, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('a');
      Assert.AreEqual(2, GetCount("a"));
      Assert.AreEqual(1, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
    }

    [Test]
    public void TestNewStart2() {
      SetupAbc();
      Assert.AreEqual(0, GetCount("a"));
      Assert.AreEqual(0, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('a');
      Assert.AreEqual(1, GetCount("a"));
      Assert.AreEqual(0, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('b');
      Assert.AreEqual(1, GetCount("a"));
      Assert.AreEqual(1, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('c');
      Assert.AreEqual(1, GetCount("a"));
      Assert.AreEqual(1, GetCount("ab"));
      Assert.AreEqual(1, GetCount("abc"));

      ks.OnTextInput('a');
      Assert.AreEqual(2, GetCount("a"));
      Assert.AreEqual(1, GetCount("ab"));
      Assert.AreEqual(1, GetCount("abc"));

      ks.OnTextInput('a');
      Assert.AreEqual(3, GetCount("a"));
      Assert.AreEqual(1, GetCount("ab"));
      Assert.AreEqual(1, GetCount("abc"));
    }
  }
}
