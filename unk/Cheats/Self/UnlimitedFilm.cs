﻿using unk.Cheats.Core;
using unk.Util;

namespace unk.Cheats
{
    internal class UnlimitedFilm : ToggleCheat
    {

        public override void Update()
        {
            if (Player.localPlayer is null || !Enabled) return;

            ItemInstance item = Player.localPlayer.data.currentItem;

            if (item.GetComponent<VideoCamera>() is null) return;

            VideoCamera camera = item.GetComponent<VideoCamera>();

            VideoInfoEntry videoInfoEntry = camera.Reflect().GetValue<VideoInfoEntry>("m_recorderInfoEntry");

            videoInfoEntry.timeLeft = videoInfoEntry.maxTime;  
        }

    }
}
