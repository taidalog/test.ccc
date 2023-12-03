# ccc

Version 0.1.1

[English README](README.md)

ccc: customizable command countdown-timer.

## 概要

- コマンドを入力して使用する、カスタマイズ可能なカウントダウンタイマーです。プレゼンテーションなどに使えると思います。

## 使い方

1. [ccc](https://taidalog.github.io/ccc/) にアクセスする。
1. コマンドエリアにコマンドを入力するか貼り付ける。
1. `Enter` キーを押す。そうするとタイマーが起動する。

## コマンド

| コマンド      | 機能           |
| ------------- | -------------- |
| `down <time>` | カウントダウン |
| `up <time>`   | カウントアップ |

## オプション

| オプション                     | 機能                                                             |
| ------------------------------ | ---------------------------------------------------------------- |
| `--color\|-c <hex code>`       | 文字色を指定する。デフォルトは `#333333`。                       |
| `--background\|-bg <hex code>` | 背景色を指定する。デフォルトは `#ffffff`。                       |
| `--message\|-m <text>`         | タイマーの下に表示するメッセージを指定する。デフォルトは空文字。 |

## 例

```
down 5:00 --message hey
```

5 分間カウントダウンする。タイマーの下には「hey」と表示する。

```
down 5:00 --message Presentation. up 120 -bg #aaccff -m Questions and answers.
```

5 分間カウントダウンする。タイマーの下には「Presentation」と表示する。その後、120 秒間（2 分間）カウントアップする。背景色は淡い青で、タイマーの下には「Questions and answers.」と表示する。

## 推奨環境

- Mozilla Firefox 120.0 (64 bit) 以降
- Google Chrome 119.0.6045.160 (64 bit) 以降
- Microsoft Edge 119.0.2151.93 (64 bit) 以降

## ご利用について

- 著作権は作成者 (taidalog) が所有しています。
- 利用に必要な通信料等は利用者の負担となります。
- 当サイトを利用したことにより、コンピュータウィルス等による被害やデータの損失、その他いかなる不利益が生じた場合も、作成者は一切の責任を負いません。
- ソースコードの利用は可能ですが、再頒布時には著作権表示とライセンス表示を消さずに残しておいてください。

## 既知の問題

-

## リリースノート

[Releases](https://github.com/taidalog/ccc/releases)

## License

This application is licensed under MIT License.