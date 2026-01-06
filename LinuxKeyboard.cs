using DSRemapper.Core;
using DSRemapper.Types;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;

namespace DSRemapper.MKOutput.Linux
{
    /// <summary>
    /// Keyboard controller class
    /// </summary>
    [EmulatedController("Keyboard/uinput")]
    public class Keyboard : IDSROutputController
    {
        private static readonly DSRLogger logger = DSRLogger.GetLogger("DSRemapper.MKLinuxOutput/ydotool");

        private (UInputMouse mouse, UInputKeyboard keyboard)? mk;

        /// <inheritdoc/>
        public bool IsConnected { get => mk != null; }
        /// <inheritdoc/>
        public IDSRInputReport State { get => null!; set => _ = value; }
        /// <summary>
        /// Keyboard controller class constructor
        /// </summary>
        public Keyboard()
        {

        }
        /// <inheritdoc/>
        public void Connect()
        {
            mk = (new UInputMouse("DSRemapper Mouse", false), new UInputKeyboard("DSRemapper Keyboard"));
        }
        /// <inheritdoc/>
        public void Disconnect()
        {
            mk?.keyboard.Dispose();
            mk?.mouse.Dispose();
            mk = null;
        }
        /// <summary>
        /// Gets the enumerations of all the keycodes supported by ydotool
        /// </summary>
        /// <returns>The <see cref="LinuxKeycode"/> enumeration type</returns>
        [CustomMethod("KeyCodes")]
        public Dictionary<string, ushort> KeyCodes()
        {
            return Enum.GetValues<LinuxKeycode>().Cast<LinuxKeycode>()
                .ToDictionary(t => t.ToString(), t => (ushort)t);
        }
        /// <summary>
        /// Gets the enumerations of all the mouse keycodes supported by ydotool
        /// </summary>
        /// <returns>The <see cref="MouseButton"/> enumeration type</returns>
        [CustomMethod("MouseCodes")]
        public Dictionary<string, ushort> MouseCodes()
        {
            return Enum.GetValues<MouseButton>().Cast<MouseButton>()
                .ToDictionary(t => t.ToString(), t => (ushort)t);
        }

        /// <summary>
        /// Queues a key-down (press) event for the specified key.
        /// </summary>
        /// <param name="keycode">The Linux keycode representing the key to press.</param>
        /// <returns>The current <see cref="Keyboard"/> instance for method chaining.</returns>
        [CustomMethod("KeyDown")]
        public Keyboard KeyDown(ushort keycode)
        {
            if (IsConnected)
                mk?.keyboard.SendKeyDown(keycode);
            return this;
        }
        /// <summary>
        /// Queues a key-up (release) event for the specified key.
        /// </summary>
        /// <param name="keycode">The Linux keycode representing the key to release.</param>
        /// <returns>The current <see cref="Keyboard"/> instance for method chaining.</returns>
        [CustomMethod("KeyUp")]
        public Keyboard KeyUp(ushort keycode)
        {
            if (IsConnected)
                mk?.keyboard.SendKeyUp(keycode);
            return this;
        }
        /// <summary>
        /// Queues a key press and release event sequence for the specified key.
        /// This simulates a single key tap.
        /// </summary>
        /// <param name="keycode">The Linux keycode representing the key to tap.</param>
        /// <returns>The current <see cref="Keyboard"/> instance for method chaining.</returns>
        [CustomMethod("KeyPress")]
        public Keyboard KeyPress(ushort keycode)
        {
            if (IsConnected)
                mk?.keyboard.SendKey(keycode);
            return this;
        }
        /// <summary>
        /// Queues a relative mouse movement event.
        /// </summary>
        /// <param name="x">The relative movement in the X direction (horizontal).</param>
        /// <param name="y">The relative movement in the Y direction (vertical).</param>
        /// <returns>The current <see cref="Keyboard"/> instance for method chaining.</returns>
        [CustomMethod("MouseMove")]
        public Keyboard MouseMove(int x, int y)
        {
            if (IsConnected)
                mk?.mouse.MoveBy(x, y);
            return this;
        }
        /// <summary>
        /// Queues a scroll movement event.
        /// </summary>
        /// <param name="x">The movement in the X direction (horizontal).</param>
        /// <param name="y">The movement in the Y direction (vertical).</param>
        /// <returns>The current <see cref="Keyboard"/> instance for method chaining.</returns>
        [CustomMethod("Scroll")]
        public Keyboard Scroll(int x, int y)
        {
            if (IsConnected)
            {
                mk?.mouse.ScrollVertical(y);
                mk?.mouse.ScrollHorizontal(x);
            }
            return this;
        }
        /// <summary>
        /// Queues a mouse button down (press) event.
        /// </summary>
        /// <param name="keycode">The <see cref="MouseButton"/> to press.</param>
        /// <returns>The current <see cref="Keyboard"/> instance for method chaining.</returns>
        [CustomMethod("MouseDown")]
        public Keyboard MouseDown(ushort keycode)
        {
            if (IsConnected)
                mk?.mouse.PressButton(keycode, true);
            return this;
        }
        /// <summary>
        /// Queues a mouse button up (release) event.
        /// </summary>
        /// <param name="keycode">The <see cref="MouseButton"/> to release.</param>
        /// <returns>The current <see cref="Keyboard"/> instance for method chaining.</returns>
        [CustomMethod("MouseUp")]
        public Keyboard MouseUp(ushort keycode)
        {
            if (IsConnected)
                mk?.mouse.PressButton(keycode, false);
            return this;
        }
        /// <summary>
        /// Queues a full mouse click (down then up) event sequence for the specified button.
        /// </summary>
        /// <param name="keycode">The <see cref="MouseButton"/> to click.</param>
        /// <returns>The current <see cref="Keyboard"/> instance for method chaining.</returns>
        [CustomMethod("MousePress")]
        public Keyboard MousePress(ushort keycode)
        {
            if (IsConnected)
                mk?.mouse.Click(keycode);
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
            
        }

    }
}