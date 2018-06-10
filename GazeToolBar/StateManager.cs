using EyeXFramework;
using EyeXFramework.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Karna.Magnification;

namespace GazeToolBar
{


    public enum SystemState { Wait, ActionButtonSelected, Zooming, ZoomWait, ApplyAction, ScrollWait }
    public enum ActionToBePerformed { RightClick, LeftClick, DoubleClick, Scroll, MicInput, MicInputOff }
    public enum SettingState { General, Zoom, Shortcut, Rearrange, Crosshair, Confirm }

    public class StateManager
    {
        public FixationDetection fixationWorker;
        public ScrollControl scrollWorker;
        Form1 toolbar;
        ZoomLens zoomer;
        Point fixationPoint;
        SystemState currentState;
        FormsEyeXHost eyeXHost;
        ShortcutKeyWorker shortCutKeyWorker;
        public ZoomMagnifier magnifier;
        bool micIsOn = false;
        bool resetFlag = false;


        public StateManager(Form1 Toolbar, ShortcutKeyWorker shortCutKeyWorker, FormsEyeXHost EyeXHost)
        {
            eyeXHost = EyeXHost;
            toolbar = Toolbar;

            SystemFlags.currentState = SystemState.Wait;

            fixationWorker = new FixationDetection(eyeXHost, 25);

            scrollWorker = new ScrollControl(200, 5, 50, 20, eyeXHost);

            SystemFlags.currentState = SystemState.Wait;

            SystemFlags.hasSelectedButtonColourBeenReset = true;

            // Instantiate the ZoomLens, this is the form that is given to magnifier
            zoomer = new ZoomLens();
            // Instantiate the magnifier, this is Sam Medlocks refactored magnifier
            // This calls the low-level API
            magnifier = CreateMagnifier();

            //Console.WriteLine(scrollWorker.deadZoneRect.LeftBound + "," + scrollWorker.deadZoneRect.RightBound + "," + scrollWorker.deadZoneRect.TopBound + "," + scrollWorker.deadZoneRect.BottomBound);

            this.shortCutKeyWorker = shortCutKeyWorker;

            Run();
        }


        public void Run()
        {
            UpdateState();
            Action();
        }

        public void EnterWaitState()
        {
            //these flags are here so that they get reset before anything else happens in the SM
            //these were previously in the action method but that causes issues because the update state is run again before all of the flags are reset.
            zoomer.ResetZoomLens();
            magnifier.Stop();
            SystemFlags.fixationRunning = false;
            SystemFlags.actionButtonSelected = false;
            SystemFlags.fixationRunning = false;
            SystemFlags.hasGaze = false;
            SystemFlags.timeOut = false;
            fixationWorker.IsZoomerFixation(false);
            currentState = SystemState.Wait;
            SystemFlags.currentState = SystemState.Wait;
            zoomer.Refresh();

            fixationWorker = new FixationDetection();
        }


        //The update method is responsible for transitioning from state to state. Once a state is changed the action() method is run
        public void UpdateState()
        {
            currentState = SystemFlags.currentState;
            switch (currentState)
            {
                case SystemState.Wait:
                    if (SystemFlags.actionButtonSelected) //if a button has been selected from the toolbar
                    {
                        if (SystemFlags.actionToBePerformed == ActionToBePerformed.MicInput && micIsOn)
                        {
                            currentState = SystemState.ApplyAction;
                        }
                        else
                        {
                            currentState = SystemState.ActionButtonSelected;
                            SystemFlags.actionButtonSelected = false;
                        }
                    }
                    else if (SystemFlags.shortCutKeyPressed)
                    {
                        currentState = SystemState.Zooming;
                    }
                    break;
                case SystemState.ActionButtonSelected:
                    SystemFlags.hasSelectedButtonColourBeenReset = false;
                    if (SystemFlags.hasGaze)
                    {
                        currentState = SystemState.Zooming;
                    }
                    else if (SystemFlags.timeOut)
                    {
                        EnterWaitState();
                        SystemFlags.timeOut = false;
                    }
                    break;
                case SystemState.Zooming:
                    if (SystemFlags.actionToBePerformed == ActionToBePerformed.Scroll)
                    {
                        currentState = SystemState.ApplyAction;
                    }
                    else
                    {
                        currentState = SystemState.ZoomWait;
                    }
                    break;
                case SystemState.ZoomWait:
                    if (SystemFlags.hasGaze)//if the second zoomGaze has happed an action needs to be performed
                    {
                        currentState = SystemState.ApplyAction;
                    }
                    else if (SystemFlags.timeOut)
                    {
                        EnterWaitState();
                        zoomer.ResetZoomLens();
                    }
                    break;
                case SystemState.ScrollWait:
                    if (!SystemFlags.scrolling)
                    {
                        EnterWaitState();
                    }
                    break;
                case SystemState.ApplyAction:
                    if (SystemFlags.scrolling)
                    {
                        currentState = SystemState.ScrollWait;
                    }
                    else
                    {
                        if (Program.readSettings.stickyLeftClick && SystemFlags.actionToBePerformed == ActionToBePerformed.LeftClick) //if stick left && left click
                        {
                            EnterWaitState();

                            SystemFlags.actionButtonSelected = true;
                            SystemFlags.actionToBePerformed = ActionToBePerformed.LeftClick;
                            SystemFlags.currentState = SystemState.ActionButtonSelected;
                            SystemFlags.hasSelectedButtonColourBeenReset = true;
                        }
                        else
                        {
                            EnterWaitState();
                        }
                    }
                    break;
            }
            SystemFlags.currentState = currentState;
        }


        //The action state is responsible for completing each action that must take place during each state
        public void Action()
        {
            switch (SystemFlags.currentState)
            {
                case SystemState.Wait:
                    scrollWorker.stopScroll();
                    if (SystemFlags.hasSelectedButtonColourBeenReset == false)
                    {
                        toolbar.resetButtonsColor();
                        SystemFlags.hasSelectedButtonColourBeenReset = true;
                    }
                    break;
                case SystemState.ActionButtonSelected:
                    scrollWorker.stopScroll();
                    if (!SystemFlags.fixationRunning)
                    {
                        fixationWorker.StartDetectingFixation();
                        SystemFlags.fixationRunning = true;
                    }
                    if (Program.readSettings.selectionFeedback) //don't show the crosshair if selection feedback is off
                    {
                        zoomer.Start();
                        zoomer.Show();
                        zoomer.CrossHairPos = fixationWorker.getXY();
                    }
                    break;
                case SystemState.Zooming:
                    scrollWorker.stopScroll();
                    fixationWorker.IsZoomerFixation(true);
                    if (SystemFlags.shortCutKeyPressed)//if a user defined click key is pressed
                    {
                        fixationPoint = shortCutKeyWorker.GetXY();
                        SystemFlags.shortCutKeyPressed = false;
                    }
                    else
                    {
                        fixationPoint = fixationWorker.getXY();//get the location the user looked
                    }

                    //TODO put zoom window in correct place before starting timer (I think...) - Done.

                    //magnifier.UpdateFixationPoint();
                    

                    magnifier.FixationPoint = fixationPoint;

                    magnifier.UpdateMagnifier();
                    //magnifier.PlaceZoomWindow(fixationPoint);

                    magnifier.Timer.Enabled = true;
                    // magnifier.UpdatePosition(fixationPoint);
                    // Give the magnifier the point on screen to magnify

                    //original position
                    //magnifier.FixationPoint = fixationPoint;
                    Point p1 = Utils.DividePoint(magnifier.Offset, magnifier.MagnifierDivAmount());
                    Point p2 = Utils.DividePoint(magnifier.SecondaryOffset, magnifier.MagnifierDivAmount());

                    Point o = Utils.SubtractPoints(p1, p2);

                    zoomer.Offset = o;                    // This initiate's the timer for drawing of the user feedback image
                    zoomer.Start();
                    zoomer.Show();
                    zoomer.CrossHairPos = magnifier.GetLookPosition();

                    //disable neccesary flags 
                    SystemFlags.hasGaze = false;
                    SystemFlags.fixationRunning = false;
                    break;
                case SystemState.ZoomWait://waiting for user to fixate
                    if (!SystemFlags.fixationRunning)
                    {
                        fixationWorker.StartDetectingFixation();
                        SystemFlags.fixationRunning = true;
                    }
                    zoomer.CrossHairPos = magnifier.GetLookPosition();
                    //SetZoomerOffset();
                    break;
                case SystemState.ApplyAction: //the fixation on the zoom lens has been detected

                    //
                    //This was settin fixationPoint twice for some reason?
                    //
                    //Wait, Whaaat? Setting the fixation point does nothing?
                    //Why was it set twice? Where is it set?

                    //fixationPoint = fixationWorker.getXY();

                    //SetZoomerOffset();

                    //fixationPoint.X += zoomer.Offset.X;
                    //fixationPoint.Y += zoomer.Offset.Y;

                    fixationPoint = magnifier.GetLookPosition();
                    zoomer.ResetZoomLens();//hide the lens
                                           //  MessageBox.Show(magnifier.SecondaryOffset.X + " " + magnifier.SecondaryOffset.Y);
                                           //Set the magnification factor back to initial value
                                           // This is done so that a "dynamic zoom in" feature can be
                                           // implemented in the future
                    magnifier.Stop();
                    RefreshZoom();
                    magnifier.ResetZoomValue();
                    


                    //execute the appropriate action
                    if (SystemFlags.actionToBePerformed == ActionToBePerformed.LeftClick)
                    {
                        VirtualMouse.LeftMouseClick(fixationPoint.X, fixationPoint.Y);
                    }
                    else if (SystemFlags.actionToBePerformed == ActionToBePerformed.RightClick)
                    {
                        VirtualMouse.RightMouseClick(fixationPoint.X, fixationPoint.Y);
                    }
                    else if (SystemFlags.actionToBePerformed == ActionToBePerformed.DoubleClick)
                    {
                        VirtualMouse.LeftDoubleClick(fixationPoint.X, fixationPoint.Y);
                    }
                    else if (SystemFlags.actionToBePerformed == ActionToBePerformed.Scroll)
                    {
                        SystemFlags.currentState = SystemState.ScrollWait;
                        SystemFlags.scrolling = true;
                        VirtualMouse.SetCursorPos(fixationPoint.X, fixationPoint.Y);
                        //VirtualMouse.Scroll(20,   //MiddleMouseButton(fixationPoint.X, fixationPoint.Y);
                        scrollWorker.StartScroll();
                    }
                    else if (SystemFlags.actionToBePerformed == ActionToBePerformed.MicInput)
                    {
                        if (Program.readSettings.micInput != Constants.KEY_FUNCTION_UNASSIGNED_MESSAGE)
                        {
                            if (!micIsOn)
                            {
                                VirtualMouse.LeftMouseClick(fixationPoint.X, fixationPoint.Y);
                                SendKeys.Send(Program.readSettings.micInput);
                                SystemFlags.hasSelectedButtonColourBeenReset = true;
                                micIsOn = true;
                            }
                            else
                            {
                                SendKeys.Send(Program.readSettings.micInputOff);
                                SystemFlags.hasSelectedButtonColourBeenReset = false;
                                micIsOn = false;
                            }
                        }
                    }
                    //
                    //Not sure why a new fixation worker is recreated here
                    //It does not seem to change anything...

                    fixationWorker = new FixationDetection();
                    break;
            }
        }

        private ZoomMagnifier CreateMagnifier()
        {
            return new ZoomMagnifier(zoomer, fixationPoint);
        }

        public void RefreshZoom()
        {
            magnifier.Stop();

            zoomer = new ZoomLens();
            magnifier = CreateMagnifier();
            zoomer.ResetZoomLens();
            magnifier.ResetZoomValue();
            EnterWaitState();
        }
    }
}
