using System.Collections;
using GameObjectExtension;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests {

    public class CanvasResizerTest {

        private GameObject go;

        [Test]
        public void ResizeTest() {
            Assert.IsTrue(this.go.HasComponent<CanvasResizer>(), "CanvasResizer がちゃんと AddComponent されている");
        }

        [SetUp]
        public void Setup() {
            this.go = new GameObject();
            this.go.AddComponent<Canvas>();
            this.go.AddComponent<CanvasScaler>();
            this.go.AddComponent<CanvasResizer>();
        }

    }

}
