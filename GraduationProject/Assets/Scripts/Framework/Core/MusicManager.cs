using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameFrameWork
{

    public class MusicManager : SingletonMono<MusicManager>
    {
        public static string BgMusicVolume_SaveKey = "BgMusicVolume";
        public static string SoundVolume_SaveKey = "SoundVolume";

        private AudioSource bgMusic = null;

        private float BgValue = 1;


        private GameObject soundObj = null;

        private List<AudioSource> soundList = new List<AudioSource>();

        private float SoundValue = 1;

        private new void Awake()
        {
            base.Awake();
            if(!PlayerPrefs.HasKey(BgMusicVolume_SaveKey) || !PlayerPrefs.HasKey(SoundVolume_SaveKey))
            {
                PlayerPrefs.SetFloat(BgMusicVolume_SaveKey, 1);
                PlayerPrefs.SetFloat(SoundVolume_SaveKey, 1);
                PlayerPrefs.Save();
            }
            BgValue = PlayerPrefs.GetFloat(BgMusicVolume_SaveKey);
            SoundValue = PlayerPrefs.GetFloat(SoundVolume_SaveKey);
        }

        private void Update()
        {
            for (int i = 0; i < soundList.Count; i++)
            {
                if (i < soundList.Count && soundList[i] == null)
                {
                    soundList.RemoveAt(i);
                    i--;
                }
                else
                {
                    if (i < soundList.Count && !soundList[i].isPlaying)
                    {
                        Object.Destroy(soundList[i]);
                        soundList.RemoveAt(i);
                        i--;
                    }
                }

            }
        }


        /// <summary>
        /// 改变所有音效的音量
        /// </summary>
        /// <param name="v">改变的值</param>
        public void ChangeSoundValue(float v)
        {
            SoundValue = v;
            foreach (var VARIABLE in soundList)
            {
                VARIABLE.volume = SoundValue;
            }

            PlayerPrefs.SetFloat(SoundVolume_SaveKey, v);
            PlayerPrefs.Save();
        }


        /// <summary>
        /// 停止某个音效
        /// </summary>
        /// <param name="source"></param>
        public void StopSound(AudioSource source)
        {
            if (!soundList.Contains(source))
                return;

            soundList.Remove(source);

            source.Stop();
            Object.Destroy(source);
        }


        /// <summary>
        /// 播放某个音效
        /// </summary>
        /// <param name="name">音效的路径</param>
        /// <param name="action">回调函数，用于处理生成的音效的AudioSource组件，可以用来设置是否循环播放等</param>
        public void PlaySound(string name, UnityAction<AudioSource> action = null)
        {
            if (soundObj == null)
            {
                soundObj = new GameObject
                {
                    name = "Sound"
                };
            }

            AudioSource source = soundObj.AddComponent<AudioSource>();
            ResourcesManager.Instance.LoadAssetAsync<AudioClip>(name,
                (clip) =>
                {
                    source.clip = clip;
                    source.volume = SoundValue;
                    source.Play();
                    soundList.Add(source);

                    action?.Invoke(source);
                }
            );
        }


        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="name"></param>
        public void PlayBgMusic(string name, UnityAction<AudioSource> action = null)
        {
            if (bgMusic == null)
            {
                GameObject obj = new GameObject();
                obj.name = "BgMusic";
                obj.transform.SetParent(transform);
                bgMusic = obj.AddComponent<AudioSource>();
                bgMusic.loop = true;
            }

            ResourcesManager.Instance.LoadAssetAsync<AudioClip>(name,
                (clip) =>
                {
                    bgMusic.clip = clip;
                    bgMusic.volume = BgValue;
                    bgMusic.Play();

                    action?.Invoke(bgMusic);
                }
            );
        }

        /// <summary>
        /// 暂停背景音乐
        /// </summary>
        public void PauseBgMusic()
        {
            if (bgMusic == null)
                return;
            bgMusic.Pause();
        }

        /// <summary>
        /// 改变背景音乐的音量
        /// </summary>
        /// <param name="v"></param>
        public void ChangeBgValue(float v)
        {
            BgValue = v;
            PlayerPrefs.SetFloat(BgMusicVolume_SaveKey, v);
            PlayerPrefs.Save();

            if (bgMusic == null)
                return;
            bgMusic.volume = BgValue;
        }
    }
}
