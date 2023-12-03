# ccc

Version 0.1.1

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
| `up <time>`   | Countup.   |

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
down 5:00 --message Presentation. up 120 -bg #aaccff -m Questions and answers.
```

Countdown for 5 minutes, with a message "Presentation." below the timer. Then countup for 120 seconds (2 minutes), with the pale blue background and a message "Questions and answers.".

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

[Releases](https://github.com/taidalog/ccc/releases)

## License

This application is licensed under MIT License.
