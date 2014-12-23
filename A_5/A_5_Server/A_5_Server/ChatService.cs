using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace A_5_Server
{
    public class ChatService : IChatServer
    {
        private static Dictionary<string, ChatUser> users = new Dictionary<string, ChatUser>();

        // 用户登录、记录、通知所有人
        public void Login(string name)
        {
            OperationContext context = OperationContext.Current;
            IChatServerCallback callback = context.GetCallbackChannel<IChatServerCallback>();
            ChatUser user = new ChatUser(name, callback);
            // 不允许用户名重复的登录
            if (ChatService.users.ContainsKey(name))
            {
                user.callback.LoginState(false);
                return;
            }

            // 添加进在线列表
            ChatService.users.Add(name, user);
            // 分发广播消息
            DeliverSystemMessageToAll(name, "进入聊天室");
            // 分发在线用户列表
            DeliverUserList();
            user.callback.LoginState(true);
            return;
        }

        // 用户登出、删除记录、通知所有人
        public void Logout(string name)
        {
            // 分发广播消息
            DeliverSystemMessageToAll(name, "退出聊天室");
            // 从在线列表里删除
            ChatService.users.Remove(name);
            // 分发在线用户列表
            DeliverUserList();
        }

        // 用户说话、通知所有人
        public void Talk(string name, string message)
        {
            DeliverMessageToAll(string.Format("[{0}] {1}:{2}", DateTime.Now, name, message));
        }

        // 系统广播，通知所有人
        private void DeliverSystemMessageToAll(string name, string message)
        {
            DeliverMessageToAll(string.Format("[{0}] [系统] 用户 {1} {2}", DateTime.Now, name, message));
        }

        // 给所有人分发消息
        private void DeliverMessageToAll(string message)
        {
            foreach (var u in ChatService.users.Values)
            {
                u.callback.ShowMessage(message);
            }
        }

        // 分发用户列表
        public void DeliverUserList()
        {
            List<string> userList;
            userList = ChatService.users.Keys.ToList();
            foreach (var v in ChatService.users.Values)
            {
                v.callback.ShowUsers(userList);
            }
        }
    }
}
