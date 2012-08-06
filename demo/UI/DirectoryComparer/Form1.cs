using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FileComparer.Move.Rules;
using System.Threading;

namespace DirectoryComparer
{
    public partial class Form1 : Form
    {
        private TextBox openDialogTextBoxSender;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerAsync();
        }


        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            FileComparer.Scan.DirectoryScanner sourceScanner = new FileComparer.Scan.DirectoryScanner(txtSource.Text);
            FileComparer.Scan.DirectoryScanner destScanner = new FileComparer.Scan.DirectoryScanner(txtDest.Text);

            sourceScanner.ScanDirectory(sourceScanner.SourceDirectory);
            destScanner.ScanDirectory(destScanner.SourceDirectory);

            FileComparer.Compare.FileComparer fileComparer = new FileComparer.Compare.FileComparer(sourceScanner.ScannedInfo, destScanner.ScannedInfo);
            fileComparer.LeftOverLocation = txtLeftOvers.Text;
            fileComparer.OnComparisonCompleted += new FileComparer.Compare.ComparisonCompletionHandler(fileComparer_OnComparisonCompleted);
            fileComparer.Compare();   
        }

        void fileComparer_OnComparisonCompleted(List<FileComparer.Move.Rules.IRule> rules)
        {
            MultiFileMover fileMover = new MultiFileMover();
            foreach (IRule rule in rules)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(fileMover.MoveFile));
                thread.Start(rule);
            }
        }

        private void txtSource_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(sender);
        }

        private void txtDest_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(sender);
        }

        private void OpenFolderDialog(object sender)
        {
            DialogResult result = folderBrowserDialog.ShowDialog(this);
            openDialogTextBoxSender = sender as TextBox;
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                openDialogTextBoxSender.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void txtLeftOvers_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(sender);
        }
    }
}
