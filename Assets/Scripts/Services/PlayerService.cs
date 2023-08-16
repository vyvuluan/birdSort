using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services
{
	
	public class PlayerService
	{
		// Key
		private const string MusicVolumeKey = "mvl";
		private const string SoundVolumeKey = "svl";
		private const string VibrateKey = "vbr";

		private const string levelKey = "lvl";
		private const string scoreKey = "scr";

		private const string quantityBoosterAddBarKey = "qba";
		private const string quantityBoosterRollbackKey = "qbr";
		private const string quantityBoosterNextMapKey = "qbn";
		private const string startGameFirstTimeKey = "fst";
		private const string skinCatKey = "sck";
		private const string skinCatOwnedKey = "sco";
		private const string skinBarKey = "snk";
		private const string skinBarOwnedKey = "sbo";
        private const string skinOwnedKey = "sko";
        private const string skinBGKey = "sbg";
		private const string skinBGOwnedKey = "bgo";
		private const string isAdsKey = "ads";

		private const string Break = "~";

		public Action<float> OnMusicVolumeChange;
		public Action<float> OnSoundVolumeChange;

		public Action<bool> OnVibrateChange;
		public float GetMusicVolume()
		{
			return PlayerPrefs.GetFloat(MusicVolumeKey, 1.0f);
		}
		public void SetMusicVolume(float volume)
		{
			PlayerPrefs.SetFloat(MusicVolumeKey, volume);
			OnMusicVolumeChange?.Invoke(volume);
		}
		public float GetSoundVolume()
		{
			return PlayerPrefs.GetFloat(SoundVolumeKey, 1.0f);
		}
		public void SetSoundVolume(float volume)
		{
			PlayerPrefs.SetFloat(SoundVolumeKey, volume);
			OnSoundVolumeChange?.Invoke(volume);
		}
		public bool GetVibrate()
		{
			return PlayerPrefs.GetInt(VibrateKey, 1) == 0 ? false : true;
		}
		public void SetVibrate(bool isVibrate)
		{
			OnVibrateChange?.Invoke(isVibrate);
			if (isVibrate == true)
			{
				PlayerPrefs.SetInt(VibrateKey, 1);
			}
			else
			{
				PlayerPrefs.SetInt(VibrateKey, 0);
			}
		}
		private void SaveList<T>(string key, List<T> value)
		{
			if (value == null)
			{
				Logger.Warning("Input list null");
				value = new List<T>();
			}
			if (value.Count == 0)
			{
				PlayerPrefs.SetString(key, string.Empty);
				return;
			}
			if (typeof(T) == typeof(string))
			{
				foreach (var item in value)
				{
					string tempCompare = item.ToString();
					if (tempCompare.Contains(Break))
					{
						throw new Exception("Invalid input. Input contain '~'.");
					}
				}
			}
			PlayerPrefs.SetString(key, string.Join(Break, value));
		}
		private List<T> GetList<T>(string key, List<T> defaultValue)
		{
			if (PlayerPrefs.HasKey(key) == false)
			{
				return defaultValue;
			}
			if (PlayerPrefs.GetString(key) == string.Empty)
			{
				return new List<T>();
			}
			string temp = PlayerPrefs.GetString(key);
			string[] listTemp = temp.Split(Break);
			List<T> list = new List<T>();

			foreach (string s in listTemp)
			{
				list.Add((T)Convert.ChangeType(s, typeof(T)));
			}
			return list;
		}
		public void SetScore(int score)
		{
			PlayerPrefs.SetInt(scoreKey, score);
		}
		public int GetScore()
		{
			return PlayerPrefs.GetInt(scoreKey, 0);
		}
		public void SetLevel(int level)
		{
			PlayerPrefs.SetInt(levelKey, level);
		}
		public int GetLevel()
		{
			return PlayerPrefs.GetInt(levelKey, 0);
		}
		public void SetQuantityBoosterAddBar(int quantityBoosterAddBar)
		{
			PlayerPrefs.SetInt(quantityBoosterAddBarKey, quantityBoosterAddBar);
		}
		public int GetQuantityBoosterAddBar()
		{
			return PlayerPrefs.GetInt(quantityBoosterAddBarKey, 0);
		}

		public void SetQuantityBoosterRollback(int quantityBoosterRollback)
		{
			PlayerPrefs.SetInt(quantityBoosterRollbackKey, quantityBoosterRollback);
		}
		public int GetQuantityBoosterRollback()
		{
			return PlayerPrefs.GetInt(quantityBoosterRollbackKey, 0);
		}
		public void SetQuantityBoosterNextMap(int quantityBoosterNextMap)
		{
			PlayerPrefs.SetInt(quantityBoosterNextMapKey, quantityBoosterNextMap);
		}
		public int GetQuantityBoosterNextMap()
		{
			return PlayerPrefs.GetInt(quantityBoosterNextMapKey, 0);
		}
		public void SetStartGameFirstTime(int check)
		{
			PlayerPrefs.SetInt(startGameFirstTimeKey, check);
		}
		public int GetStartGameFirstTime()
		{
			return PlayerPrefs.GetInt(startGameFirstTimeKey, 0);
		}
		public void SetSkinCat(List<int> lists)
        {
			SaveList<int>(skinCatKey, lists);
        }
		public List<int> GetSkinCat()
		{
			return GetList<int>(skinCatKey, new());
		}
		public void SetSkinCatOwned(List<int> lists)
		{
			SaveList<int>(skinCatOwnedKey, lists);
		}
		public List<int> GetSkinCatOwned()
		{
			return GetList<int>(skinCatOwnedKey, new());
		}

		public void SetSkinBar(int skinBar)
		{
			PlayerPrefs.SetInt(skinBarKey, skinBar);
		}
		public int GetSkinBar()
		{
			return PlayerPrefs.GetInt(skinBarKey, 0);
		}
		public void SetSkinBarOwned(List<int> lists)
		{
			SaveList<int>(skinBarOwnedKey, lists);
		}
		public List<int> GetSkinBarOwned()
		{
			return GetList<int>(skinBarOwnedKey, new());
		}
		public void SetSkinBG(int skinBG)
		{
			PlayerPrefs.SetInt(skinBGKey, skinBG);
		}
		public int GetSkinBG()
		{
			return PlayerPrefs.GetInt(skinBGKey, 0);
		}
		public void SetSkinBGOwned(List<int> lists)
		{
			SaveList<int>(skinBGOwnedKey, lists);
		}
		public List<int> GetSkinBGOwned()
		{
			return GetList<int>(skinBGOwnedKey, new());
		}

        public void SetSkinOwned(List<int> lists)
        {
            SaveList<int>(skinOwnedKey, lists);
        }
        public List<int> GetSkinOwned()
        {
            return GetList<int>(skinOwnedKey, new());
        }
		public void SetAds(int number)
		{
			PlayerPrefs.SetInt(isAdsKey, number);
		}
		public int GetAds()
		{
			return PlayerPrefs.GetInt(isAdsKey, 0);
		}
        public void Save()
		{
			PlayerPrefs.Save();
		}
	}
}
