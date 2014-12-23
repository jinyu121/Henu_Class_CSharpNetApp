using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace A_5_Server
{
    [ServiceContract(
        Namespace = "A_5_Server",
        SessionMode=SessionMode.Required,
        CallbackContract=typeof(IChatServerCallback))]
    public interface IChatServer
    {
        [OperationContract(IsOneWay = true)]
        void Login(string name);

        [OperationContract(IsOneWay = true)]
        void Logout(string name);

        [OperationContract(IsOneWay = true)]
        void Talk(string name, string message);
    }

    public interface IChatServerCallback
    {
        [OperationContract(IsOneWay = true)]
        void ShowMessage(string message);

        [OperationContract(IsOneWay = true)]
        void ShowUsers(List<string> users);

        [OperationContract(IsOneWay = true)]
        void LoginState(Boolean state);
    }

    [DataContract]
    public class ChatUser
    {
        public string username { get; set; }
        public readonly IChatServerCallback callback;
        public ChatUser(string username, IChatServerCallback callback)
        {
            this.username = username;
            this.callback = callback;
        }
    }
}