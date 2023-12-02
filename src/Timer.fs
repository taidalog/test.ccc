namespace Ccc

open System
open Browser.Dom
open Browser.Types
open Fable.Core
open Fable.Core.JsInterop
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
          RunningStatus = RunningStatus.NotStarted }

    let initState =
        { Stop =
            { StartTime = DateTime.MinValue
              Acc = TimeSpan.Zero }
          IntervalId = -1
          RunningStatus = RunningStatus.NotStarted }

    let timeSpanToDisplay (timeSpan: TimeSpan) =
        let h = timeSpan.Hours |> string |> String.padLeft 2 '0'
        let m = timeSpan.Minutes |> string |> String.padLeft 2 '0'
        let s = timeSpan.Seconds |> string |> String.padLeft 2 '0'
        let ms = timeSpan.Milliseconds |> string |> String.padLeft 3 '0'
        $"%s{h}:%s{m}:%s{s}.%s{ms}"

    let countDown (for': TimeSpan) (till: TimeSpan) : unit =
        let intervalId =
            setInterval
                (fun _ ->
                    let elapsedTime = for' - (DateTime.Now - state.Stop.StartTime + state.Stop.Acc)

                    if elapsedTime < till then
                        document.getElementById("timerArea").innerText <- timeSpanToDisplay TimeSpan.Zero
                        clearInterval state.IntervalId
                    else
                        document.getElementById("timerArea").innerText <- timeSpanToDisplay elapsedTime)
                10

        state <- { state with IntervalId = intervalId }

    let countUp (from': TimeSpan) (till: TimeSpan) : unit =
        let intervalId =
            setInterval
                (fun _ ->
                    let elapsedTime = DateTime.Now - state.Stop.StartTime + state.Stop.Acc

                    if elapsedTime > till then
                        document.getElementById("timerArea").innerText <- timeSpanToDisplay till
                        clearInterval state.IntervalId
                    else
                        document.getElementById("timerArea").innerText <- timeSpanToDisplay elapsedTime)
                10

        state <- { state with IntervalId = intervalId }

    let invoke (action: Action) =
        match action with
        | Action.CountDown(time, color, bgcolor, message) ->
            let now = DateTime.Now

            state <-
                { initState with
                    Stop =
                        { initState.Stop with
                            StartTime = now
                            Acc = TimeSpan.MinValue }
                    RunningStatus = RunningStatus.Running }

            document.body.setAttribute ("style", (sprintf "color: %s; background-color: %s;" color bgcolor))
            countDown time TimeSpan.Zero
            (document.getElementById "messageArea").innerText <- message
        | Action.CountUp(time, color, bgcolor, message) ->
            let now = DateTime.Now

            state <-
                { initState with
                    Stop =
                        { initState.Stop with
                            StartTime = now
                            Acc = TimeSpan.MinValue }
                    RunningStatus = RunningStatus.Running }

            document.body.setAttribute ("style", (sprintf "color: %s; background-color: %s;" color bgcolor))
            countUp TimeSpan.Zero time
            (document.getElementById "messageArea").innerText <- message
        | Action.Invalid -> ()
