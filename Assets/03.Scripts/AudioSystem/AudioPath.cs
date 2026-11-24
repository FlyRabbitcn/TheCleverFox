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

        public const string Die = "Assets/05.Audios/SFX/Die.wav";

        public const string AttackMiss1 = "Assets/05.Audios/SFX/Attack-Miss1.wav";
        public const string AttackMiss2 = "Assets/05.Audios/SFX/Attack-Miss2.wav";

        /// <summary>获得一个随机跳跃音效地址</summary>
        public static string GetRandomJumpPath()
        {
            int number = Random.Range(1, 3);
            return number switch
            {
                1 => Jump1,
                2 => Jump2,
                _ => Jump1,
            };
        }

        public static string GetRandomAttackMissPath()
        {
            int number = Random.Range(1, 3);
            return number switch
            {
                1 => AttackMiss1,
                2 => AttackMiss2,
                _ => AttackMiss1,
            };
        }
    }
}
