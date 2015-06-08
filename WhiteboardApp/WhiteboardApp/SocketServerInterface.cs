using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.Web;

namespace WhiteboardApp
{
    class SocketServerInterface
    {
        private readonly string SocketUrl = "ws://104.236.134.122:9090/ws";
        private MessageWebSocket messageWebSocket;
        private DataWriter messageWriter;

        public SocketServerInterface()
        {
            Connect();
        }

        public async void SendMessage(string message, StorageFile file)
        {
            messageWriter.WriteString(message);
            await messageWriter.StoreAsync();

            HttpServerInterface http = new HttpServerInterface();
            http.PostInkFile(file);
        }

        private async void Connect()
        {
            try
            {
                MessageWebSocket webSocket = messageWebSocket;

                if (webSocket == null)
                {
                    Uri server = new Uri(SocketUrl);
                    webSocket = new MessageWebSocket();
                    webSocket.Control.MessageType = SocketMessageType.Utf8;

                    //Set up Callbacks
                    webSocket.MessageReceived += MessageReceived;
                    webSocket.Closed += Closed;

                    await webSocket.ConnectAsync(server);
                    messageWebSocket = webSocket;
                    messageWriter = new DataWriter(webSocket.OutputStream);
                }
            }
            catch (Exception ex)
            {
                WebErrorStatus status = WebSocketError.GetStatus(ex.GetBaseException().HResult);
            }
        }

        private async void MessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            try
            {
                using (DataReader reader = args.GetDataReader())
                {
                    reader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                    string read = reader.ReadString(reader.UnconsumedBufferLength);

                    HttpServerInterface http = new HttpServerInterface();
                    await http.GetInkFile();
                }
            }
            catch (Exception ex)
            {
                WebErrorStatus status = WebSocketError.GetStatus(ex.GetBaseException().HResult);
            }
        }

        private void Closed(IWebSocket sender, WebSocketClosedEventArgs args)
        {
            // You can add code to log or display the code and reason
            // for the closure (stored in args.Code and args.Reason)

            // This is invoked on another thread so use Interlocked 
            // to avoid races with the Start/Close/Reset methods.
            MessageWebSocket webSocket = Interlocked.Exchange(ref messageWebSocket, null);
            if (webSocket != null)
            {
                webSocket.Dispose();
            }
        }
    }
}
