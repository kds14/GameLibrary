using System;
using Lidgren.Network;

/// <summary>
/// Basic peer to peer network.
/// </summary>
public class Network
{
    private readonly NetPeer peer;
    /// <summary>
    /// A function for logging purposes. Example: Debug.Log in Unity.
    /// </summary>
    private readonly Action<string> logMessage;

    private bool connected = false;

    public Network(Action<string> logMessage, int port)
    {
        NetPeerConfiguration config = new NetPeerConfiguration("DefaultConfig")
        {
            Port = port,
            AcceptIncomingConnections = true
        };

        peer = new NetPeer(config);
        peer.Start();

        this.logMessage = logMessage;
    }

    /// <summary>
    /// Connect to the other player. Only one player must do this.
    /// </summary>
    /// <param name="host">The host address</param>
    /// <param name="port">The host port</param>
    public void Connect(string host, int port)
    {
        peer.Connect(host, port);
    }

    /// <summary>
    /// Reads incoming messages. Call in a game loop.
    /// </summary>
    /// <param name="msgHandler">A function that handles data messages.</param>
    /// <param name="statusHandler">A function that handles status messages.</param>
    public void ReadMessages(Action<NetIncomingMessage> msgHandler,
        Action<NetConnectionStatus> statusHandler)
    {
        NetIncomingMessage msg;
        while ((msg = peer.ReadMessage()) != null)
        {
            switch (msg.MessageType)
            {
                case NetIncomingMessageType.StatusChanged:
                    NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                    if (status == NetConnectionStatus.Connected)
                    {
                        connected = true;
                    }
                    statusHandler(status);
                    break;
                case NetIncomingMessageType.ConnectionApproval:
                    if (connected)
                    {
                        msg.SenderConnection.Deny();
                    }
                    else
                    {
                        msg.SenderConnection.Approve();
                    }
                    break;
                case NetIncomingMessageType.VerboseDebugMessage:
                case NetIncomingMessageType.DebugMessage:
                case NetIncomingMessageType.WarningMessage:
                case NetIncomingMessageType.Error:
                    logMessage(msg.ReadString());
                    break;
                case NetIncomingMessageType.Data:
                    msgHandler(msg);
                    break;
                default:
                    logMessage($"Unhandled type: {msg.MessageType}");
                    break;
            }
            peer.Recycle(msg);
        }
    }

    /// <summary>
    /// Sends a message based on data serialized by a serialize function.
    /// </summary>
    /// <param name="serialize">The function that serializes the data.</param>
    public void SendMessage(Action<NetOutgoingMessage> serialize)
    {
        if (peer.ConnectionsCount > 0)
        {
            NetOutgoingMessage sendMsg = peer.CreateMessage();
            serialize(sendMsg);
            peer.SendMessage(sendMsg, peer.Connections, NetDeliveryMethod.ReliableOrdered, 0);
        }
    }

    /// <summary>
    /// Sends a simple string message.
    /// </summary>
    /// <param name="msg">The string message to send.</param>
    public void SendMessage(string msg)
    {
        if (peer.ConnectionsCount > 0)
        {
            NetOutgoingMessage sendMsg = peer.CreateMessage();
            sendMsg.Write(msg);
            peer.SendMessage(sendMsg, peer.Connections, NetDeliveryMethod.ReliableOrdered, 0);
        }
    }

    public void Shutdown(string bye = "Shutting down")
    {
        peer.Shutdown(bye);
    }
}
