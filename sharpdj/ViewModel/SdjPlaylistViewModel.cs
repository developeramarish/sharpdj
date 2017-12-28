﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SharpDj.Enums;
using SharpDj.ViewModel.Model;

namespace SharpDj.ViewModel
{
    public class SdjPlaylistViewModel : BaseViewModel
    {
        #region .ctor

        public SdjPlaylistViewModel(SdjMainViewModel main)
        {
            SdjMainViewModel = main;

            for (int i = 0; i < 10; i++)
            {
                var tracks = new ObservableCollection<PlaylistTrackModel>();
                for (int j = 0; j < i; j++)
                {
                    tracks.Add(new PlaylistTrackModel(main) { AuthorName = "Crisey " + i + j, SongDuration = "3:20", SongName = "Monstercat jakis tam" });
                    tracks.Add(new PlaylistTrackModel(main) { AuthorName = "Zonk " + j + i + 2, SongDuration = "2:13", SongName = "Tylko chińskie xD" });
                }
                PlaylistCollection.Add(new PlaylistModel(main) { PlaylistName = "Chińska playlista", Tracks = tracks });
                PlaylistCollection.Add(new PlaylistModel(main) { PlaylistName = "Zonkowate cos", Tracks = tracks });
            }

        }

        #endregion .ctor

        #region Properties

        private ObservableCollection<PlaylistTrackModel> _trackCollection;

        public ObservableCollection<PlaylistTrackModel> TrackCollection
        {
            get => _trackCollection;
            set
            {
                if (_trackCollection == value) return;
                _trackCollection = value;
                OnPropertyChanged("TrackCollection");
            }
        }
        public ObservableCollection<PlaylistModel> PlaylistCollection { get; set; } = new ObservableCollection<PlaylistModel>();


        private SdjMainViewModel _sdjMainViewModel;
        public SdjMainViewModel SdjMainViewModel
        {
            get => _sdjMainViewModel;
            set
            {
                if (_sdjMainViewModel == value) return;
                _sdjMainViewModel = value;
                OnPropertyChanged("SdjMainViewModel");
            }
        }


        private Playlist _playlistVisibility = Playlist.Collapsed;
        public Playlist PlaylistVisibility
        {
            get => _playlistVisibility;
            set
            {
                if (_playlistVisibility == value) return;
                _playlistVisibility = value;
                OnPropertyChanged("PlaylistVisibility");
            }
        }

        #endregion Properties

        #region Methods


        #endregion Methods

        #region Commands

        #region ActivatePlaylistCommand
        private RelayCommand _activatePlaylistCommand;
        public RelayCommand ActivatePlaylistCommand
        {
            get
            {
                return _activatePlaylistCommand
                       ?? (_activatePlaylistCommand = new RelayCommand(ActivatePlaylistCommandExecute, ActivatePlaylistCommandCanExecute));
            }
        }

        public bool ActivatePlaylistCommandCanExecute()
        {
            return true;
        }

        public void ActivatePlaylistCommandExecute()
        {
            var setActive = PlaylistCollection.FirstOrDefault(x => x.IsSelected);
            if (setActive != null)
            {
                foreach (var playlist in PlaylistCollection)
                {
                    playlist.IsActive = false;
                }
                setActive.IsActive = true;
            }
        }
        #endregion

        #region PlaylistAddModel
        private RelayCommand _playlistAddModel;
        public RelayCommand PlaylistAddModel
        {
            get
            {
                return _playlistAddModel
                       ?? (_playlistAddModel = new RelayCommand(PlaylistAddModelExecute, PlaylistAddModelCanExecute));
            }
        }

        public bool PlaylistAddModelCanExecute()
        {
            return true;
        }

        public void PlaylistAddModelExecute()
        {

        }
        #endregion

        #region PlaylistDeleteModel
        private RelayCommand _playlistDeleteModel;
        public RelayCommand PlaylistDeleteModel
        {
            get
            {
                return _playlistDeleteModel
                       ?? (_playlistDeleteModel = new RelayCommand(PlaylistDeleteModelExecute, PlaylistDeleteModelCanExecute));
            }
        }

        public bool PlaylistDeleteModelCanExecute()
        {
            return true;
        }

        public void PlaylistDeleteModelExecute()
        {

        }
        #endregion

        #region PlaylistEditModel
        private RelayCommand _playlistEditModel;
        public RelayCommand PlaylistEditModel
        {
            get
            {
                return _playlistEditModel
                       ?? (_playlistEditModel = new RelayCommand(PlaylistEditModelExecute, PlaylistEditModelCanExecute));
            }
        }

        public bool PlaylistEditModelCanExecute()
        {
            return true;
        }

        public void PlaylistEditModelExecute()
        {

        }
        #endregion

        #endregion Commands
    }
}
