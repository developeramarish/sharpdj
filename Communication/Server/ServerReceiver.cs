﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Communication.Server;
using Communication.Shared;
using Hik.Communication.Scs.Communication.Messages;
using Hik.Communication.Scs.Server;

namespace Communication.Server
{
    public class ServerReceiver
    {
        public class MessagesPattern
        {
            /// <summary>
            /// <para>
            /// 1 - Login
            /// 2 - Password
            /// 3 - Email
            /// </para>
            /// </summary>
            public const string RegisterRgx = Commands.Register + "(.*) (.*) (.*)";

            /// <summary>
            /// LoginRgx
            /// <para>
            /// 1 - Login
            /// 2 - Password
            /// </para>
            /// </summary>
            public const string LoginRgx = Commands.Login + "(.*) (.*)";

            /// <summary>
            /// GroupsRgx:
            /// <para>
            /// 1 - Login <br />
            /// 2 - Password <br />
            /// 3 - New password <br />
            /// </para>
            /// </summary>
            public const string ChangePasswordRgx = Commands.UserAccount.ChangePassword + "(.*) (.*)";

            /// <summary>
            /// GroupsRgx:
            /// <para>
            /// 1 - Password <br />
            /// 2 - New username <br />
            /// </para>
            /// </summary>
            public const string ChangeUsernameRgx = Commands.UserAccount.ChangeUsername + "(.*) (.*)";

            /// <summary>
            /// GroupsRgx:
            /// <para>
            /// 1 - Password <br />
            /// 2 - New login <br />
            /// </para>
            /// </summary>
            public const string ChangeLoginRgx = Commands.UserAccount.ChangeLogin + "(.*) (.*)";

            /// <summary>
            /// GroupsRgx:
            /// <para>
            /// 1 - Password <br />
            /// 2 - New rank <br />
            /// </para>
            /// </summary>
            public const string ChangeRankRgx = Commands.UserAccount.ChangeRank + "(.*) (.*)";

        }

        public void ParseMessage(IScsServerClient client, string message)
        {
            #region Disconnect

            if (message.Equals(Commands.Disconnect))
            {
                OnDisconnect(new DisconnectEventArgs(client));
            }
            #endregion
            #region Login
            else if (message.StartsWith(Commands.Login))
            {
                Regex rgx = new Regex(ServerReceiver.MessagesPattern.LoginRgx);
                Match match = rgx.Match(message);
                if (match.Success)
                {
                    var login = match.Groups[1].Value;
                    var password = match.Groups[2].Value;

                    OnLogin(new LoginEventArgs(login, password, client));
                }
                else
                {
                    Console.WriteLine(Commands.Errors.LoginErr);
                    ServerSender.Error.LoginError(client);
                }
            }
            #endregion
            #region Register
            else if (message.StartsWith(Commands.Register))
            {
                Regex rgx = new Regex(MessagesPattern.RegisterRgx);
                Match match = rgx.Match(message);
                if (match.Success)
                {
                    var login = match.Groups[1].Value;
                    var password = match.Groups[2].Value;
                    var email = match.Groups[3].Value;

                    OnRegister(new RegisterEventArgs(login, password, email, client));
                }
                else
                {
                    Console.WriteLine("Fail in register");
                    ServerSender.Error.RegisterError(client);
                }
            }

            #endregion
            #region ChangePassword
            else if (message.StartsWith(Commands.UserAccount.ChangePassword))
            {
                Regex rgx = new Regex(MessagesPattern.ChangePasswordRgx);
                Match match = rgx.Match(message);
                if (match.Success)
                {
                    var password = match.Groups[1].Value;
                    var newPassword = match.Groups[2].Value;

                    OnChangePassword(new ChangePasswordEventArgs(client, password, newPassword));
                }
                else
                {
                    Console.WriteLine("Fail with changing password");
                    ServerSender.Error.ChangePasswordError(client);
                }
            }
            #endregion
            #region ChangeUsername
            else if (message.StartsWith(Commands.UserAccount.ChangeUsername))
            {
                Regex rgx = new Regex(MessagesPattern.ChangeUsernameRgx);
                Match match = rgx.Match(message);
                if (match.Success)
                {
                    var password = match.Groups[1].Value;
                    var newUsername = match.Groups[2].Value;

                    OnChangeUsername(new ChangeUsernameEventArgs(client, password, newUsername));
                }
                else
                {
                    Console.WriteLine("Fail with changing username");
                    ServerSender.Error.ChangeUsernameError(client);
                }
            }
            #endregion
            #region ChangeLogin
            else if (message.StartsWith(Commands.UserAccount.ChangeLogin))
            {
                Regex rgx = new Regex(MessagesPattern.ChangeLoginRgx);
                Match match = rgx.Match(message);
                if (match.Success)
                {
                    var password = match.Groups[1].Value;
                    var newLogin = match.Groups[2].Value;

                    OnChangeLogin(new ChangeLoginEventArgs(client, password, newLogin));
                }
                else
                {
                    Console.WriteLine("Fail with changing login");
                    ServerSender.Error.ChangeLoginError(client);
                }
            }
            #endregion
            #region ChangeRank
            else if (message.StartsWith(Commands.UserAccount.ChangeRank))
            {
                Regex rgx = new Regex(MessagesPattern.ChangeRankRgx);
                Match match = rgx.Match(message);
                if (match.Success)
                {
                    var password = match.Groups[1].Value;
                    var newRank = match.Groups[2].Value;

                    OnChangeRank(new ChangeRankEventArgs(client, password, (Rank)Enum.Parse(typeof(Rank), newRank, true)));
                }
                else
                {
                    Console.WriteLine("Fail with changing rank");
                    ServerSender.Error.ChangeRankError(client);
                }
            }
            #endregion
            #region GetPeoples
            else if (message.Equals(Commands.GetPeoples))
            {
                OnGetPeople(new GetPeopleEventArgs(client));
            }
            #endregion
        }
        #region Methods

        #endregion

        #region Events
        public event EventHandler<DisconnectEventArgs> Disconnect;
        public event EventHandler<LoginEventArgs> Login;
            public event EventHandler<RegisterEventArgs> Register;
        public event EventHandler<GetPeopleEventArgs> GetPeople;
      

        #region UserAccount
        public event EventHandler<ChangePasswordEventArgs> ChangePassword;
        public event EventHandler<ChangeUsernameEventArgs> ChangeUsername;
        public event EventHandler<ChangeLoginEventArgs> ChangeLogin;
        public event EventHandler<ChangeRankEventArgs> ChangeRank;


        protected virtual void OnChangeRank(ChangeRankEventArgs e)
        {
            var handler = ChangeRank;
            handler?.Invoke(this, e);
        }
        protected virtual void OnChangeLogin(ChangeLoginEventArgs e)
        {
            var handler = ChangeLogin;
            handler?.Invoke(this, e);
        }
        protected virtual void OnChangeUsername(ChangeUsernameEventArgs e)
        {
            var handler = ChangeUsername;
            handler?.Invoke(this, e);
        }
        protected virtual void OnChangePassword(ChangePasswordEventArgs e)
        {
            var handler = ChangePassword;
            handler?.Invoke(this, e);
        }


        public class ChangeRankEventArgs : System.EventArgs
        {
            public ChangeRankEventArgs(IScsServerClient client, string password, Rank rank)
            {
                this.Client = client;
                this.Password = password;
                this.Rank = rank;
            }

            public IScsServerClient Client { get; private set; }
            public string Password { get; private set; }
            public Rank Rank { get; private set; }
        }
        public class ChangeLoginEventArgs : System.EventArgs
        {
            public ChangeLoginEventArgs(IScsServerClient client, string password, string newLogin)
            {
                this.Client = client;
                this.Password = password;
                this.NewLogin = newLogin;
            }

            public IScsServerClient Client { get; private set; }
            public string Password { get; private set; }
            public string NewLogin { get; private set; }
        }
        public class ChangeUsernameEventArgs : System.EventArgs
        {
            public ChangeUsernameEventArgs(IScsServerClient client, string password, string newUsername)
            {
                this.Client = client;
                this.Password = password;
                this.NewUsername = newUsername;
            }
            public IScsServerClient Client { get; private set; }
            public string Password { get; private set; }
            public string NewUsername { get; private set; }

        }
        public class ChangePasswordEventArgs : System.EventArgs
        {
            public ChangePasswordEventArgs(IScsServerClient client, string password, string newPassword)
            {
                this.Client = client;
                this.Password = password;
                this.NewPassword = newPassword;
            }

            public IScsServerClient Client { get; private set; }
            public string Password { get; private set; }
            public string NewPassword { get; private set; }
        }
        #endregion

        protected virtual void OnGetPeople(GetPeopleEventArgs e)
        {
            EventHandler<GetPeopleEventArgs> eh = GetPeople;
            eh?.Invoke(this, e);
        }
        internal virtual void OnDisconnect(DisconnectEventArgs e)
        {
            EventHandler<DisconnectEventArgs> eh = Disconnect;
            eh?.Invoke(this, e);
        }
        internal virtual void OnLogin(LoginEventArgs e)
        {
            EventHandler<LoginEventArgs> eh = Login;
            eh?.Invoke(this, e);
        }
        internal virtual void OnRegister(RegisterEventArgs e)
        {
            var handler = Register;
            handler?.Invoke(this, e);
        }

        public class RegisterEventArgs : System.EventArgs
        {
            public RegisterEventArgs(string login, string password, string email, IScsServerClient client)
            {
                this.Login = login;
                this.Password = password;
                this.Email = email;
                this.Client = client;
            }

            public string Login { get; private set; }
            public string Password { get; private set; }
            public string Email { get; private set; }
            public IScsServerClient Client { get; private set; }
        }
        public class LoginEventArgs : System.EventArgs
        {
            public LoginEventArgs(string login, string password, IScsServerClient client)
            {
                this.Login = login;
                this.Password = password;
                this.Client = client;
            }

            public string Login { get; private set; }
            public string Password { get; private set; }
            public IScsServerClient Client { get; private set; }
        }
        public class DisconnectEventArgs : System.EventArgs
        {
            public DisconnectEventArgs(IScsServerClient client)
            {
                this.Client = client;
            }

            public IScsServerClient Client { get; private set; }
        }
        public class GetPeopleEventArgs : System.EventArgs
        {
            public GetPeopleEventArgs(IScsServerClient client)
            {
                this.Client = client;
            }

            public IScsServerClient Client { get; private set; }
        }
        #endregion
    }
}