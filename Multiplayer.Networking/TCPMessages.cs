﻿using RoWa;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using static Multiplayer.Networking.Helpers;

namespace Multiplayer.Networking
{
    [Serializable]
    /// <summary>
    /// Base message to send over the network. DO NOT USE!
    /// </summary>
    public class TcpMessage
    {
        public string Header = "";
        [XmlElement]
        public XML.XMLDictionary Data = new XML.XMLDictionary();

        public virtual byte[] Serialize()
        {
            return Helpers.Serialize(this);
        }

        public static TcpMessage Deserialize(byte[] array)
        {
            return Helpers.Deserialize<TcpMessage>(array);
        }
    }

    [Serializable]
    /// <summary>
    /// [Client Only] Login message used to send a login request from the client to the server
    /// </summary>
    public class TcpLogin : TcpMessage
    {
        public TcpLogin()
        {
        }

        /// <summary>
        /// [Client Only] Login message used to send a login request from the client to the server.
        /// Uses Helpers.GetUniqueID() as uniqueid to identify the user.
        /// </summary>
        /// <param name="username">The username which will be saved with the server</param>
        /// <param name="password">The servers password</param>
        public TcpLogin(string username, string password)
        {
            Header = "login";
            Data.Add("username", username);
            Data.Add("password", password);
            Data.Add("uniqueid", GetUniqueID());
        }

        public override byte[] Serialize()
        {
            return Helpers.Serialize(this);
        }

        public static new TcpLogin Deserialize(byte[] array)
        {
            return Helpers.Deserialize<TcpLogin>(array);
        }
    }

    [Serializable]
    /// <summary>
    /// [Client/Server] GameWorld message used to update the GameWorld. Can be used by the Server and the Client!
    /// </summary>
    public class TcpGameWorld : TcpMessage
    {
        public TcpGameWorld()
        {
        }

        /// <summary>
        /// [Client/Server] GameWorld message used to update the GameWorld. Can be used by the Server and the Client!
        /// </summary>
        /// <param name="worldchanges">GameWorld.Server.CompareWorlds() for Server or a new GameWorld.World with the changes for the client</param>
        /// <param name="isAddition">If the content from worldchanges should be added or removed from the GameWorld</param>
        public TcpGameWorld(GameWorld.World worldchanges, bool isAddition)
        {
            Header = "gameworld";
            Data.Add("addition", isAddition);
            Data.Add("changes", worldchanges);
        }

        public override byte[] Serialize()
        {
            return Helpers.Serialize(this);
        }

        public static new TcpGameWorld Deserialize(byte[] array)
        {
            return Helpers.Deserialize<TcpGameWorld>(array);
        }
    }

    [Serializable]
    /// <summary>
    /// [Server Only] A response from the server (For example a response to a TcpLogin message from the client)
    /// </summary>
    public class TcpResponse : TcpMessage
    {
        public TcpResponse() { }

        /// <summary>
        /// [Server Only] A response from the server (For example a response to a TcpLogin message from the client)
        /// </summary>
        /// <param name="type">The type of the response, for a TcpLogin response it would be "login" for example</param>
        /// <param name="response">The response as a string, if the password for the login is wrong it would be "wrong_password"</param>
        public TcpResponse(string type, object response)
        {
            Header = "response";
            Data.Add("type", type);
            Data.Add("data", response);
        }

        public override byte[] Serialize()
        {
            return Helpers.Serialize(this);
        }

        public static new TcpResponse Deserialize(byte[] array)
        {
            return Helpers.Deserialize<TcpResponse>(array);
        }
    }

    public enum TcpServerChatType
    {
        Info,
        Error,
        Warn,
    }

    [Serializable]
    public class TcpServerChat : TcpMessage
    {
        public TcpServerChat() { }
        public TcpServerChat(string message, TcpServerChatType type)
        {
            Header = "serverchat";
            Data.Add("message", message);
            Data.Add("type", type);
        }
        public override byte[] Serialize()
        {
            return Helpers.Serialize(this);
        }

        public static new TcpServerChat Deserialize(byte[] array)
        {
            return Helpers.Deserialize<TcpServerChat>(array);
        }
    }
    [Serializable]
    public class TcpPrivateChat : TcpMessage
    {
        public TcpPrivateChat(Helpers.User sender, string reciever, string message)
        {
            Header = "pm";
            Data.Add("sender", sender);
            Data.Add("reciever", reciever);
            Data.Add("message", message);
        }
        public override byte[] Serialize()
        {
            return Helpers.Serialize(this);
        }

        public static new TcpPrivateChat Deserialize(byte[] array)
        {
            return Helpers.Deserialize<TcpPrivateChat>(array);
        }
    }

    [Serializable]
    public class TcpChat : TcpMessage
    {
        public TcpChat() { }
        /**public TcpChat(string message, User receiver, User sender)
        {
            Header = "chat";
            Data.Add("isPrivate", true);
            Data.Add("sender", sender);
            Data.Add("receiver", receiver);
            Data.Add("message", message);
        }**/
        public TcpChat(string message, User sender = null)
        {
            Header = "chat";
            Data.Add("sender", sender);
            Data.Add("receiver", null);
            Data.Add("message", message);
        }

        public override byte[] Serialize()
        {
            return Helpers.Serialize(this);
        }

        public static new TcpChat Deserialize(byte[] array)
        {
            return Helpers.Deserialize<TcpChat>(array);
        }
    }

    [Serializable]
    public class TcpRequest : TcpMessage
    {
        public TcpRequest() { }

        public TcpRequest(string request)
        {
            Header = "request";
            Data.Add("request", request);
        }

        public override byte[] Serialize()
        {
            return Helpers.Serialize(this);
        }

        public static new TcpRequest Deserialize(byte[] array)
        {
            return Helpers.Deserialize<TcpRequest>(array);
        }
    }

    [Serializable]
    public class TcpData : TcpMessage
    {
        public TcpData() { }

        public TcpData(Dictionary<object, object> dict)
        {
            Header = "data";
            Data = new XML.XMLDictionary(dict);
        }

        public TcpData(params string[] keyvalues)
        {
            Header = "data";
            foreach (string keyvalue in keyvalues)
            {
                string k = keyvalue.Split('|')[0];
                string v = keyvalue.Split('|')[1];
                Data = new XML.XMLDictionary();
                Data.Add(k, v);
            }
        }

        public TcpData(string key, string value)
        {
            Header = "data";
            Data.Add(key, value);
        }

        public override byte[] Serialize()
        {
            return Helpers.Serialize(this);
        }

        public static new TcpData Deserialize(byte[] array)
        {
            return Helpers.Deserialize<TcpData>(array);
        }
    }

    [Serializable]
    public class TcpGamespeed : TcpMessage
    {
        public TcpGamespeed() { }
        /// <summary>
        /// Todo
        /// </summary>
        /// <param name="speed">The GameTime.GameSpeed object</param>
        /// <param name="type">The type of request. 0 = skip vote (should only be used by server/admin. 1 = request</param>
        public TcpGamespeed(int speed, int type = 0)
        {
            Header = "gamespeed";
            Data.Add("type", type);
            Data.Add("speed", speed);
        }

        public override byte[] Serialize()
        {
            return Helpers.Serialize(this);
        }

        public static new TcpGamespeed Deserialize(byte[] array)
        {
            return Helpers.Deserialize<TcpGamespeed>(array);
        }
    }

}
