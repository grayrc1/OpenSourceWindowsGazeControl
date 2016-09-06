﻿using EyeXFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazeToolBar
{
    partial class Settings 
    {
        private void connectBehaveMap()
        {
            Program.EyeXHost.Connect(bhavSettingMap);

            //Temp for 100 
            //Will change later
            bhavSettingMap.Add(btnAutoStart, new GazeAwareBehavior(OnbtnAutoStart_Click) { DelayMilliseconds = 100 });
            bhavSettingMap.Add(btnGaze, new GazeAwareBehavior(OnbtnGaze_Click) { DelayMilliseconds = 100 });
            bhavSettingMap.Add(btnSave, new GazeAwareBehavior(OnbtnSave_Click) { DelayMilliseconds = 100 });
            bhavSettingMap.Add(btnCancel, new GazeAwareBehavior(OnbtnCancel_Click) { DelayMilliseconds = 100 });
            bhavSettingMap.Add(btnChangeLanguage, new GazeAwareBehavior(OnbtnChangeLanguage_Click) { DelayMilliseconds = 100 });
            bhavSettingMap.Add(btnChangeSide, new GazeAwareBehavior(OnbtnChangeSide_Click) { DelayMilliseconds = 100 });
            bhavSettingMap.Add(btnSizeLarge, new GazeAwareBehavior(OnbtnSizeLarge_Click) { DelayMilliseconds = 100 });
            bhavSettingMap.Add(btnSwitch, new GazeAwareBehavior(OnbtnSwitch_Click) { DelayMilliseconds = 100 });
            bhavSettingMap.Add(trackBarPrecision, new GazeAwareBehavior(OntrackBarPrecision_Scroll) { DelayMilliseconds = 100 });
            bhavSettingMap.Add(trackBarSpeed, new GazeAwareBehavior(OntrackBarSpeed_Scroll) { DelayMilliseconds = 100 });
            bhavSettingMap.Add(btnWordPredictionOnOff, new GazeAwareBehavior(OnbtnWordPredictionOnOff_Click) { DelayMilliseconds = 100 });
            bhavSettingMap.Add(btnSizeSmall, new GazeAwareBehavior(OnbtnSizeSmall_Click) { DelayMilliseconds = 100 });
            bhavSettingMap.Add(btnSoundFeedback, new GazeAwareBehavior(OnbtnSoundFeedback_Click) { DelayMilliseconds = 100 });
            bhavSettingMap.Add(trackBarGazeTypingSpeed, new GazeAwareBehavior(OntrackBarGazeTypingSpeed_Scroll) { DelayMilliseconds = 100 });
        }

        private void OntrackBarGazeTypingSpeed_Scroll(object sender, GazeAwareEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnbtnSoundFeedback_Click(object sender, GazeAwareEventArgs e)
        {
            btnSoundFeedback.PerformClick();
        }

        private void OnbtnSizeSmall_Click(object sender, GazeAwareEventArgs e)
        {
            btnSizeSmall.PerformClick();
        }

        private void OnbtnWordPredictionOnOff_Click(object sender, GazeAwareEventArgs e)
        {
            btnWordPredictionOnOff.PerformClick();
        }

        private void OntrackBarSpeed_Scroll(object sender, GazeAwareEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OntrackBarPrecision_Scroll(object sender, GazeAwareEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnbtnSwitch_Click(object sender, GazeAwareEventArgs e)
        {
            btnSwitch.PerformClick();
        }

        private void OnbtnSizeLarge_Click(object sender, GazeAwareEventArgs e)
        {
            btnSizeLarge.PerformClick();
        }

        private void OnbtnChangeSide_Click(object sender, GazeAwareEventArgs e)
        {
            btnChangeSide.PerformClick();
        }

        private void OnbtnChangeLanguage_Click(object sender, GazeAwareEventArgs e)
        {
            btnChangeLanguage.PerformClick();
        }

        private void OnbtnCancel_Click(object sender, GazeAwareEventArgs e)
        {
            btnCancel.PerformClick();
        }

        private void OnbtnSave_Click(object sender, GazeAwareEventArgs e)
        {
            btnSave.PerformClick();
        }

        private void OnbtnGaze_Click(object sender, GazeAwareEventArgs e)
        {
            btnGaze.PerformClick();
        }

        private void OnbtnAutoStart_Click(object sender, GazeAwareEventArgs e)
        {
            btnAutoStart.PerformClick();
        }
    }
}
