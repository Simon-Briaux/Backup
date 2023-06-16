using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Backup
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            Logs.Items.Add("D�but du programme");

            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType == DriveType.Removable && d.IsReady)
                {
                    Logs.Items.Add("Drive type: " + d.DriveType);
                    Logs.Items.Add("Volume label: " + d.VolumeLabel);
                    Logs.Items.Add("File system: " + d.DriveFormat);
                    Logs.Items.Add("Available space to current user: " + d.AvailableFreeSpace.ToString("N0") + " bytes");
                    Logs.Items.Add("Total available space: " + d.TotalFreeSpace.ToString("N0") + " bytes");
                    Logs.Items.Add("Total size of drive: " + d.TotalSize.ToString("N0") + " bytes");

                    string logpath = Path.Combine(d.Name, "Logs.txt");
                    string paramPath = Path.Combine(d.Name, "Param.txt");

                    using (StreamWriter writer = new StreamWriter(logpath))
                    {
                        writer.WriteLine("Fichier log :");
                    }

                    Logs.Items.Add(paramPath);
                    bool fileExists = File.Exists(paramPath);

                    using (StreamWriter writer = new StreamWriter(logpath, true))
                    {
                        writer.WriteLine(fileExists ? "param.txt existe" : "Le fichier param.txt n'existe pas");
                    }

                    if (fileExists)
                    {
                        Logs.Items.Add("File exists.");
                        string paramContent = File.ReadAllText(paramPath);
                        Logs.Items.Add(paramContent);

                        string user = Environment.UserName;
                        Logs.Items.Add(user);

                        if (paramContent == user)
                        {
                            Logs.Items.Add("Correspondence entre le fichier et l'utilisateur");
                            using (StreamWriter writer = new StreamWriter(logpath, true))
                            {
                                writer.WriteLine("Correspondence entre le fichier et l'utilisateur");
                            }

                            string backupDir = Path.Combine(d.Name, user + "Backup");

                            if (!Directory.Exists(backupDir))
                            {
                                Directory.CreateDirectory(backupDir);
                                progressBar1.Value = 100;
                            }

                            string sourceDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                            //string sourceDir = @"C:\test";
                            string destinationDir = backupDir;

                            CopyDirectoryWithProgressBar(sourceDir, destinationDir, true, logpath);
                            Logs.Items.Add("Copie termin�e.");
                            using (StreamWriter writer = new StreamWriter(logpath, true))
                            {
                                writer.WriteLine("Copie termin�e");
                            }
                        }
                        else
                        {
                            Logs.Items.Add("Aucune correspondance entre le fichier et l'utilisateur");
                            using (StreamWriter writer = new StreamWriter(logpath, true))
                            {
                                writer.WriteLine("Aucune correspondance entre le fichier et l'utilisateur");
                            }
                        }
                    }
                    else
                    {
                        using (StreamWriter writer = new StreamWriter(logpath, true))
                        {
                            writer.WriteLine("Le fichier param.txt n'existe pas");
                        }
                        Logs.Items.Add("File does not exist.");
                    }
                }
            }
        }



        static DateTime GetDateWithoutSeconds(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
        }

        private void Logs_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100; // La valeur maximale doit �tre ajust�e en fonction de votre logique de progression
            progressBar1.Value = 0;

        }



        private void button2_Click(object sender, EventArgs e)
        {
            string user = Environment.UserName;
            Logs.Items.Add(user);
            string path = @"d:\param.txt";
            string folderPath = @"d\" + user + "Backup";

            try
            {
                // Cr�er le fichier, ou le remplacer s'il existe d�j�.
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(user);
                    fs.Write(info, 0, info.Length);
                }

                // Ouvrir le flux et le lire.
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            try
            {
                // Cr�er le fichier, ou le remplacer s'il existe d�j�.
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(user);
                    // Ajouter des informations au fichier.
                    fs.Write(info, 0, info.Length);
                }

                // Ouvrir le flux et le lire.
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    Logs.Items.Add(folderPath);
                }
                else
                {
                    Console.WriteLine("Le dossier existe d�j� : " + folderPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la cr�ation du dossier : " + ex.ToString());
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void CopyDirectoryWithProgressBar(string sourceDir, string destinationDir, bool recursive, string logpath)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDir);

            if (!dir.Exists)
                throw new DirectoryNotFoundException($"R�pertoire source introuvable : {dir.FullName}");

            if (!Directory.Exists(destinationDir))
                Directory.CreateDirectory(destinationDir);

            int fileCount = dir.GetFiles().Length;
            progressBar1.Maximum = fileCount;
            int currentFile = 0;

            Queue<string> lockedFilesQueue = new Queue<string>(); // File d'attente des fichiers bloqu�s

            foreach (string filePath in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(filePath);

                if (fileName.Equals("NTuser.dat", StringComparison.OrdinalIgnoreCase))
                    continue;

                if (fileName.Equals("NTuser.dat.LOG1", StringComparison.OrdinalIgnoreCase))
                    continue;

                CheckFileLockStatus(filePath, lockedFilesQueue);

                try
                {
                    string targetFilePath = Path.Combine(destinationDir, Path.GetFileName(filePath));
                    FileInfo targetFile = new FileInfo(targetFilePath);

                    if (targetFile.Exists)
                    {
                        targetFile.Refresh();
                        FileInfo file = new FileInfo(filePath);
                        file.Refresh();
                        TimeSpan timeDifference = file.LastWriteTime - targetFile.LastWriteTime;
                        TimeSpan threshold = TimeSpan.FromSeconds(20);

                        if (targetFile.Length != file.Length || timeDifference > threshold)
                        {
                            File.Copy(filePath, targetFilePath, true);
                            Logs.Items.Add($"Le fichier {file.FullName} pr�sent sur le disque dur {GetDateWithoutSeconds(file.LastWriteTime)} est aussi pr�sent sur la cl� {GetDateWithoutSeconds(targetFile.LastWriteTime)}");
                        }
                        else
                        {
                            Logs.Items.Add($"Ce fichier a �t� copi� : {fileName}");
                        }
                    }
                    else
                    {
                        File.Copy(filePath, targetFilePath, true);
                        Logs.Items.Add($"Ce fichier a �t� copi� : {fileName}");
                    }
                }
                catch (IOException ex)
                {
                    Logs.Items.Add($"Erreur lors de la copie du fichier {fileName} : {ex.Message}");
                }

                currentFile++;
                progressBar1.Value = currentFile;
            }

            if (recursive)
            {
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    try
                    {
                        string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                        CopyDirectoryWithProgressBar(subDir.FullName, newDestinationDir, true, logpath);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Logs.Items.Add($"Acc�s refus� pour le r�pertoire : {subDir.FullName}");
                        Logs.Items.Add(ex.Message);
                    }
                }
            }
        }

        private void CheckFileLockStatus(string filePath, Queue<string> lockedFilesQueue)
        {
            try
            {
                using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    // Le fichier est accessible en lecture, il n'est pas verrouill�
                    if (lockedFilesQueue.Contains(filePath))
                    {
                        lockedFilesQueue.Dequeue(); // Retirer le fichier de la file d'attente
                    }
                }
            }
            catch (IOException)
            {
                // Le fichier est verrouill�
                if (!lockedFilesQueue.Contains(filePath))
                {
                    lockedFilesQueue.Enqueue(filePath); // Ajouter le fichier � la file d'attente s'il n'est pas d�j� pr�sent
                }
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            // Sp�cifier le lien de t�l�chargement du fichier ZIP
            string downloadLink = "https://ec.ccm2.net/www.commentcamarche.net/download/files/Usb_Autorun_1.0.zip";

            // Sp�cifier la lettre de lecteur de la cl� USB
            string usbDriveLetter = "D";

            // Sp�cifier le chemin de destination pour enregistrer le fichier ZIP
            string zipFilePath = usbDriveLetter + @":\Usb_Autorun_1.0.zip";

            // T�l�charger le fichier ZIP
            using (var webClient = new WebClient())
            {
                webClient.DownloadFile(downloadLink, zipFilePath);
            }

            // D�compresser le fichier ZIP dans la destination souhait�e
            string destinationDirectory = usbDriveLetter + @":\";
            ZipFile.ExtractToDirectory(zipFilePath, destinationDirectory);

            // Chemin du fichier "autorun.inf" � r��crire
            string autorunFilePath = destinationDirectory + "autorun.inf";

            // Contenu � �crire dans le fichier "autorun.inf"
            string autorunContent = @"Backup\Backup\bin\Debug\net6.0-windows\Backup.exe";

            // R��crire le fichier "autorun.inf"
            File.WriteAllText(autorunFilePath, autorunContent);
        }
    }
}
