using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioPath
{
    public class BGM
    {
        public const string Home = "Assets/05.Audios/BGM/Home.wav";
        public const string PenguinPop = "Assets/05.Audios/BGM/PenguinPop.wav";
        public const string Appreciate = "Assets/05.Audios/BGM/Appreciate.wav";
    }

    public class SFX
    {
        public const string Open = "Assets/05.Audios/SFX/Open.wav";
        public const string Close = "Assets/05.Audios/SFX/Close.wav";
        public const string WaterDrop = "Assets/05.Audios/SFX/WaterDrop.wav";
        public const string Knock = "Assets/05.Audios/SFX/Knock.wav";
        public const string Denied = "Assets/05.Audios/SFX/Denied.wav";
        public const string Collection = "Assets/05.Audios/SFX/Collection.wav";

        public const string Jump1 = "Assets/05.Audios/SFX/Jump1.wav";
        public const string Jump2 = "Assets/05.Audios/SFX/Jump2.wav";
        public const string Jump3 = "Assets/05.Audios/SFX/Jump3.wav";
        public const string Jump4 = "Assets/05.Audios/SFX/Jump4.wav";

        /// <summary>获得一个随机跳跃音效地址</summary>
        public static string GetRandomJumpPath()
        {
            int number = Random.Range(1, 5);
            return number switch
            {
                1 => Jump1,
                2 => Jump2,
                3 => Jump3,
                4 => Jump4,
                _ => Jump1,
            };
        }
    }
}
