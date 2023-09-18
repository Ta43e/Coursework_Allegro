using SampleMVVM.DataBase.UnitOfWorks;
using SampleMVVM.Model.BD;
using System;
using System.Collections.Generic;
using System.Numerics;
using WMPLib;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SampleMVVM.Managers
{
    public class MusicManager
    {

        public static MusicManager Instance { get; set; }

        public UnitOfWork unitOfWork = new UnitOfWork();
        private WindowsMediaPlayer player = new WindowsMediaPlayer(); //private
        public Songs selectSongs = new Songs();
        private int currentIndex = -1;
        public List<Songs> SongsList = new List<Songs>();

        static MusicManager()
        {
            Instance = new MusicManager();
        }
        public void PlaySong()
        {
            if (selectSongs == null)
            {
                return;
            }
            currentIndex = SongsList.IndexOf(selectSongs);
            player.URL = selectSongs.SongsPath;
            player.controls.play();
        }
        public void Pause()
        {
            player.controls.pause();
        }
        public void PlayingAfterPause()
        {
            player.controls.play();
        }
        public void PlayNext()
        {
            if (SongsList.Count == 0)
            {
                return;
            }
            if (currentIndex < SongsList.Count - 1)
            {
                currentIndex++;
            }
            else
            {
                currentIndex = 0;
            }
            selectSongs = SongsList[currentIndex];
            PlaySong();
        }
        public void PlayPrevious()
        {
            if (SongsList.Count == 0)
            {
                return;
            }
            if (currentIndex > 0)
            {
                currentIndex--;
            }
            else
            {
                currentIndex = SongsList.Count - 1;
            }
            selectSongs = SongsList[currentIndex];
            PlaySong();
        }
        public bool IsPlaying()
        {
            return player.playState == WMPPlayState.wmppsPlaying;
        }
        public double GetCurrentPosition()
        {
            return player.controls.currentPosition;
        }
        public double GetSongDuration()
        {
            return player.currentMedia.duration;
        }

        public void SetPosition(double position)
        {
            player.controls.currentPosition = position;
        }

    }
}

