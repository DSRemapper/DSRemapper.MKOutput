using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DSRemapper.MKOutput.Linux
{
    internal abstract class UInputDevice : IDisposable
    {
        // --------------------------------------------------------------------
        // uinput constants
        // --------------------------------------------------------------------
        protected const int UINPUT_MAX_NAME_SIZE = 80;
        protected const int UI_DEV_SETUP    = 0x405C5503;
        protected const int UI_DEV_CREATE   = 0x5501;
        protected const int UI_DEV_DESTROY  = 0x5502;
        protected const int UI_SET_EVBIT    = 0x40045564;
        protected const int UI_SET_KEYBIT   = 0x40045565;
        protected const int UI_SET_RELBIT   = 0x40045566;
        protected const int UI_SET_ABSBIT   = 0x40045567;
        protected const int UI_ABS_SETUP    = 0x40185504;

        protected const ushort EV_SYN = 0x00;
        protected const ushort EV_KEY = 0x01;
        protected const ushort EV_REL = 0x02;
        protected const ushort EV_ABS = 0x03;

        // --------------------------------------------------------------------
        // Native structs
        // --------------------------------------------------------------------
        [StructLayout(LayoutKind.Sequential)]
        protected struct uinput_setup
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = UINPUT_MAX_NAME_SIZE)]
            public string name;
            public uint id_bustype;
            public uint id_vendor;
            public uint id_product;
            public uint id_version;
            public uint ff_effects_max;
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct input_event
        {
            public long tv_sec;
            public long tv_usec;
            public ushort type;
            public ushort code;
            public int value;
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct input_absinfo
        {
            public int value;
            public int minimum;
            public int maximum;
            public int fuzz;
            public int flat;
            public int resolution;
        }

        // --------------------------------------------------------------------
        // P/Invoke
        // --------------------------------------------------------------------
        [DllImport("libc", SetLastError = true)]
        private static extern int open(string pathname, int flags);

        [DllImport("libc", SetLastError = true)]
        private static extern int close(int fd);

        [DllImport("libc", SetLastError = true)]
        private static extern int write(int fd, byte[] buf, int count);

        [DllImport("libc", SetLastError = true)]
        private static extern int ioctl(int fd, int request, ref uinput_setup usetup);

        [DllImport("libc", SetLastError = true)]
        private static extern int ioctl(int fd, int request, int bit);

        [DllImport("libc", SetLastError = true)]
        private static extern int ioctl(int fd, int request, ref input_absinfo absinfo);

        private const int O_WRONLY   = 0x0001;
        private const int O_NONBLOCK = 0x0004;

        // --------------------------------------------------------------------
        // Fields
        // --------------------------------------------------------------------
        protected int Fd { get; private set; } = -1;
        private bool _disposed = false;

        // --------------------------------------------------------------------
        // Constructor – called by derived classes
        // --------------------------------------------------------------------
        protected UInputDevice(string deviceName)
        {
            Fd = open("/dev/uinput", O_WRONLY | O_NONBLOCK);
            if (Fd < 0) Throw("open /dev/uinput");

            try
            {
                ConfigureDevice();
                CreateDevice(deviceName);
            }
            catch
            {
                Cleanup();
                throw;
            }
        }

        // Derived classes override this to set EVBIT, KEYBIT, RELBIT, ABSBIT
        protected virtual void ConfigureDevice() { }

        private void CreateDevice(string name)
        {
            var usetup = new uinput_setup
            {
                name = name,
                id_bustype = 0x03, // BUS_USB
                id_vendor  = 0x1,
                id_product = 0x1,
                id_version = 0x100,
                ff_effects_max = 0
            };

            if (ioctl(Fd, UI_DEV_SETUP, ref usetup) < 0)
                Throw("UI_DEV_SETUP");

            if (ioctl(Fd, UI_DEV_CREATE, ref usetup) < 0)
                Throw("UI_DEV_CREATE");
        }

        // --------------------------------------------------------------------
        // Event emission
        // --------------------------------------------------------------------
        protected void WriteEvent(ushort type, ushort code, int value)
        {
            var ev = new input_event
            {
                tv_sec  = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                tv_usec = (DateTimeOffset.UtcNow.Millisecond * 1000) % 1000000,
                type    = type,
                code    = code,
                value   = value
            };

            byte[] buf = new byte[Marshal.SizeOf<input_event>()];
            IntPtr ptr = Marshal.AllocHGlobal(buf.Length);
            Marshal.StructureToPtr(ev, ptr, false);
            Marshal.Copy(ptr, buf, 0, buf.Length);
            Marshal.FreeHGlobal(ptr);

            if (write(Fd, buf, buf.Length) < 0)
                Throw("write");
        }

        protected void SynReport() => WriteEvent(EV_SYN, 0, 0);

        // --------------------------------------------------------------------
        // Ioctl helpers
        // --------------------------------------------------------------------
        protected void SetEvBit(ushort bit)  => Ioctl(UI_SET_EVBIT,  bit);
        protected void SetKeyBit(ushort bit) => Ioctl(UI_SET_KEYBIT, bit);
        protected void SetRelBit(ushort bit) => Ioctl(UI_SET_RELBIT, bit);
        protected void SetAbsBit(ushort bit) => Ioctl(UI_SET_ABSBIT, bit);

        private void Ioctl(int request, int bit)
        {
            if (ioctl(Fd, request, bit) < 0)
                Throw($"ioctl 0x{request:X}");
        }

        protected void SetAbsParams(ushort code, int min, int max)
        {
            var info = new input_absinfo
            {
                minimum = min,
                maximum = max,
                value   = 0,
                fuzz    = 0,
                flat    = 0,
                resolution = 1
            };
            
            if (ioctl(Fd, UI_ABS_SETUP, ref info) < 0)
            {
                Console.WriteLine("UI_ABS_SETUP failed (old kernel?)");
            }
        }

        // --------------------------------------------------------------------
        // Cleanup
        // --------------------------------------------------------------------
        public void Dispose()
        {
            if (!_disposed)
            {
                Cleanup();
                _disposed = true;
            }
        }

        private void Cleanup()
        {
            if (Fd >= 0)
            {
                ioctl(Fd, UI_DEV_DESTROY, 0);
                close(Fd);
                Fd = -1;
            }
        }

        private static void Throw(string msg)
        {
            int err = Marshal.GetLastWin32Error();
            throw new InvalidOperationException($"{msg} (errno={err})");
        }
    }

    internal sealed class UInputKeyboard : UInputDevice
    {
        public UInputKeyboard(string name = "Virtual C# Keyboard") : base(name) { }

        protected override void ConfigureDevice()
        {
            // Enable key events and register keys
            SetEvBit(EV_KEY);

            foreach (var code in Enum.GetValues<LinuxKeycode>().Distinct())
                SetKeyBit((ushort)code);
        }

        public void SendKeyDown(ushort code) => Press(code, true);
        public void SendKeyUp(ushort code)   => Press(code, false);
        public void SendKey(ushort code, int delayMs = 10)
        {
            Press(code, true);
            Thread.Sleep(delayMs);
            Press(code, false);
        }

        public void SendString(string text, int keyDelayMs = 30)
        {
            foreach (char c in text)
            {
                ushort code = CharToKeyCode(c);
                if (code != 0)
                    SendKey(code, keyDelayMs);
            }
        }

        private void Press(ushort code, bool down)
        {
            WriteEvent(EV_KEY, code, down ? 1 : 0);
            SynReport();
        }

        private static ushort CharToKeyCode(char c)
        {
            if (c >= 'a' && c <= 'z') return (ushort)(30 + (c - 'a'));
            if (c >= 'A' && c <= 'Z') return (ushort)(30 + (c - 'A'));
            if (c >= '0' && c <= '9') return (ushort)(2 + (c - '0'));
            return c switch
            {
                ' ' => 57,
                '\n' => 28,
                '\t' => 15,
                '.' => 52,
                ',' => 51,
                _ => 0
            };
        }
    }

    internal sealed class UInputMouse : UInputDevice
    {
        // Buttons
        private const ushort BTN_LEFT   = 0x110;
        private const ushort BTN_RIGHT  = 0x111;
        private const ushort BTN_MIDDLE = 0x112;

        // Relative
        private const ushort REL_X      = 0x00;
        private const ushort REL_Y      = 0x01;
        private const ushort REL_WHEEL  = 0x08;
        private const ushort REL_HWHEEL = 0x07;

        // Absolute
        private const ushort ABS_X = 0x00;
        private const ushort ABS_Y = 0x01;
        private const int ABS_MAX = 65535;

        private readonly bool _absolute;

        public UInputMouse(string name = "Virtual C# Mouse", bool absolute = true)
            : base(name) { }
        protected override void ConfigureDevice()
        {
            SetEvBit(EV_KEY);
            SetEvBit(EV_REL);
            //if (_absolute) SetEvBit(EV_ABS);

            // Buttons
            SetKeyBit(BTN_LEFT);
            SetKeyBit(BTN_RIGHT);
            SetKeyBit(BTN_MIDDLE);

            // Relative axes
            SetRelBit(REL_X);
            SetRelBit(REL_Y);
            SetRelBit(REL_WHEEL);
            SetRelBit(REL_HWHEEL);

            // Absolute axes
            /*if (_absolute)
            {
                SetAbsBit(ABS_X);
                SetAbsBit(ABS_Y);
                SetAbsParams(ABS_X, 0, ABS_MAX);
                SetAbsParams(ABS_Y, 0, ABS_MAX);
            }*/
        }

        public void MoveTo(int x, int y)
        {
            if (!_absolute) throw new InvalidOperationException("MoveTo requires absolute mode");
            WriteEvent(EV_ABS, ABS_X, x);
            WriteEvent(EV_ABS, ABS_Y, y);
            SynReport();
        }

        public void MoveBy(int dx, int dy)
        {
            WriteEvent(EV_REL, REL_X, dx);
            WriteEvent(EV_REL, REL_Y, dy);
            SynReport();
        }

        public void ClickLeft()   => Click(BTN_LEFT);
        public void ClickRight()  => Click(BTN_RIGHT);
        public void ClickMiddle() => Click(BTN_MIDDLE);

        public void PressButton(ushort btn, bool down)
        {
            WriteEvent(EV_KEY, btn, down ? 1 : 0);
            SynReport();
        }

        public void ScrollVertical(int amount)   => Scroll(REL_WHEEL, amount);
        public void ScrollHorizontal(int amount) => Scroll(REL_HWHEEL, amount);

        public void Click(ushort btn, int delayMs = 50)
        {
            PressButton(btn, true);
            Thread.Sleep(delayMs);
            PressButton(btn, false);
        }

        private void Scroll(ushort axis, int amount)
        {
            WriteEvent(EV_REL, axis, amount);
            SynReport();
        }
    }
}