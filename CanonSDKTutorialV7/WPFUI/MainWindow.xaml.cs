using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using EDSDKLib;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables

        SDKHandler CameraHandler;
        List<int> AvList;
        List<int> TvList;
        List<int> ISOList;
        List<Camera> CamList;
        bool IsInit = false;
        int BulbTime = 30;
        ImageSource EvfImage;
        JpegBitmapDecoder dec;
        ImageBrush bgbrush = new ImageBrush();
        ThreadStart SetImageAction;
        System.Windows.Forms.FolderBrowserDialog SaveFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            CameraHandler = new SDKHandler();
            CameraHandler.CameraAdded += new SDKHandler.CameraAddedHandler(SDK_CameraAdded);
            CameraHandler.LiveViewUpdated += new SDKHandler.StreamUpdate(SDK_LiveViewUpdated);
            CameraHandler.ProgressChanged += new SDKHandler.ProgressHandler(SDK_ProgressChanged);
            CameraHandler.CameraHasShutdown += SDK_CameraHasShutdown;
            SavePathTextBox.Text = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "RemotePhoto");
            SetImageAction = new ThreadStart(delegate { bgbrush.ImageSource = EvfImage; });
            SaveFolderBrowser.Description = "Save Images To...";
            RefreshCamera();
            IsInit = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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
            if(CameraHandler.IsLiveViewOn)
            {
                img.Position = 0;
                dec = new JpegBitmapDecoder(img, BitmapCreateOptions.None, BitmapCacheOption.None);
                EvfImage = dec.Frames[0];
                Application.Current.Dispatcher.Invoke(SetImageAction);
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

        private void OpenSessionButton_Click(object sender, RoutedEventArgs e)
        {
            if (CameraHandler.CameraSessionOpen) CloseSession();
            else OpenSession();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshCamera();
        }

        #endregion

        #region Settings

        private void AvCoBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CameraHandler.SetSetting(EDSDK.PropID_Av, CameraValues.AV((string)AvCoBox.SelectedItem));
        }

        private void TvCoBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CameraHandler.SetSetting(EDSDK.PropID_Tv, CameraValues.TV((string)TvCoBox.SelectedItem));
            if ((string)TvCoBox.SelectedItem == "Bulb")
            {
                BulbBox.IsEnabled = true;
                BulbSlider.IsEnabled = true;
            }
            else
            {
                BulbBox.IsEnabled = false;
                BulbSlider.IsEnabled = false;
            }
        }

        private void ISOCoBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CameraHandler.SetSetting(EDSDK.PropID_ISOSpeed, CameraValues.ISO((string)ISOCoBox.SelectedItem));
        }

        private void WBCoBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void TakePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)STComputerRdButton.IsChecked || (bool)STBothRdButton.IsChecked) Directory.CreateDirectory(SavePathTextBox.Text);
            CameraHandler.ImageSaveDirectory = SavePathTextBox.Text;
            if ((string)TvCoBox.SelectedItem == "Bulb") CameraHandler.TakePhoto((uint)BulbTime);
            else CameraHandler.TakePhoto();
        }

        private void VideoButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraHandler.IsFilming)
            {
                if ((bool)STComputerRdButton.IsChecked || (bool)STBothRdButton.IsChecked)
                {
                    Directory.CreateDirectory(SavePathTextBox.Text);
                    CameraHandler.StartFilming(SavePathTextBox.Text);
                }
                else CameraHandler.StartFilming();
                VideoButtonText.Inlines.Clear();
                VideoButtonText.Inlines.Add("Stop");
                VideoButtonText.Inlines.Add(new LineBreak());
                VideoButtonText.Inlines.Add("Video");
            }
            else
            {
                CameraHandler.StopFilming();
                VideoButtonText.Inlines.Clear();
                VideoButtonText.Inlines.Add("Record");
                VideoButtonText.Inlines.Add(new LineBreak());
                VideoButtonText.Inlines.Add("Video");
            }
        }

        private void BulbSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsInit) BulbBox.Text = BulbSlider.Value.ToString();
        }

        private void BulbBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsInit)
            {
                int b;
                if (int.TryParse(BulbBox.Text, out b) && b != BulbTime)
                {
                    BulbTime = b;
                    BulbSlider.Value = BulbTime;
                }
                else BulbBox.Text = BulbTime.ToString();
            }
        }
        
        private void SaveToRdButton_Checked(object sender, RoutedEventArgs e)
        {
            if (IsInit)
            {
                if ((bool)STCameraRdButton.IsChecked)
                {
                    CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Camera);
                    BrowseButton.IsEnabled = false;
                    SavePathTextBox.IsEnabled = false;
                }
                else if ((bool)STComputerRdButton.IsChecked)
                {
                    CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Host);
                    CameraHandler.SetCapacity();
                    BrowseButton.IsEnabled = true;
                    SavePathTextBox.IsEnabled = true;
                }
                else if ((bool)STBothRdButton.IsChecked)
                {
                    CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Both);
                    CameraHandler.SetCapacity();
                    BrowseButton.IsEnabled = true;
                    SavePathTextBox.IsEnabled = true;
                }
            }
        }
        
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(SavePathTextBox.Text)) SaveFolderBrowser.SelectedPath = SavePathTextBox.Text;
            if (SaveFolderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK) SavePathTextBox.Text = SaveFolderBrowser.SelectedPath;
        }

        #endregion

        #region Live view
        
        private void StarLVButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraHandler.IsLiveViewOn)
            {
                LVCanvas.Background = bgbrush;
                CameraHandler.StartLiveView();
                StarLVButton.Content = "Stop LV";
            }
            else
            {
                CameraHandler.StopLiveView();
                StarLVButton.Content = "Start LV";
                LVCanvas.Background = Brushes.LightGray;
            }
        }

        private void LVCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CameraHandler.IsLiveViewOn && CameraHandler.IsCoordSystemSet)
            {
                Point p = e.GetPosition(this);
                ushort x = (ushort)((p.X / LVCanvas.Width) * CameraHandler.Evf_CoordinateSystem.width);
                ushort y = (ushort)((p.Y / LVCanvas.Height) * CameraHandler.Evf_CoordinateSystem.height);
                CameraHandler.SetManualWBEvf(x, y);
            }
        }

        private void FocusNear3Button_Click(object sender, RoutedEventArgs e)
        {
            CameraHandler.SetFocus(EDSDK.EvfDriveLens_Near3);
        }

        private void FocusNear2Button_Click(object sender, RoutedEventArgs e)
        {
            CameraHandler.SetFocus(EDSDK.EvfDriveLens_Near2);
        }

        private void FocusNear1Button_Click(object sender, RoutedEventArgs e)
        {
            CameraHandler.SetFocus(EDSDK.EvfDriveLens_Near1);
        }

        private void FocusFar1Button_Click(object sender, RoutedEventArgs e)
        {
            CameraHandler.SetFocus(EDSDK.EvfDriveLens_Far1);
        }

        private void FocusFar2Button_Click(object sender, RoutedEventArgs e)
        {
            CameraHandler.SetFocus(EDSDK.EvfDriveLens_Far2);
        }

        private void FocusFar3Button_Click(object sender, RoutedEventArgs e)
        {
            CameraHandler.SetFocus(EDSDK.EvfDriveLens_Far3);
        }

        #endregion

        #region Subroutines
        
        private void CloseSession()
        {
            CameraHandler.CloseSession();
            AvCoBox.Items.Clear();
            TvCoBox.Items.Clear();
            ISOCoBox.Items.Clear();
            SettingsGroupBox.IsEnabled = false;
            LVGroupBox.IsEnabled = false;            
            SessionButton.Content = "Open Session";
            SessionLabel.Content = "No open session";
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
                SessionButton.Content = "Close Session";
                string cameraname = CameraHandler.MainCamera.Info.szDeviceDescription;
                SessionLabel.Content = cameraname;
                if (CameraHandler.GetSetting(EDSDK.PropID_AEMode) != EDSDK.AEMode_Manual) MessageBox.Show("Camera is not in manual mode. Some features might not work!");
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
                SettingsGroupBox.IsEnabled = true;
                LVGroupBox.IsEnabled = true;
            }
        }

        #endregion
    }
}
