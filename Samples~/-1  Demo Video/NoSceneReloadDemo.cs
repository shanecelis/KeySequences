using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoSceneReloadDemo : MonoBehaviour {
  private static string myPrivateInt;
    // Start is called before the first frame update
    void Start()
    {
      Debug.Log($"myPrivateInt {myPrivateInt}");
      myPrivateInt += "a";
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
