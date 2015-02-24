using Apache.NMS;
using PathinfoActiveMQClient.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.IO;
using System.Windows.Forms;
using EDSDKLib;
//using CanonSDKCamera;


namespace PathinfoActiveMQClient.Impl
{
    class DefaultClient : Client
    {
        private Logger logger = new Logger();
        protected long counter = 0;
        protected Thread publisher;
        protected Thread listener;
        protected ConcurrentLinkedList<Message> messageSource = new ConcurrentLinkedList<Message>();
        protected ConcurrentDictionary<String, Message> messageResponse = new ConcurrentDictionary<String, Message>();

        //camera members
        private SDKHandler CameraHandler;
        private List<Camera> CamList;
        private string ImageSaveFolder;
        private string ImageSaveName;
        private bool isCameraStarted;

        public DefaultClient(String publisherTopic, String listenerTopic)
        {
            
            string user = System.Configuration.ConfigurationSettings.AppSettings["user"];
            string password = System.Configuration.ConfigurationSettings.AppSettings["password"];
            string host = System.Configuration.ConfigurationSettings.AppSettings["host"];
            string port = System.Configuration.ConfigurationSettings.AppSettings["port"];

            String brokerUri = "activemq:tcp://" + host + ":" + port;
            NMSConnectionFactory factory = new NMSConnectionFactory(brokerUri);
            PublisherThread publisherThread = new PublisherThread(factory, messageSource, publisherTopic, user, password);
            publisher = new Thread(new ThreadStart(publisherThread.ThreadRun));
            publisher.Start();

            ListenerThread listenerThread = new ListenerThread(factory, messageResponse, listenerTopic, user, password);
            listener = new Thread(new ThreadStart(listenerThread.ThreadRun));
            listener.Start();
        }

        ~DefaultClient() {
            if (CameraHandler.CameraSessionOpen)
            {
                CloseSession();
                CameraHandler.Dispose();
            }
        }

        protected long Counter()
        {
            lock (this)
            {
                return counter++;
            }
        }

        public Message SendAndWait(Message request)
        {
            String id = Counter().ToString();
            request.SetId(id);

            messageSource.AddLast(request);

            logger.Log("Message " + id + " pushed. Wait for response ...");

            DateTime timeout = DateTime.Now.AddSeconds(10);
            while (DateTime.Now.CompareTo(timeout) < 0)
            {
                Message response = messageResponse.Remove(id);
                if (response != null)
                {
                    return response;
                }
            }
            logger.Log("SendAndWait timeout on id " + id);
            return null;
        }

        public void WaitAndResponse(MessageHandler handler)
        {
            while (true)
            {
                if (messageResponse.Count > 0)
                {
                    lock (this)
                    {
                        // camera process
                        takeOnePicture();

                        // response message
                        KeyValuePair<String, Message> pair = messageResponse.PopNext();
                        logger.Log("Got request message " + pair.Value.GetId());
                        Message response = handler.Handle(pair.Value);
                        messageSource.AddLast(response);
                        logger.Log("Pushed response " + response.GetId());
                    }
                }
            }
        }

        // camera functions
        private void takeOnePicture() 
        {
            int err = 0;
            try
            {
                err = initCamera();
                if (err < 0)
                    return;
            }
            catch
            {
                logger.Log("Cannot open camera. It may be in use now.");
            }
            while (true) { 
                TakeOneCanonPhoto(this.ImageSaveFolder);
                System.Threading.Thread.Sleep(500);
                logger.Log("take one pickture");
            }
            CloseSession();
            CameraHandler.Dispose();
        }

        private int initCamera() 
        {
            CameraHandler = new SDKHandler();
            CameraHandler.CameraAdded += new SDKHandler.CameraAddedHandler(SDK_CameraAdded);
            CameraHandler.CameraHasShutdown += CameraHandler_CameraHasShutdown;
            
            ImageSaveFolder = "D:\\pis\\image\\gross\\";
            ImageSaveName = "gross.jpg";
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            String year = datevalue.Year.ToString();
            ImageSaveFolder += (year + '\\');
            //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "RemotePhoto");
            if (!Directory.Exists(ImageSaveFolder))
            {
                Directory.CreateDirectory(ImageSaveFolder);
            }
            
            //RefreshCamera();


            int err = RefreshCamera();
            OpenSession();
            CameraHandler.ImageSaveDirectory = ImageSaveFolder;
            CameraHandler.ImageSaveName = ImageSaveName;
            System.Threading.Thread.Sleep(500);
            CameraHandler.TakePhoto();
            /*
            if (err >= 0)
                SetSaveTo(1); // save to 0 camera, 1 host, 2 both
            else
                return -1;
            */
            return 0;
        }
        private void CloseSession()
        {
            CameraHandler.CloseSession();

        }

        private int RefreshCamera()
        {
            try
            {
                CloseSession();
                CamList = CameraHandler.GetCameraList();
                //OpenSession();
            }
            catch {
                MessageBox.Show("Cannot get camera list! 佳能相机是否连接？");
                return -1;
            }
            return 0;
        }

        private void SDK_CameraAdded()
        {
            RefreshCamera();
        }

        private void CameraHandler_CameraHasShutdown(object sender, EventArgs e)
        {
            CloseSession();
        }
        

        private void OpenSession()
        {
            if (CamList.Count == 0)
                MessageBox.Show("无法连接相机！");
                
            CameraHandler.OpenSession(CamList[0]);
            string cameraname = CameraHandler.MainCamera.Info.szDeviceDescription;
                
            if (CameraHandler.GetSetting(EDSDK.PropID_AEMode) != EDSDK.AEMode_Manual)
                MessageBox.Show("相机不在手动设置(manual). 某些功能无法使用!");
            CameraHandler.ImageSaveDirectory = ImageSaveFolder;
            CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Both);
            CameraHandler.SetCapacity();
        }

        private void SetSaveTo(int k)
        {
            if (k==0)
            {
                CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Camera);
            }
            else if (k==1)
            {
                CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Host);
                CameraHandler.SetCapacity();
            }
            else if (k==2)
            {
                CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Both);
                CameraHandler.SetCapacity();
            }
        }

        private void TakeOneCanonPhoto(string savePath)
        {
           
            CameraHandler.TakePhoto();
            //CameraHandler.DownloadImage(CameraHandler., savePath);
        }
    }
}
