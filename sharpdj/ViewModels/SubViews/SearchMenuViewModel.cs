﻿using System;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Caliburn.Micro;
using SharpDj.Enums;
using SharpDj.Models;
using SharpDj.Views.SubViews;

namespace SharpDj.ViewModels.SubViews
{
    public class SearchMenuViewModel : Screen, IHandle<RollingMenuVisibilityEnum>
    {
        private readonly IEventAggregator _eventAggregator;
        private SearchMenuView SearchMenuView { get; set; }
        public BindableCollection<ConversationModel> ConversationsCollection { get; private set; }

        public SearchMenuViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            var dicPic = "../../../Images/1.jpg";

            ConversationsCollection = new BindableCollection<ConversationModel>()
            {
                new ConversationModel(){IsReaded = false, Color = Brushes.DeepPink, Name = "Test Diggins", ImagePath = dicPic},
                new ConversationModel(){IsReaded = true, Color = Brushes.BlueViolet, Name = "Test Diggins", ImagePath = dicPic},
                new ConversationModel(){IsReaded = true, Color = Brushes.Black, Name = "Jeff Diggins", ImagePath = dicPic},
                new ConversationModel(){IsReaded = false, Color = Brushes.LimeGreen, Name = "Jeff Diggins", ImagePath = dicPic},
                new ConversationModel(){IsReaded = false, Color = Brushes.DeepSkyBlue, Name = "Test Diggins", ImagePath = dicPic},
                new ConversationModel(){IsReaded = false, Color = Brushes.Gray, Name = "Test Diggins", ImagePath = dicPic},
            };
        }

        public SearchMenuViewModel()
        {
            
        }

        public void ConversationClick(ConversationModel model)
        {
            model.IsOpen = !model.IsOpen;
        }

        public void ConversationDeleteClick(ConversationModel model)
        {
            ConversationsCollection.Remove(model);
        }

        public void Home()
        {
            _eventAggregator.PublishOnUIThread(NavigateMainView.Home);
        }

        public void ShowOptionsPanel()
        {
            _eventAggregator.PublishOnUIThread(RollingMenuVisibilityEnum.Options);
        }

        public void ShowConversationsPanel()
        {
            _eventAggregator.PublishOnUIThread(RollingMenuVisibilityEnum.Conversations);
        }

        public void Handle(RollingMenuVisibilityEnum message)
        {
            RollingMenuVisibility = RollingMenuVisibility != message ? message : RollingMenuVisibilityEnum.Void;
        }


        private RollingMenuVisibilityEnum _rollingMenuVisibility;
        public RollingMenuVisibilityEnum RollingMenuVisibility
        {
            get => _rollingMenuVisibility;
            set
            {
                if (_rollingMenuVisibility == value) return;
                _rollingMenuVisibility = value;
                NotifyOfPropertyChange(() => RollingMenuVisibility);
            }
        }
    }
}
