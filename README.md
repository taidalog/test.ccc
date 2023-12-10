# ccc

Version 0.2.1

[Japanese README](README.ja.md)

ccc: customizable command countdown-timer.

## Synopsis

- A customizable command countdown-timer. Suitable for presentation.

## Usage

1. Visit [ccc](https://taidalog.github.io/ccc/).
1. Type or paste commands.
1. Press `Enter` to start the timer.

## Commands

| Command       | Feature    |
| ------------- | ---------- |
| `down <time>` | Countdown. |
| `up <time>`   | Count up.  |

## Options

| Options                        | Feature                                                              |
| ------------------------------ | -------------------------------------------------------------------- |
| `--color\|-c <hex code>`       | Specifies text color. Default: `#333333`.                            |
| `--background\|-bg <hex code>` | Specifies background color. Default: `#ffffff`.                      |
| `--message\|-m <text>`         | Specifies message to show below the timer. Default: an empty string. |

## Examples

```
down 5:00 --message hey
```

Countdown for 5 minutes, with a message "hey" below the timer.

```
down 5:00 down 90
```

Countdown for 5 minutes, with no message below the timer. Then countdown for 90 seconds (1 minute and 30 seconds), with no message.

```
down 5:00 --color #222266 --background #aaccff --message hello
```

Countdown for 5 minutes, with the dark blue text, the pale blue background and a message "hello" below the timer.

```
down 5:00 --message Presentation. up 120 -bg #aaccff -m Questions and answers. -c #222266
```

Countdown for 5 minutes, with a message "Presentation." below the timer. Then count up for 120 seconds (2 minutes), with the dark blue text and the pale blue background and a message "Questions and answers.".

## Recommended environment

- Mozilla Firefox 120.0 (64 bit) or later.
- Google Chrome 119.0.6045.160 (64 bit) or later.
- Microsoft Edge 119.0.2151.93 (64 bit) or later.

## Terms of Service

- The copyright is owned by the creator (I).
- The communications charge required for use of this site will be borne by the user.
- The creator is not responsible for any damage caused by computer viruses, data loss, or any other disadvantages caused by usinfg this site.
- You can use the source code, but please keep the copyright notice and license notice when redistributing.

## Known Issue

-

## Release Notes

[Releases on GitHub](https://github.com/taidalog/ccc/releases)

## License

This application is licensed under MIT License.
