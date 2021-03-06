﻿using System;
using Caliburn.Micro;
using Network;
using SCPackets.Disconnect;
using SharpDj.Enums;
using SharpDj.PubSubModels;

namespace SharpDj.Logic.ActionToServer
{
    public class ClientDisconnectAction
    {
        private readonly IEventAggregator _eventAggregator;

        public ClientDisconnectAction(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Action(DisconnectResponse response, Connection connection)
        {
            if(response.Result == Result.Success)
                _eventAggregator.PublishOnUIThread(new NotLoggedIn());
        }
    }
}

