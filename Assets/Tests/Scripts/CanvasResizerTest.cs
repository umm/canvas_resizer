using GameObjectExtension;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Tests {

    public class CanvasResizerTest {

        private GameObject go;

        [Test]
        public void ResizeTest() {
            // FIXME: これは、必ずどこかで転ける。 Screen.SetResolution ではエディタの GameView の解像度を弄れないため。

            Screen.SetResolution(1136, 640, false);
            this.go.GetComponent<CanvasResizer>().Resize();
            Assert.IsTrue(
                Mathf.Approximately(this.go.GetComponent<CanvasScaler>().matchWidthOrHeight, 0.0f),
                "iPhone 7"
            );

            Screen.SetResolution(2048, 1536, false);
            this.go.GetComponent<CanvasResizer>().Resize();
            Assert.IsTrue(
                Mathf.Approximately(this.go.GetComponent<CanvasScaler>().matchWidthOrHeight, 1.0f),
                "iPad"
            );

            Screen.SetResolution(1024, 640, false);
            this.go.GetComponent<CanvasResizer>().Resize();
            Assert.IsTrue(
                Mathf.Approximately(this.go.GetComponent<CanvasScaler>().matchWidthOrHeight, 0.0f),
                "Android"
            );
        }

        [SetUp]
        public void Setup() {
            this.go = new GameObject();
            this.go.AddComponent<Canvas>();
            this.go.AddComponent<CanvasScaler>(
                (x) => {
                    x.referenceResolution = new Vector2(2272.0f, 1536.0f);
                }
            );
            this.go.AddComponent<CanvasResizer>();
        }

    }

}
