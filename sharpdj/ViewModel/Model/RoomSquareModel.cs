﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Communication.Client;
using Communication.Shared;
using Communication.Shared.Data;
using Hik.Communication.Scs.Communication.Messages;
using Newtonsoft.Json;
using SharpDj.Core;
using SharpDj.Enums.Menu;
using SharpDj.Logic.Helpers;
using YoutubeExplode;
using YoutubeExplode.Models;

namespace SharpDj.ViewModel.Model
{
    public class RoomSquareModel : BaseViewModel
    {
        #region .ctor
        public RoomSquareModel(SdjMainViewModel main)
        {
            SdjMainViewModel = main;
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
                OnPropertyChanged("SdjMainViewModel");
            }
        }

        private bool _isFavorite;
        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                if (_isFavorite == value) return;
                _isFavorite = value;
                OnPropertyChanged("IsFavorite");
            }
        }

        private int? _peopleInRoom;
        public int? PeopleInRoom
        {
            get => _peopleInRoom;
            set
            {
                if (_peopleInRoom == value) return;
                _peopleInRoom = value;
                OnPropertyChanged("PeopleInRoom");
            }
        }

        private int? _adminsInRoom;
        public int? AdminsInRoom
        {
            get => _adminsInRoom;
            set
            {
                if (_adminsInRoom == value) return;
                _adminsInRoom = value;
                OnPropertyChanged("AdminsInRoom");
            }
        }

        private string _roomName;
        public string RoomName
        {
            get => _roomName;
            set
            {
                if (_roomName == value) return;
                _roomName = value;
                OnPropertyChanged("RoomName");
            }
        }

        private string _roomDescription;
        public string RoomDescription
        {
            get => _roomDescription;
            set
            {
                if (_roomDescription == value) return;
                _roomDescription = value;
                OnPropertyChanged("RoomDescription");
            }
        }

        private string _hostName;
        public string HostName
        {
            get => _hostName;
            set
            {
                if (_hostName == value) return;
                _hostName = value;
                OnPropertyChanged("HostName");
            }
        }


        private int _roomId;
        public int RoomId
        {
            get => _roomId;
            set
            {
                if (_roomId == value) return;
                _roomId = value;
                OnPropertyChanged("RoomId");
            }
        }

        private bool _isMouseOver;
        public bool IsMouseOver
        {
            get => _isMouseOver;
            set
            {
                if (_isMouseOver == value) return;
                _isMouseOver = value;
                OnPropertyChanged("IsMouseOver");
            }
        }

        #endregion Properties

        #region Methods

        private  void GetInfoAboutRoom(Room.InsindeInfo inside)
        {
            SdjMainViewModel.SdjBottomBarViewModel.BottomBarRoomId = RoomId;
            SdjMainViewModel.MainViewVisibility = MainView.Room;
            SdjMainViewModel.SdjRoomViewModel.RoomId = RoomId;
            SdjMainViewModel.SdjBottomBarViewModel.BottomBarRoomName = RoomName;
            SdjMainViewModel.SdjRoomViewModel.RoomName = RoomName;
            SdjMainViewModel.SdjRoomViewModel.HostName = HostName;
            SdjMainViewModel.SdjRoomViewModel.UserList = new SdjRoomViewModel.MyUserList(SdjMainViewModel);
            inside.Clients.ForEach(x => SdjMainViewModel.SdjRoomViewModel.UserList.Add(x));

            SdjMainViewModel.SdjBottomBarViewModel.BottomBarSizeOfPlaylistInRoom = inside.Djs.Count;
            SdjMainViewModel.SdjBottomBarViewModel.BottomBarMaxSizeOfPlaylistInRoom = 30;
            SdjMainViewModel.SdjRoomViewModel.SongsQueue = (sbyte)inside.Djs.SelectMany(dj => dj.Track).Count();

            var tmp = YoutubeSingleton.Instance.YtClient.GetVideoAsync(inside.Djs[0].Track[0].Id).Result;
            SdjMainViewModel.SdjRoomViewModel.SongTitle = tmp.Title;
            SdjMainViewModel.SdjBottomBarViewModel.BottomBarTitleOfActuallySong = tmp.Title;
        }

        #endregion Methods

        #region Commands

        #region MainOnRoomClickCommand
        private RelayCommand _mainOnRoomClickCommand;
        public RelayCommand MainOnRoomClickCommand
        {
            get
            {
                return _mainOnRoomClickCommand
                       ?? (_mainOnRoomClickCommand = new RelayCommand(MainOnRoomClickCommandExecute, MainOnRoomClickCommandCanExecute));
            }
        }

        public bool MainOnRoomClickCommandCanExecute()
        {
            return true;
        }

        public void MainOnRoomClickCommandExecute()
        {
            Task.Factory.StartNew(() =>
            {
                var resp = SdjMainViewModel.Client.Sender.RoomJoin(RoomId);
                if (string.IsNullOrEmpty(resp)) return;

                var inside = JsonConvert.DeserializeObject<Room.InsindeInfo>(resp);
                GetInfoAboutRoom(inside);
            });
        }
        #endregion

        #region SetRoomAsFavoriteCommand
        private RelayCommand _setRoomAsFavoriteCommand;
        public RelayCommand SetRoomAsFavoriteCommand
        {
            get
            {
                return _setRoomAsFavoriteCommand
                       ?? (_setRoomAsFavoriteCommand = new RelayCommand(SetRoomAsFavoriteCommandExecute, SetRoomAsFavoriteCommandCanExecute));
            }
        }

        public bool SetRoomAsFavoriteCommandCanExecute()
        {
            return true;
        }

        public void SetRoomAsFavoriteCommandExecute()
        {
            IsFavorite = !IsFavorite;
        }
        #endregion

        #region OnMouseEnterCommand
        private RelayCommand _onMouseEnterCommand;
        public RelayCommand OnMouseEnterCommand
        {
            get
            {
                return _onMouseEnterCommand
                       ?? (_onMouseEnterCommand = new RelayCommand(OnMouseEnterCommandExecute, OnMouseEnterCommandCanExecute));
            }
        }

        public bool OnMouseEnterCommandCanExecute()
        {
            return true;
        }

        public void OnMouseEnterCommandExecute()
        {
            IsMouseOver = true;
        }
        #endregion

        #region OnMouseLeaveCommand
        private RelayCommand _onMouseLeaveCommand;
        public RelayCommand OnMouseLeaveCommand
        {
            get
            {
                return _onMouseLeaveCommand
                       ?? (_onMouseLeaveCommand = new RelayCommand(OnMouseLeaveCommandExecute, OnMouseLeaveCommandCanExecute));
            }
        }

        public bool OnMouseLeaveCommandCanExecute()
        {
            return true;
        }

        public void OnMouseLeaveCommandExecute()
        {
            IsMouseOver = false;
        }
        #endregion

        #endregion Commands
    }
}
