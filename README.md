## unity-multi-audio-sample
* UnityAudio、CRI ADX2、Wwiseで切り替え可能なオーディオサンプルになります。
  * ゲーム側からは使用しているオーディオライブラリは意識しないよう実装してあります。
* 下記のようなサンプルを用意しています。
  * 基本機能
    * BGMフェード再生
    * SEの3D再生
    * DSPエフェクト
  * オーディオスペクトラムの表示 (UnityAudio、CRI ADX2のみ)
  * インタラクティブミュージック (CRI ADX2、Wwiseのみ)
    * ブロック遷移による切替
    * オーディオ側に設定したイベントコールバック制御
    * ゲームパラメータによるサウンド変化
    * ビートに合わせたオブジェクトの伸縮

### バージョン
* Unity
  * 2022.3.16f1
* CRI ADX2
  * CRI Atom Craft LE
    * 3.50.06
  * CRIWARE Unity Plug-in
    * 3.09.01
* Wwise
  * 2023.1.1.8417

### シーン構成
* 各シーンは <a href="/Assets/GameSample/Scenes">Assets/GameSample/Scenes</a> 配下に格納しています。

| Scene名 | 概要                                                                                                                                                                                                                                                             | UnityAudio | CRI ADX2 | Wwise
| -- |----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------| -- | -- | -- |
| SoundTest.unity | 基本機能のサンプル集<br>・BGMクロスフェード<br>・SEの3D再生<br>・DSPエフェクト<br><img width=400 src="/ReadMeContents/SoundTest_01.png"><br>・カテゴリによる音量設定画面<br><img width=400 src="/ReadMeContents/SoundTest_02.png">                                                                       | 〇 | 〇 | 〇 |
| Spectrum.unity | オーディオスペクトラムの実装サンプル<br>周波数データを取得して表示する<br>・LineRendererによる描画<br><img width=400 src="/ReadMeContents/Spectrum_01.png"><br>・3DCubeによる描画<br><img width=400 src="/ReadMeContents/Spectrum_02.png">                                                                  | 〇 | 〇 | × |
| Interactive.unity | インタラクティブミュージックのサンプル<br>プレイヤーの動きに合わせてBGMを変化させる<br>・ブロック再生による切替<br>・シーケンスコールバックによるイベント処理<br>・AISACコントロールによるサウンド変化<br>・BeatSyncによるビート同期操作<br><img width=400 src="/ReadMeContents/Interactive_01.png"><br><img width=400 src="/ReadMeContents/Interactive_02.png"> | × | 〇 | 〇 |

### フォルダ構成
TODO

### オーディオライブラリの切替
TODO

### ゲーム側での実装
TODO
