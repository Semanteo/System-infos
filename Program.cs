using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Management;
using System.Windows.Forms;

namespace Internet
{
    class Program
    {
        static void Main(string[] args){
            Console.WriteLine("Informations PC :");
            Console.WriteLine("");
            string host = Dns.GetHostName();
            Console.WriteLine("Nom hôte : {0}", host);
            Console.WriteLine("Nom utilisateur : {0}", Environment.UserName);  
            string ip = Dns.GetHostEntry(host).AddressList[3].ToString();
            Console.WriteLine("Adresse IP : {0}", ip);
            Console.WriteLine("--------------------------------------------------------------");   
            Console.WriteLine("Informations Disque :");
            Console.WriteLine("");    
            foreach (DriveInfo CurrentDrive in DriveInfo.GetDrives()) 
            { 
            if (CurrentDrive.DriveType == DriveType.Fixed) 
            { 
            Double pourcentageLibre = ((Double)CurrentDrive.AvailableFreeSpace / CurrentDrive.TotalSize) * 100; 
            Console.WriteLine("Espace libre de {0} : {1}%", CurrentDrive.Name, Convert.ToInt16(pourcentageLibre)); 
            } 
            }
            Console.WriteLine("System directory : {0}", Environment.SystemDirectory);
            int countProc = Environment.ProcessorCount;
            Console.WriteLine("--------------------------------------------------------------");   
            Console.WriteLine("Informations Système :");
            Console.WriteLine("");
            {
	        ManagementObjectSearcher searcher = new ManagementObjectSearcher("select NAME from win32_processor");
	        foreach (ManagementObject name in searcher.Get()) 
	        {
		    Console.WriteLine("Processeur: {0}", name["Name"].ToString());
	        }
            }
            Console.WriteLine("Nombre de coeurs: {0}", countProc);
            {   
            ManagementObjectSearcher objvide = new ManagementObjectSearcher("select * from Win32_VideoController");

            foreach (ManagementObject obj in objvide.Get())
            {
            Console.WriteLine("Carte Graphique : {0}", obj["Name"]);
            }
            }
            {
	        ManagementObjectSearcher searche = new ManagementObjectSearcher("select * from win32_BaseBoard");
	        foreach (ManagementObject nae in searche.Get()) 
	        {
		    Console.WriteLine("Carte mère : {0} {1}", nae["Manufacturer"].ToString(), nae["Product"].ToString());
	        }
            }
            ManagementObjectSearcher search = new ManagementObjectSearcher("Select * From Win32_PhysicalMemory");

            foreach (ManagementObject ram in search.Get())
            {
            Console.WriteLine("RAM: \nPartNumber : {0}\tCapacity : {1}", ram.GetPropertyValue("PartNumber"), Convert.ToDouble(ram.GetPropertyValue("Capacity")) / 1073741824 + "GB");
            }
            Console.WriteLine("--------------------------------------------------------------");   
            Console.WriteLine("Informations Utilisation:");
            Console.WriteLine("");
            PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory("Network Interface");
            string instance = performanceCounterCategory.GetInstanceNames()[0];
            PerformanceCounter performanceCounterSent = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
            PerformanceCounter performanceCounterReceived = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);
            Console.WriteLine("Débit Internet utilisé :");
            float d = 0;
            float u = 0;
            int r = 10;
            for (int i = 0; i < r; i++)
            {
            d = d+(performanceCounterSent.NextValue()/1024);
            u = u+(performanceCounterReceived.NextValue()/1024);
            System.Threading.Thread.Sleep(500);
            if(i == r/2){
            Console.WriteLine("Test en cours...");
            }
            }
            float md = d/r;
            float mu = u/r;
            Console.WriteLine("Download: {0} Kb/s\tUpload: {1} Kb/s", md, mu);
            Console.WriteLine("Appuyez sur r pour recommencer le programme ou sur une autre touche pour quitter");
            string rest = Console.ReadLine();
            if(rest == "r"){
            Console.WriteLine("Redémarrage du programme");
            }
            else 
            {
            Console.Write("Press any key to exit...");
            Console.ReadKey(true);
            }            
        }
    }
}