using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeGen_Demo
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FB = new FolderBrowserDialog();
            DialogResult DR;

            try
            {
                FB.ShowNewFolderButton = true;
                FB.SelectedPath = "C:\\";
                DR = FB.ShowDialog();
                if (DR == System.Windows.Forms.DialogResult.OK)                
                    txtOutputDir.Text = FB.SelectedPath;                
            }

            catch (Exception ex)
            {
                //Common.WriteToErrorFile(new StackFrame(true), ex.TargetSite.ReflectedType.Name, ex.TargetSite.Name, MethodBase.GetCurrentMethod().Name, ex.Message);
                MessageBox.Show(ex.TargetSite.ReflectedType.Name + " - " + ex.TargetSite.Name + " - " + ex.Message);
            }

        }


        private void btnGO_Click(object sender, EventArgs e)
        {
            BuildFiles bf;
            LanguageType CodeChoice;
            BuildFiles.VS_Version Version = BuildFiles.VS_Version.v2010;
            string FileName;
            string FilePath;


            Cursor = Cursors.WaitCursor;

            if (rdoCS.Checked)
                CodeChoice = LanguageType.CSharp;
            else
                CodeChoice = LanguageType.VB;

            if (rdo2008.Checked)
                Version = BuildFiles.VS_Version.v2008;
            else
                if (rdo2010.Checked)
                    Version = BuildFiles.VS_Version.v2010;

            FilePath = txtOutputDir.Text;
            if (!Directory.Exists(FilePath))
                Directory.CreateDirectory(FilePath);

            FilePath = txtOutputDir.Text + "\\" + Constant.NameRoot;
            if (!Directory.Exists(FilePath))
                Directory.CreateDirectory(FilePath);

            bf = new BuildFiles(Version);
            UI presentation = new UI(Constant.NameRoot);
            presentation.Language = CodeChoice;
            presentation.Create(Constant.NamePrefix + "_" + Constant.NameRoot);
            FileName = presentation.FileNameCode;
            using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(FilePath + "\\" + FileName))
                outputFile.Write(presentation.Body);
            bf.AddFile(FileName, BuildFiles.CodeFileType.Form);
            FileName = presentation.FileNameDesigner;
            using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(FilePath + "\\" + FileName))
                outputFile.Write(presentation.BodyDesigner);

            FileName = presentation.FileNameProgram;
            using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(FilePath + "\\" + FileName))
                outputFile.Write(presentation.Program);
            bf.AddFile(FileName, BuildFiles.CodeFileType.Program);
            FileName = presentation.FileNameResourceForm;
            using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(FilePath + "\\" + FileName))
                outputFile.Write(presentation.ResourceForm);
            bf.AddFile(FileName, BuildFiles.CodeFileType.Resource, presentation.FileNameCode);

            bf.Name = Constant.NameRoot;
            bf.Path = FilePath;
            bf.Language = CodeChoice;
            //bf.TypePrefix = "frm";
            bf.BuildSolutionFile();
            bf.BuildAssemblyInfo(Constant.NameRoot, Constant.NamePrefix);
            bf.BuildProjectFile(Constant.NamePrefix + "_" + Constant.NameRoot);

            Cursor = Cursors.Default;

            MessageBox.Show("Done!");

        }

    }
}
