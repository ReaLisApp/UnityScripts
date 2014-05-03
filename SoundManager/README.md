SoundManager
============
@SingletonObject<br/>
AudioClip再生管理を行うClass<br/>
音声ファイルごとにVolumeを変更することも可能<br/>
　<br/>
　<br/>
　<br/>
=== 使用方法 ===　<br/>
新規GameObjectを作成 <br/>
SoundManagerをGameObjectに追加 <br/>
GameObjectにSoundManagerタグを追加 <br/>


<h2>設定</h2>
-concurrentPlayCount ... SE, Voiceの最大同時再生数
-Mute ...音声再生を許可するかどうか<br/>
-BGM  ...BGMリソース<br/>
-- AudioClip ファイル<br/>
-- Volume    音量<br/>
<br/>
![Alt img](./00_image/1.png)
<br/>
<h2>再生方法</h2>
**BGM<br/>
SoundManager.Instance.PlayBGM("file name");<br/>
<br/>
**SE<br/>
SoundManager.Instance.PlaySE("file name");<br/>
SoundManager.Instance.StopSE();<br/>
<br/>
**VOICE<br/>
SoundManager.Instance.PlayVoice("file name");<br/>
