# Canvas Resizer

## What

* Automatic resize [Canvas](https://docs.unity3d.com/ScriptReference/Canvas.html) by screen resolution

## Installation

### With Unity Package Manager

```bash
upm add package dev.upm-packages.canvas-resizer
```

Note: `upm` command is provided by [this repository](https://github.com/upm-packages/upm-cli).

You can also edit `Packages/manifest.json` directly.

```jsonc
{
  "dependencies": {
    // (snip)
    "dev.upm-packages.canvas-resizer": "[latest version]",
    // (snip)
  },
  "scopedRegistries": [
    {
      "name": "Unofficial Unity Package Manager Registry",
      "url": "https://upm-packages.dev",
      "scopes": [
        "dev.upm-packages"
      ]
    }
  ]
}
```

### Any other else (classical umm style)

```shell
yarn add "umm/canvas_resizer#^1.0.0"
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
