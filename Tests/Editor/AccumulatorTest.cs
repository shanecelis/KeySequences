using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SeawispHunter.KeySequences;

namespace SeawispHunter.KeySequences.Tests {
  public class AccumulatorTest {
    private KeySequence ks;
    private Dictionary<string, int> counts;
    [SetUp]
    public void Setup() {
      ks = new KeySequence();
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

    private void AccumEqual(string key) {
      Assert.AreEqual(key, ks.accumulated);
    }

    [Test]
    public void TestOtherKey() {
      SetupAbc();
      Assert.AreEqual(0, GetCount("a"));
      Assert.AreEqual(0, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      AccumEqual("");
      ks.OnTextInput('a');
      AccumEqual("a");
      Assert.AreEqual(1, GetCount("a"));
      Assert.AreEqual(0, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('z');
      AccumEqual("");
      Assert.AreEqual(1, GetCount("a"));
      Assert.AreEqual(0, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('b');
      AccumEqual("");
      Assert.AreEqual(1, GetCount("a"));
      Assert.AreEqual(0, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
    }

    [Test]
    public void TestSetAction() {
      ks["a"] = IncrCounter;
      ks.Enable();
      AccumEqual("");
      Assert.AreEqual(0, GetCount("a"));
      ks.OnTextInput('b');
      AccumEqual("");
      Assert.AreEqual(0, GetCount("a"));
      ks.OnTextInput('a');
      AccumEqual("a");
      Assert.AreEqual(1, GetCount("a"));
    }

    [Test]
    public void TestAddAction() {
      ks["a"] += IncrCounter;
      ks.Enable();
      AccumEqual("");
      Assert.AreEqual(0, GetCount("a"));
      ks.OnTextInput('b');
      AccumEqual("");
      Assert.AreEqual(0, GetCount("a"));
      ks.OnTextInput('a');
      AccumEqual("a");
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
      AccumEqual("");
      ks.OnTextInput('a');
      AccumEqual("a");
      Assert.AreEqual(1, GetCount("a"));
      Assert.AreEqual(0, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('a');
      AccumEqual("a");
      Assert.AreEqual(2, GetCount("a"));
      Assert.AreEqual(0, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('b');
      AccumEqual("ab");
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
      AccumEqual("");
      ks.OnTextInput('a');
      AccumEqual("a");
      Assert.AreEqual(1, GetCount("a"));
      Assert.AreEqual(0, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('b');
      AccumEqual("ab");
      Assert.AreEqual(1, GetCount("a"));
      Assert.AreEqual(1, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('a');
      AccumEqual("a");
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
      AccumEqual("");
      ks.OnTextInput('a');
      AccumEqual("a");
      Assert.AreEqual(1, GetCount("a"));
      Assert.AreEqual(0, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('b');
      AccumEqual("ab");
      Assert.AreEqual(1, GetCount("a"));
      Assert.AreEqual(1, GetCount("ab"));
      Assert.AreEqual(0, GetCount("abc"));
      ks.OnTextInput('c');
      AccumEqual("abc");
      Assert.AreEqual(1, GetCount("a"));
      Assert.AreEqual(1, GetCount("ab"));
      Assert.AreEqual(1, GetCount("abc"));

      ks.OnTextInput('a');
      AccumEqual("a");
      Assert.AreEqual(2, GetCount("a"));
      Assert.AreEqual(1, GetCount("ab"));
      Assert.AreEqual(1, GetCount("abc"));

      ks.OnTextInput('a');
      AccumEqual("a");
      Assert.AreEqual(3, GetCount("a"));
      Assert.AreEqual(1, GetCount("ab"));
      Assert.AreEqual(1, GetCount("abc"));
    }
  }
}
