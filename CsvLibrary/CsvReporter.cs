using Entities;
using System;
using System.IO;

namespace CsvLibrary
{
    public class CsvReporter
    {
        bool rgvkIsNew = false;
        bool sfkIsNew = false;

        string rgvkPath = @"C:\Users\Public\Documents\Reports\";
        string sfkPath = @"C:\Users\Public\Documents\Reports_ekb\";
        public void CreateRgvkFile()
        {
            using (StreamWriter writer = new StreamWriter(rgvkPath + $"RGVK_KARAKURTBARAJIVEHES_{DateTime.Now.Date.ToString("yyyyMMdd")}.csv", true))
            {
                writer.WriteLine("GERILIM REFERANS DEGERI ILETILEN BARANIN ADI:;KARAKURT_BARAJI_VE_HES");
                writer.WriteLine("ILGILI BIRIMIN UNITELERININ NOMINAL AKTIF GUCU(Pnom) VE MINIMUM KARARLI URETIM DUZEYI (MKUD) (MW):;44,04;9;44,04;9;8,91;1,9");
                writer.WriteLine("ILGILI BIRIMIN UNITELERININ ASIRI VE DUSUK ZORUNLU MVAR DEGERLERI (MVAR):;27,29;-14,47;27,29;-14,47;5,52;-2,92");
                writer.WriteLine("VARSA ILGILI ANLASMA DAHILINDE SENKRON KOMPANSATOR HIZMET VEREBILEN UNITELERIN SENKRON KOMPASATOR DURUMUNDAKI ZORUNLU MVAR DEGERLERI (MVAR):;0");
                writer.WriteLine("TARIH;SAAT;SIRA_NO;BARA_GER_kV;BARA_GER_SET_DEG_kV;BRM_DONEN_ASIRIIKAZ_ZOR_MVAR;BRM_DONEN_DUSUKIKAZ_ZOR_MVAR;BRM_GEN_TER_TOPLAM_MVAR;UNI_1_GEN_TER_AKT_CIK_GUCU_MW;UNI_1_GEN_TER_REA_GUCU_MVAr;UNI_1_GEN_TER_GER_kV;UNI_1_GEN_MOD;UNI_2_GEN_TER_AKT_CIK_GUCU_MW;UNI_2_GEN_TER_REA_GUCU_MVAr;UNI_2_GEN_TER_GER_kV;UNI_2_GEN_MOD;UNI_3_GEN_TER_AKT_CIK_GUCU_MW;UNI_3_GEN_TER_REA_GUCU_MVAr;UNI_3_GEN_TER_GER_kV;UNI_3_GEN_MOD");
            }
           /* string textFile = path + "test.txt";
            DateTime past = DateTime.Now.AddDays(-1);
            string zipFile = path + $"RGVK_KARAKURT_{past.ToString("yyyyMMdd")}.zip";

            var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create);
            archive.CreateEntryFromFile(textFile, Path.GetFileName(textFile));
            archive.Dispose(); */
        }

        public void CreateSfkFile()
        {
            using (StreamWriter writer = new StreamWriter(sfkPath + $"EKB_KARAKURTHES_{DateTime.Now.Date.ToString("yyyyMMdd")}.csv", true))
            {
                writer.WriteLine("SANTRAL ADI:;KARAKURT BARAJI VE HES(BILSEV)");
                writer.WriteLine("FREKANS KONTROLUNE KATILAN BIRIMIN ADI:; KARAKURT BARAJI VE HES(BILSEV)");
                writer.WriteLine("FREKANS KONTROLUNE KATILAN BIRIMIN (UEVCB YA DA AGC'DEN REFERANS DEGER GELEN BIRIM) (SANTRAL/KOMBINE CEVRIM BLOGU/UNITE) NOMINAL AKTIF GUCU (KURULU GUCU) (MW):; 96,99");
                writer.WriteLine("ILGILI BIRIMIN AKTIF UNITELERININ PRIMER FREKANS KONTROLU ICIN GECERLI DROOP DEGERLERI (%):;4;4");
                writer.WriteLine("ILGILI BIRIMIN AKTIF UNITELERININ GECERLI DROOP DEGERLERI ILE, 200 mHz'LIK BASAMAK FREKANS DEGISIMINE VERDIGI TEPKININ ORTALAMA DENGEYE GELME SURESI (sn):;43;43");
                writer.WriteLine("ILGILI BIRIMIN AKTIF UNITELERININ YUK ALMA - ATMA HIZLARI (MW/dak):;13,8;15,4");
                writer.WriteLine("ILGILI BIRIMIN AKTIF UNITELERININ HIZ REGULATORUNE GIDEN HIZ BILGISINDEKI +/- ISTEKSEL OLU-BAND DEGERI (mHz):;0;0");
                writer.WriteLine("TARIH;SAAT;SIRA_NO;FRE_HZ;BRM_GUC_REF_DEG_MW;BRM_AKT_CIK_GUCU_BRUTMW;BRM_AKT_CIK_GUCU_NETMW;BRM_PFK_TPLM_NOM_GUCMW;BRM_SEK_MAK_MW;BRM_SEK_MIN_MW;BRM_PRI_MAKC_MW;BRM_PRI_MINC_MW;BRM_GNCL_KPR_MW/HZ;BRM_SFK_REZ_MIK_MW;BRM_PFK_REZ_MIK_MW;AGC_AKT;PFCO_AKT");
            }
        }

        public bool WriteRgvkData(Unit unit, int counter)
        {

            if (!File.Exists(rgvkPath + $"RGVK_KARAKURTBARAJIVEHES_{DateTime.Now.Date.ToString("yyyyMMdd")}.csv"))
            {
                CreateRgvkFile();

                rgvkIsNew = true;
                counter = 1;
            }

            using (StreamWriter writer = new StreamWriter(rgvkPath + $"RGVK_KARAKURTBARAJIVEHES_{DateTime.Now.Date.ToString("yyyyMMdd")}.csv", true))
            {
                writer.WriteLine(DateTime.Now.ToString("dd.MM.yyyy") + ";" + DateTime.Now.ToString("HH:mm:00") + ";" + counter.ToString() + ";" + unit.BusbarVoltage.ToString("0.00") + ";" + 
                    unit.BusbarSetVoltage + ";" + unit.BrmHighReactivePower.ToString("0.00") + ";" + unit.BrmLowReactivePower.ToString("0.00") + ";" + unit.BrmTotalReactivePower.ToString("0.00") + ";" + 
                    unit.Unit1ActivePower.ToString("0.00") + ";" + unit.Unit1ReactivePower.ToString("0.00") + ";" + unit.Unit1Voltage.ToString("0.00") + ";" + unit.Unit1Mode + ";" +
                    unit.Unit2ActivePower.ToString("0.00") + ";" + unit.Unit2ReactivePower.ToString("0.00") + ";" + unit.Unit2Voltage.ToString("0.00") + ";" + unit.Unit2Mode + ";" +
                    unit.Unit3ActivePower.ToString("0.00") + ";" + unit.Unit3ReactivePower.ToString("0.00") + ";" + unit.Unit3Voltage.ToString("0.00") + ";" + unit.Unit3Mode);
            }

            if (rgvkIsNew)
            {
                rgvkIsNew = false;
                return true;
            }
            else
            {
                return false;
            }
        }

   
        public bool WriteSfkData(SfkReport sfkReport, int counter)
        {

            if (!File.Exists(sfkPath + $"EKB_KARAKURTHES_{DateTime.Now.Date.ToString("yyyyMMdd")}.csv"))
            {
                CreateSfkFile();
                sfkIsNew = true;
                counter = 1;
            }

            using (StreamWriter writer = new StreamWriter(sfkPath + $"EKB_KARAKURTHES_{DateTime.Now.Date.ToString("yyyyMMdd")}.csv", true))
            {
                writer.WriteLine(DateTime.Now.ToString("dd.MM.yyyy") + ";" + DateTime.Now.ToString("HH:mm:ss") + ";" + counter.ToString() + ";" + sfkReport.FRE_HZ.ToString("0.000") + ";" +
                    sfkReport.BRM_GUC_REF_DEG_MW.ToString("0.000") + ";" + sfkReport.BRM_AKT_CIK_GUCU_BRUTMW.ToString("0.000") + ";" + sfkReport.BRM_AKT_CIK_GUCU_NETMW.ToString("0.000") + ";" + sfkReport.BRM_PFK_TPLM_NOM_GUCMW.ToString("0.000") + ";" +
                    sfkReport.BRM_SEK_MAK_MW.ToString("0.000") + ";" + sfkReport.BRM_SEK_MIN_MW.ToString("0.000") + ";" + sfkReport.BRM_PRI_MAKC_MW.ToString("0.000") + ";" + sfkReport.BRM_PRI_MINC_MW.ToString("0.000") + ";" +
                    sfkReport.BRM_GNCL_KPR_MW_HZ.ToString("0.000") + ";" + sfkReport.BRM_SFK_REZ_MIK_MW.ToString("0.000") + ";" + sfkReport.BRM_PFK_REZ_MIK_MW.ToString("0.000") + ";" +
                    sfkReport.AGC_AKT.ToString("0") + ";" + sfkReport.PFCO_AKT.ToString("0"));
            }

            if (sfkIsNew)
            {
                sfkIsNew = false;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
