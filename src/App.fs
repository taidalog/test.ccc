// ccc Version 0.2.1
// https://github.com/taidalog/ccc
// Copyright (c) 2023 taidalog
// This software is licensed under the MIT License.
// https://github.com/taidalog/ccc/blob/main/LICENSE
namespace Ccc

open Browser.Dom
open Browser.Types
open Fable.Core
open Fable.Core.JsInterop
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
            let helpWindow = document.getElementById "helpWindow"

            let isHelpWindowActive =
                helpWindow.classList
                |> (fun x -> JS.Constructors.Array?from(x))
                |> Array.contains "active"

            let informationPolicyWindow = document.getElementById "informationPolicyWindow"

            let isInformationPolicyWindowActive =
                informationPolicyWindow.classList
                |> (fun x -> JS.Constructors.Array?from(x))
                |> Array.contains "active"

            match e.key with
            | "Escape" ->
                if isHelpWindowActive then
                    helpWindow.classList.remove "active"

                if isInformationPolicyWindowActive then
                    informationPolicyWindow.classList.remove "active"

                if not isHelpWindowActive && not isInformationPolicyWindowActive then
                    stop ()
            | "Enter" -> start ()
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
            // timer feature
            let commandInput = document.getElementById "commandInput" :?> HTMLInputElement

            (document.getElementById "inputArea").onsubmit <-
                fun _ ->
                    commandInput.blur ()

                    commandInput.value
                    |> Command.ofString
                    |> List.map string
                    |> List.iter (printfn "%s")

                    //commandInput.value |> splitInput' |> Array.map parse |> Array.toList |> start
                    start ()
                    false

            // help window
            [ "helpButton"; "helpClose" ]
            |> List.iter (fun x ->
                (document.getElementById x :?> HTMLButtonElement).onclick <-
                    fun _ -> (document.getElementById "helpWindow").classList.toggle "active")

            // information policy window
            (document.getElementById "informationPolicyLink").onclick <-
                fun event ->
                    event.preventDefault ()
                    (document.getElementById "informationPolicyWindow").classList.add "active"

            (document.getElementById "informationPolicyClose").onclick <-
                fun _ -> (document.getElementById "informationPolicyWindow").classList.remove "active"

            // keyboard shortcut
            document.onkeydown <- fun (e: KeyboardEvent) -> keyboardshortcut e)
    )
