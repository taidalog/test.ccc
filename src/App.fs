namespace Ccc

open Browser.Dom
open Browser.Types
open Fable.Core
open Fable.Core.JsInterop
open Command
open Timer'

module App =
    let keyboardshortcut (e: KeyboardEvent) =
        let helpWindow = document.getElementById "helpWindow"

        let isHelpWindowActive =
            helpWindow.classList
            |> (fun x -> JS.Constructors.Array?from(x))
            |> Array.contains "active"

        match document.activeElement.id with
        | "commandInput" ->
            match e.key with
            | "Escape" -> (document.getElementById "commandInput").blur ()
            | _ -> ()
        | _ ->
            match e.key with
            | "Escape" ->
                if isHelpWindowActive then
                    helpWindow.classList.remove "active"
            | "?" ->
                if not isHelpWindowActive then
                    helpWindow.classList.add "active"
            | "\\" ->
                (document.getElementById "commandInput").focus ()
                e.preventDefault ()
            | _ -> ()

    window.addEventListener (
        "DOMContentLoaded",
        (fun _ ->
            let commandInput = document.getElementById "commandInput" :?> HTMLInputElement

            (document.getElementById "inputArea").onsubmit <-
                fun _ ->
                    commandInput.blur ()

                    commandInput.value
                    |> splitInput'
                    |> Array.map (parse >> string)
                    |> Array.iter (printfn "%s")

                    commandInput.value |> splitInput' |> Array.map parse |> Array.toList |> start
                    false

            [ "helpButton"; "helpClose" ]
            |> List.iter (fun x ->
                (document.getElementById x :?> HTMLButtonElement).onclick <-
                    fun _ -> (document.getElementById "helpWindow").classList.toggle "active" |> ignore)

            document.onkeydown <- fun (e: KeyboardEvent) -> keyboardshortcut e)
    )
