using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests {

    public class CanvasResizerTest {

        [UnityTest]
        public void ResizeTest() {
            GameObject go = new GameObject();
            go.AddComponent<Canvas>();
            CanvasScaler canvasScaler = go.GetComponent<CanvasScaler>();

            go.AddComponent<CanvasResizer>();
        }

    }

}
