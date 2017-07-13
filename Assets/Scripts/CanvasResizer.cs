using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スクリーンサイズに応じて Canvas のリサイズを行う
/// </summary>
[RequireComponent(typeof(CanvasScaler))]
public class CanvasResizer : MonoBehaviour {

    /// <summary>
    /// CanvasScaler の実体
    /// </summary>
    private CanvasScaler canvasScaler;

    /// <summary>
    /// CanvasScaler
    /// </summary>
    public CanvasScaler CanvasScaler {
        get {
            if (this.canvasScaler == default(CanvasScaler)) {
                this.canvasScaler = this.gameObject.GetComponent<CanvasScaler>();
            }
            return this.canvasScaler;
        }
        set {
            this.canvasScaler = value;
        }
    }

    /// <summary>
    /// リサイズする
    /// </summary>
    /// <remarks>内部的には CanvasScaler.matchWidthOrHeight を書き換えてるだけ</remarks>
    public void Resize() {
        this.CanvasScaler.matchWidthOrHeight = this.CalculateMatchWidthOrHeight();
    }

    /// <summary>
    /// Unity lifecycle: Start
    /// </summary>
    /// <remarks>
    /// (主に実機で) 初期化時にスクリーンサイズに応じた Canvas リサイズ処理を走らせる
    /// </remarks>
    private void Start() {
        this.Resize();
    }

    /// <summary>
    /// Width と Height のどちらに併せるべきかを計算する
    /// </summary>
    /// <returns>計算結果</returns>
    [SuppressMessage("ReSharper", "RedundantCast")]
    private float CalculateMatchWidthOrHeight() {
        if ((float)Screen.height / (float)Screen.width < this.CanvasScaler.referenceResolution.y / this.CanvasScaler.referenceResolution.x) {
            return 0.0f;
        }
        return 1.0f;
    }

#if UNITY_EDITOR

    /// <summary>
    /// 描画するギズモの色リスト
    /// </summary>
    /// <remarks>
    /// この中身をサイクルする
    /// </remarks>
    private static readonly List<Color> COLOR_LIST = new List<Color>() {
        Color.cyan,
        Color.magenta,
        Color.yellow,
    };

    /// <summary>
    /// 画面比率のリスト
    /// </summary>
    private static readonly List<Vector2> SIZE_LIST = new List<Vector2>() {
        new Vector2(16.0f,  9.0f), // iPhone 5 以降の iOS ハンドセット
        new Vector2(16.0f, 10.0f), // 主要な Android ハンドセット/タブレット
        new Vector2( 3.0f,  2.0f), // iPhone 4S までの iOS ハンドセット
        new Vector2( 4.0f,  3.0f), // iPad などの iOS タブレット
    };

    /// <summary>
    /// ギズモを描画
    /// </summary>
    /// <remarks>
    /// 専用のエディタ拡張クラスとして [DrawGizmo()] 属性使って表現しようかとも思ったが、
    /// ちと役割違うなぁと思い直して、今の形に至っている。
    /// </remarks>
    private void OnDrawGizmos() {
        Color originalColor = Gizmos.color;
        Transform canvasResizerTransform = this.gameObject.transform;
        Vector2 referenceResolution = this.CanvasScaler.referenceResolution;
        for (int i = 0; i < SIZE_LIST.Count; i++) {
            Gizmos.color = COLOR_LIST[i % COLOR_LIST.Count];
            Vector2 size = SIZE_LIST[i];
            float unitSize = (
                size.y / size.x < referenceResolution.y / referenceResolution.x
                    ? referenceResolution.x / size.x
                    : referenceResolution.y / size.y
            );
            Gizmos.DrawWireCube(
                canvasResizerTransform.TransformPoint(canvasResizerTransform.localPosition),
                new Vector3(size.x * unitSize * canvasResizerTransform.localScale.x, size.y * unitSize * canvasResizerTransform.localScale.y, 0.0f)
            );
        }
        Gizmos.color = originalColor;
    }

#endif

}