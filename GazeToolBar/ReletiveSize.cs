﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace GazeToolBar
{
    /*
        Date: 17/05/2016
        Name: Derek Dai
        Description: All the reletive size and position will be in this class
    */
    static class ReletiveSize
    {
        public static Point panelSaveAndCancel(int width, int height)
        {
            int x = ((Constants.SCREEN_SIZE.Width / 2) - (width / 2));
            int y = Constants.SCREEN_SIZE.Height - height - (int)(Constants.SCREEN_SIZE.Height * 0.01);
            return new Point(x, y);
        }

        public static Size panelGeneralSize(int botY, int Y)
        {
            int w = Constants.SCREEN_SIZE.Width;
            int h = botY - Y - (int)(Constants.SCREEN_SIZE.Height * 0.01);
            return new Size(w, h);
        }

        public static Size panelRearrangeSize(int botY, int Y)
        {
            int w = Constants.SCREEN_SIZE.Width;
            int h = botY - Y - (int)(Constants.SCREEN_SIZE.Height * -0.1);
            return new Size(w, h);
        }

        public static Point panelSwitchSettingLocation(int width, int height)
        {
            int x = Constants.SCREEN_SIZE.Width / 2 - width / 2;
            int y = (int)(Constants.SCREEN_SIZE.Height * 0.01);
            return new Point(x, y);
        }

        public static Point mainPanelLocation(int _y, int height)
        {
            int x = 0;
            int y = _y + height + (int)(Constants.SCREEN_SIZE.Height * 0.10);
            return new Point(x, y);
        }

        public static Point distributeToBottom(Panel parent, int thisElementX, int thisElementHeight, int position, int totalElement, String flag, double per)
        {
            double percent = (100 / totalElement) / 100.0;
            double widthPercent = per;
            if (flag == "h")
            {
                int parentHeight = parent.Size.Height;
                int thisElementLocationY = (int)(percent * parentHeight * position);
                thisElementLocationY -= thisElementHeight;
                return new Point(thisElementX, thisElementLocationY);
            }
            else
            {
                return new Point();
            }
        }

        public static Point distribute(Panel parent, int thisElementXorY, int position, int totalElement, String flag, double per)
        {
            double percent = (100 / totalElement) / 100.0;
            double widthPercent = per;
            if (flag == "h")
            {
                int parentHeight = parent.Size.Height;
                int thisElementLocationY = (int)(percent * parentHeight * (position - 1));
                return new Point(thisElementXorY, thisElementLocationY);
            }
            else if (flag == "w")
            {
                int parentWidth = parent.Size.Width;
                int thisElementLocationX = (int)(widthPercent * parentWidth);
                return new Point(thisElementLocationX, thisElementXorY);
            }
            else
            {
                return new Point();
            }
        }

        public static Size controlLength(Panel parent, int thisElementHeight, double percent)
        {
            int parentLength = parent.Size.Width;
            int length = (int)(parentLength * percent);
            return new Size(length, thisElementHeight);
        }

        public static Size controlLength(Control first, Control second, int thisElementHeight)
        {
            int length = (second.Location.X + second.Size.Width + second.Parent.Location.X) - (first.Location.X + first.Parent.Location.X);
            return new Size(length, thisElementHeight);
        }

        public static Point labelPosition(Panel parent, Label label)
        {
            int parentWidth = parent.Size.Width;
            int labelX = (int)(parentWidth * 0.02);
            return new Point(labelX, label.Location.Y);
        }

        public static Point reletiveLocation(Control relativeTo, int thisControlXorY, int space, char hov)
        {
            Point p = new Point();
            switch (hov)
            {
                case 'h':
                    p.X = thisControlXorY;
                    p.Y = relativeTo.Location.Y + space + relativeTo.Size.Height;
                    break;
                case 'v':
                    p.X = relativeTo.Location.X + space + relativeTo.Size.Width;
                    p.Y = thisControlXorY;
                    break;
            }
            return p;
        }

        public static Point centerLocation(Control parent, Control itemToCenter)
        {
            Point p = new Point();
            p.X = (parent.Width / 2) - (itemToCenter.Width / 2);
            p.Y = (parent.Height / 2) - (itemToCenter.Height / 2);
            return p;
        }

        public static void evenlyDistrubute(Panel parentPanel)
        {
            float percent = 0.0f;
            foreach (Control c in parentPanel.Controls)
            {
                percent += 0.1f;
            }
        }

        public static Size TabControlSize = new Size(Constants.SCREEN_SIZE.Width, Constants.SCREEN_SIZE.Height - 56 * 2);
    }
}
