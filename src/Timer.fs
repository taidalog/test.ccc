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

    type Notes =
        { Finished: (int * string) list
          NotFinished: (int * string) list }

    type State =
        { Stop: TimeAcc
          Next: TimeAcc
          IntervalId: int
          Notes: Notes
          RunningStatus: RunningStatus }

    [<Emit("setInterval($0, $1)")>]
    let setInterval (callback: unit -> unit) (interval: int) : int = jsNative

    [<Emit("clearInterval($0)")>]
    let clearInterval (intervalID: int) : unit = jsNative

    let mutable state =
        { Stop =
            { StartTime = DateTime.MinValue
              Acc = TimeSpan.Zero }
          Next =
            { StartTime = DateTime.MinValue
              Acc = TimeSpan.Zero }
          IntervalId = -1
          Notes = { Finished = []; NotFinished = [] }
          RunningStatus = RunningStatus.NotStarted }

    let timeSpanToDisplay (timeSpan: TimeSpan) =
        let h = timeSpan.Hours |> string |> String.padLeft 2 '0'
        let m = timeSpan.Minutes |> string |> String.padLeft 2 '0'
        let s = timeSpan.Seconds |> string |> String.padLeft 2 '0'
        let ms = timeSpan.Milliseconds |> string |> String.padLeft 3 '0'
        $"%s{h}:%s{m}:%s{s}.%s{ms}"

    let countUp () =
        let intervalId =
            setInterval
                (fun _ ->
                    let elapsedTime = DateTime.Now - state.Stop.StartTime + state.Stop.Acc
                    document.getElementById("timer").innerText <- timeSpanToDisplay elapsedTime)
                10

        state <- { state with IntervalId = intervalId }

    let invoke (action: Action) =
        match action with
        | Action.CountDown(time, color, bgcolor, message) ->
            document.body.setAttribute ("style", (sprintf "color: %s; background-color: %s;" color bgcolor))
            (document.getElementById "timerArea").innerText <- timeSpanToDisplay time
            (document.getElementById "messageArea").innerText <- message
        | Action.CountUp(time, color, bgcolor, message) ->
            document.body.setAttribute ("style", (sprintf "color: %s; background-color: %s;" color bgcolor))
            (document.getElementById "timerArea").innerText <- timeSpanToDisplay time
            (document.getElementById "messageArea").innerText <- message
        | Action.Invalid -> ()
