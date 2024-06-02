// ccc Version 0.5.0
// https://github.com/taidalog/ccc
// Copyright (c) 2023-2024 taidalog
// This software is licensed under the MIT License.
// https://github.com/taidalog/ccc/blob/main/LICENSE
namespace Ccc

open System
open System.Text.RegularExpressions

module CommandM =
    [<StructuredFormatDisplay("{DisplayText}")>]
    type CommandM =
        | CountDown of Time: TimeSpan * Color: string * Background: string * Message: string
        | CountUp of Time: TimeSpan * Color: string * Background: string * Message: string
        | Invalid

        override this.ToString() =
            match this with
            | CommandM.CountDown(time, color, bgcolor, message) ->
                sprintf
                    "Countdown for %s, color: %s; background-color: %s; message: %s"
                    (string time)
                    color
                    bgcolor
                    message
            | CommandM.CountUp(time, color, bgcolor, message) ->
                sprintf
                    "Countup for %s, color: %s; background-color: %s; message: %s"
                    (string time)
                    color
                    bgcolor
                    message
            | CommandM.Invalid -> "An invalid input"

        member this.DisplayText = this.ToString()

    let time (commandm: CommandM) : TimeSpan =
        match commandm with
        | CommandM.CountDown(time, _, _, _) -> time
        | CommandM.CountUp(time, _, _, _) -> time

    let f (value: string) : string * string * string =
        value.Split([| ':' |])
        |> Array.toList
        |> function
            | [ h; m; s ] -> h, m, s
            | [ m; s ] -> "", m, s
            | [ s ] -> "", "", s

    let g (h, m, s) : int * int * int =
        let f x =
            match x with
            | "" -> 0
            | _ -> int x

        f h, f m, f s

    let toTimeSpan: string -> TimeSpan = f >> g >> TimeSpan

    let splitInput' (input: string) : string array =
        Regex.Split(input, "(?=down|up)")
        |> Array.map (fun x -> x.Trim())
        |> Array.filter (String.IsNullOrWhiteSpace >> not)

    let splitCommandM (input: string) : string array =
        Regex.Split(input, "(?= -{1,2}[A-Za-z][0-9A-Za-z]* ?)")
        |> Array.map (fun x -> x.Trim())
        |> Array.filter (String.IsNullOrWhiteSpace >> not)

    let parseInput input =
        let time =
            Regex.Match(input, "^(down|up) ((\d{1,2}:){0,2}\d+)").Groups.[2].Value
            |> toTimeSpan

        let color =
            let pattern = "(--color|-c) (#[0-9A-Fa-f]{6}|#[0-9A-Fa-f]{3})"

            if Regex.IsMatch(input, pattern) then
                Regex.Match(input, pattern).Groups.[2].Value
            else
                ""

        let bgcolor =
            let pattern = "(--background|-bg) (#[0-9A-Fa-f]{6}|#[0-9A-Fa-f]{3})"

            if Regex.IsMatch(input, pattern) then
                Regex.Match(input, pattern).Groups.[2].Value
            else
                ""

        let message =
            let pattern = "(--message|-m)"

            if Regex.IsMatch(input, pattern) then
                input
                |> splitCommandM
                |> Array.filter (fun x -> Regex.IsMatch(x, pattern))
                |> Array.last
                |> fun x -> Regex.Match(x, ".*(--message|-m) (.+)").Groups.[2].Value
            else
                ""

        time, color, bgcolor, message

    let parse (input: string) : CommandM =
        match input with
        | x when Regex.IsMatch(x, "^down") -> x |> parseInput |> CommandM.CountDown
        | x when Regex.IsMatch(x, "^up") -> x |> parseInput |> CommandM.CountUp
        | _ -> CommandM.Invalid

module Command =
    [<StructuredFormatDisplay("{DisplayText}")>]
    type Command =
        | CountDown of Duration: TimeSpan * Delay: TimeSpan * Color: string * Background: string * Message: string
        | CountUp of Duration: TimeSpan * Delay: TimeSpan * Color: string * Background: string * Message: string
        | Invalid

        override this.ToString() =
            match this with
            | Command.CountDown(duration, delay, color, bgcolor, message) ->
                sprintf
                    "Countdown for %s, delayed %s, color: %s; background-color: %s; message: %s"
                    (string duration)
                    (string delay)
                    color
                    bgcolor
                    message
            | Command.CountUp(duration, delay, color, bgcolor, message) ->
                sprintf
                    "Countup for %s, delayed %s, color: %s; background-color: %s; message: %s"
                    (string duration)
                    (string delay)
                    color
                    bgcolor
                    message
            | Command.Invalid -> "An invalid input"

        member this.DisplayText = this.ToString()

    let ofCommands commands =
        let delays =
            commands
            |> List.map CommandM.time
            |> List.scan (+) TimeSpan.Zero
            |> (List.rev >> List.tail >> List.rev)

        (commands, delays)
        ||> List.map2 (fun x y ->
            match x with
            | CommandM.CountDown(time, color, background, message) ->
                Command.CountDown(time, y, color, background, message)
            | CommandM.CountUp(time, color, background, message) ->
                Command.CountUp(time, y, color, background, message))

    let ofString input =
        input
        |> CommandM.splitInput'
        |> Array.map CommandM.parse
        |> Array.toList
        |> ofCommands

    let duration command =
        match command with
        | Command.CountDown(duration, _, _, _, _) -> duration
        | Command.CountUp(duration, _, _, _, _) -> duration

    let delay command =
        match command with
        | Command.CountDown(_, delay, _, _, _) -> delay
        | Command.CountUp(_, delay, _, _, _) -> delay

module Command2 =
    type DownCommand =
        { Duration: TimeSpan
          Delay: TimeSpan
          Color: string
          Background: string
          Message: string }

    type UpCommand =
        { Duration: TimeSpan
          Delay: TimeSpan
          Color: string
          Background: string
          Message: string }

    type Command2 =
        | Down of DownCommand
        | Up of UpCommand

    let duration (x: Command2) : TimeSpan =
        match x with
        | Command2.Down v -> v.Duration
        | Command2.Up v -> v.Duration

    let delay (x: Command2) : TimeSpan option =
        match x with
        | Command2.Down v -> Some(v.Delay)
        | Command2.Up v -> Some(v.Delay)

    let color (x: Command2) : string option =
        match x with
        | Command2.Down v -> Some(v.Color)
        | Command2.Up v -> Some(v.Color)

    let background (x: Command2) : string option =
        match x with
        | Command2.Down v -> Some(v.Background)
        | Command2.Up v -> Some(v.Background)

    let message (x: Command2) : string option =
        match x with
        | Command2.Down v -> Some(v.Message)
        | Command2.Up v -> Some(v.Message)

    let defaultDown: DownCommand =
        { Duration = TimeSpan.Zero
          Delay = TimeSpan.Zero
          Color = ""
          Background = ""
          Message = "" }

    let defaultUp: UpCommand =
        { Duration = TimeSpan.Zero
          Delay = TimeSpan.Zero
          Color = ""
          Background = ""
          Message = "" }

    let update (command: Command2) (options: Parsing.Options) : Command2 =
        match command with
        | Command2.Down d ->
            match options with
            | Parsing.Options.Color v -> { d with Color = v }
            | Parsing.Options.Background v -> { d with Background = v }
            | Parsing.Options.Message v -> { d with Message = v }
            |> Command2.Down
        | Command2.Up u ->
            match options with
            | Parsing.Options.Color v -> { u with Color = v }
            | Parsing.Options.Background v -> { u with Background = v }
            | Parsing.Options.Message v -> { u with Message = v }
            |> Command2.Up

    let build' (x: Parsing.CommandAndOptions) : Command2 =
        match x with
        | Parsing.CommandAndOptions.Down(t, vs) -> List.fold update (Command2.Down { defaultDown with Duration = t }) vs
        | Parsing.CommandAndOptions.Up(t, vs) -> List.fold update (Command2.Up { defaultUp with Duration = t }) vs

    let withDelay (xs: Command2 list) =
        let delays =
            xs
            |> List.map duration
            |> List.scan (+) TimeSpan.Zero
            |> (List.rev >> List.tail >> List.rev)

        (xs, delays)
        ||> List.map2 (fun x y ->
            match x with
            | Command2.Down v -> (Command2.Down { v with Delay = y })
            | Command2.Up v -> (Command2.Up { v with Delay = y }))
