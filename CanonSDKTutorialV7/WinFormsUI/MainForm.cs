using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using EDSDKLib;

namespace WinFormsUI
{
    public partial class MainForm : Form
    {
        #region Variables

        SDKHandler CameraHandler;
        List<int> AvList;
        List<int> TvList;
        List<int> ISOList;
        List<string> taskList = new List<string>();
        List<Camera> CamList;
        Bitmap Evf_Bmp;
        int LVBw, LVBh, w, h;
        float LVBratio, LVration;
        string taskFolder = "C:\\tmp\\LumanImagingTasks\\";
        string taskOutputFolder = "C:\\tmp\\LumanImagingFinished\\";
        string taskImageTempFolder = "C:\\tmp\\LumanImagingTemp\\";
        #endregion

        public MainForm()
        {
            InitializeComponent();
            CameraHandler = new SDKHandler();
            CameraHandler.CameraAdded += new SDKHandler.CameraAddedHandler(SDK_CameraAdded);
            CameraHandler.LiveViewUpdated += new SDKHandler.StreamUpdate(SDK_LiveViewUpdated);
            CameraHandler.ProgressChanged += new SDKHandler.ProgressHandler(SDK_ProgressChanged);
            CameraHandler.CameraHasShutdown += SDK_CameraHasShutdown;
            
           // SavePathTextBox.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "RemotePhoto");
            DateTime dt;
            string year_str = DateTime.Now.ToString("yyyy");
            string month_str = DateTime.Now.ToString("MM");
            string image_folder = "C:\\pis\\image\\gross\\" + year_str + "\\" + month_str;
            if(!Directory.Exists(image_folder))
                    Directory.CreateDirectory(image_folder);
            if (!Directory.Exists(taskOutputFolder))
                Directory.CreateDirectory(taskOutputFolder);
            if (!Directory.Exists(taskFolder))
                Directory.CreateDirectory(taskFolder);
            if (!Directory.Exists(taskImageTempFolder))
                Directory.CreateDirectory(taskImageTempFolder);
            //empty image temp folder
            System.IO.DirectoryInfo imageTempFolderInfo = new DirectoryInfo(taskImageTempFolder);
            foreach (FileInfo file in imageTempFolderInfo.GetFiles())
            {
                file.Delete();
            }
            RefreshTasks();
            SavePathTextBox.Text = image_folder;
            LVBw = LiveViewPicBox.Width;
            LVBh = LiveViewPicBox.Height;
            RefreshCamera();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CameraHandler.Dispose();
        }

        #region SDK Events

        private void SDK_ProgressChanged(int Progress)
        {
            if (Progress == 100) Progress = 0;
            MainProgressBar.Value = Progress;
        }

        private void SDK_LiveViewUpdated(Stream img)
        {
            Evf_Bmp = new Bitmap(img);
            using (Graphics g = LiveViewPicBox.CreateGraphics())
            {
                LVBratio = LVBw / (float)LVBh;
                LVration = Evf_Bmp.Width / (float)Evf_Bmp.Height;
                if(LVBratio < LVration)
                {
                    w = LVBw;
                    h = (int)(LVBw / LVration);
                }
                else
                {
                    w = (int)(LVBh * LVration);
                    h = LVBh;
                }
                g.DrawImage(Evf_Bmp, 0, 0, w, h);
            }
        }

        private void SDK_CameraAdded()
        {
            RefreshCamera();
        }

        private void SDK_CameraHasShutdown(object sender, EventArgs e)
        {
            CloseSession();
        }

        #endregion

        #region Session

        private void SessionButton_Click(object sender, EventArgs e)
        {
            if (CameraHandler.CameraSessionOpen) CloseSession();
            else OpenSession();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RefreshCamera();
        }

        #endregion

        #region Live view

        private void LiveViewButton_Click(object sender, EventArgs e)
        {
            if (!CameraHandler.IsLiveViewOn) { CameraHandler.StartLiveView(); LiveViewButton.Text = "Stop LV"; }
            else { CameraHandler.StopLiveView(); LiveViewButton.Text = "Start LV"; }
        }

        private void LiveViewPicBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (CameraHandler.IsLiveViewOn && CameraHandler.IsCoordSystemSet)
            {
                ushort x = (ushort)((e.X / (double)LiveViewPicBox.Width) * CameraHandler.Evf_CoordinateSystem.width);
                ushort y = (ushort)((e.Y / (double)LiveViewPicBox.Height) * CameraHandler.Evf_CoordinateSystem.height);
                CameraHandler.SetManualWBEvf(x, y);
            }
        }
        
        private void LiveViewPicBox_SizeChanged(object sender, EventArgs e)
        {
            LVBw = LiveViewPicBox.Width;
            LVBh = LiveViewPicBox.Height;
        }

        private void FocusNear3Button_Click(object sender, EventArgs e)
        {
            CameraHandler.SetFocus(EDSDK.EvfDriveLens_Near3);
        }

        private void FocusNear2Button_Click(object sender, EventArgs e)
        {
            CameraHandler.SetFocus(EDSDK.EvfDriveLens_Near2);
        }

        private void FocusNear1Button_Click(object sender, EventArgs e)
        {
            CameraHandler.SetFocus(EDSDK.EvfDriveLens_Near1);
        }

        private void FocusFar1Button_Click(object sender, EventArgs e)
        {
            CameraHandler.SetFocus(EDSDK.EvfDriveLens_Far1);
        }

        private void FocusFar2Button_Click(object sender, EventArgs e)
        {
            CameraHandler.SetFocus(EDSDK.EvfDriveLens_Far2);
        }

        private void FocusFar3Button_Click(object sender, EventArgs e)
        {
            CameraHandler.SetFocus(EDSDK.EvfDriveLens_Far3);
        }

        #endregion

        #region Settings

        private void TakePhotoButton_Click(object sender, EventArgs e)
        {
            if (STComputerButton.Checked || STBothButton.Checked) 
                Directory.CreateDirectory(SavePathTextBox.Text);
            CameraHandler.ImageSaveDirectory = taskImageTempFolder;//SavePathTextBox.Text;
            
            if ((string)TvCoBox.SelectedItem == "Bulb")
                CameraHandler.TakePhoto((uint)BulbUpDo.Value);
            else
            {
                //for (int i = 0; i < 5;i++ )
                    CameraHandler.TakePhoto();
            }
        }

        private void RecordVideoButton_Click(object sender, EventArgs e)
        {
            if (!CameraHandler.IsFilming)
            {
                if (STComputerButton.Checked || STBothButton.Checked)
                {
                    Directory.CreateDirectory(SavePathTextBox.Text);
                    CameraHandler.StartFilming(SavePathTextBox.Text);
                }
                else CameraHandler.StartFilming();
                RecordVideoButton.Text = "停止录像";
            }
            else
            {
                CameraHandler.StopFilming();
                RecordVideoButton.Text = "录像";
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(SavePathTextBox.Text)) SaveFolderBrowser.SelectedPath = SavePathTextBox.Text;
            if (SaveFolderBrowser.ShowDialog() == DialogResult.OK) SavePathTextBox.Text = SaveFolderBrowser.SelectedPath;
        }

        private void AvCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CameraHandler.SetSetting(EDSDK.PropID_Av, CameraValues.AV((string)AvCoBox.SelectedItem));
        }

        private void TvCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CameraHandler.SetSetting(EDSDK.PropID_Tv, CameraValues.TV((string)TvCoBox.SelectedItem));
            if ((string)TvCoBox.SelectedItem == "Bulb") BulbUpDo.Enabled = true;
            else BulbUpDo.Enabled = false;
        }

        private void ISOCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CameraHandler.SetSetting(EDSDK.PropID_ISOSpeed, CameraValues.ISO((string)ISOCoBox.SelectedItem));
        }

        private void WBCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (WBCoBox.SelectedIndex)
            {
                case 0: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Auto); break;
                case 1: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Daylight); break;
                case 2: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Cloudy); break;
                case 3: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Tangsten); break;
                case 4: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Fluorescent); break;
                case 5: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Strobe); break;
                case 6: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_WhitePaper); break;
                case 7: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Shade); break;
            }
        }
        
        private void SaveToButton_CheckedChanged(object sender, EventArgs e)
        {
            if (STCameraButton.Checked)
            {
                CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Camera);
                BrowseButton.Enabled = false;
                SavePathTextBox.Enabled = false;
            }
            else if (STComputerButton.Checked)
            {
                CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Host);
                CameraHandler.SetCapacity();
                BrowseButton.Enabled = true;
                SavePathTextBox.Enabled = true;
            }
            else if (STBothButton.Checked)
            {
                CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Both);
                CameraHandler.SetCapacity();
                BrowseButton.Enabled = true;
                SavePathTextBox.Enabled = true;
            }
        }

        #endregion

        #region Subroutines

        private void CloseSession()
        {
            CameraHandler.CloseSession();
            AvCoBox.Items.Clear();
            TvCoBox.Items.Clear();
            ISOCoBox.Items.Clear();
            SettingsGroupBox.Enabled = false;
            LiveViewGroupBox.Enabled = false;
            SessionButton.Text = "连接相机";
            SessionLabel.Text = "尚未连接相机";
        }

        private void RefreshCamera()
        {
            CloseSession();
            CameraListBox.Items.Clear();
            CamList = CameraHandler.GetCameraList();
            foreach (Camera cam in CamList) CameraListBox.Items.Add(cam.Info.szDeviceDescription);
            if (CamList.Count > 0) CameraListBox.SelectedIndex = 0;
        }

        private void OpenSession()
        {
            if (CameraListBox.SelectedIndex >= 0)
            {
                CameraHandler.OpenSession(CamList[CameraListBox.SelectedIndex]);
                
                string cameraname = CameraHandler.MainCamera.Info.szDeviceDescription;
                SessionLabel.Text = cameraname;
                try
                {
                    if (CameraHandler.GetSetting(EDSDK.PropID_AEMode) != EDSDK.AEMode_Manual) { 
                        MessageBox.Show("相机不在手动模式(manual mode)!");
                    }
                }
                catch {
                    MessageBox.Show("连接相机出错。请重试。");
                    return;
                }
                AvList = CameraHandler.GetSettingsList((uint)EDSDK.PropID_Av);
                TvList = CameraHandler.GetSettingsList((uint)EDSDK.PropID_Tv);
                ISOList = CameraHandler.GetSettingsList((uint)EDSDK.PropID_ISOSpeed);
                foreach (int Av in AvList) AvCoBox.Items.Add(CameraValues.AV((uint)Av));
                foreach (int Tv in TvList) TvCoBox.Items.Add(CameraValues.TV((uint)Tv));
                foreach (int ISO in ISOList) ISOCoBox.Items.Add(CameraValues.ISO((uint)ISO));
                AvCoBox.SelectedIndex = AvCoBox.Items.IndexOf(CameraValues.AV((uint)CameraHandler.GetSetting((uint)EDSDK.PropID_Av)));
                TvCoBox.SelectedIndex = TvCoBox.Items.IndexOf(CameraValues.TV((uint)CameraHandler.GetSetting((uint)EDSDK.PropID_Tv)));
                ISOCoBox.SelectedIndex = ISOCoBox.Items.IndexOf(CameraValues.ISO((uint)CameraHandler.GetSetting((uint)EDSDK.PropID_ISOSpeed)));
                int wbidx = (int)CameraHandler.GetSetting((uint)EDSDK.PropID_WhiteBalance);
                switch (wbidx)
                {
                    case EDSDK.WhiteBalance_Auto: WBCoBox.SelectedIndex = 0; break;
                    case EDSDK.WhiteBalance_Daylight: WBCoBox.SelectedIndex = 1; break;
                    case EDSDK.WhiteBalance_Cloudy: WBCoBox.SelectedIndex = 2; break;
                    case EDSDK.WhiteBalance_Tangsten: WBCoBox.SelectedIndex = 3; break;
                    case EDSDK.WhiteBalance_Fluorescent: WBCoBox.SelectedIndex = 4; break;
                    case EDSDK.WhiteBalance_Strobe: WBCoBox.SelectedIndex = 5; break;
                    case EDSDK.WhiteBalance_WhitePaper: WBCoBox.SelectedIndex = 6; break;
                    case EDSDK.WhiteBalance_Shade: WBCoBox.SelectedIndex = 7; break;
                    default: WBCoBox.SelectedIndex = -1; break;
                }
                SettingsGroupBox.Enabled = true;
                LiveViewGroupBox.Enabled = true;
                //added to initialize save-to options at session opening
                CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Both);
                CameraHandler.SetCapacity();
                SessionButton.Enabled = false;
                RefreshButton.Enabled = false;

                //SessionButton.Text = "断开相机";
            }
        }

        private void RefreshTasks()
        {
            this.taskList.Clear();
            this.taskListBox.Items.Clear();
            string[] taskFiles = Directory.GetFiles(this.taskFolder, "*.task");
            foreach (string taskFile in taskFiles)
            {
                string fileName = Path.GetFileName(taskFile);
                this.taskList.Add(fileName);
                this.taskListBox.Items.Add(fileName);
            }
        }
        #endregion

        private void SettingsGroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void TaskRefreshButton_Click(object sender, EventArgs e)
        {
            RefreshTasks();
            MessageBox.Show("refresh complete!");
        }
    }
}
