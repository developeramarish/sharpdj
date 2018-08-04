﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDj.Core;

namespace SharpDj.ViewModel.Model
{
    public class PlaylistToAddTrack : BaseViewModel
    {
        #region .ctor
        public PlaylistToAddTrack(SdjMainViewModel main, PlaylistModel playlist, PlaylistTrackModel track)
        {
            SdjMainViewModel = main;
            PlaylistName = playlist.PlaylistName;
            TrackCount = playlist.TracksInPlaylist;
            MainPlaylistModel = playlist;
            Track = track;
        }
        #endregion .ctor

        #region Properties
        private SdjMainViewModel _sdjMainViewModel;
        public SdjMainViewModel SdjMainViewModel
        {
            get => _sdjMainViewModel;
            set
            {
                if (_sdjMainViewModel == value) return;
                _sdjMainViewModel = value;
            }
        }


        private string _playlistName;
        public string PlaylistName
        {
            get => _playlistName;
            set
            {
                if (_playlistName == value) return;
                _playlistName = value;
                OnPropertyChanged("PlaylistName");
            }
        }


        private int _trackCount;
        public int TrackCount
        {
            get => _trackCount;
            set
            {
                if (_trackCount == value) return;
                _trackCount = value;
                OnPropertyChanged("TrackCount");
            }
        }


        private PlaylistModel _mainPlaylistModel;
        public PlaylistModel MainPlaylistModel
        {
            get => _mainPlaylistModel;
            set
            {
                if (_mainPlaylistModel == value) return;
                _mainPlaylistModel = value;
                OnPropertyChanged("MainPlaylistModel");
            }
        }


        private PlaylistTrackModel _track;
        public PlaylistTrackModel Track
        {
            get => _track;
            set
            {
                if (_track == value) return;
                _track = value;
                OnPropertyChanged("Track");
            }
        }



        #endregion Properties

        #region Methods


        #endregion Methods

        #region Commands

        #region SelectPlaylistCommand
        private RelayCommand _selectPlaylistCommand;
        public RelayCommand SelectPlaylistCommand
        {
            get
            {
                return _selectPlaylistCommand
                       ?? (_selectPlaylistCommand = new RelayCommand(SelectPlaylistCommandExecute, SelectPlaylistCommandCanExecute));
            }
        }

        public bool SelectPlaylistCommandCanExecute()
        {
            return true;
        }

        public void SelectPlaylistCommandExecute()
        {
            var tmp = MainPlaylistModel.Tracks.FirstOrDefault(x => x.SongId.Equals(Track.SongId));
            if (tmp == null)
            {
                MainPlaylistModel.AddTrack(Track);
            }

            SdjMainViewModel.SdjAddTrackToPlaylistCollectionViewModel.SdjTitleBarForUserControlsViewModel
                .CloseFormExecute();
        }
        #endregion


        #endregion Commands


    }
}