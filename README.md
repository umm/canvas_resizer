# Canvas Resizer

## What

* Automatic resize [Canvas](https://docs.unity3d.com/ScriptReference/Canvas.html) by screen resolution

## Installation

```shell
yarn add "umm-projects/canvas_resizer#^1.0.0"
```

## Usage

* Attach `CanvasResizer` component into Gameobject what attached [Canvas](https://docs.unity3d.com/ScriptReference/Canvas.html) and [CanvasScaler](https://docs.unity3d.com/ScriptReference/CanvasScaler.html).

## Parameters

| Name | Type | Description |
| --- | --- | --- |
| Standard Resolution | Vector2 | Resolution for determining whether to fit to vertically or horizontally. |
| Minimum Resolution | Vector2 | Minimum resolution ratio in fitted direction. |
| Maximum Resolution | Vector2 | Maximum resolution ratio in fitted direction. |

## License

Copyright (c) 2017 Tetsuya Mori

Released under the MIT license, see [LICENSE.txt](LICENSE.txt)

