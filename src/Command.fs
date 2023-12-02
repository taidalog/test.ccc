namespace Ccc

open System
open System.Text.RegularExpressions

module Command =
    //    [<StructuredFormatDisplay("{DisplayText}")>]
    //    type Command =
    //        | Down of TimeSpan
    //        | Up of TimeSpan
    //        | Repeat of int
    //        | RepeatTillStopped
    //        | Dark
    //        | Invalid
    //
    //        override this.ToString() =
    //            match this with
    //            | Command.Down x -> sprintf "Countdown for %s" (string x)
    //            | Command.Up x -> sprintf "Countup for %s" (string x)
    //            | Command.Repeat x -> sprintf "Repeat %d time(s)" x
    //            | Command.RepeatTillStopped -> sprintf "Repeat till stopped"
    //            | Command.Dark -> "Dark mode"
    //            | Command.Invalid -> "An invalid input"
    //
    //        member this.DisplayText = this.ToString()
    //

    [<StructuredFormatDisplay("{DisplayText}")>]
    type Action =
        | CountDown of Time: TimeSpan * Color: string * Background: string * Message: string
        | CountUp of Time: TimeSpan * Color: string * Background: string * Message: string
        | Invalid

        override this.ToString() =
            match this with
            | Action.CountDown(time, color, bgcolor, message) ->
                sprintf
                    "Countdown for %s, color: %s; background-color: %s; message: %s"
                    (string time)
                    color
                    bgcolor
                    message
            | Action.CountUp(time, color, bgcolor, message) ->
                sprintf
                    "Countup for %s, color: %s; background-color: %s; message: %s"
                    (string time)
                    color
                    bgcolor
                    message
            | Action.Invalid -> "An invalid input"

        member this.DisplayText = this.ToString()

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
    //
    //    let splitInput (input: string) : string array =
    //        Regex.Split(input, " (?=-{1,2}[A-Za-z0-9]+)")
    //
    //    let commandAndOption (input: string) = input.Split([| ' ' |], 2)
    //
    //    let duf (value: string) : TimeSpan =
    //        Regex.Match(value, "((\d{1,2}:){1,2}\d{1,2}|\d+)").Value |> toTimeSpan
    //
    //    let downf (value: string) : Command = value |> duf |> Command.Down
    //    let upf (value: string) : Command = value |> duf |> Command.Up
    //    let intf (value: string) : int = Regex.Match(value, "\d+").Value |> int
    //    let repeatf (value: string) : Command = value |> intf |> Command.Repeat
    //
    //    let c (value: string) : Command =
    //        match value with
    //        | x when Regex.IsMatch(x, "^(--down|-d) (\d{1,2}:){0,2}\d{1,2}") -> downf x
    //        | x when Regex.IsMatch(x, "^(--up|-u) (\d{1,2}:){0,2}\d{1,2}") -> upf x
    //        | x when Regex.IsMatch(x, "^(--repeat|-r \d+)") -> repeatf x
    //        | x when Regex.IsMatch(x, "^(--repeat|-r)$") -> Command.RepeatTillStopped
    //        | x when Regex.IsMatch(x, "^--dark") -> Command.Dark
    //        | _ -> Command.Invalid

    let splitInput' (input: string) : string array =
        Regex.Split(input, "(?=down|up)")
        |> Array.map (fun x -> x.Trim())
        |> Array.filter (String.IsNullOrWhiteSpace >> not)

    let splitCommand (input: string) : string array =
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
                "#333333"

        let bgcolor =
            let pattern = "(--background|-bg) (#[0-9A-Fa-f]{6}|#[0-9A-Fa-f]{3})"

            if Regex.IsMatch(input, pattern) then
                Regex.Match(input, pattern).Groups.[2].Value
            else
                "#ffffff"

        let message =
            let pattern = "(--message|-m)"

            if Regex.IsMatch(input, pattern) then
                input
                |> splitCommand
                |> Array.filter (fun x -> Regex.IsMatch(x, pattern))
                |> Array.last
                |> fun x -> Regex.Match(x, ".*(--message|-m) (.+)").Groups.[2].Value
            else
                ""

        time, color, bgcolor, message

    let parse (input: string) : Action =
        match input with
        | x when Regex.IsMatch(x, "^down") -> x |> parseInput |> Action.CountDown
        | x when Regex.IsMatch(x, "^up") -> x |> parseInput |> Action.CountUp
        | _ -> Action.Invalid
