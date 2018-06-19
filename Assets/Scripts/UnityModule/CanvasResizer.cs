using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UnityModule
{
    /// <inheritdoc />
    /// <summary>
    /// スクリーンサイズに応じて Canvas のリサイズを行う
    /// </summary>
    [RequireComponent(typeof(CanvasScaler))]
    [ExecuteInEditMode]
    [PublicAPI]
    public class CanvasResizer : ObservableLifecycleMonoBehaviour
    {
        /// <summary>
        /// 解像度計算時のバッファ
        /// </summary>
        /// <remarks>どちらかの辺が最小解像度ギリギリの場合を考慮してバッファを設定します</remarks>
        private const float CalculateResolutionBuffer = 10.0f;

        /// <summary>
        /// デフォルト標準解像度
        /// </summary>
        private static readonly Vector2 DefaultStandardResolution = new Vector2(2272.0f, 1536.0f);

        /// <summary>
        /// デフォルト最小解像度
        /// </summary>
        private static readonly Vector2 DefaultMinimumResolution = new Vector2(2048.0f, 1278.0f);

        /// <summary>
        /// デフォルト最大解像度
        /// </summary>
        private static readonly Vector2 DefaultMaximumResolution = new Vector2(2768.0f, 1536.0f);

        /// <summary>
        /// CanvasScaler の実体
        /// </summary>
        private CanvasScaler canvasScaler;

        /// <summary>
        /// CanvasScaler
        /// </summary>
        private CanvasScaler CanvasScaler
        {
            get
            {
                if (canvasScaler == default(CanvasScaler))
                {
                    canvasScaler = gameObject.GetComponent<CanvasScaler>();
                }

                return canvasScaler;
            }
        }

        /// <summary>
        /// 標準解像度の実体
        /// </summary>
        [SerializeField] private Vector2 standardResolution = DefaultStandardResolution;

        /// <summary>
        /// 標準解像度
        /// </summary>
        private Vector2 StandardResolution => standardResolution;

        /// <summary>
        /// 最小解像度の実体
        /// </summary>
        [SerializeField] private Vector2 minimumResolution = DefaultMinimumResolution;

        /// <summary>
        /// 最小解像度
        /// </summary>
        private Vector2 MinimumResolution => minimumResolution;

        /// <summary>
        /// 最大解像度の実体
        /// </summary>
        [SerializeField] private Vector2 maximumResolution = DefaultMaximumResolution;

        /// <summary>
        /// 最大解像度
        /// </summary>
        private Vector2 MaximumResolution => maximumResolution;

        /// <summary>
        /// リサイズする
        /// </summary>
        /// <remarks>内部的には CanvasScaler.matchWidthOrHeight を書き換えてるだけ</remarks>
        public void Resize()
        {
            CanvasScaler.matchWidthOrHeight = CalculateMatchWidthOrHeight();

            var screenSize = new Vector2(Screen.width, Screen.height);
            var referenceResolution = StandardResolution;
            // 標準解像度を基準に引き延ばした場合のサイズ
            var extendedResolution = new Vector2(
                StandardResolution.y * Screen.width / Screen.height,
                StandardResolution.x * Screen.height / Screen.width
            );
            float matchWidthOrHeight;
            if (screenSize.y / screenSize.x > StandardResolution.y / StandardResolution.x)
            {
                /* 参考解像度よりも大きい場合 */
                /* iPad や正方形の端末 (あるのか！？) など */
                if (extendedResolution.x + CalculateResolutionBuffer > MinimumResolution.x)
                {
                    /* 最小解像度XよりもXが大きい場合 */
                    /* iPad など */
                    // 縦併せにする
                    matchWidthOrHeight = 1.0f;
                }
                else
                {
                    /* 最小解像度XよりもXが小さくなる場合 */
                    /* (無いとは思うけど) 正方形端末や Landscape 状態で縦長になる端末など */
                    if (extendedResolution.y > MaximumResolution.y)
                    {
                        /* 最大解像度YよりもYが大きい場合 */
                        /* Landscape 状態で縦長になる端末など */
                        // 縦併せにする
                        matchWidthOrHeight = 1.0f;
                        // 縦を最大解像度Yに制限する
                        referenceResolution = new Vector2(MaximumResolution.x, MaximumResolution.y);
                    }
                    else
                    {
                        /* 最大解像度YよりもYが小さい場合 */
                        /* 正方形な端末など */
                        // 横併せにする
                        matchWidthOrHeight = 0.0f;
                        // 縦を計算した解像度まで引き延ばす
                        referenceResolution = new Vector2(referenceResolution.x, extendedResolution.y);
                    }
                }
            }
            else
            {
                /* 参考参考解像度よりも小さい場合 */
                /* iPhone 4s, iPhone 5, iPhone X, Android など */
                if (extendedResolution.y + CalculateResolutionBuffer > MinimumResolution.y)
                {
                    /* 最小参考解像度YよりもYが大きい場合 */
                    /* iPhone 4s, iPhone 5, Android など */
                    matchWidthOrHeight = 0.0f;
                }
                else
                {
                    /* 最小解像度YよりもYが小さくなる場合 */
                    /* iPhone X など */
                    if (extendedResolution.x > MaximumResolution.x)
                    {
                        /* 最大解像度XよりもXが大きい場合 */
                        /* 相当細長い端末 (1 : 3 とか) など */
                        // 横併せにする
                        matchWidthOrHeight = 0.0f;
                        // 横を最大解像度Xに制限する
                        referenceResolution = new Vector2(MaximumResolution.x, MaximumResolution.y);
                    }
                    else
                    {
                        /* 最大解像度XよりもXが小さい場合 */
                        /* iPhone X など */
                        // 縦併せにする
                        matchWidthOrHeight = 1.0f;
                        // 横を計算した解像度まで引き延ばす
                        referenceResolution = new Vector2(extendedResolution.x, referenceResolution.y);
                    }
                }
            }

            CanvasScaler.referenceResolution = referenceResolution;
            CanvasScaler.matchWidthOrHeight = matchWidthOrHeight;
        }

        /// <summary>
        /// Unity lifecycle: Start
        /// </summary>
        /// <remarks>
        /// (主に実機で) 初期化時にスクリーンサイズに応じた Canvas リサイズ処理を走らせる
        /// </remarks>
        protected override void OnStart()
        {
            base.OnStart();
            Resize();
        }

#if UNITY_EDITOR

        private Vector2Int CurrentScreenResolution { get; set; } = Vector2Int.zero;

        private void Update()
        {
            if (CurrentScreenResolution.x == Screen.width && CurrentScreenResolution.y == Screen.height)
            {
                return;
            }

            CurrentScreenResolution = new Vector2Int(Screen.width, Screen.height);
            Resize();
        }
#endif

        /// <summary>
        /// Width と Height のどちらに併せるべきかを計算する
        /// </summary>
        /// <returns>計算結果</returns>
        [SuppressMessage("ReSharper", "RedundantCast")]
        private float CalculateMatchWidthOrHeight()
        {
            if ((float) Screen.height / (float) Screen.width < CanvasScaler.referenceResolution.y / CanvasScaler.referenceResolution.x)
            {
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
        private static readonly List<Color> ColorList = new List<Color>()
        {
            Color.cyan,
            Color.magenta,
            Color.yellow,
            Color.green,
        };

        /// <summary>
        /// 画面比率のリスト
        /// </summary>
        private static readonly List<Vector2> SizeList = new List<Vector2>()
        {
            new Vector2(812.0f, 375.0f), // iPhone X 以降の iOS ハンドセット
            new Vector2(16.0f, 9.0f), // iPhone 5 以降の iOS ハンドセット
            new Vector2(16.0f, 10.0f), // 主要な Android ハンドセット/タブレット
            new Vector2(3.0f, 2.0f), // iPhone 4S までの iOS ハンドセット
            new Vector2(4.0f, 3.0f), // iPad などの iOS タブレット
        };

        /// <summary>
        /// Unity lifecycle: OnGUI
        /// </summary>
        /// <remarks>エディタ編集時にリサイズ処理を走らせる</remarks>
        private void OnGUI()
        {
            Resize();
        }

        /// <summary>
        /// ギズモを描画
        /// </summary>
        /// <remarks>
        /// 専用のエディタ拡張クラスとして [DrawGizmo()] 属性使って表現しようかとも思ったが、
        /// ちと役割違うなぁと思い直して、今の形に至っている。
        /// </remarks>
        /// <remarks>Unity の仕様上、ギズモは毎フレーム描画処理を行わないとダメなのだが、計算結果をキャッシュするくらいはしても良いかも。</remarks>
        private void OnDrawGizmos()
        {
            var originalColor = Gizmos.color;
            var canvasResizerTransform = gameObject.transform;
            for (var i = 0; i < SizeList.Count; i++)
            {
                Gizmos.color = ColorList[i % ColorList.Count];
                var size = SizeList[i];
                var extended = new Vector2(StandardResolution.y * size.x / size.y, StandardResolution.x * size.y / size.x);
                float unitSize;
                if (size.y / size.x > StandardResolution.y / StandardResolution.x)
                {
                    if (extended.x + CalculateResolutionBuffer > MinimumResolution.x)
                    {
                        unitSize = StandardResolution.y / size.y;
                    }
                    else
                    {
                        if (extended.y > MaximumResolution.y)
                        {
                            unitSize = MaximumResolution.y / size.y;
                        }
                        else
                        {
                            unitSize = StandardResolution.x / size.x;
                        }
                    }
                }
                else
                {
                    if (extended.y + CalculateResolutionBuffer > MinimumResolution.y)
                    {
                        unitSize = StandardResolution.x / size.x;
                    }
                    else
                    {
                        if (extended.x > MaximumResolution.x)
                        {
                            unitSize = MaximumResolution.x / size.x;
                        }
                        else
                        {
                            unitSize = StandardResolution.y / size.y;
                        }
                    }
                }

                Gizmos.DrawWireCube(
                    canvasResizerTransform.TransformPoint(canvasResizerTransform.localPosition),
                    new Vector3(size.x * unitSize * canvasResizerTransform.localScale.x, size.y * unitSize * canvasResizerTransform.localScale.y, 0.0f)
                );
            }

            Gizmos.color = originalColor;
        }

#endif
    }
}