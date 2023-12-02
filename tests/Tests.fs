module Tests

open System
open Xunit
open Ccc.Command

[<Fact>]
let ``Command.f-1`` () =
    let expected = "1", "23", "45"
    let actual = f "1:23:45"
    Assert.Equal(expected, actual)

[<Fact>]
let ``Command.f-2`` () =
    let expected = "", "23", "45"
    let actual = f "23:45"
    Assert.Equal(expected, actual)

[<Fact>]
let ``Command.f-3`` () =
    let expected = "", "", "45"
    let actual = f "45"
    Assert.Equal(expected, actual)

[<Fact>]
let ``Command.g-1`` () =
    let expected = 1, 23, 45
    let actual = g ("1", "23", "45")
    Assert.Equal(expected, actual)

[<Fact>]
let ``Command.g-2`` () =
    let expected = 0, 23, 45
    let actual = g ("", "23", "45")
    Assert.Equal(expected, actual)

[<Fact>]
let ``Command.g-3`` () =
    let expected = 0, 0, 45
    let actual = g ("", "", "45")
    Assert.Equal(expected, actual)

[<Fact>]
let ``Command.toTimeSpan-1`` () =
    let expected = TimeSpan(1, 23, 45)
    let actual = toTimeSpan "1:23:45"
    Assert.Equal(expected, actual)

[<Fact>]
let ``Command.toTimeSpan-2`` () =
    let expected = TimeSpan(0, 23, 45)
    let actual = toTimeSpan "23:45"
    Assert.Equal(expected, actual)

[<Fact>]
let ``Command.toTimeSpan-3`` () =
    let expected = TimeSpan(0, 0, 45)
    let actual = toTimeSpan "45"
    Assert.Equal(expected, actual)

[<Fact>]
let ``Command.splitInput'-1`` () =
    let expected =
        [| "down 5:00"; "up 0:30 -c #123456"; "down 120 --background #ff0000 -r" |]

    let actual =
        splitInput' "down 5:00 up 0:30 -c #123456 down 120 --background #ff0000 -r"

    Assert.Equal<string array>(expected, actual)

[<Fact>]
let ``Command.splitInput'-2`` () =
    let expected =
        [| "down 5:00 -m hey;"
           "up 0:30 -c #123456 -m writing comments..."
           "down 120 --background #ff0000 --message next presentors come on! -r" |]

    let actual =
        splitInput'
            "down 5:00 -m hey; up 0:30 -c #123456 -m writing comments... down 120 --background #ff0000 --message next presentors come on! -r"

    Assert.Equal<string array>(expected, actual)

[<Fact>]
let ``Command.splitInput'-3`` () =
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
let ``Command.splitCommand-1`` () =
    let expected = [| "down 5:00"; "-m hey;" |]

    let actual = splitCommand "down 5:00 -m hey;"

    Assert.Equal<string array>(expected, actual)

[<Fact>]
let ``Command.splitCommand-2`` () =
    let expected = [| "up 0:30"; "-c #123456"; "-m writing comments..." |]

    let actual = splitCommand "up 0:30 -c #123456 -m writing comments..."

    Assert.Equal<string array>(expected, actual)

[<Fact>]
let ``Command.splitCommand-3`` () =
    let expected =
        [| "down 120"
           "--background #ff0000"
           "--message next presentors come on!"
           "-r" |]

    let actual =
        splitCommand "down 120 --background #ff0000 --message next presentors come on! -r"

    Assert.Equal<string array>(expected, actual)

[<Fact>]
let ``Command.parseInput-1`` () =
    let expected = TimeSpan(0, 5, 0), "#333333", "#ffffff", "hey;"
    let actual = parseInput "down 5:00 -m hey;"
    Assert.Equal(expected, actual)

[<Fact>]
let ``Command.parseInput-2`` () =
    let expected = TimeSpan(0, 0, 30), "#123456", "#ffffff", "writing comments..."
    let actual = parseInput "up 0:30 -c #123456 -m writing comments..."
    Assert.Equal(expected, actual)

[<Fact>]
let ``Command.parseInput-3`` () =
    let expected = TimeSpan(0, 2, 0), "#333333", "#ff0000", "next presentors come on!"

    let actual =
        parseInput "down 120 --background #ff0000 --message next presentors come on!"

    Assert.Equal(expected, actual)

[<Fact>]
let ``Command.parse-1`` () =
    let expected = Action.CountDown(TimeSpan(0, 5, 0), "#333333", "#ffffff", "hey;")
    let actual = parse "down 5:00 -m hey;"
    Assert.Equal(expected, actual)

[<Fact>]
let ``Command.parse-2`` () =
    let expected = Action.CountUp(TimeSpan(0, 0, 30), "#123456", "#ffffff", "hey hey")
    let actual = parse "up 0:30 -c #123456 -m hey hey"
    Assert.Equal(expected, actual)

[<Fact>]
let ``Command.parse-3`` () =
    let expected = Action.CountDown(TimeSpan(0, 2, 0), "#333333", "#ff0000", "ho ho ho")
    let actual = parse "down 120 --background #ff0000 --message ho ho ho"
    Assert.Equal(expected, actual)
