module Test

let container = Browser.Dom.document.getElementById("app")
Fable.React.ReactDomBindings.ReactDom.render(ViewTree.root, container)
