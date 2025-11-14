using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace DSRemapper.MKLinuxOutput
{
    // --- Constants derived from Linux uinput headers ---
    internal static class UInputConstants
    {
        // Event Types
        public const ushort EV_SYN = 0x00;
        public const ushort EV_KEY = 0x01;
        public const ushort EV_REL = 0x02;

        // Synchronization Codes
        public const ushort SYN_REPORT = 0x00;

        // Relative Axis Codes
        public const ushort REL_X = 0x00;
        public const ushort REL_Y = 0x01;
    }

    /// <summary>
    /// Represents a raw Linux uinput event structure for communication with ydotoold.
    /// WARNING: This struct relies on specific assumptions about Linux 64-bit architecture (24-byte size) 
    /// to match the wire format (including timeval padding) used by the ydotoold daemon.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    internal struct InputEvent
    {
        // Padding for struct timeval (16 bytes on 64-bit Linux: 8s + 8us)
        [FieldOffset(16)]
        public ushort type;      // 2 bytes
        [FieldOffset(18)]
        public ushort code;      // 2 bytes
        [FieldOffset(20)]
        public int value;        // 4 bytes
    }


    /// <summary>
    /// A specialized client for interacting directly with the <c>ydotool</c> daemon socket,
    /// using an injected logger for all diagnostics.
    /// </summary>
    public class YDoToolClient : IDisposable
    {
        private const string DaemonPath = "/usr/bin/ydotoold";
        private const string SocketPath = "/tmp/.ydotool_socket";
        private readonly ILogger _logger;
        private Socket? daemonSocket;

        /// <summary>
        /// Gets a value indicating whether the client is currently connected to the ydotoold daemon socket.
        /// </summary>
        public bool IsConnected {get => daemonSocket?.Connected ?? false;}

        // Stores tuples of (type, code, value)
        private readonly Queue<(ushort type, ushort code, int value)> pendingEvents = new();

        /// <summary>
        /// Initializes the YDoToolClient and attempts to connect to the ydotoold socket.
        /// </summary>
        /// <param name="logger">The logger instance to use for diagnostic output.</param>
        public YDoToolClient(ILogger logger)
        {
            _logger = logger;
            ConnectSocket();
        }

        /// <summary>
        /// Finds the ydotoold socket path based on environment variables or default locations 
        /// and establishes a connection using a Unix Domain Socket.
        /// </summary>
        /// <exception cref="SocketException">Thrown if socket creation or connection fails.</exception>
        private void ConnectSocket()
        {
            Process.Start("pkill","ydotoold").WaitForExit();
            
            string uid = Environment.GetEnvironmentVariable("SUDO_UID") ?? "1000";
            string gid = Environment.GetEnvironmentVariable("SUDO_GID") ?? "1000";

            /*var uidProcess = Process.Start("id", "-u");
            uidProcess.WaitForExit();
            var uid = uidProcess.ExitCode; 

            var gidProcess = Process.Start("id", "-g");
            gidProcess.WaitForExit();
            var gid = gidProcess.ExitCode;*/
            _logger.LogInformation($"uid: {uid}, gid: {gid}");
            string daemonCommand = $"{DaemonPath} --socket-path=\"{SocketPath}\" --socket-own=\"{uid}:{gid}\"";

            // Use sh -c to execute the command string in the background
            Process.Start("sh", $"-c \"{daemonCommand} &\"");

            //Process.Start(DaemonPath,$"--socket-path=\"{SocketPath}\" --socket-own=\"$(id -u):$(id -g)\"");

            Thread.Sleep(1000);

            _logger.LogInformation($"Attempting to connect to ydotoold socket at: {SocketPath}");

            try
            {
                daemonSocket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
                var endpoint = new UnixDomainSocketEndPoint(SocketPath);
                daemonSocket.Connect(endpoint);
                _logger.LogInformation("Successfully connected to ydotoold socket.");
            }
            catch (Exception ex) when (ex is SocketException || ex is FileNotFoundException)
            {
                _logger.LogError($"Failed to connect socket '{SocketPath}': {ex.Message}");
                _logger.LogWarning("Please ensure 'ydotoold' is running and accessible (check permissions/socket path).");
                // Re-throw to indicate connection failure to the caller (DoTool)
                throw;
            }
        }

        /// <summary>
        /// Sends a raw uinput event to the ydotoold socket.
        /// </summary>
        private void UinputEmit(ushort type, ushort code, int value)
        {
            if (daemonSocket is null || !daemonSocket.Connected)
            {
                _logger.LogError("Socket not connected. Cannot send input event.");
                return;
            }

            // --- Event Struct Population ---
            InputEvent ie = new()
            {
                type = type,
                code = code,
                value = value
            };

            // Convert struct to byte array using marshalling
            byte[] buffer = new byte[Marshal.SizeOf<InputEvent>()];
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf<InputEvent>());
            Marshal.StructureToPtr(ie, ptr, false);
            Marshal.Copy(ptr, buffer, 0, Marshal.SizeOf<InputEvent>());
            Marshal.FreeHGlobal(ptr);

            // --- Send Event ---
            try
            {
                daemonSocket.Send(buffer);
                //_logger.Log(LogLevel.Debug, $"Sent Uinput: Type={type}, Code={code}, Value={value}");
            }
            catch (SocketException ex)
            {
                _logger.LogError($"Socket Error during send: {ex.Message}");
            }
        }
        
        
        /// <summary>
        /// Queues a key-up (release) event for the specified key.
        /// </summary>
        /// <param name="keyCode">The Linux keycode representing the key to release.</param>
        /// <returns>The current <see cref="YDoToolClient"/> instance for method chaining.</returns>
        public YDoToolClient KeyUp(ushort keyCode)
        {
            if (daemonSocket is not null && daemonSocket.Connected)
                daemonSocket.Send(Encoding.ASCII.GetBytes($"key {keyCode}:0"));
            //pendingEvents.Enqueue((UInputConstants.EV_KEY, (ushort)keyCode, 0));
            return this;
        }
        
        /// <summary>
        /// Queues a key-down (press) event for the specified key.
        /// </summary>
        /// <param name="keyCode">The Linux keycode representing the key to press.</param>
        /// <returns>The current <see cref="YDoToolClient"/> instance for method chaining.</returns>
        public YDoToolClient KeyDown(ushort keyCode)
        {
            if (daemonSocket is not null && daemonSocket.Connected)
                daemonSocket.Send(Encoding.ASCII.GetBytes($"key {keyCode}:1"));
            //pendingEvents.Enqueue((UInputConstants.EV_KEY, (ushort)keyCode, 1));
            return this;
        }
        /// <summary>
        /// Queues a key press and release event sequence for the specified key.
        /// This simulates a single key tap.
        /// </summary>
        /// <param name="keyCode">The Linux keycode representing the key to tap.</param>
        /// <returns>The current <see cref="YDoToolClient"/> instance for method chaining.</returns>
        public YDoToolClient KeyPress(ushort keyCode)
        {
            if (daemonSocket is not null && daemonSocket.Connected)
                daemonSocket.Send(Encoding.ASCII.GetBytes($"key {keyCode}:1 {keyCode}:0"));
            //pendingEvents.Enqueue((UInputConstants.EV_KEY, (ushort)keyCode, 1));
            //pendingEvents.Enqueue((UInputConstants.EV_KEY, (ushort)keyCode, 0));
            return this;
        }
        /// <summary>
        /// Queues a relative mouse movement event.
        /// </summary>
        /// <param name="x">The relative movement in the X direction (horizontal).</param>
        /// <param name="y">The relative movement in the Y direction (vertical).</param>
        /// <returns>The current <see cref="YDoToolClient"/> instance for method chaining.</returns>
        public YDoToolClient MouseMove(int x, int y)
        {
            if (x != 0)
            {
                //pendingEvents.Enqueue((UInputConstants.EV_REL, UInputConstants.REL_X, x));
            }
            if (y != 0)
            {
                //pendingEvents.Enqueue((UInputConstants.EV_REL, UInputConstants.REL_Y, y));
            }
            return this;
        }
/// <summary>
        /// Queues a mouse button down (press) event.
        /// </summary>
        /// <param name="button">The <see cref="MouseButton"/> to press.</param>
        /// <returns>The current <see cref="YDoToolClient"/> instance for method chaining.</returns>
        public YDoToolClient MouseDown(ushort button)
        {
            pendingEvents.Enqueue((UInputConstants.EV_KEY, (ushort)button, 1));
            return this;
        }
/// <summary>
        /// Queues a mouse button up (release) event.
        /// </summary>
        /// <param name="button">The <see cref="MouseButton"/> to release.</param>
        /// <returns>The current <see cref="YDoToolClient"/> instance for method chaining.</returns>
        public YDoToolClient MouseUp(ushort button)
        {
            pendingEvents.Enqueue((UInputConstants.EV_KEY, (ushort)button, 0));
            return this;
        }
/// <summary>
        /// Queues a full mouse click (down then up) event sequence for the specified button.
        /// </summary>
        /// <param name="button">The <see cref="MouseButton"/> to click.</param>
        /// <returns>The current <see cref="YDoToolClient"/> instance for method chaining.</returns>
        public YDoToolClient MouseClick(ushort button)
        {
            pendingEvents.Enqueue((UInputConstants.EV_KEY, (ushort)button, 1));
            pendingEvents.Enqueue((UInputConstants.EV_KEY, (ushort)button, 0));
            return this;
        }

        // --- Execution ---

        /// <summary>
        /// Executes all currently queued uinput events by sending them to the ydotoold socket.
        /// A synchronization report (<c>EV_SYN</c>) is sent after all events to ensure they are processed.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task ExecuteAsync()
        {
            if (daemonSocket is null || !daemonSocket.Connected)
            {
                _logger.LogError("Socket not connected. Cannot send input event.");
                return Task.CompletedTask;
            }
            //_logger.Log(LogLevel.Debug, $"Executing {pendingEvents.Count} pending uinput events.");

            // Send all queued events
            while (pendingEvents.TryDequeue(out var ev))
            {
                //UinputEmit(ev.type, ev.code, ev.value);
                //UinputEmit(UInputConstants.EV_SYN, UInputConstants.SYN_REPORT, 0);
                //var str = JsonSerializer.Serialize(new {cmd="key",args=$"{ev.code}:{ev.value}"});
                //List<byte> bytes = [..Encoding.ASCII.GetBytes(str)];
                //bytes.Add(0);
                //_logger.LogInformation($"Seding key: {ev.code}:{ev.value}");
                //Process.Start("sh", $"-c \"ydotool key {ev.code}:{ev.value}\"").WaitForExit();
                //daemonSocket.Send(Encoding.ASCII.GetBytes($"key {ev.code}:{ev.value}"));
            }

            // Send a single SYN_REPORT to synchronize all inputs (value 0)

            return Task.CompletedTask;
        }

        // --- IDisposable Implementation ---

        /// <summary>
        /// Closes the socket connection to the ydotoold daemon.
        /// </summary>
        public void Dispose()
        {
            if (daemonSocket != null)
            {
                if (daemonSocket.Connected)
                {
                    _logger.LogInformation("Shutting down ydotoold socket connection.");
                    daemonSocket.Shutdown(SocketShutdown.Both);
                }
                daemonSocket.Dispose();
                daemonSocket = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}