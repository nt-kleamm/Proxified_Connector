﻿using Microsoft.Win32;

namespace Proxified_Connector
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // PROXY LIST VARIABLES
            string[] proxyListItems;
            int proxyListLine = 0;
            string proxyListPath = string.Empty;
            bool isProxyFound = false;
            string proxyItemAccessor = string.Empty;
            int proxyItemReader = 0;
            string proxyConnectionString = string.Empty;

            // PROXY IP & PORT
            string proxyIP = string.Empty;
            string proxyPort = string.Empty;

            // DRIVE VARIABLES
            string[] driveArray = new string[50];
            string driveNames = String.Empty;
            string selectedDrive = String.Empty;
            int driveIndex = 0;
            bool isDriveValid = false;

            // PROXY CHANGE SYSTEM
            RegistryKey registryKey;

            // TAKES DRIVER INFORMATION
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            // WRITES DRIVERS
            foreach (DriveInfo d in allDrives)
            {
                // SETS THE ARRAY & SETS THE WHOLE STRING
                driveArray[driveIndex] = d.Name.Substring(0, 2);
                driveNames += d.Name.Substring(0, 2) + " ";

                // INCREASES INDEX OF ARRAY
                driveIndex++;
            }

            // CLEARS THE LAST SPACE
            driveNames = driveNames.Substring(0, driveNames.Length - 1);

            // SETS THE DRIVE
            do
            {
                Console.Write($"Select Drive ({driveNames}) --> ");
                selectedDrive = Console.ReadLine();

                // SETS THE STRING TO UPPER
                selectedDrive = selectedDrive.ToUpper();

                // ADDS THE COLON IF IT DOESN'T CONTAINS
                if (!selectedDrive.Contains(":"))
                {
                    selectedDrive += ":";
                }

                // LINE SPACE
                Space();

                for (int i = 0; i < driveArray.Length - 1; i++)
                {
                    if (selectedDrive == driveArray[i])
                    {
                        isDriveValid = true;
                        break;
                    }
                }
            } while (!isDriveValid);

            proxyListPath = selectedDrive + @"\Proxified\Proxylist.txt";

            if (File.Exists(proxyListPath))
            {
                // INFORMATION LOG
                Console.WriteLine("|######################################|");
                Console.WriteLine("|         Proxified List Found         |");
                Console.WriteLine("|######################################|");

                // SETS BOOL
                isProxyFound = true;

                // LINE SPACE
                Space();
            }
            else
            {
                // INFORMATION LOG
                Console.WriteLine("|######################################|");
                Console.WriteLine("|       Proxified List Not Found       |");
                Console.WriteLine("|######################################|");

                // SETS BOOL
                isProxyFound = false;

                // LINE SPACE
                Space();
            }

            // SETS THE PROXY LIST LINES
            proxyListItems = File.ReadAllLines(proxyListPath);

            // SETS THE PROXYLIST PROXY QUANTITY
            proxyListLine = File.ReadAllLines(proxyListPath).Length;

            if (isProxyFound)
            {
                //  PROXY LINES
                for (int i = 0; i < proxyListLine; i++)
                {
                    // SETS THE PROXY LINES ONE BY ONE
                    proxyItemAccessor = proxyListItems[i];

                    // READ IP ADDRESS
                    while (proxyItemAccessor[proxyItemReader].ToString() != "#")
                    {
                        proxyIP += proxyItemAccessor[proxyItemReader];
                        proxyItemReader++;
                    }

                    // INCREASES FOR PORT 
                    proxyItemReader++;

                    // READ PORT
                    while (proxyItemAccessor[proxyItemReader] != '#')
                    {
                        proxyPort += proxyItemAccessor[proxyItemReader];
                        proxyItemReader++;
                    }

                    // CONNECTS TO A PROXY
                    registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
                    proxyConnectionString = proxyIP + ":" + proxyPort;
                    registryKey.SetValue("ProxyEnable", 1);
                    registryKey.SetValue("ProxyServer", proxyConnectionString);

                    // INFORMATION LOG
                    Console.WriteLine("|######################################|");
                    Console.WriteLine("|      Connected To A New Location     |");
                    Console.WriteLine("|######################################|");

                    // LINE SPACE
                    Space();

                    Console.WriteLine($"- {proxyConnectionString}");

                    // LINE SPACE
                    Space();

                    // EMPTIFY IP & PROXY
                    proxyItemReader = 0;
                    proxyIP = string.Empty;
                    proxyPort = string.Empty;

                    // WAITS FOR PROXYCHANGE
                    Thread.Sleep(45000);
                }
            }

            Console.ReadLine();
        }

        private static void Space()
        {
            Console.WriteLine("");
        }
    }
}