using CsvLibrary;
using Entities;
using Karakurt.MailManager;
using ModbusLibrary;
using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using System.IO.Compression;
using PrecisionTiming;

namespace AppService
{
    public partial class Service1 : ServiceBase
    {
        PrecisionTimer _timer;
        PrecisionTimer _sfkTimer;
        //Timer _sfkTimer;
        ModbusTcpClient _client;
        CsvReporter _reporter;

        public Service1()
        {
            InitializeComponent();
            _timer = new PrecisionTimer();
            _sfkTimer = new PrecisionTimer();
            //_sfkTimer = new Timer();
            _reporter = new CsvReporter();
             _client = new ModbusTcpClient("172.16.4.100", 502);
           // _client = new ModbusTcpClient("127.0.0.1", 502);
        }

        protected override void OnStart(string[] args)
        {
            _timer.SetInterval(() => _timer_Elapsed(), 60000);
            _timer.Start();

            _sfkTimer.SetInterval(() => _sfkTimer_Elapsed(), 1000);
            _sfkTimer.Start();


        }

        private async void _sfkTimer_Elapsed()
        {
            try
            {
                _client.Connect();
                bool mbdurum = _client.Available(500);

                if(mbdurum==true)
                {
                    SfkReport sfkReport = new SfkReport()
                    {
                        FRE_HZ = _client.GetHoldingRegisterData(1036),
                        BRM_GUC_REF_DEG_MW = _client.GetHoldingRegisterData(1028),
                        BRM_AKT_CIK_GUCU_BRUTMW = _client.GetHoldingRegisterData(1030),
                        BRM_AKT_CIK_GUCU_NETMW = _client.GetHoldingRegisterData(1032),
                        BRM_PFK_TPLM_NOM_GUCMW = _client.GetHoldingRegisterData(1034),
                        BRM_SEK_MAK_MW = _client.GetHoldingRegisterData(1014),
                        BRM_SEK_MIN_MW = _client.GetHoldingRegisterData(1018),
                        BRM_PRI_MAKC_MW = _client.GetHoldingRegisterData(1016),
                        BRM_PRI_MINC_MW = _client.GetHoldingRegisterData(1020),
                        BRM_GNCL_KPR_MW_HZ = _client.GetHoldingRegisterData(1012),
                        BRM_SFK_REZ_MIK_MW = _client.GetHoldingRegisterData(1026),
                        BRM_PFK_REZ_MIK_MW = _client.GetHoldingRegisterData(1024),
                        AGC_AKT = Convert.ToInt32(_client.GetHoldingRegisterData(1010)),
                        PFCO_AKT = Convert.ToInt32(_client.GetHoldingRegisterData(1022))
                    };
                    _client.Disconnect();
                    int currentCounterSecond = CalculateCounterSecond();
                    bool isNew = _reporter.WriteSfkData(sfkReport, currentCounterSecond);
                }
                else
                {

                }
            }
            catch
            {
                
               try
                {
                    SfkReport sfkReport = new SfkReport()
                    {
                        FRE_HZ = 0,
                        BRM_GUC_REF_DEG_MW = 0,
                        BRM_AKT_CIK_GUCU_BRUTMW = 0,
                        BRM_AKT_CIK_GUCU_NETMW = 0,
                        BRM_PFK_TPLM_NOM_GUCMW = 0,
                        BRM_SEK_MAK_MW = 0,
                        BRM_SEK_MIN_MW = 0,
                        BRM_PRI_MAKC_MW = 0,
                        BRM_PRI_MINC_MW = 0,
                        BRM_GNCL_KPR_MW_HZ = 0,
                        BRM_SFK_REZ_MIK_MW = 0,
                        BRM_PFK_REZ_MIK_MW = 0,
                        AGC_AKT = 0,
                        PFCO_AKT = 0,
                    };
                    int currentCounterSecond = CalculateCounterSecond();
                    bool isNew = _reporter.WriteSfkData(sfkReport, currentCounterSecond);
                }
                catch
                {
                  
                }
            }
            
        }

        private void _timer_Elapsed()
        {
            try
            {
                _client.Connect();
                Unit unit = new Unit()
                 {
                     BusbarVoltage = _client.GetHoldingRegisterData(543) / 1000,
                     BusbarSetVoltage = _client.GetHoldingRegisterData(545),
                     BrmHighReactivePower = _client.GetHoldingRegisterData(531),
                     BrmLowReactivePower = _client.GetHoldingRegisterData(533),
                     BrmTotalReactivePower = _client.GetHoldingRegisterData(535),
                     Unit1ActivePower = _client.GetHoldingRegisterData(516) / 1000000,
                     Unit1ReactivePower = _client.GetHoldingRegisterData(518) / 1000000,
                     Unit1Voltage = _client.GetHoldingRegisterData(522) / 1000,
                     Unit1Mode = _client.GetHoldingRegisterData(537),
                     Unit2ActivePower = _client.GetHoldingRegisterData(508) / 1000000,
                     Unit2ReactivePower = _client.GetHoldingRegisterData(510) / 1000000,
                     Unit2Voltage = _client.GetHoldingRegisterData(514) / 1000,
                     Unit2Mode = _client.GetHoldingRegisterData(539),
                     Unit3ActivePower = _client.GetHoldingRegisterData(500) / 1000000,
                     Unit3ReactivePower = _client.GetHoldingRegisterData(502) / 1000000,
                     Unit3Voltage = _client.GetHoldingRegisterData(506) / 1000,
                     Unit3Mode = _client.GetHoldingRegisterData(541)
                 };

                 _client.Disconnect();
                int currentCounter = CalculateCounter();
                bool isNew = _reporter.WriteRgvkData(unit, currentCounter);
                
                
                if (DateTime.Now.Hour == 9 && DateTime.Now.Minute == 30)

                {
                    string path_ekb = @"C:\Users\Public\Documents\Reports_ekb\";
                    DateTime past_day_ekb = DateTime.Now.AddDays(-1);
                    string textFile_ekb = path_ekb + $"EKB_KARAKURTHES_{past_day_ekb.ToString("yyyyMMdd")}.csv";
                    string zipFile_ekb = path_ekb + $"EKB_KARAKURT_{past_day_ekb.ToString("yyyyMMdd")}.zip";

                    var archive = ZipFile.Open(zipFile_ekb, ZipArchiveMode.Create);
                    archive.CreateEntryFromFile(textFile_ekb, Path.GetFileName(textFile_ekb));
                    archive.Dispose();
                    MailSender.Send(path_ekb + $"EKB_KARAKURT_{past_day_ekb.ToString("yyyyMMdd")}.zip", false);
                }

            }
            catch           
            {
               try
                {
                    Unit unit = new Unit()
                    {
                        BusbarVoltage = 0,
                        BusbarSetVoltage = 0,
                        BrmHighReactivePower = 0,
                        BrmLowReactivePower = 0,
                        BrmTotalReactivePower = 0,
                        Unit1ActivePower = 0,
                        Unit1ReactivePower = 0,
                        Unit1Voltage = 0,
                        Unit1Mode = 0,
                        Unit2ActivePower = 0,
                        Unit2ReactivePower = 0,
                        Unit2Voltage = 0,
                        Unit2Mode = 0,
                        Unit3ActivePower = 0,
                        Unit3ReactivePower = 0,
                        Unit3Voltage = 0,
                        Unit3Mode = 0
                    };
                    int currentCounter = CalculateCounter();
                    bool isNew = _reporter.WriteRgvkData(unit, currentCounter);
                }
                catch
                {
                   
                }
                

            }
           
        }

        protected override void OnStop()
        {
            _timer.Stop();
            _timer.Dispose();

            _sfkTimer.Stop();
            _sfkTimer.Dispose();
    
        }

        private int CalculateCounter()
        {
            int hour = DateTime.Now.Hour;
            int minute = DateTime.Now.Minute;

            return (hour * 60) + minute;
        }

        private int CalculateCounterSecond()
        {
            int hour = DateTime.Now.Hour;
            int minute = DateTime.Now.Minute;
            int second = DateTime.Now.Second;

            return (hour * 3600) + (minute * 60) + second;
        }
    }
}
