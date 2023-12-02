namespace Ccc

open Browser.Dom
open Browser.Types
open Command
open Timer'

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
                    commandInput.value
                    |> splitInput'
                    |> Array.map (parse >> string)
                    |> Array.iter (printfn "%s")

                    commandInput.value |> splitInput' |> Array.map parse |> Array.head |> invoke
                    false

            document.onkeydown <- fun (e: KeyboardEvent) -> keyboardshortcut e)
    )
