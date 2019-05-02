# Fable.MaterialUI.MaterialDesignIcons

This package provides Fable bindings for the `<SvgIcon>` React components in `mdi-material-ui`. You can use it with [fable-material-ui](https://github.com/mvsmal/fable-material-ui) (>= 3.0.0 due to module/namespace clashes) or on its own.

For more icons, see [Fable.MaterialUI.Icons](https://github.com/cmeeren/Fable.MaterialUI.Icons).

## Installation

* Install the `mdi-material-ui` npm package and its peer dependency `@material-ui/core`:
  * using npm: `npm install mdi-material-ui @material-ui/core`
  * using yarn: `yarn add mdi-material-ui @material-ui/core`

* Install the bindings:
  * using dotnet: `dotnet add package Fable.MaterialUI.MaterialDesignIcons`
  * using paket: `paket add Fable.MaterialUI.MaterialDesignIcons`

## Usage

```f#
open Fable.Helpers.React
open Fable.MaterialUI.MaterialDesignIcons

let view =
  div [ ] [
    homeIcon [ ]
  ]
```

For icon-specific properties, use [fable-material-ui](https://github.com/mvsmal/fable-material-ui) and see its [SvgIcon documentation](https://mvsmal.github.io/fable-material-ui/#/api/svg-icon).

## Missing new icons?

If the bindings are outdated, please file an issue and I'll update them. It's quick and simple (they're auto-generated), but I don't have the capacity to manually check for changes in `mdi-material-ui`.

## Contributing/updating

1. Run `yarn upgrade --latest`
2. Run `./fake.cmd build -t BuildTest`
3. Check the HTML file in the `output` folder to verify that all icons render correctly
4. Update the version number and release notes in `src/Fable.MaterialUI.MaterialDesignIcons/Fable.MaterialUI.MaterialDesignIcons.fsproj`
5. Commit, tag (to trigger release from AppVeyor), and push
