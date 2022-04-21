using Xunit;
using System.Collections.Generic;
using SeawispHunter.KeySequences;

namespace SeawispHunter.KeySequences.Tests;

public class UnitTest1 {
  private KeySequence ks = new KeySequence();
  private Dictionary<string, int> counts = new Dictionary<string, int>();

  public UnitTest1() {
  }

  void SetupAbc() {
    AddCounter("a");
    AddCounter("ab");
    AddCounter("abc");
    ks.Enable();
  }

  int GetCount(string key) => counts.GetValueOrDefault(key, 0);

  private void AddCounter(string key) {
    ks[key] = IncrCounter;
  }

  private void IncrCounter(string key) {
    if (! counts.TryGetValue(key, out int count))
      count = 0;
    counts[key] = ++count;
  }

  [Fact]
  public void TestOtherKey() {
    SetupAbc();
    Assert.Equal(0, GetCount("a")); 
    Assert.Equal(0, GetCount("ab")); 
    Assert.Equal(0, GetCount("abc")); 
    ks.OnTextInput('a');
    Assert.Equal(1, GetCount("a")); 
    Assert.Equal(0, GetCount("ab")); 
    Assert.Equal(0, GetCount("abc")); 
    ks.OnTextInput('z');
    Assert.Equal(1, GetCount("a")); 
    Assert.Equal(0, GetCount("ab")); 
    Assert.Equal(0, GetCount("abc")); 
    ks.OnTextInput('b');
    Assert.Equal(1, GetCount("a")); 
    Assert.Equal(0, GetCount("ab")); 
    Assert.Equal(0, GetCount("abc")); 
  }

  [Fact]
  public void TestSetAction() {
    ks["a"] = IncrCounter;
    ks.Enable();
    Assert.Equal(0, GetCount("a")); 
    ks.OnTextInput('b');
    Assert.Equal(0, GetCount("a")); 
    ks.OnTextInput('a');
    Assert.Equal(1, GetCount("a")); 
  }
  
  [Fact]
  public void TestAddAction() {
    ks["a"] += IncrCounter;
    ks.Enable();
    Assert.Equal(0, GetCount("a")); 
    ks.OnTextInput('b');
    Assert.Equal(0, GetCount("a")); 
    ks.OnTextInput('a');
    Assert.Equal(1, GetCount("a")); 
  }
  
  [Fact]
  public void TestAddMultipleAction() {
    ks["a"] += IncrCounter;
    ks["a"] += IncrCounter;
    ks.Enable();
    Assert.Equal(0, GetCount("a")); 
    ks.OnTextInput('b');
    Assert.Equal(0, GetCount("a")); 
    ks.OnTextInput('a');
    Assert.Equal(2, GetCount("a")); 
  }
  
  [Fact]
  public void TestRemoveAction() {
    ks["a"] += IncrCounter;
    ks["a"] += IncrCounter;
    ks.Enable();
    Assert.Equal(0, GetCount("a")); 
    ks.OnTextInput('b');
    Assert.Equal(0, GetCount("a")); 
    ks.OnTextInput('a');
    Assert.Equal(2, GetCount("a")); 
    ks["a"] -= IncrCounter;
    ks.OnTextInput('a');
    Assert.Equal(3, GetCount("a")); 
    ks["a"] -= IncrCounter;
    ks.OnTextInput('a');
    Assert.Equal(3, GetCount("a")); 
  }
  
  [Fact]
  public void TestRepeated() {
    SetupAbc();
    Assert.Equal(0, GetCount("a")); 
    Assert.Equal(0, GetCount("ab")); 
    Assert.Equal(0, GetCount("abc")); 
    ks.OnTextInput('a');
    Assert.Equal(1, GetCount("a")); 
    Assert.Equal(0, GetCount("ab")); 
    Assert.Equal(0, GetCount("abc")); 
    ks.OnTextInput('a');
    Assert.Equal(2, GetCount("a")); 
    Assert.Equal(0, GetCount("ab")); 
    Assert.Equal(0, GetCount("abc")); 
    ks.OnTextInput('b');
    Assert.Equal(2, GetCount("a")); 
    Assert.Equal(1, GetCount("ab")); 
    Assert.Equal(0, GetCount("abc")); 
  }
  [Fact]
  public void TestSequential() {
    SetupAbc();
    Assert.Equal(0, GetCount("a")); 
    Assert.Equal(0, GetCount("ab")); 
    Assert.Equal(0, GetCount("abc")); 
    ks.OnTextInput('a');
    Assert.Equal(1, GetCount("a")); 
    Assert.Equal(0, GetCount("ab")); 
    Assert.Equal(0, GetCount("abc")); 
    ks.OnTextInput('b');
    Assert.Equal(1, GetCount("a")); 
    Assert.Equal(1, GetCount("ab")); 
    Assert.Equal(0, GetCount("abc")); 
  }
  
  [Fact]
  public void TestNewStart() {
    SetupAbc();
    Assert.Equal(0, GetCount("a")); 
    Assert.Equal(0, GetCount("ab")); 
    Assert.Equal(0, GetCount("abc")); 
    ks.OnTextInput('a');
    Assert.Equal(1, GetCount("a")); 
    Assert.Equal(0, GetCount("ab")); 
    Assert.Equal(0, GetCount("abc")); 
    ks.OnTextInput('b');
    Assert.Equal(1, GetCount("a")); 
    Assert.Equal(1, GetCount("ab")); 
    Assert.Equal(0, GetCount("abc")); 
    ks.OnTextInput('a');
    Assert.Equal(2, GetCount("a")); 
    Assert.Equal(1, GetCount("ab")); 
    Assert.Equal(0, GetCount("abc")); 
  }
  
  [Fact]
  public void TestNewStart2() {
    SetupAbc();
    Assert.Equal(0, GetCount("a")); 
    Assert.Equal(0, GetCount("ab")); 
    Assert.Equal(0, GetCount("abc")); 
    ks.OnTextInput('a');
    Assert.Equal(1, GetCount("a")); 
    Assert.Equal(0, GetCount("ab")); 
    Assert.Equal(0, GetCount("abc")); 
    ks.OnTextInput('b');
    Assert.Equal(1, GetCount("a")); 
    Assert.Equal(1, GetCount("ab")); 
    Assert.Equal(0, GetCount("abc")); 
    ks.OnTextInput('c');
    Assert.Equal(1, GetCount("a")); 
    Assert.Equal(1, GetCount("ab")); 
    Assert.Equal(1, GetCount("abc")); 
    
    ks.OnTextInput('a');
    Assert.Equal(2, GetCount("a")); 
    Assert.Equal(1, GetCount("ab")); 
    Assert.Equal(1, GetCount("abc")); 
    
    ks.OnTextInput('a');
    Assert.Equal(3, GetCount("a")); 
    Assert.Equal(1, GetCount("ab")); 
    Assert.Equal(1, GetCount("abc")); 
  }
}
