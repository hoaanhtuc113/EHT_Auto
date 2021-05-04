using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region data
        Bitmap CountDown;
        Bitmap TeadyReady;
        Bitmap BitmapCheck;
        Bitmap Ketthucthamhiem;
        Bitmap Thongtinquandich;
        Bitmap Yeutinhvang;
        Bitmap NutThoat;
        Bitmap HuyTapKich;
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            

        }

        void LoadData()
        {
            CountDown = (Bitmap)Bitmap.FromFile("Data/isCountdown.png");
            TeadyReady = (Bitmap)Bitmap.FromFile("Data/teddyisReady.png");
            BitmapCheck = (Bitmap)Bitmap.FromFile("Data/Kiemtratinhhinh.png");
            Ketthucthamhiem = (Bitmap)Bitmap.FromFile("Data/ketthucthamhiem.png");
            Thongtinquandich = (Bitmap)Bitmap.FromFile("Data/thongtinquandich.png");
            NutThoat = (Bitmap)Bitmap.FromFile("Data/LeaveButton.png"); 
            Yeutinhvang = (Bitmap)Bitmap.FromFile("Data/gapyeutinhvang.png"); 
            HuyTapKich = (Bitmap)Bitmap.FromFile("Data/huytapkich.png");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task t = new Task(() =>
            {
                isStop = false;
                Auto();
            });
            t.Start();
        }
        bool isStop = false;
        string teddy = "";
        void Auto()
        {
            // Lấy ra danh sách devices id để dùng
            List<string> devices = new List<string>();
            devices = KAutoHelper.ADBHelper.GetDevices();
    
            //For each deviceID để tạo từng task chạy độc lập
            foreach (var deviceID in devices)
            {
                //Tạo luồng xử lý riêng
                Task t = new Task(() =>
                {
                    while (true)
                    {
                        CountUpTime();
                        //Check mà k bấm nút stop thì chạy hoài
                        if (isStop)
                            return;

                        //KAutoHelper.ADBHelper.Tap(deviceID, 754, 1816);
                        
                        Boss(deviceID);
                        Delay(5);
                        
                        if (isStop)
                            return;
                        if (teddy != "")
                        {
                            KAutoHelper.ADBHelper.TapByPercent(deviceID, 78.6, 80.4);
                            Delay(2);
                            funcTeddy(teddy, deviceID);
                            //An vao nut boss.
                            Delay(4);
                            KAutoHelper.ADBHelper.TapByPercent(deviceID, 92.4, 79.9);
                            
                        }

                    }
                });
                t.Start();

            }
        }
        void Delay(int delay)
        {
            while (delay > 0)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                delay--;
                if (isStop)
                    break;
            }
        }

        void funcTeddy(string teddy,string deviceID)
        {
            //An vao con gau
            KAutoHelper.ADBHelper.Tap(deviceID, 397, 870);

            //Kiem tra status 
            Delay(5);
            var screenteddy = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
            var countDownPoint1 = KAutoHelper.ImageScanOpenCV.FindOutPoint(screenteddy, TeadyReady);
            var countDownPoint2 = KAutoHelper.ImageScanOpenCV.FindOutPoint(screenteddy, BitmapCheck);
            var countDownPoint3 = KAutoHelper.ImageScanOpenCV.FindOutPoint(screenteddy, Ketthucthamhiem); 

            if (countDownPoint1 != null)
            {
                pushLogToTextBox("Teddy đang rảnh\n");
                Delay(1);
                if (teddy == "0")
                {
                    //Bang gia
                    KAutoHelper.ADBHelper.Tap(deviceID, 263, 483);
                    Delay(1);
                    //Sua doi
                    KAutoHelper.ADBHelper.Tap(deviceID, 761, 876);
                    Delay(1);
                    selectMemberForTeddy(deviceID);
                }
                else if (teddy == "1")
                {
                    //Nui lua
                    KAutoHelper.ADBHelper.Tap(deviceID, 946, 837);
                    Delay(1);
                    //Sua doi
                    KAutoHelper.ADBHelper.Tap(deviceID, 761, 876);
                    Delay(1);
                    selectMemberForTeddy(deviceID);
                }
                else if (teddy == "2")
                {
                    //Khu rung
                    KAutoHelper.ADBHelper.Tap(deviceID, 751, 990);
                    Delay(1);
                    //Sua doi
                    KAutoHelper.ADBHelper.Tap(deviceID, 761, 876);
                    Delay(1);
                    selectMemberForTeddy(deviceID);
                }
                else if (teddy == "3")
                {
                    //Tu vien
                    KAutoHelper.ADBHelper.Tap(deviceID, 227, 1016);
                    Delay(1);
                    //Sua doi
                    KAutoHelper.ADBHelper.Tap(deviceID, 761, 876);
                    Delay(1);
                    selectMemberForTeddy(deviceID);
                }

                Delay(1);
                // An vao nut close
                KAutoHelper.ADBHelper.Tap(deviceID, 1004, 77);
                Delay(1);
            }
            else if (countDownPoint2 != null)
            {
                pushLogToTextBox("Kiểm tra tình hình\n");
                Delay(1);
                //An vao nut kiem tra
                KAutoHelper.ADBHelper.TapByPercent(deviceID, 22.0, 96.3);
                Delay(5);
                //An vao nut bat dau kiem tra
                
                //Delay(30);


                var screenteddy2 = KAutoHelper.ADBHelper.ScreenShoot(deviceID); 
                var ThongtinquandichPoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screenteddy2, Thongtinquandich);
                var ThongtinquandichPoint2 = KAutoHelper.ImageScanOpenCV.FindOutPoint(screenteddy2, Yeutinhvang);
                if (ThongtinquandichPoint != null)
                {
                    pushLogToTextBox("Gặp địch oánh liền\n");

                    //FightTeddy.Content = (Convert.ToInt32(FightTeddy.Content) + 1).ToString();
                    Dispatcher.BeginInvoke(
                        new ThreadStart(() => FightTeddy.Content = (Convert.ToInt32(FightTeddy.Content) + 1).ToString()));
                    Delay(2);
                    //An vao nut danh khi gap quai vat
                    KAutoHelper.ADBHelper.TapByPercent(deviceID, 27.7, 67.2);
                    
                    Delay(5);
                    //An vao nut co sau khi chon danh quai vat
                    KAutoHelper.ADBHelper.TapByPercent(deviceID, 21.7, 59.4);
                    while (true)
                    {
                        Delay(5);
                        pushLogToTextBox("Đang đợi đánh nhau trong thám hiểm\n");
                        screenteddy2 = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
                        var ThongtinquandichPoint3 = KAutoHelper.ImageScanOpenCV.FindOutPoint(screenteddy2, NutThoat);
                        if (ThongtinquandichPoint3 != null)
                        {
                            pushLogToTextBox("Đánh thám hiểm xong\n");
                            break;
                        }
                        
                        
                    }
                    Delay(3);
                    //An vao nut thoat sau khi danh xong | danh yeu tinh vang cũng thoát được bth
                    KAutoHelper.ADBHelper.TapByPercent(deviceID, 49.7, 69.9);

                    
                }
                else if (ThongtinquandichPoint2 != null)
                {
                    pushLogToTextBox("Gặp yêu tinh vàng\n");
                    //GoldTeddy.Content = (Convert.ToInt32(GoldTeddy.Content) + 1).ToString();
                    Dispatcher.BeginInvoke(
                        new ThreadStart(() => GoldTeddy.Content = (Convert.ToInt32(GoldTeddy.Content) + 1).ToString()));
                    Delay(2);
                    //An vao nut danh khi gap quai vat
                    KAutoHelper.ADBHelper.TapByPercent(deviceID, 27.7, 67.2);

                    Delay(1);
                    //An vao nut co sau khi chon danh quai vat
                    KAutoHelper.ADBHelper.TapByPercent(deviceID, 21.7, 59.4);

                    Delay(35);
                    //An vao nut thoat sau khi danh xong | danh yeu tinh vang cũng thoát được bth
                    KAutoHelper.ADBHelper.TapByPercent(deviceID, 49.7, 69.9);
                }
                else
                {
                    pushLogToTextBox("Các trường hợp còn lại của khám phá\n");
                    //OtherTeddy.Content = (Convert.ToInt32(OtherTeddy.Content) + 1).ToString();
                    Dispatcher.BeginInvoke(
                        new ThreadStart(() => OtherTeddy.Content = (Convert.ToInt32(OtherTeddy.Content) + 1).ToString()));
                    Delay(2);
                    //KAutoHelper.ADBHelper.Tap(deviceID, 407, 1320);
                    KAutoHelper.ADBHelper.TapByPercent(deviceID, 37.4, 66.3);
                }


                Delay(2);
                //An vao nut close
                KAutoHelper.ADBHelper.Tap(deviceID, 1004, 77);
                Delay(1);
            }
            else if (countDownPoint3 != null)
            {
                pushLogToTextBox("Kết thúc thám hiểm\n");
                //Teddycount.Content = (Convert.ToInt32(Teddycount.Content) + 1).ToString(); 
                Dispatcher.BeginInvoke(
                        new ThreadStart(() => Teddycount.Content = (Convert.ToInt32(Teddycount.Content) + 1).ToString()));
                Delay(1);
                //An vao nut ket thuc
                KAutoHelper.ADBHelper.TapByPercent(deviceID, 22.0, 96.3);
                Delay(1);
                // An vao nut nhan thuong
                KAutoHelper.ADBHelper.TapByPercent(deviceID, 39.8, 65.3);
                Delay(2);
                // An vao nut close
                KAutoHelper.ADBHelper.Tap(deviceID, 1004, 77);
                Delay(20);
            }
            else
            {
                pushLogToTextBox("Đang thám hiểm quay lại\n");
                Delay(1);
                // An vao nut close
                KAutoHelper.ADBHelper.Tap(deviceID, 1004, 77);
                Delay(1);
            }
            //Xac nhan ket thuc tham hiem
            //KAutoHelper.ADBHelper.Tap(deviceID, 442.5, 1289.6);
        }

        void selectMemberForTeddy(string deviceID)
        {
            KAutoHelper.ADBHelper.Swipe(deviceID, 530, 1016, 559, 1335, 100);
            Delay(1);
            KAutoHelper.ADBHelper.Swipe(deviceID, 530, 1016, 559, 1335, 100);

            //Chon con so 2
            KAutoHelper.ADBHelper.Tap(deviceID, 533, 1029);
            Delay(1);
            //Vuot len
            KAutoHelper.ADBHelper.Swipe(deviceID, 559, 1335, 530, 1016, 100);
            Delay(1);
            //Chon con so 1 hang so 3
            KAutoHelper.ADBHelper.Tap(deviceID, 312, 1006);
            Delay(1);
            //Chon con so 3 hang so 3
            KAutoHelper.ADBHelper.Tap(deviceID, 757, 1019);
            Delay(1);
            //An vao hoan tat
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 33.4, 83.8);
            Delay(1);
            //An vao xuat phat
            KAutoHelper.ADBHelper.TapByPercent(deviceID, 24.7, 81.2);
            Delay(2);
            // An vao nut huy neu tho san met
            var screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
            var cancelPoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, HuyTapKich);

            //Save images for test
            //var aaaa = KAutoHelper.ImageScanOpenCV.Find(screen, HuyTapKich);
            //aaaa.Save("aaaa.png");
            if (cancelPoint != null)
            {
                pushLogToTextBox("Thợ săn mệt cancel");
                KAutoHelper.ADBHelper.Tap(deviceID, cancelPoint.Value.X, cancelPoint.Value.Y);

                Delay(2);
                KAutoHelper.ADBHelper.Tap(deviceID, cancelPoint.Value.X, cancelPoint.Value.Y);
            }    
                
            Delay(2);
            //KAutoHelper.ADBHelper.Tap(deviceID, 1004, 77);
        }
        void Boss(string deviceID)
        {
            var screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
            var countDownPoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, CountDown);
            if (countDownPoint != null)
            {
                
                pushLogToTextBox("Boss đang hồi đợi ...\n");
                //Textbox1.ScrollToEnd();
            }

            else
            {
                Delay(5);
                KAutoHelper.ADBHelper.Tap(deviceID, 975, 288);
                Delay(2);
                KAutoHelper.ADBHelper.Tap(deviceID, 975, 288);
                Delay(2);
                KAutoHelper.ADBHelper.Tap(deviceID, 566, 1104);

                while (true)
                {
                    Delay(5);
                    pushLogToTextBox("Đang đánh boss\n");
                    screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
                    countDownPoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, CountDown);

                    if (countDownPoint != null)
                    {
                        if (isStop)
                            return;
                        pushLogToTextBox("Đánh xong boss Đợi 20s sau đó nhặt đồ\n");
                        //Bosskill.Content = (Convert.ToInt32(Bosskill.Content) + 1).ToString();
                        Dispatcher.BeginInvoke(
                        new ThreadStart(() => Bosskill.Content = (Convert.ToInt32(Bosskill.Content) + 1).ToString()));
                        Delay(20);
                        if (isStop)
                            return;
                        //Nhat item
                        KAutoHelper.ADBHelper.Tap(deviceID, 315, 743);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 387, 717);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 439, 691);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 455, 675);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 553, 649);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 588, 610);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 663, 584);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 413, 782);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 458, 743);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 530, 714);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 637, 688);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 689, 642);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 761, 616);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 481, 828);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 549, 789);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 640, 759);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 712, 694);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 790, 655);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 601, 896);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 696, 867);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 770, 824);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 806, 811);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 871, 772);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 926, 750);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 731, 980);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 793, 935);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 832, 906);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 939, 867);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 995, 808);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 783, 1036);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 887, 954);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 946, 938);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 1014, 883);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 865, 1088);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 939, 1003);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 1037, 909);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 1037, 909);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 998, 1042);
                        Delay(1);
                        KAutoHelper.ADBHelper.Tap(deviceID, 978, 1110);
                        pushLogToTextBox("Nhặt xong đồ\n");
                        break;
                    }

                }

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            isStop = true;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            teddy = cb.SelectedIndex.ToString();
            
        }

        void pushLogToTextBox(string logMessage)
        {
            var linesLog = Textbox1.LineCount.CompareTo(20);
            if (linesLog > 0)
            {
                Dispatcher.BeginInvoke(
                new ThreadStart(() => Textbox1.Clear()));
                
            }
            Dispatcher.BeginInvoke(
            new ThreadStart(() => Textbox1.Text += logMessage));
        }

        void CountUpTime()
        {
            if (isStop)
                return;
            Process currentProcess = Process.GetCurrentProcess();
            var runtime = DateTime.Now - Process.GetCurrentProcess().StartTime;
            Dispatcher.BeginInvoke(
            new ThreadStart(() => UpTime.Content = runtime)); 
    
            
        }
    }
}
