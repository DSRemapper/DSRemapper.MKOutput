namespace  DSRemapper.MKOutput.Linux
{
    /// <summary>
    /// Defines standard mouse button codes.
    /// These values map to buttons 1, 2, and 3 used by xdotool, 
    /// and are converted to the appropriate keycodes for ydotool.
    /// </summary>
    public enum MouseButton
    {
        /// <summary>The primary mouse button (Left Click).</summary>
        LEFT = 0x110,
        /// <summary>The middle mouse button (Middle Click/Scroll Wheel).</summary>
        MIDDLE = 0x112,
        /// <summary>The secondary mouse button (Right Click).</summary>
        RIGHT = 0x111
    }

    /// <summary>
    /// Represents the Linux Event Keycodes (E_V_KEY) used for input synthesis,
    /// such as with the ydotool utility.
    /// </summary>
    public enum LinuxKeycode : ushort
    {
        // === Primary Keyboard Keys ===
        /// <summary>Escape Key</summary>
        ESC = 1,
        /// <summary>Number 1 Key (Row)</summary>
        KEY_1 = 2,
        /// <summary>Number 2 Key (Row)</summary>
        KEY_2 = 3,
        /// <summary>Number 3 Key (Row)</summary>
        KEY_3 = 4,
        /// <summary>Number 4 Key (Row)</summary>
        KEY_4 = 5,
        /// <summary>Number 5 Key (Row)</summary>
        KEY_5 = 6,
        /// <summary>Number 6 Key (Row)</summary>
        KEY_6 = 7,
        /// <summary>Number 7 Key (Row)</summary>
        KEY_7 = 8,
        /// <summary>Number 8 Key (Row)</summary>
        KEY_8 = 9,
        /// <summary>Number 9 Key (Row)</summary>
        KEY_9 = 10,
        /// <summary>Number 0 Key (Row)</summary>
        KEY_0 = 11,
        /// <summary>Minus / Hyphen Key</summary>
        MINUS = 12,
        /// <summary>Equals Key</summary>
        EQUAL = 13,
        /// <summary>Backspace Key</summary>
        BACKSPACE = 14,
        /// <summary>Tab Key</summary>
        TAB = 15,
        /// <summary>Q Key</summary>
        KEY_Q = 16,
        /// <summary>W Key</summary>
        KEY_W = 17,
        /// <summary>E Key</summary>
        KEY_E = 18,
        /// <summary>R Key</summary>
        KEY_R = 19,
        /// <summary>T Key</summary>
        KEY_T = 20,
        /// <summary>Y Key</summary>
        KEY_Y = 21,
        /// <summary>U Key</summary>
        KEY_U = 22,
        /// <summary>I Key</summary>
        KEY_I = 23,
        /// <summary>O Key</summary>
        KEY_O = 24,
        /// <summary>P Key</summary>
        KEY_P = 25,
        /// <summary>Left Bracket / Left Brace Key ([)</summary>
        LEFTBRACE = 26,
        /// <summary>Right Bracket / Right Brace Key (])</summary>
        RIGHTBRACE = 27,
        /// <summary>Enter / Return Key</summary>
        ENTER = 28,
        // /// <inheritdoc cref="ENTER"/>
        //RETURN = ENTER,
        /// <summary>Left Control Key</summary>
        LEFTCTRL = 29,
        /// <summary>A Key</summary>
        KEY_A = 30,
        /// <summary>S Key</summary>
        KEY_S = 31,
        /// <summary>D Key</summary>
        KEY_D = 32,
        /// <summary>F Key</summary>
        KEY_F = 33,
        /// <summary>G Key</summary>
        KEY_G = 34,
        /// <summary>H Key</summary>
        KEY_H = 35,
        /// <summary>J Key</summary>
        KEY_J = 36,
        /// <summary>K Key</summary>
        KEY_K = 37,
        /// <summary>L Key</summary>
        KEY_L = 38,
        /// <summary>Semicolon Key (;)</summary>
        SEMICOLON = 39,
        /// <summary>Apostrophe / Quote Key (')</summary>
        APOSTROPHE = 40,
        /// <summary>Grave Accent / Tilde Key (`/~)</summary>
        GRAVE = 41,
        /// <summary>Left Shift Key</summary>
        LEFTSHIFT = 42,
        /// <summary>Backslash Key (\)</summary>
        BACKSLASH = 43,
        /// <summary>Z Key</summary>
        KEY_Z = 44,
        /// <summary>X Key</summary>
        KEY_X = 45,
        /// <summary>C Key</summary>
        KEY_C = 46,
        /// <summary>V Key</summary>
        KEY_V = 47,
        /// <summary>B Key</summary>
        KEY_B = 48,
        /// <summary>N Key</summary>
        KEY_N = 49,
        /// <summary>M Key</summary>
        KEY_M = 50,
        /// <summary>Comma Key (,)</summary>
        COMMA = 51,
        /// <summary>Dot / Period Key (.)</summary>
        DOT = 52,
        /// <summary>Slash Key (/)</summary>
        SLASH = 53,
        /// <summary>Right Shift Key</summary>
        RIGHTSHIFT = 54,
        /// <summary>Numpad Asterisk / Multiply Key (*)</summary>
        KPASTERISK = 55,
        /// <summary>Left Alt Key</summary>
        LEFTALT = 56,
        /// <summary>Space Bar</summary>
        SPACE = 57,
        /// <summary>Caps Lock Key</summary>
        CAPSLOCK = 58,
        /// <summary>Function Key 1</summary>
        F1 = 59,
        /// <summary>Function Key 2</summary>
        F2 = 60,
        /// <summary>Function Key 3</summary>
        F3 = 61,
        /// <summary>Function Key 4</summary>
        F4 = 62,
        /// <summary>Function Key 5</summary>
        F5 = 63,
        /// <summary>Function Key 6</summary>
        F6 = 64,
        /// <summary>Function Key 7</summary>
        F7 = 65,
        /// <summary>Function Key 8</summary>
        F8 = 66,
        /// <summary>Function Key 9</summary>
        F9 = 67,
        /// <summary>Function Key 10</summary>
        F10 = 68,
        /// <summary>Num Lock Key</summary>
        NUMLOCK = 69,
        /// <summary>Scroll Lock Key</summary>
        SCROLLLOCK = 70,
        /// <summary>Numpad 7 Key</summary>
        KP7 = 71,
        /// <summary>Numpad 8 Key</summary>
        KP8 = 72,
        /// <summary>Numpad 9 Key</summary>
        KP9 = 73,
        /// <summary>Numpad Minus Key (-)</summary>
        KPMINUS = 74,
        /// <summary>Numpad 4 Key</summary>
        KP4 = 75,
        /// <summary>Numpad 5 Key</summary>
        KP5 = 76,
        /// <summary>Numpad 6 Key</summary>
        KP6 = 77,
        /// <summary>Numpad Plus Key (+)</summary>
        KPPLUS = 78,
        /// <summary>Numpad 1 Key</summary>
        KP1 = 79,
        /// <summary>Numpad 2 Key</summary>
        KP2 = 80,
        /// <summary>Numpad 3 Key</summary>
        KP3 = 81,
        /// <summary>Numpad 0 Key</summary>
        KP0 = 82,
        /// <summary>Numpad Dot Key (.)</summary>
        KPDOT = 83,

        // === Extended Keys ===
        /// <summary>Japanese Zenkaku/Hankaku Key</summary>
        ZENKAKUHANKAKU = 85,
        /// <summary>The key between LShift and Z on ISO keyboards</summary>
        KEY_102ND = 86,
        /// <summary>Function Key 11</summary>
        F11 = 87,
        /// <summary>Function Key 12</summary>
        F12 = 88,
        /// <summary>Japanese Ro Key (usually next to P)</summary>
        RO = 89,
        /// <summary>Japanese Katakana Conversion Key</summary>
        KATAKANA = 90,
        /// <summary>Japanese Hiragana Conversion Key</summary>
        HIRAGANA = 91,
        /// <summary>Japanese Henkan Key (Conversion)</summary>
        HENKAN = 92,
        /// <summary>Japanese Katakana/Hiragana Toggle</summary>
        KATAKANAHIRAGANA = 93,
        /// <summary>Japanese Muhenkan Key (Non-Conversion)</summary>
        MUHENKAN = 94,
        /// <summary>Keypad Japanese Comma</summary>
        KPJPCOMMA = 95,
        /// <summary>Keypad Enter Key (distinguished from main ENTER)</summary>
        KPEENTER = 96,
        /// <summary>Right Control Key</summary>
        RIGHTCTRL = 97,
        /// <summary>Keypad Slash / Divide Key (/)</summary>
        KPSLASH = 98,
        /// <summary>System Request / Print Screen Key</summary>
        SYSRQ = 99,
        /// <summary>Right Alt Key (often Alt Gr)</summary>
        RIGHTALT = 100,
        /// <summary>Line Feed (Legacy/Special Key)</summary>
        LINEFEED = 101,
        /// <summary>Home Key</summary>
        HOME = 102,
        /// <summary>Up Arrow Key</summary>
        UP = 103,
        /// <summary>Page Up Key</summary>
        PAGEUP = 104,
        /// <summary>Left Arrow Key</summary>
        LEFT = 105,
        /// <summary>Right Arrow Key</summary>
        RIGHT = 106,
        /// <summary>End Key</summary>
        END = 107,
        /// <summary>Down Arrow Key</summary>
        DOWN = 108,
        /// <summary>Page Down Key</summary>
        PAGEDOWN = 109,
        /// <summary>Insert Key</summary>
        INSERT = 110,
        /// <summary>Delete Key</summary>
        DELETE = 111,
        /// <summary>Macro Key</summary>
        MACRO = 112,
        /// <summary>Mute Volume Key</summary>
        MUTE = 113,
        /// <summary>Volume Down Key</summary>
        VOLUMEDOWN = 114,
        /// <summary>Volume Up Key</summary>
        VOLUMEUP = 115,
        /// <summary>System Power Down Key</summary>
        POWER = 116,
        /// <summary>Keypad Equals Key (=)</summary>
        KPEQUAL = 117,
        /// <summary>Keypad Plus/Minus Key</summary>
        KPPLUSMINUS = 118,
        /// <summary>Pause/Break Key</summary>
        PAUSE = 119,
        /// <summary>AL Compiz Scale (Expose) Key</summary>
        SCALE = 120,

        /// <summary>Keypad Comma Key (,)</summary>
        KPCOMMA = 121,
        /// <summary>Korean Hangeul/English Toggle Key</summary>
        HANGEUL = 122,
        // KEY_HANGUEL is an alias for KEY_HANGEUL (122)
        /// <summary>Korean Hanja Key</summary>
        HANJA = 123,
        /// <summary>Yen Key</summary>
        YEN = 124,
        /// <summary>Left Meta / Super / Windows Key</summary>
        LEFTMETA = 125,
        /// <summary>Right Meta / Super / Windows Key</summary>
        RIGHTMETA = 126,
        /// <summary>Compose Key (for generating special characters)</summary>
        COMPOSE = 127,

        // === Consumer Control and Multimedia Keys ===
        /// <summary>AC Stop</summary>
        STOP = 128,
        /// <summary>Again Key</summary>
        AGAIN = 129,
        /// <summary>AC Properties</summary>
        PROPS = 130,
        /// <summary>AC Undo</summary>
        UNDO = 131,
        /// <summary>Front Key</summary>
        FRONT = 132,
        /// <summary>AC Copy</summary>
        COPY = 133,
        /// <summary>AC Open</summary>
        OPEN = 134,
        /// <summary>AC Paste</summary>
        PASTE = 135,
        /// <summary>AC Find / Search</summary>
        FIND = 136,
        /// <summary>AC Cut</summary>
        CUT = 137,
        /// <summary>AL Integrated Help Center</summary>
        HELP = 138,
        /// <summary>Menu (show menu) Key</summary>
        MENU = 139,
        /// <summary>AL Calculator Launch Key</summary>
        CALC = 140,
        /// <summary>Setup Key</summary>
        SETUP = 141,
        /// <summary>SC System Sleep</summary>
        SLEEP = 142,
        /// <summary>System Wake Up</summary>
        WAKEUP = 143,
        /// <summary>AL Local Machine Browser / File Manager</summary>
        FILE = 144,
        /// <summary>Send File Key</summary>
        SENDFILE = 145,
        /// <summary>Delete File Key</summary>
        DELETEFILE = 146,
        /// <summary>Transfer Key</summary>
        XFER = 147,
        /// <summary>Program 1 Key</summary>
        PROG1 = 148,
        /// <summary>Program 2 Key</summary>
        PROG2 = 149,
        /// <summary>AL Internet Browser / WWW</summary>
        WWW = 150,
        /// <summary>MS-DOS Mode Key</summary>
        MSDOS = 151,
        /// <summary>AL Terminal Lock/Screensaver Key</summary>
        COFFEE = 152,
        // KEY_SCREENLOCK is an alias for KEY_COFFEE (152)
        /// <summary>Display orientation for e.g. tablets</summary>
        ROTATE_DISPLAY = 153,
        // KEY_DIRECTION is an alias for KEY_ROTATE_DISPLAY (153)
        /// <summary>Cycle Windows Key</summary>
        CYCLEWINDOWS = 154,
        /// <summary>Mail Client Launch Key</summary>
        MAIL = 155,
        /// <summary>AC Bookmarks</summary>
        BOOKMARKS = 156,
        /// <summary>Computer Key</summary>
        COMPUTER = 157,
        /// <summary>AC Back</summary>
        BACK = 158,
        /// <summary>AC Forward</summary>
        FORWARD = 159,
        /// <summary>Close CD Tray Key</summary>
        CLOSECD = 160,
        /// <summary>Eject CD Tray Key</summary>
        EJECTCD = 161,
        /// <summary>Eject/Close CD Tray Key</summary>
        EJECTCLOSECD = 162,
        /// <summary>Next Track Key</summary>
        NEXTSONG = 163,
        /// <summary>Play/Pause Toggle Key</summary>
        PLAYPAUSE = 164,
        /// <summary>Previous Track Key</summary>
        PREVIOUSSONG = 165,
        /// <summary>Stop CD Key</summary>
        STOPCD = 166,
        /// <summary>Record Key</summary>
        RECORD = 167,
        /// <summary>Rewind Key</summary>
        REWIND = 168,
        /// <summary>Media Select Telephone</summary>
        PHONE = 169,
        /// <summary>ISO Key</summary>
        ISO = 170,
        /// <summary>AL Consumer Control Configuration</summary>
        CONFIG = 171,
        /// <summary>AC Home Key</summary>
        HOMEPAGE = 172,
        /// <summary>AC Refresh Key</summary>
        REFRESH = 173,
        /// <summary>AC Exit Key</summary>
        EXIT = 174,
        /// <summary>Move Key</summary>
        MOVE = 175,
        /// <summary>Edit Key</summary>
        EDIT = 176,
        /// <summary>Scroll Up Key</summary>
        SCROLLUP = 177,
        /// <summary>Scroll Down Key</summary>
        SCROLLDOWN = 178,
        /// <summary>Keypad Left Parenthesis Key</summary>
        KPLEFTPAREN = 179,
        /// <summary>Keypad Right Parenthesis Key</summary>
        KPRIGHTPAREN = 180,
        /// <summary>AC New Key</summary>
        NEW = 181,
        /// <summary>AC Redo/Repeat Key</summary>
        REDO = 182,

        /// <summary>Function Key 13</summary>
        F13 = 183,
        /// <summary>Function Key 14</summary>
        F14 = 184,
        /// <summary>Function Key 15</summary>
        F15 = 185,
        /// <summary>Function Key 16</summary>
        F16 = 186,
        /// <summary>Function Key 17</summary>
        F17 = 187,
        /// <summary>Function Key 18</summary>
        F18 = 188,
        /// <summary>Function Key 19</summary>
        F19 = 189,
        /// <summary>Function Key 20</summary>
        F20 = 190,
        /// <summary>Function Key 21</summary>
        F21 = 191,
        /// <summary>Function Key 22</summary>
        F22 = 192,
        /// <summary>Function Key 23</summary>
        F23 = 193,
        /// <summary>Function Key 24</summary>
        F24 = 194,

        /// <summary>Play CD Key</summary>
        PLAYCD = 200,
        /// <summary>Pause CD Key</summary>
        PAUSECD = 201,
        /// <summary>Program 3 Key</summary>
        PROG3 = 202,
        /// <summary>Program 4 Key</summary>
        PROG4 = 203,
        /// <summary>AC Desktop Show All Applications / Dashboard</summary>
        ALL_APPLICATIONS = 204,
        // KEY_DASHBOARD is an alias for KEY_ALL_APPLICATIONS (204)
        /// <summary>Suspend Key</summary>
        SUSPEND = 205,
        /// <summary>AC Close Key</summary>
        CLOSE = 206,
        /// <summary>Play Key (Media)</summary>
        PLAY = 207,
        /// <summary>Fast Forward Key</summary>
        FASTFORWARD = 208,
        /// <summary>Bass Boost Key</summary>
        BASSBOOST = 209,
        /// <summary>AC Print Key</summary>
        PRINT = 210,
        /// <summary>HP Key</summary>
        HP = 211,
        /// <summary>Camera Key</summary>
        CAMERA = 212,
        /// <summary>Sound Key</summary>
        SOUND = 213,
        /// <summary>Question Key</summary>
        QUESTION = 214,
        /// <summary>Email Launch Key</summary>
        EMAIL = 215,
        /// <summary>Chat Launch Key</summary>
        CHAT = 216,
        /// <summary>Search Key</summary>
        SEARCH = 217,
        /// <summary>Connect Key</summary>
        CONNECT = 218,
        /// <summary>AL Checkbook/Finance Launch Key</summary>
        FINANCE = 219,
        /// <summary>Sport Key</summary>
        SPORT = 220,
        /// <summary>Shop Launch Key</summary>
        SHOP = 221,
        /// <summary>Alternate Erase Key</summary>
        ALTERASE = 222,
        /// <summary>AC Cancel Key</summary>
        CANCEL = 223,
        /// <summary>Brightness Down Key</summary>
        BRIGHTNESSDOWN = 224,
        /// <summary>Brightness Up Key</summary>
        BRIGHTNESSUP = 225,
        /// <summary>Media Select Key</summary>
        MEDIA = 226,

        /// <summary>Cycle between available video outputs (Monitor/LCD/TV-out/etc)</summary>
        SWITCHVIDEOMODE = 227,
        /// <summary>Keyboard Illumination Toggle Key</summary>
        KBDILLUMTOGGLE = 228,
        /// <summary>Keyboard Illumination Down Key</summary>
        KBDILLUMDOWN = 229,
        /// <summary>Keyboard Illumination Up Key</summary>
        KBDILLUMUP = 230,

        /// <summary>AC Send Key</summary>
        SEND = 231,
        /// <summary>AC Reply Key</summary>
        REPLY = 232,
        /// <summary>AC Forward Mail Key</summary>
        FORWARDMAIL = 233,
        /// <summary>AC Save Key</summary>
        SAVE = 234,
        /// <summary>Documents Launch Key</summary>
        DOCUMENTS = 235,

        /// <summary>Battery Status Key</summary>
        BATTERY = 236,

        /// <summary>Bluetooth Toggle Key</summary>
        BLUETOOTH = 237,
        /// <summary>WLAN / WiFi Toggle Key</summary>
        WLAN = 238,
        /// <summary>Ultra-Wideband Key</summary>
        UWB = 239,

        /// <summary>Unknown Key Code</summary>
        UNKNOWN = 240,

        /// <summary>Drive Next Video Source Key</summary>
        VIDEO_NEXT = 241,
        /// <summary>Drive Previous Video Source Key</summary>
        VIDEO_PREV = 242,
        /// <summary>Brightness Cycle Key (up, after max is min)</summary>
        BRIGHTNESS_CYCLE = 243,
        /// <summary>Set Auto Brightness (Brightness Zero)</summary>
        BRIGHTNESS_AUTO = 244,
        // KEY_BRIGHTNESS_ZERO is an alias for KEY_BRIGHTNESS_AUTO (244)
        /// <summary>Display device to off state</summary>
        DISPLAY_OFF = 245,

        /// <summary>Wireless WAN (LTE, UMTS, GSM, etc.) Toggle Key</summary>
        WWAN = 246,
        // KEY_WIMAX is an alias for KEY_WWAN (246)
        /// <summary>Key that controls all radios (RF Kill)</summary>
        RFKILL = 247,

        /// <summary>Mute / Unmute the microphone</summary>
        MICMUTE = 248
    }
}