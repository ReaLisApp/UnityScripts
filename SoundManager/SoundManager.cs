using UnityEngine;
using System;
using System.Collections.Generic;

/**
 * 	 @Class   SoundManager 
 *   Singleton 音管理クラス
 * 	 ScriptAddObjectにSoundManagerタグを追加してください
 **/ 
public class SoundManager : MonoBehaviour {
		
	// インスタンス保持
	protected static SoundManager instance;
	
	/**
	 * インスタンス取得
	 **/
	public static SoundManager Instance {
		get { 
			if(instance == null) {
				instance = (SoundManager) FindObjectOfType(typeof(SoundManager));
				
				if (instance == null) {
					Debug.LogError("SoundManager Instance Error  \n ObjectにSoundManagerタグを追加してください ");
				}
			}	 
			return instance;
		}
	}
	
	/** === public === */	
	public bool mute = false;						// ミュート
	public int concurrentPlayCount = 16;			// 最大同時再生数
	
	public SoundInfo[] BGM; 						// BGM
	public SoundInfo[] SE;							// SE
	public SoundInfo[] Voice;						// 音声
	
	
	/** === Private === */
	// Resouce管理 
	private Dictionary<string,SoundInfo> resouceDictionary = new Dictionary<string,SoundInfo>();
	
	// 再生用AudioSource
	private AudioSource BGMsource;				// BGM
	private AudioSource[] SEsources;			// SE
	private AudioSource[] VoiceSources;			// 音声
	
	
	void Awake (){
		GameObject[] obj = GameObject.FindGameObjectsWithTag("SoundManager");
		if( obj.Length > 1 ){ Destroy(gameObject); }			// 既に存在しているなら削除
		else 				{ DontDestroyOnLoad(gameObject);}	// 音管理はシーン遷移では破棄させない
		
		// 初期化
		SEsources 		= new AudioSource[concurrentPlayCount];
		VoiceSources 	= new AudioSource[concurrentPlayCount];
		
		// 全ての再生用AudioSourceコンポーネントを追加する
		// BGM AudioSource
		BGMsource = gameObject.AddComponent<AudioSource>();
		BGMsource.loop = true;
		
		// SE AudioSource
		for(int i = 0 ; i < SEsources.Length ; i++ ){
			SEsources[i] = gameObject.AddComponent<AudioSource>();
		}
		// 音声 AudioSource
		for(int i = 0 ; i < VoiceSources.Length ; i++ ){
			VoiceSources[i] = gameObject.AddComponent<AudioSource>();
		}
		// souceは連想配列で保持
		foreach ( SoundInfo info in BGM ) {
			resouceDictionary[info.audioClip.name] = info;
		}
		foreach ( SoundInfo info in SE ) {
			resouceDictionary[info.audioClip.name] = info;
			
		}
		foreach ( SoundInfo info in Voice ) {
			resouceDictionary[info.audioClip.name] = info;
		}
		BGM = null;
		SE = null;
		Voice = null;
	}
	
	
	void Update () {
		// ミュート設定
		BGMsource.mute = mute;
		foreach(AudioSource source in SEsources )	{ source.mute = mute; }
		foreach(AudioSource source in VoiceSources ){ source.mute = mute;}
	}
	
	
	/**
	 * BGM再生
	 * @param string filename : ファイルネーム
	 **/
	public void PlayBGM(string filename)
	{	
		// サウンド情報の取得
		SoundInfo si = resouceDictionary[filename];
		
		// 同じBGMの場合は何もしない
		if( BGMsource.clip == si.audioClip ){ return; }
		
		BGMsource.Stop();
		BGMsource.volume 	= si.volime;
		BGMsource.clip 		= si.audioClip;
		BGMsource.Play();
	}
	
	/**
 	 * BGM停止
 	 **/ 
	public void StopBGM(){
		BGMsource.Stop();
		BGMsource.clip = null;
	}
	
	
	/**
	 * SE再生
	 * @param string filename : ファイルネーム
	 **/
	public void PlaySE(string filename){
		
		// サウンド情報の取得
		SoundInfo si = resouceDictionary[filename];
		
		// 再生中で無いAudioSouceで鳴らす
		foreach(AudioSource source in SEsources){
			if( false == source.isPlaying ){
				source.volume = si.volime;
				source.clip = si.audioClip;
				source.Play();
				return;
			}
		}  
	}
	
	/**
	 * SE停止
	 **/
	public void StopSE(){
		// 全てのSE用のAudioSouceを停止する
		foreach(AudioSource source in SEsources){
			source.Stop();
			source.clip = null;
		}  
	}
	
	
	/**
	 * 音声再生
	 * @param string filename : ファイルネーム
	 **/
	public void PlayVoice(string filename){
		
		SoundInfo si = resouceDictionary[filename];
		
		// 再生中で無いAudioSouceで鳴らす
		foreach(AudioSource source in VoiceSources){
			if( false == source.isPlaying ){
				source.volume 	= si.volime;
				source.clip 	=  si.audioClip;
				source.Play();
				return;
			}
		} 
	}
	
	/**
	 *	音声停止
	 **/
	public void StopVoice(){
		// 全ての音声用のAudioSouceを停止する
		foreach(AudioSource source in VoiceSources){
			source.Stop();
			source.clip = null;
		}  
	}
}



/**
 * SoundInfomation管理インナーClass
 **/
[Serializable]
public class SoundInfo
{
	public AudioClip audioClip; 
	public float volime = 1.0f;
}