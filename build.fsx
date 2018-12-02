#r "paket: groupref FakeBuild //"
#load ".fake/build.fsx/intellisense.fsx"

open System
open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.IO.FileSystemOperators
open Fake.JavaScript

Target.create "Clean" (fun _ ->
  !! "src/**/bin"
  ++ "src/**/obj"
  ++ "output"
  |> Shell.cleanDirs
)

Target.create "DotnetRestore" (fun _ ->
  DotNet.restore
    (DotNet.Options.withWorkingDirectory __SOURCE_DIRECTORY__)
    "Fable.MaterialUI.MaterialDesignIcons.sln"
)

Target.create "YarnInstall" (fun _ ->
  Yarn.install id
)

Target.create "Generate" (fun _ ->
  let firstLower (str: string) = 
    string (Char.ToLower str.[0]) + str.Substring 1
  let names =
    !! "node_modules/mdi-material-ui/*.d.ts"
    |> Seq.filter (fun path -> not <| path.EndsWith("index.d.ts"))
    |> Seq.map (fun path ->
        let fullName = FileInfo.ofPath(path).Name
        fullName.Substring(0, fullName.Length - 5)
    )
  let bindingStart = """
//--------------------------------------------//
// This file is auto-generated, see build.fsx //
//--------------------------------------------//

module Fable.MaterialUI.MaterialDesignIcons

open Fable.Core
open Fable.Core.JsInterop
open Fable.Helpers.React
open Fable.Import.React

"""
  let bindingLines =
    names |> Seq.collect (fun name ->
      [
        yield sprintf "let inline %sIcon b : ReactElement = " (firstLower name)
        yield sprintf "  ofImport \"default\" \"mdi-material-ui/%s\" (keyValueList CaseRules.LowerFirst b) []" name
        yield ""
      ]
    )
  File.replaceContent "src/Fable.MaterialUI.MaterialDesignIcons/Icons.fs" bindingStart
  File.write true "src/Fable.MaterialUI.MaterialDesignIcons/Icons.fs" bindingLines  

  let testStart = """
//--------------------------------------------//
// This file is auto-generated, see build.fsx //
//--------------------------------------------//

module ViewTree

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.MaterialUI.MaterialDesignIcons

let root =
  fragment [] ([|
"""
  let testLines =
    names |> Seq.map (fun name ->
      sprintf "    %sIcon [ Id \"%s\" ]" (firstLower name) name
    )
  File.replaceContent "src/Test/ViewTree.fs" testStart
  File.write true "src/Test/ViewTree.fs" testLines
  File.write true "src/Test/ViewTree.fs" ["  |] |> Array.toList)"]
)

Target.create "Build" (fun _ ->
  !! "src/Fable.MaterialUI.MaterialDesignIcons/Fable.MaterialUI.MaterialDesignIcons.fsproj"
  |> Seq.iter (DotNet.build (fun x ->
      { x with
          Configuration = DotNet.BuildConfiguration.Release
      }
  ))
)

Target.create "Pack" (fun _ ->
  "src/Fable.MaterialUI.MaterialDesignIcons/"
  |> DotNet.pack (fun x ->
      { x with
          Configuration = DotNet.BuildConfiguration.Release
      }
  )
)

Target.create "BuildTest" (fun _ ->
  Yarn.exec "build" id
)

Target.create "DevTest" (fun _ ->
  Yarn.exec "start" id
)

// Build order
"Clean"
  ==> "DotnetRestore"
  ==> "YarnInstall"
  ==> "Generate"
  ==> "Build"
  ==> "BuildTest"
  ==> "Pack"

"Build"
  ==> "DevTest"

// start build
Target.runOrDefault "Pack"
