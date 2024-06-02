// ccc Version 0.5.0
// https://github.com/taidalog/ccc
// Copyright (c) 2023-2024 taidalog
// This software is licensed under the MIT License.
// https://github.com/taidalog/ccc/blob/main/LICENSE
module Tests

open System
open Xunit
open Ccc
open Ccc.CommandM
open Ccc.Command2

[<Fact>]
let ``CommandM.f-1`` () =
    let expected = "1", "23", "45"
    let actual = f "1:23:45"
    Assert.Equal(expected, actual)

[<Fact>]
let ``CommandM.f-2`` () =
    let expected = "", "23", "45"
    let actual = f "23:45"
    Assert.Equal(expected, actual)

[<Fact>]
let ``CommandM.f-3`` () =
    let expected = "", "", "45"
    let actual = f "45"
    Assert.Equal(expected, actual)

[<Fact>]
let ``CommandM.g-1`` () =
    let expected = 1, 23, 45
    let actual = g ("1", "23", "45")
    Assert.Equal(expected, actual)

[<Fact>]
let ``CommandM.g-2`` () =
    let expected = 0, 23, 45
    let actual = g ("", "23", "45")
    Assert.Equal(expected, actual)

[<Fact>]
let ``CommandM.g-3`` () =
    let expected = 0, 0, 45
    let actual = g ("", "", "45")
    Assert.Equal(expected, actual)

[<Fact>]
let ``CommandM.toTimeSpan-1`` () =
    let expected = TimeSpan(1, 23, 45)
    let actual = toTimeSpan "1:23:45"
    Assert.Equal(expected, actual)

[<Fact>]
let ``CommandM.toTimeSpan-2`` () =
    let expected = TimeSpan(0, 23, 45)
    let actual = toTimeSpan "23:45"
    Assert.Equal(expected, actual)

[<Fact>]
let ``CommandM.toTimeSpan-3`` () =
    let expected = TimeSpan(0, 0, 45)
    let actual = toTimeSpan "45"
    Assert.Equal(expected, actual)

[<Fact>]
let ``CommandM.splitInput'-1`` () =
    let expected =
        [| "down 5:00"; "up 0:30 -c #123456"; "down 120 --background #ff0000 -r" |]

    let actual =
        splitInput' "down 5:00 up 0:30 -c #123456 down 120 --background #ff0000 -r"

    Assert.Equal<string array>(expected, actual)

[<Fact>]
let ``CommandM.splitInput'-2`` () =
    let expected =
        [| "down 5:00 -m hey;"
           "up 0:30 -c #123456 -m writing comments..."
           "down 120 --background #ff0000 --message next presentors come on! -r" |]

    let actual =
        splitInput'
            "down 5:00 -m hey; up 0:30 -c #123456 -m writing comments... down 120 --background #ff0000 --message next presentors come on! -r"

    Assert.Equal<string array>(expected, actual)

[<Fact>]
let ``CommandM.splitInput'-3`` () =
    let expected =
        [| "down 5:00"
           "up 0:30 -c #123456 -m writing comments..."
           "down 120 --background #ff0000 --message next presentors come on! -r"
           "up 0:30 --color #123456 -bg #ff0000 -m writing comments..." |]

    let actual =
        splitInput'
            "down 5:00 up 0:30 -c #123456 -m writing comments... down 120 --background #ff0000 --message next presentors come on! -r up 0:30 --color #123456 -bg #ff0000 -m writing comments..."

    Assert.Equal<string array>(expected, actual)

[<Fact>]
let ``CommandM.splitCommandM-1`` () =
    let expected = [| "down 5:00"; "-m hey;" |]

    let actual = splitCommandM "down 5:00 -m hey;"

    Assert.Equal<string array>(expected, actual)

[<Fact>]
let ``CommandM.splitCommandM-2`` () =
    let expected = [| "up 0:30"; "-c #123456"; "-m writing comments..." |]

    let actual = splitCommandM "up 0:30 -c #123456 -m writing comments..."

    Assert.Equal<string array>(expected, actual)

[<Fact>]
let ``CommandM.splitCommandM-3`` () =
    let expected =
        [| "down 120"
           "--background #ff0000"
           "--message next presentors come on!"
           "-r" |]

    let actual =
        splitCommandM "down 120 --background #ff0000 --message next presentors come on! -r"

    Assert.Equal<string array>(expected, actual)

[<Fact>]
let ``CommandM.parseInput-1`` () =
    let expected = TimeSpan(0, 5, 0), "", "", "hey;"
    let actual = parseInput "down 5:00 -m hey;"
    Assert.Equal(expected, actual)

[<Fact>]
let ``CommandM.parseInput-2`` () =
    let expected = TimeSpan(0, 0, 30), "#123456", "", "writing comments..."
    let actual = parseInput "up 0:30 -c #123456 -m writing comments..."
    Assert.Equal(expected, actual)

[<Fact>]
let ``CommandM.parseInput-3`` () =
    let expected = TimeSpan(0, 2, 0), "", "#ff0000", "next presentors come on!"

    let actual =
        parseInput "down 120 --background #ff0000 --message next presentors come on!"

    Assert.Equal(expected, actual)

[<Fact>]
let ``CommandM.parse-1`` () =
    let expected = CommandM.CountDown(TimeSpan(0, 5, 0), "", "", "hey;")
    let actual = parse "down 5:00 -m hey;"
    Assert.Equal(expected, actual)

[<Fact>]
let ``CommandM.parse-2`` () =
    let expected = CommandM.CountUp(TimeSpan(0, 0, 30), "#123456", "", "hey hey")
    let actual = parse "up 0:30 -c #123456 -m hey hey"
    Assert.Equal(expected, actual)

[<Fact>]
let ``CommandM.parse-3`` () =
    let expected = CommandM.CountDown(TimeSpan(0, 2, 0), "", "#ff0000", "ho ho ho")

    let actual = parse "down 120 --background #ff0000 --message ho ho ho"
    Assert.Equal(expected, actual)

[<Fact>]
let ``Command2.update 1`` () =
    let seed: DownCommand =
        { Duration = TimeSpan(0, 5, 0)
          Delay = TimeSpan.Zero
          Color = ""
          Background = ""
          Message = "" }

    let updated = { seed with Color = "#65a2ac" }
    let expected = Command2.Down(updated)

    let actual =
        Command2.update (Command2.Command2.Down seed) (Parsing.Options.Color "#65a2ac")

    Assert.Equal(expected, actual)

[<Fact>]
let ``Command2.build' 1`` () =
    let seed: Parsing.CommandAndOptions =
        Parsing.CommandAndOptions.Down(
            TimeSpan(0, 5, 0),
            [ Parsing.Options.Color "#ffffff"
              Parsing.Options.Background "#65a2ac"
              Parsing.Options.Message "hey hey" ]
        )

    let expected: Command2 =
        { DownCommand.Duration = TimeSpan(0, 5, 0)
          Delay = TimeSpan.Zero
          Color = "#ffffff"
          Background = "#65a2ac"
          Message = "hey hey" }
        |> Command2.Down

    let actual: Command2 = Command2.build' seed

    Assert.Equal(expected, actual)

[<Fact>]
let ``Command2.withDelay 1`` () =
    let seeds: Parsing.CommandAndOptions list =
        [ Parsing.CommandAndOptions.Down(
              TimeSpan(0, 5, 0),
              [ Parsing.Options.Color "#ffffff"
                Parsing.Options.Background "#65a2ac"
                Parsing.Options.Message "hey hey" ]
          )
          Parsing.CommandAndOptions.Up(
              TimeSpan(0, 2, 0),
              [ Parsing.Options.Color "#333333"
                Parsing.Options.Background "#ffffff"
                Parsing.Options.Message "hey" ]
          ) ]

    let expected: Command2 list =
        [ Command2.Down
              { Duration = TimeSpan(0, 5, 0)
                Delay = TimeSpan.Zero
                Color = "#ffffff"
                Background = "#65a2ac"
                Message = "hey hey" }
          Command2.Up
              { Duration = TimeSpan(0, 2, 0)
                Delay = TimeSpan(0, 5, 0)
                Color = "#333333"
                Background = "#ffffff"
                Message = "hey" } ]

    let actual: Command2 list = seeds |> List.map Command2.build' |> withDelay

    Assert.Equal<Command2 list>(expected, actual)
