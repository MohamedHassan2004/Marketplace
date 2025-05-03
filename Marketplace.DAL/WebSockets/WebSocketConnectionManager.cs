using System.Net.WebSockets;
using System.Text;

namespace Marketplace.DAL.WebSockets
{
    public class WebSocketConnectionManager
    {
        private readonly Dictionary<string, WebSocket> _connections = new();

        public void AddConnection(string vendorId, WebSocket socket)
        {
            if (!_connections.ContainsKey(vendorId))
            {
                _connections[vendorId] = socket;
            }
        }

        public WebSocket? GetSocketByVendorId(string vendorId)
        {
            _connections.TryGetValue(vendorId, out var socket);
            return socket;
        }

        public void RemoveConnection(string vendorId)
        {
            if (_connections.ContainsKey(vendorId))
            {
                _connections.Remove(vendorId);
            }
        }

        public async Task BroadcastMessageAsync(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            var tasks = _connections.Values.Select(socket =>
                socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None));
            await Task.WhenAll(tasks);
        }

        public async Task HandleConnectionAsync(string vendorId, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    RemoveConnection(vendorId);
                }
                else if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    // Handle the received message (e.g., broadcast or process it)
                    await BroadcastMessageAsync($"Vendor {vendorId}: {message}");
                }
            }
        }
    }
}
