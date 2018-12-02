module Test

open Fable.Import.Browser

let container = document.getElementById("app")
Fable.Import.ReactDom.render(ViewTree.root, container)
