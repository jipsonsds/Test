using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SevenZip;
using SevenZip.Compression;
namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        string _CopyPath = Application.StartupPath + "\\7ZipCopy";
        string _CopyPathAES = Application.StartupPath + "\\AESCopy";
        public Form1()
        {
            InitializeComponent();
            My7Zip.Test();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            ofd7Zip.Filter = "7Z files (*.7Z)|*.7z|All files (*.*)|*.*";
            
            this.ofd7Zip.Multiselect = false;
            if (!Directory.Exists(_CopyPath))
            {
                Directory.CreateDirectory(_CopyPath);
            }
            if (ofd7Zip.ShowDialog() == DialogResult.OK)
            {
                txtFileName.Text = ofd7Zip.FileName;
                File.Copy(ofd7Zip.FileName, _CopyPath + @"\\" + ofd7Zip.SafeFileName,true);
                SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
                //decoder.
               
            }
        }

        private void Unzip(string zippedFilePath, string outputFolderPath, string password = null)
        {
            SevenZipExtractor.SetLibraryPath(@"C:\Program Files\7-Zip\7z.dll");
            SevenZipExtractor zip = new SevenZipExtractor(zippedFilePath);
            zip.ExtractArchive(_CopyPath);

            SevenZipExtractor tmp = null;
            if (!string.IsNullOrEmpty(password))
                tmp = new SevenZipExtractor(zippedFilePath, password);
            else
                tmp = new SevenZipExtractor(zippedFilePath);

            tmp.ExtractArchive(outputFolderPath);
        }

        private void btnDecompress_Click(object sender, EventArgs e)
        {
            //Unzip(_CopyPath + @"\\" + ofd7Zip.SafeFileName, _CopyPath);
            My7Zip.DecompressFileLZMA(_CopyPath + @"\\" + ofd7Zip.SafeFileName, _CopyPath + @"\\" + "outfolder");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //ofd7Zip.Filter = "7Z files (*.7Z)|*.7z|All files (*.*)|*.*";

            this.ofd7Zip.Multiselect = false;
            if (!Directory.Exists(_CopyPathAES))
            {
                Directory.CreateDirectory(_CopyPathAES);
            }
            if (ofd7Zip.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd7Zip.FileName;
                File.Copy(ofd7Zip.FileName, _CopyPathAES + @"\\" + ofd7Zip.SafeFileName, true);
                //SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
                //decoder.

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var random = new RNGCryptoServiceProvider())
            {
                MyAES myAES = new MyAES();
                encrypted = myAES.FileToByteArray(_CopyPathAES + @"\\" + ofd7Zip.SafeFileName);
                key = new byte[16];
                random.GetBytes(key);
                txtDecrypt.Text = myAES.DecryptStringFromBytes_Aes(encrypted, key);
            //myAES.DecryptFile(_CopyPathAES + @"\\" + ofd7Zip.SafeFileName, _CopyPathAES + @"\\ABCD");
            }
        }

        byte[] key = new byte[16];
        byte[] encrypted = null;
        private void button4_Click(object sender, EventArgs e)
        {
            using (var random = new RNGCryptoServiceProvider())
            {
                key = new byte[16];
                random.GetBytes(key);
                MyAES myAES = new MyAES();
                encrypted = myAES.EncryptStringToBytes_Aes(txtinput.Text, key);
                txtEncrypt.Text = Convert.ToBase64String(encrypted);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MyAES myAES = new MyAES();
            string roundtrip = myAES.DecryptStringFromBytes_Aes(encrypted, key);
            txtDecrypt.Text = roundtrip;

        }
    }
}
