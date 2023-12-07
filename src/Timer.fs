// ccc Version 0.2.0
// https://github.com/taidalog/ccc
// Copyright (c) 2023 taidalog
// This software is licensed under the MIT License.
// https://github.com/taidalog/ccc/blob/main/LICENSE
namespace Ccc

open System
open Browser.Dom
open Browser.Types
open Fable.Core
open Command
open Fermata

module Timer' =
    type RunningStatus =
        | NotStarted = 0
        | Running = 1
        | Stopping = 2
        | Finished = 4

    type TimeAcc = { StartTime: DateTime; Acc: TimeSpan }

    type State =
        { Stop: TimeAcc
          IntervalId: int
          Commands: Command list
          RunningStatus: RunningStatus }

    [<Emit("setInterval($0, $1)")>]
    let setInterval (callback: unit -> unit) (interval: int) : int = jsNative

    [<Emit("clearInterval($0)")>]
    let clearInterval (intervalID: int) : unit = jsNative

    let mutable state =
        { Stop =
            { StartTime = DateTime.MinValue
              Acc = TimeSpan.Zero }
          IntervalId = -1
          Commands = []
          RunningStatus = RunningStatus.NotStarted }

    let initState =
        { Stop =
            { StartTime = DateTime.MinValue
              Acc = TimeSpan.Zero }
          IntervalId = -1
          Commands = []
          RunningStatus = RunningStatus.NotStarted }

    let timeSpanToDisplay (timeSpan: TimeSpan) =
        let h = timeSpan.Hours |> string |> String.padLeft 2 '0'
        let m = timeSpan.Minutes |> string |> String.padLeft 2 '0'
        let s = timeSpan.Seconds |> string |> String.padLeft 2 '0'
        let ms = timeSpan.Milliseconds |> string |> String.padLeft 3 '0'
        $"""%s{h}:%s{m}:%s{s}<span class="decimals">.%s{ms}</span>"""

    let rec start () : unit =
        match state.RunningStatus with
        | RunningStatus.NotStarted
        | RunningStatus.Finished ->
            document.getElementById("timerArea").classList.remove "finished"
            document.getElementById("messageArea").classList.remove "finished"

            state <-
                { initState with
                    Stop =
                        { initState.Stop with
                            StartTime = DateTime.Now
                            Acc = TimeSpan.Zero }
                    Commands =
                        (document.getElementById "commandInput" :?> HTMLInputElement).value
                        |> splitInput'
                        |> Array.map parse
                        |> Array.toList
                    RunningStatus = RunningStatus.Running }

            start ()
        | RunningStatus.Running ->
            match state.Commands with
            | [] ->
                document.getElementById("timerArea").classList.add "finished"
                document.getElementById("messageArea").classList.add "finished"

                state <-
                    { initState with
                        RunningStatus = RunningStatus.Finished }
            | h :: t ->
                document.getElementById("timerArea").classList.remove "finished"
                document.getElementById("messageArea").classList.remove "finished"

                state <-
                    { initState with
                        Stop =
                            { initState.Stop with
                                StartTime = DateTime.Now
                                Acc = TimeSpan.Zero }
                        RunningStatus = RunningStatus.Running }

                match h with
                | Command.Invalid ->
                    state <- { state with Commands = t }
                    start ()
                | Command.CountDown(time, color, bgcolor, message) ->
                    document.body.setAttribute ("style", (sprintf "color: %s; background-color: %s;" color bgcolor))
                    (document.getElementById "messageArea").innerText <- message

                    let intervalId =
                        setInterval
                            (fun _ ->
                                let elapsedTime = time - (DateTime.Now - state.Stop.StartTime + state.Stop.Acc)

                                if elapsedTime >= TimeSpan.Zero then
                                    document.getElementById("timerArea").innerHTML <- timeSpanToDisplay elapsedTime
                                else
                                    document.getElementById("timerArea").innerHTML <- timeSpanToDisplay TimeSpan.Zero
                                    clearInterval state.IntervalId
                                    state <- { state with Commands = t }
                                    start ())
                            10

                    state <- { state with IntervalId = intervalId }
                | Command.CountUp(time, color, bgcolor, message) ->
                    document.body.setAttribute ("style", (sprintf "color: %s; background-color: %s;" color bgcolor))
                    (document.getElementById "messageArea").innerText <- message

                    let intervalId =
                        setInterval
                            (fun _ ->
                                let elapsedTime = DateTime.Now - state.Stop.StartTime + state.Stop.Acc

                                if elapsedTime <= time then
                                    document.getElementById("timerArea").innerHTML <- timeSpanToDisplay elapsedTime
                                else
                                    document.getElementById("timerArea").innerHTML <- timeSpanToDisplay time
                                    clearInterval state.IntervalId
                                    state <- { state with Commands = t }
                                    start ())
                            10

                    state <- { state with IntervalId = intervalId }
        | _ -> ()
