using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazeToolBar
{
    class KeyPressZoom
    {

        WindowsInput.InputSimulator inputSim = new WindowsInput.InputSimulator();


        public void win_zoom_in()
        {
            inputSim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.LWIN);
            inputSim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.OEM_PLUS);
            inputSim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.LWIN);
        }

        public void closeMagnifier()
        {
            inputSim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.LWIN);
            inputSim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.ESCAPE);
            inputSim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.LWIN);
        }

    }
}
