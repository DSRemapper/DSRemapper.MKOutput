using DSRemapper.Core;
using DSRemapper.Types;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;

namespace DSRemapper.MKLinuxOutput
{
    /// <summary>
    /// Keyboard controller class
    /// </summary>
    [EmulatedController("Keyboard/ydotool")]
    public class Keyboard : IDSROutputController
    {
        private static readonly DSRLogger logger = DSRLogger.GetLogger("DSRemapper.MKLinuxOutput/ydotool");

        private readonly YDoToolClient ydotool;

        /// <inheritdoc/>
        public bool IsConnected { get; private set; }
        /// <inheritdoc/>
        public IDSRInputReport State { get => null!; set => _ = value; }
        /// <summary>
        /// Keyboard controller class constructor
        /// </summary>
        public Keyboard()
        {
            ydotool = new(logger.Logger);
        }
        /// <inheritdoc/>
        public void Connect()
        {
            IsConnected = ydotool.IsConnected;
        }
        /// <inheritdoc/>
        public void Disconnect()
        {
            IsConnected = false;
        }
        /// <summary>
        /// Gets the enumerations of all the keycodes supported by ydotool
        /// </summary>
        /// <returns>The <see cref="LinuxKeycode"/> enumeration type</returns>
        [CustomMethod("KeyCodes")]
        public Dictionary<string,ushort> KeyCodes(){
            return Enum.GetValues<LinuxKeycode>().Cast<LinuxKeycode>()
                .ToDictionary(t=> t.ToString(), t => (ushort)t);
        }
        /// <summary>
        /// Gets the enumerations of all the mouse keycodes supported by ydotool
        /// </summary>
        /// <returns>The <see cref="MouseButton"/> enumeration type</returns>
        [CustomMethod("MouseCodes")]
        public Dictionary<string,ushort> MouseCodes(){
            return Enum.GetValues<MouseButton>().Cast<MouseButton>()
                .ToDictionary(t=> t.ToString(), t => (ushort)t);
        }
        
        /// <inheritdoc cref="YDoToolClient.KeyDown(ushort)"/>
        [CustomMethod("KeyDown")]
        public Keyboard KeyDown(ushort keycode){
            if (IsConnected)
                ydotool.KeyDown(keycode);
            return this;
        }
        /// <inheritdoc cref="YDoToolClient.KeyUp(ushort)"/>
        [CustomMethod("KeyUp")]
        public Keyboard KeyUp(ushort keycode){
            if (IsConnected)
                ydotool.KeyUp(keycode);
            return this;
        }
        /// <inheritdoc cref="YDoToolClient.KeyPress(ushort)"/>
        [CustomMethod("KeyPress")]
        public Keyboard KeyPress(ushort keycode){
            if (IsConnected)
                ydotool.KeyPress(keycode);
            return this;
        }
        /// <inheritdoc cref="YDoToolClient.MouseMove(int, int)"/>
        [CustomMethod("MouseMove")]
        public Keyboard MouseMove(int x, int y){
            if (IsConnected)
                ydotool.MouseMove(x, y);
            return this;
        }
        /// <inheritdoc cref="YDoToolClient.MouseDown(ushort)"/>
        [CustomMethod("MouseDown")]
        public Keyboard MouseDown(ushort keycode){
            if (IsConnected)
                ydotool.MouseDown(keycode);
            return this;
        }
        /// <inheritdoc cref="YDoToolClient.MouseUp(ushort)"/>
        [CustomMethod("MouseUp")]
        public Keyboard MouseUp(ushort keycode){
            if (IsConnected)
                ydotool.MouseUp(keycode);
            return this;
        }
        /// <inheritdoc cref="YDoToolClient.MouseClick(ushort)"/>
        [CustomMethod("MousePress")]
        public Keyboard MousePress(ushort keycode){
            if (IsConnected)
                ydotool.MouseClick(keycode);
            return this;
        }
        /// <inheritdoc/>
        public void Dispose()
        {
            Disconnect();
            GC.SuppressFinalize(this);
        }
        /// <inheritdoc/>
        public IDSROutputReport GetFeedbackReport() => new DefaultDSROutputReport();
        /// <inheritdoc/>
        public void Update()
        {
            if (IsConnected)
                ydotool.ExecuteAsync();
        }

    }
}