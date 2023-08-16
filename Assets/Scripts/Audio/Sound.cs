using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Audio
{
	[System.Serializable]
	public struct Sound
	{
		public string Name;
		public AudioClip AudioClip;
		[Range(0f, 1f)]
		public float Volume;
	}
}
