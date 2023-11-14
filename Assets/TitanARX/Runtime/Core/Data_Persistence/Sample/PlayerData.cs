using System;
using UnityEngine;

    [Serializable]
    public class PlayerData
    {
        public Sprite PlayerPhoto;
        public string PlayerAlias;
        public int PlayerIncome;
        public TimeSpan PlayerTotalInGameTime;
        public string PlayerTotalInGameTimeString => string.Format("{0} hours {1} mins", PlayerTotalInGameTime.Hours, PlayerTotalInGameTime.Minutes);
    }
