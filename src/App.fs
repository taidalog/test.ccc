namespace Ccc

open Fable.Core
open Fable.Core.JsInterop
open Browser.Dom
open Browser.Types
open Command

module App =
    let keyboardshortcut (e: KeyboardEvent) =
        match document.activeElement.id with
        | "commandInput" ->
            match e.key with
            | "Escape" -> (document.getElementById "commandInput").blur ()
            | _ -> ()
        | _ ->
            match e.key with
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
                    printfn "%s" commandInput.value

                    commandInput.value
                    |> splitInput'
                    |> Array.map (parse >> string)
                    |> Array.iter (printfn "%s")

                    commandInput.value
                    |> splitInput'
                    |> Array.map (parse >> string)
                    |> String.concat "\n"
                    |> fun x -> (document.getElementById "outputArea").innerText <- x

                    false

            document.onkeydown <- fun (e: KeyboardEvent) -> keyboardshortcut e

        )
    )
