﻿using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using SharpDj.Enums;
using SharpDj.Logic;
using SharpDj.Logic.Helpers;
using SharpDj.PubSubModels;
using SharpDj.ViewModels.SubViews;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpDj.ViewModels
{
    public sealed class ShellViewModel : Conductor<object>.Collection.OneActive,
        IHandle<ILoginPublish>, IHandle<IMessageQueue>, IHandle<IReconnect>, IHandle<INotLoggedIn>
    {
        #region Fields
        private readonly IEventAggregator _eventAggregator;
        private readonly Config _config;

        private ClientConnection _client;
        private bool _reconnecting;

        #endregion Fields

        #region .ctor
        public ShellViewModel()
        {
            _eventAggregator = new EventAggregator();
            _eventAggregator.Subscribe(this);

            _config = new Config(_eventAggregator)
                .BuildIfNotExist();

            while (!_config.Load())
            {
                Console.ReadLine();
            }

            MessagesQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(3750));

            TopMenuViewModel = new TopMenuViewModel();
            AfterLoginScreenViewModel = new AfterLoginScreenViewModel(_eventAggregator, _config);
            BeforeLoginScreenViewModel = new BeforeLoginScreenViewModel(_eventAggregator);

            ActivateItem(BeforeLoginScreenViewModel);

            _client = new ClientConnection(_eventAggregator, _config);
            Task.Factory.StartNew(() =>
            {
                _client.Init();
            });
        }
        #endregion .ctor

        #region ViewModels
        public AfterLoginScreenViewModel AfterLoginScreenViewModel { get; private set; }
        public BeforeLoginScreenViewModel BeforeLoginScreenViewModel { get; private set; }
        public TopMenuViewModel TopMenuViewModel { get; private set; }
        #endregion ViewModels

        #region Props

        private SnackbarMessageQueue _messagesQueue;
        public SnackbarMessageQueue MessagesQueue
        {
            get => _messagesQueue;
            set
            {
                if (_messagesQueue == value) return;
                _messagesQueue = value;
                NotifyOfPropertyChange(() => MessagesQueue);
            }
        }

        #endregion Props

        #region Handle's
        public void Handle(ILoginPublish message)
        {
            UserInfoSingleton.Instance.UserClient = message.Client;
            ActivateItem(AfterLoginScreenViewModel);
        }

        public void Handle(IMessageQueue message)
        {
            Task.Factory.StartNew(() =>
                MessagesQueue.Enqueue($"{message.ViewName}: {message.Message}"));
        }

        public void Handle(IReconnect message)
        {
            if (!_reconnecting)
            {
                _reconnecting = true;
                _client = new ClientConnection(_eventAggregator, _config);
                Task.Factory.StartNew(() =>
                {
                    _client.Init();
                    if (ActiveItem == AfterLoginScreenViewModel)
                        _eventAggregator.PublishOnUIThread(new NotLoggedIn());
                    _reconnecting = false;
                });
            }
        }

        public void Handle(INotLoggedIn message)
        {
            _eventAggregator.PublishOnUIThread(new MessageQueue("Connection", "You are not anymore logged in"));
            _eventAggregator.PublishOnUIThread(NavigateMainView.Home);
            ActivateItem(BeforeLoginScreenViewModel);
        }
        #endregion Handle's
    }
}
