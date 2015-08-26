using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        string imageOutputFolder = "";
        string accessionIDWebService = "";
        string imageDoctorWebService = "";
        string operatorWebService = "";
        string dataContent = "image";
        bool recordReadyFlag = false;
        bool cameraConnectFlag = false;
        #endregion

        public MainForm()
        {
            //AcquirePatientRecordFromWebService("SP-15-4155");
            InitializeComponent();
            initFromConfig();
            
            
            initControlsExtra();
            CameraHandler = new SDKHandler();
            CameraHandler.CameraAdded += new SDKHandler.CameraAddedHandler(SDK_CameraAdded);
            CameraHandler.LiveViewUpdated += new SDKHandler.StreamUpdate(SDK_LiveViewUpdated);
            CameraHandler.ProgressChanged += new SDKHandler.ProgressHandler(SDK_ProgressChanged);
            CameraHandler.CameraHasShutdown += SDK_CameraHasShutdown;
            //SDKHandler.
            // SavePathTextBox.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "RemotePhoto");
           // DateTime dt;
            string year_str = DateTime.Now.ToString("yyyy");
            string month_str = DateTime.Now.ToString("MM");
            //string image_folder = "C:\\pis\\image\\gross\\" + year_str + "\\" + month_str;
            imageOutputFolder = Path.Combine(taskOutputFolder, year_str, month_str);
            if(!Directory.Exists(imageOutputFolder))
                Directory.CreateDirectory(imageOutputFolder);
            if (!Directory.Exists(taskOutputFolder))
                Directory.CreateDirectory(taskOutputFolder);
            if (!Directory.Exists(taskFolder))
                Directory.CreateDirectory(taskFolder);
            if (!Directory.Exists(taskImageTempFolder))
                Directory.CreateDirectory(taskImageTempFolder);
            //empty image temp folder
            CleanupImageTempFolder();
            RefreshTasks();
            
            SavePathTextBox.Text = imageOutputFolder;
            LVBw = LiveViewPicBox.Width;
            LVBh = LiveViewPicBox.Height;
            RefreshCamera();
           // TakePhotoButton.Enabled = recordReadyFlag;
        }

        private void initControlsExtra() {
            initGenderListBox();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CameraHandler.Dispose();
        }

        #region SDK Events

        private void SDK_ProgressChanged(int Progress)
        {
            if (Progress == 100)
            {
                Progress = 0;
                if (!moveImageToFinishedFolder() || dataContent.CompareTo("video") == 0)
                {
                    MainProgressBar.Value = Progress;
                    return;
                }
                //string image_file = getImageFileStandardName(); //
                
                string image_file = imageFileNameBox.Text;
                string image_path = Path.Combine(imageOutputFolder, image_file);
                outputPISDataToFinishedFolder(image_path);
                outputTaskFile(image_path);
            }
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
            if (CameraHandler.CameraSessionOpen) 
                CloseSession();
            else 
                OpenSession();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RefreshCamera();
        }

        #endregion

        #region Live view

        private void LiveViewButton_Click(object sender, EventArgs e)
        {
            if (!CameraHandler.IsLiveViewOn) { CameraHandler.StartLiveView(); LiveViewButton.Text = "停止实时"; }
            else { CameraHandler.StopLiveView(); LiveViewButton.Text = "开始实时"; }
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
            dataContent = "image";
            if ((string)TvCoBox.SelectedItem == "Bulb")
                CameraHandler.TakePhoto((uint)BulbUpDo.Value);
            else
            {
                //for (int i = 0; i < 5;i++ )
               // CleanupImageTempFolder();
                    CameraHandler.TakePhoto();
            }
            // copy image from tmp to finished
            //moveImageToFinishedFolder();
        }

        private void RecordVideoButton_Click(object sender, EventArgs e)
        {
            if (!CameraHandler.IsFilming)
            {
                dataContent = "video";
                if (STComputerButton.Checked || STBothButton.Checked)
                {
                    //Directory.CreateDirectory(taskImageTempFolder);
                    CameraHandler.StartFilming(taskImageTempFolder);
                }
                else 
                    CameraHandler.StartFilming();
                if(CameraHandler.IsFilming)
                    RecordVideoButton.Text = "停止录像";
            }
            else
            {
                CameraHandler.StopFilming();
                if (!CameraHandler.IsFilming)
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

        private void initFromConfig() {
            const string configFile = "config.json";
            if (File.Exists(configFile) == false)
                return;
            string jsonStr = System.IO.File.ReadAllText(configFile);
            var d = JObject.Parse(jsonStr);

            Dictionary<string, string> configDict = d.ToObject<Dictionary<string, string>>();
            if (configDict.ContainsKey("imageCaptureTempFolder"))
                taskImageTempFolder = configDict["imageCaptureTempFolder"];
            if (configDict.ContainsKey("imageDestinationParentFolder"))
                taskOutputFolder = configDict["imageDestinationParentFolder"];
            if (configDict.ContainsKey("accessionIDWebService"))
                accessionIDWebService = configDict["accessionIDWebService"];
            if (configDict.ContainsKey("operatorWebService"))
                operatorWebService = configDict["operatorWebService"];
            if (configDict.ContainsKey("imageDoctorWebService"))
                imageDoctorWebService = configDict["imageDoctorWebService"];
            if (configDict.ContainsKey("taskFolder"))
                taskFolder = configDict["taskFolder"];

            updateImageDoctorList();
            updateOperatorList();
        }

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
                LiveViewButton.Enabled = true;
                cameraConnectFlag = true;
                updateRecordReadyControls();
                //if (recordReadyFlag) 
                //    TakePhotoButton.Enabled = true;
                //added to initialize save-to options at session opening
                CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Both);
                CameraHandler.SetCapacity();
                SessionButton.Enabled = false;
                RefreshButton.Enabled = false;
                
                //SessionButton.Text = "断开相机";
            }
        }

        private void initGenderListBox(){
            List<string> _items = new List<string>(); 
	    _items.Add("男"); // <-- Add these
	    _items.Add("女");
	    _items.Add("未知");
        patientGenderListBox.DataSource = _items;
        }

        private bool moveImageToFinishedFolder() {
            System.IO.DirectoryInfo imageTempFolderInfo = new DirectoryInfo(taskImageTempFolder);
           
            string image_file = imageFileNameBox.Text; //
            string base_name = Path.GetFileNameWithoutExtension(image_file);
            string image_path = Path.Combine(imageOutputFolder, image_file);
            
            foreach (FileInfo file in imageTempFolderInfo.GetFiles())
            {   
                FileInfo destFile;
                if(dataContent.CompareTo("image")==0)
                    destFile = new FileInfo(image_path);
                else if (dataContent.CompareTo("video") == 0)
                {
                    string ext = Path.GetExtension(file.ToString());
                    destFile = new FileInfo(Path.Combine(imageOutputFolder, base_name + ext));
                }
                else {
                    MessageBox.Show("unknown output data format:" + dataContent);
                    return false;
                }

                if (destFile.Exists)
                {
                    DialogResult r = MessageBox.Show("是否覆盖上次照相文件？",
                        "图像保存", MessageBoxButtons.YesNo);
                    if (r == DialogResult.No)
                        return  false;
                    //in case not writable
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    
                    destFile.Delete();
                }

                try
                {

                    File.Copy(file.FullName, destFile.ToString(), true);
                    //file.CopyTo(image_path, true);
                    
                }
                catch(Exception e) {
                    MessageBox.Show(e.ToString()+" 图像保存失败！目标图像被其它程序锁定？");
                    return false;
                }
                
                // whether to delete temp image file?
                

                break; // assume only one to copy
               
            }
            return true;
        }
        private void CleanupImageTempFolder() {
            System.IO.DirectoryInfo imageTempFolderInfo = new DirectoryInfo(taskImageTempFolder);
            foreach (FileInfo file in imageTempFolderInfo.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch (Exception e)
                {
                    MessageBox.Show("无法删除零时图像文件：" + e.ToString());
                }
                // throw and error?
               
            }
        }

        private Dictionary<string, string> loadImagingTask()
        {
            string[] taskFiles = Directory.GetFiles(this.taskFolder, "*.json");
            Dictionary<string, string> taskDict;
            string taskFile = loadTaskFile();
            if (taskFile.Length > 0)
            {
                string text = System.IO.File.ReadAllText(taskFile);
                var d = JObject.Parse(text);
                taskDict = d.ToObject<Dictionary<string, string>>();
                
            }
            else {
                taskDict = new Dictionary<string, string>();
            }
            updateWithRecord(taskDict);
           
            return taskDict;
        }

        private string loadTaskFile() { 
            string[] taskFiles = Directory.GetFiles(this.taskFolder, "*.json");
            if(taskFiles.Length > 0 )
                return taskFiles[0];
            else
                return "";
        }

        private void RefreshTasks() {
            Dictionary<string, string> taskDict = loadImagingTask();

        }

        private class MyWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest w = base.GetWebRequest(uri);
                w.Timeout = 4000;
                return w;
            }
        }
        public static class Http
        {
            public static byte[] Post(string uri, NameValueCollection pairs)
            {
                byte[] response = null;
                using (MyWebClient client = new MyWebClient())
                {
                    response = client.UploadValues(uri, pairs);
                }
                return response;
            }
        }


        private string ConnectToWebServiceForPathNumber(string pathNumberString) {
            string jsonStr="";
            byte[] response;
            try
            {
                response = Http.Post(accessionIDWebService, new NameValueCollection() {
                { "no", pathNumberString }
                    });
            }
            catch {
                MessageBox.Show("无法连接服务器:" + accessionIDWebService);
                return jsonStr;
            }
            jsonStr = System.Text.Encoding.UTF8.GetString(response);
            return jsonStr;
        }

        private Dictionary<string, string> AcquirePatientRecordFromWebService(string pathNumberString) {
            string jsonStr = ConnectToWebServiceForPathNumber(pathNumberString);
            
            var d = JObject.Parse(jsonStr);
            Dictionary<string, string> record = new Dictionary<string,string>();
            if (d["result"].ToObject<bool>()) {
                record = d["data"].ToObject<Dictionary<string, string>>();
                record["PathNumber"] = pathNumberString;
            }

            return record;
           
        }

        private string ConnectToWebServiceForText(string webUrl)
        {
            string responseText="";
            HttpWebRequest request = WebRequest.Create(webUrl) as HttpWebRequest;
            request.Method = "GET";
            request.Timeout = 4000;
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch { // error connecting to server
                return responseText;
            }
            WebHeaderCollection header = response.Headers;
            
            var encoding = ASCIIEncoding.UTF8;
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {
                responseText = reader.ReadToEnd();
            }
            
            return responseText;
        }

        private List<string> AcquireStringListFromWebService(string webService)
        {
            List<string> record = new List<string>();
            string jsonStr = ConnectToWebServiceForText(webService);
            if (jsonStr.Length == 0)
                return record;
            try
            {
                var d = JObject.Parse(jsonStr);
                if (d["result"].ToObject<bool>())
                {
                    List<Dictionary<string, string>> dt = d["data"].ToObject<List<Dictionary<string, string>>>();
                    foreach (Dictionary<string, string> item in dt)
                    {
                        record.Add(item["name"]);
                    }
                }

                return record;
            }
            catch {
                MessageBox.Show("连接服务器出错:"+webService);
                return record;
            }
        }

        //update control buttons when recordReadyFlag changed
        private void updateImageFileNameBox(){
            try
            {
                imageFileNameBox.Text = getImageFileStandardName();
            }
            catch
            {
                imageFileNameBox.Text = "";
            }
        
        }
        
        private void updateImageDoctorList(){
            List<string> doctorList = AcquireStringListFromWebService(imageDoctorWebService);
            string[] doctorArray = doctorList.ToArray();
            captureDoctorComboBox.DataSource = doctorArray;
            captureDoctorComboBox.SelectedIndex = -1;
            return;
        }

        private void updateOperatorList() {
            List<string> operatorList = AcquireStringListFromWebService(operatorWebService);
            string[] operatorArray = operatorList.ToArray();
            operatorComboBox.DataSource = operatorArray;
            operatorComboBox.SelectedIndex = -1;
        
        }

        private void updateRecordReadyControls()
        {
            if (recordReadyFlag)
            {
                if (cameraConnectFlag)
                {  //both camera and accession_id record are ready
                    TakePhotoButton.Enabled = true;
                    RecordVideoButton.Enabled = true;
                }
                else {
                    TakePhotoButton.Enabled = false;
                    RecordVideoButton.Enabled = false;
                }
                updateImageFileNameBox();
            }
            else
            {
                TakePhotoButton.Enabled = false;
                RecordVideoButton.Enabled = false;
                imageFileNameBox.Text = "";
            }
            imageFileNameBox.Show();
        }

        private string getImageFileStandardName() {
            string imageName;
            string sp = accessionNumberBox.Text.Substring(0, 2);
            string yearStr = "20" + accessionNumberBox.Text.Substring(3, 2);
            string typeCode = "g";
            int lastHypenIndex =  accessionNumberBox.Text.LastIndexOf("-");
            string accessionNumber = accessionNumberBox.Text.Substring(lastHypenIndex + 1);
            string seqNumber = "01";
            imageName = sp + yearStr + "-" + accessionNumber + typeCode + seqNumber+".jpg";
            return imageName;
        }

        private void outputTaskFile(string imagePath) {
            string imageFolder = Path.GetDirectoryName(imagePath);
            string imageFile = Path.GetFileName(imagePath);
            string baseName = Path.GetFileNameWithoutExtension(imageFile);
            string taskFile = baseName + "_task.json";
            string taskPath = Path.Combine(imageFolder, taskFile);
            FileInfo destFile = new FileInfo(taskPath);
            if (destFile.Exists)
            {
                //in case not writable
                GC.Collect();
                GC.WaitForPendingFinalizers();
                destFile.Delete();
            }
            string taskJsonFile = loadTaskFile();
            try
            {

                File.Copy(taskJsonFile, taskPath, true);
                //file.CopyTo(image_path, true);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString() + " 任务文件移动失败！"+taskJsonFile);
                
            }
        }

        private void outputPISDataToFinishedFolder(string imagePath){
            Dictionary<string, string> pisDict = getPISDictionary(imagePath);
            string imageFolder = Path.GetDirectoryName(imagePath);
            string imageFile = Path.GetFileName(imagePath);
            string baseName = Path.GetFileNameWithoutExtension(imageFile);
            string txtFile = baseName + ".txt";
            string txtPath = Path.Combine(imageFolder, txtFile);
            //if (File.Exists(txtPath)) {
            //    MessageBox.Show(txtPath + " will be overwritten!");
            //}
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(txtPath))
            {
                foreach (string key in pisDict.Keys)
                {
                    string line = key + "\t\t\t\t\t" + pisDict[key];
                    file.WriteLine(line);

                }
                file.Close();
            }
        }

        private Dictionary<string, string> getPISDictionary(string imagePath) { 
            Dictionary<string, string> pis = new Dictionary<string,string>();
            Dictionary<string, string> im_d = getImageProperties(imagePath);
            Dictionary<string, string> camera_dict = getCameraSettings();
            Dictionary<string, string> misc_dict = getMISCDictionary();
            foreach (KeyValuePair<string, string> entry in im_d)
            {
                pis.Add(entry.Key, entry.Value);
            }
            foreach (KeyValuePair<string, string> entry in camera_dict)
            {
                pis.Add(entry.Key, entry.Value);
            }
            foreach (KeyValuePair<string, string> entry in misc_dict)
            {
                pis.Add(entry.Key, entry.Value);
            } 
           return pis;
        }

        private Dictionary<string, string> getMISCDictionary() { 
            Dictionary<string, string> misc_dict = new Dictionary<string,string>();
            misc_dict["image_category"] = "gross";
            misc_dict["included_in_report"] = "";
            misc_dict["accession_id"] = accessionNumberBox.Text;
            misc_dict["specimen_id"] = accessionNumberBox.Text;
            
            misc_dict["operator_pathologist"] = captureDoctorComboBox.Text;
            
            misc_dict["operator_technician"] = operatorComboBox.Text;
            misc_dict["image_description"] = imageDescriptionBox.Text;
            return misc_dict;
        } 

        private Dictionary<string, string> getImageProperties(string imagePath) { 
            Dictionary<string, string> imagePropertyDict = new Dictionary<string,string>();
            FileInfo f = new FileInfo(imagePath);
            FileStream fs = new FileStream(f.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            Image pim = Image.FromStream(fs, false, false);
            Bitmap photo = new Bitmap(imagePath);
            PropertyItem[] props = photo.PropertyItems;
            ASCIIEncoding encodings = new ASCIIEncoding();
            //byte[] make =  props..Single(x => x.Id == 0x0100).Value;
            int count = 0;
            imagePropertyDict["id"] = Path.GetFileNameWithoutExtension(Path.GetFileName(imagePath));
            imagePropertyDict["image_width"] = pim.Width.ToString();
            imagePropertyDict["image_height"] = pim.Height.ToString();
            imagePropertyDict["resolution"] = pim.HorizontalResolution.ToString();
            imagePropertyDict["image_type"] = "JPEG";
            imagePropertyDict["file_name"] = Path.GetFileName(imagePath);
            imagePropertyDict["pixel_numbers"] = (pim.Width*pim.Height).ToString();
            imagePropertyDict["path"] = imagePath;
            imagePropertyDict["color_depth"] = "3";
            string createDateTime = encodings.GetString(pim.GetPropertyItem(0x0132).Value);
            //imagePropertyDict["createDate"] = props.GetValue()
            string createDate = createDateTime.Substring(0, 10);
            imagePropertyDict["create_date"] = createDate;
            imagePropertyDict["acquisition_date"] = createDate;
            string createTime = createDateTime.Substring(11);
            imagePropertyDict["acquisition_time"] = createTime;
            imagePropertyDict["camera_name"] = encodings.GetString(pim.GetPropertyItem(0x0110).Value);
            fs.Close();
            return imagePropertyDict;
        }

        private Dictionary<string, string> getCameraSettings() { 
            Dictionary<string, string> cameraSettingsDict = new Dictionary<string,string>();
            cameraSettingsDict["setting_av"] = AvCoBox.Text;
            cameraSettingsDict["setting_wb"] = WBCoBox.Text;
            cameraSettingsDict["setting_tv"] = TvCoBox.Text;
            cameraSettingsDict["setting_bulbs"] = BulbUpDo.Value.ToString();
            cameraSettingsDict["setting_ISO"] = ISOCoBox.Text;
            return cameraSettingsDict;
        }







        #endregion

        private void SettingsGroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void TaskRefreshButton_Click(object sender, EventArgs e)
        {
            RefreshTasks();
            //MessageBox.Show("refresh complete!");
        }

        private void updateWithRecord(Dictionary<string, string> record){
            string genderString;
             if (record.Count <= 0)
            {
                //accessionNumberBox.Text = "";
                patientNameBox.Text = "";
                genderString = "";
                patientAgeBox.Text = "";
                inPatientNumberBox.Text = "";
                //accessionNumberBox.Text = "";
                recordReadyFlag = false;
            }
            else
            {//update following edit boxes
                accessionNumberBox.Text = record["accessionNo"];
                genderString = record["gender"];
                patientNameBox.Text = record["name"];
                patientAgeBox.Text = record["age"];
                inPatientNumberBox.Text = record["inPatientNo"]+"-"+record["outPatientNo"];
                recordReadyFlag = true;
            }
            updateGenderBoxWithString(genderString);
            patientGenderListBox.Show();
            patientNameBox.Show();
            inPatientNumberBox.Show();
            patientAgeBox.Show();
            updateRecordReadyControls();
        }
        private void updateGenderBoxWithString(string genderString){
            if (genderString.Length == 0) {
                patientGenderListBox.SelectedIndex = 2; // unknown    
            }
            for(int i = 0;i<patientGenderListBox.Items.Count; i++){
                if(patientGenderListBox.Items[i].ToString() == genderString)
                    patientGenderListBox.SelectedIndex = i;
            }
            
        }

        private void accessionNumberBox_TextChanged(object sender, EventArgs e)
        {
            string text = accessionNumberBox.Text;
            Dictionary<string, string> record = AcquirePatientRecordFromWebService(text);
            // first check record
            
                updateWithRecord(record);
           
                // second check accession number only
                updateImageFileNameBox();
            
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
