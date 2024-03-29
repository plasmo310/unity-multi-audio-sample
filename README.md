## unity-multi-audio-sample
* UnityAudio、CRI ADX、Wwiseで切り替え可能なオーディオサンプルになります。
  * ゲーム側からはオーディオライブラリは意識せずに実行できるよう実装してあります。
* 下記のサンプルを用意しています。
  * 基本機能
    * BGMフェード再生
    * SEの3D再生
    * DSPエフェクト
  * オーディオスペクトラムの表示 ※UnityAudio、CRI ADXのみ
  * インタラクティブミュージック ※CRI ADX、Wwiseのみ
    * ブロック遷移による切替
    * オーディオ側に設定したイベントによる制御
    * ゲームパラメータ値によるサウンド変化
    * ビートに合わせたオブジェクトの伸縮

### バージョン
* Unity
  * 2022.3.16f1
* CRI ADX
  * CRI Atom Craft LE
    * 3.50.06
  * CRIWARE Unity Plug-in
    * 3.09.01
* Wwise
  * 2023.1.1.8417

### シーン構成
* 各シーンは <a href="/Assets/GameSample/Scenes">Assets/GameSample/Scenes</a> 配下に格納しています。
  * <code>Tools/Switch AudioLib</code>より使用するライブラリを切り替えて実行してください。<br><img width=240 src="/ReadMeContents/editor_switch_audio.png">
    * ※選択されたライブラリに応じて、下記Defineを切り替えるよう実装しています。
      ```
      AUDIO_LIB_UNITY_AUDIO
      AUDIO_LIB_CRI
      AUDIO_LIB_WWISE
      ```

| Scene名 | 概要                                                                                                                                                                                                                                                             | UnityAudio | CRI ADX2 | Wwise
| -- |----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------| -- | -- | -- |
| SoundTest.unity | 基本機能のサンプル集<br>・BGMクロスフェード<br>・SEの3D再生<br>・DSPエフェクト<br><img width=400 src="/ReadMeContents/SoundTest_01.png"><br>・カテゴリによる音量設定画面<br><img width=400 src="/ReadMeContents/SoundTest_02.png">                                                                       | 〇 | 〇 | 〇 |
| Spectrum.unity | オーディオスペクトラムの実装サンプル<br>周波数データを取得して表示する<br>・LineRendererによる描画<br><img width=400 src="/ReadMeContents/Spectrum_01.png"><br>・3DCubeによる描画<br><img width=400 src="/ReadMeContents/Spectrum_02.png">                                                                  | 〇 | 〇 | × |
| Interactive.unity | インタラクティブミュージックのサンプル<br>プレイヤーの動きに合わせてBGMを変化させる<br>・ブロック遷移による切替<br>・オーディオ側に設定したイベントによる制御<br>・ゲームパラメータ値によるサウンド変化<br>・ビートに合わせたオブジェクトの伸縮<br><img width=400 src="/ReadMeContents/Interactive_01.png"><br><img width=400 src="/ReadMeContents/Interactive_02.png"> | × | 〇 | 〇 |

### フォルダ構成

##### 全体構成
| フォルダパス | 概要 |
| -- | -- |
| <a href="Assets/AudioLib">Assets/AudioLib</a> | 各オーディオライブラリの処理を実行する処理 |
| <a href="Assets/GameSample">Assets/GameSample</a> | サンプルゲームに関連する処理 |
| <a href="MW_CriAtomCraftProject">MW_CriAtomCraftProject</a> | CriAtomCraftプロジェクト |
| <a href="MW_WwiseProject">MW_WwiseProject</a> | Wwiseプロジェクト |

##### UnityAudio関連
| フォルダパス | 概要 |
| -- | -- |
| <a href="Assets/AudioLib/UnityAudio">Assets/AudioLib/UnityAudio</a> | APIを実行する処理 |
| <a href="Assets/GameSample/Runtime/Audio/UnityAudio">Assets/GameSample/Runtime/Audio/UnityAudio</a> | サンプルゲーム固有のオーディオ関連処理 |
| <a href="Assets/GameSample/Resources/UnityAudio">Assets/GameSample/Resources/UnityAudio</a> | UnityAudioで実行するためのサウンドデータ |

##### CRI ADX関連
| フォルダパス | 概要 |
| -- | -- |
| <a href="Assets/AudioLib/CriAdx">Assets/AudioLib/CriAdx</a> | APIを実行する処理 |
| <a href="Assets/GameSample/Runtime/Audio/CriAdx">Assets/GameSample/Runtime/Audio/CriAdx</a> | サンプルゲーム固有のオーディオ関連処理 |
| <a href="Assets/StreamingAssets/Audio/CriAdx">Assets/StreamingAssets/Audio/CriAdx</a> | CriAtomCraftプロジェクトから出力したサウンドデータ |

##### Wwise関連
| フォルダパス | 概要 |
| -- | -- |
| <a href="Assets/AudioLib/Wwise">Assets/AudioLib/Wwise</a> | APIを実行する処理 |
| <a href="Assets/GameSample/Runtime/Audio/Wwise">Assets/GameSample/Runtime/Audio/Wwise</a> | サンプルゲーム固有のオーディオ関連処理 |
| <a href="Assets/StreamingAssets/Audio/Wwise">Assets/StreamingAssets/Audio/Wwise</a> | Wwiseプロジェクトから出力したサウンドデータ |

### オーディオ関連処理の設計について
* パッケージ構成
  * <a href="Assets/GameSample">Assets/GameSample</a>配下にサンプルシーン関連の処理、<br><a href="Assets/AudioLib">Assets/AudioLib</a>配下に各APIへアクセスするための処理をそれぞれ格納しています。<br>
    <img width=280 src="/ReadMeContents/uml_package.png">
* クラス図
  * <code>GameSample.Audio</code>配下でゲーム固有のオーディオ関連の処理を実装し、<br>各オーディオライブラリへのアクセスは<code>AudioLib</code>配下の各サービスから行うようにしています。<br>
    <img width=800 src="/ReadMeContents/uml_class.png">
  * ゲーム側からは<code>ServiceLocator</code>を通じて、<code>IGameAudioService</code>、<code>IGameAudioSettings</code>に対してオーディオ関連の処理を実行しています。
    ```
    // ===== 例: ServiceLocatorへの登録 =====
    
            /// <summary>
            /// シーンのロード後の初期化処理
            /// </summary>
            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
            private static void InitializeAfterSceneLoad()
            {
                // サービス登録
    #if AUDIO_LIB_UNITY_AUDIO
                ServiceLocator.Register<IGameAudioSettings>(new GameUnityAudioSettings());
                ServiceLocator.Register<IGameAudioService>(new GameUnityAudioService());
    #elif AUDIO_LIB_CRI
                ServiceLocator.Register<IGameAudioSettings>(new GameCriAdxAudioSettings());
                ServiceLocator.Register<IGameAudioService>(new GameCriAdxAudioService());
    #elif AUDIO_LIB_WWISE
                ServiceLocator.Register<IGameAudioSettings>(new GameWwiseAudioSettings());
                ServiceLocator.Register<IGameAudioService>(new GameWwiseAudioService());
    #endif
            }
    
    // ===== 例: オーディオ再生 =====
    
            /// <summary>
            /// Audioサービス
            /// </summary>
            private IGameAudioService GameAudioService => ServiceLocator.Resolve<IGameAudioService>();
            private IGameAudioSettings GameAudioSettings => ServiceLocator.Resolve<IGameAudioSettings>();
    
            private void PlayBgm01()
            {
                GameAudioService.PlaySoundEvent(GameAudioSettings.SoundEventName_BgmSpaceWould);
            }
    ```
