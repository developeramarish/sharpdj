﻿using Caliburn.Micro;
using SharpDj.Interfaces;
using SharpDj.Models;
using SharpDj.PubSubModels;
using SharpDj.ViewModels.SubViews.MainViewComponents.PlaylistViewComponents;
using System;

namespace SharpDj.ViewModels.SubViews.MainViewComponents
{
    public class PlaylistViewModel : PropertyChangedBase,
        INavMainView,
        IHandle<IPlaylistCollectionChanged>, IHandle<INewPlaylistCreated>
    {
        private readonly IEventAggregator _eventAggregator;

        #region Properties
        public SearchNewMediaDialogViewModel SearchNewMediaDialogViewModel { get; private set; }
        public PlaylistCreationViewModel PlaylistCreationViewModel { get; private set; }

        private BindableCollection<PlaylistModel> _playlistCollection = new BindableCollection<PlaylistModel>();
        public BindableCollection<PlaylistModel> PlaylistCollection
        {
            get => _playlistCollection;
            set
            {
                if (_playlistCollection == value) return;
                _playlistCollection = value;
                NotifyOfPropertyChange(() => PlaylistCollection);
            }
        }

        private BindableCollection<TrackModel> _trackCollection = new BindableCollection<TrackModel>();
        public BindableCollection<TrackModel> TrackCollection
        {
            get => _trackCollection;
            set
            {
                if (_trackCollection == value) return;
                _trackCollection = value;
                NotifyOfPropertyChange(() => TrackCollection);
            }
        }

        private bool _playlistCreationIsVisible;
        public bool PlaylistCreationIsVisible
        {
            get => _playlistCreationIsVisible;
            set
            {
                if (_playlistCreationIsVisible == value) return;
                _playlistCreationIsVisible = value;
                NotifyOfPropertyChange(() => PlaylistCreationIsVisible);
            }
        }

        #endregion Properties

        #region .ctor
        public PlaylistViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            SearchNewMediaDialogViewModel = new SearchNewMediaDialogViewModel(_eventAggregator);
            PlaylistCreationViewModel = new PlaylistCreationViewModel(_eventAggregator);
        }

        public PlaylistViewModel()
        {
            SearchNewMediaDialogViewModel = new SearchNewMediaDialogViewModel();
            PlaylistCreationViewModel = new PlaylistCreationViewModel();
        }
        #endregion .ctor

        public void OnActivePlaylistChanged(PlaylistModel model)
        {
            TrackCollection = model.TrackCollection;
        }

        public void SetPlaylistCreationVisibility(int isPlaylistCreation)
        {
            PlaylistCreationIsVisible = Convert.ToBoolean(isPlaylistCreation);
        }

        #region Handlers
        public void Handle(IPlaylistCollectionChanged message)
        {
            PlaylistCollection = new BindableCollection<PlaylistModel>(message.PlaylistCollection);
        }

        public void Handle(INewPlaylistCreated message)
        {
            PlaylistCollection.Add(message.Model);
        }
        #endregion Handlers

    }
}
