using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGen_Demo
{
    public class UI
    {

        private string _Body;
        private string _BodyDesigner;
        private bool _CreateResources;
        private string _Domain;
        private FieldsList _Fields;
        private string _FileNameCode;
        private string _FileNameDesigner;
        private string _FileNameProgram;
        private string _FileNameResourceDesigner;
        private string _FileNameResourceForm;
        private FormInfo _Form;
        private LanguageType _Language;
        private string _Name;
        private string _NameSpace;
        private string _Prefix;
        private string _Program;
        private string _TypePrefix;
        private string _ResourceForm;


        public UI(string Name)
        {
            _Name = Name;
        }

        public string Body
        {
            get { return _Body; }
        }

        public string BodyDesigner
        {
            get { return _BodyDesigner; }
        }

        public bool CreateResources
        {
            get { return _CreateResources; }
            set { _CreateResources = value; }
        }

        public string Domain
        {
            get { return _Domain; }
            set { _Domain = value; }
        }

        public FieldsList Fields
        {
            get { return _Fields; }
            set { _Fields = value; }
        }

        public string FileNameCode
        {
            get { return _FileNameCode; }
        }

        public string FileNameDesigner
        {
            get { return _FileNameDesigner; }
        }

        public string FileNameProgram
        {
            get { return _FileNameProgram; }
        }

        public string FileNameResourceDesigner
        {
            get { return _FileNameResourceDesigner; }
        }

        public string FileNameResourceForm
        {
            get { return _FileNameResourceForm; }
        }

        public FormInfo FormFields
        {
            get { return _Form; }
            set { _Form = value; }
        }

        public LanguageType Language
        {
            get { return _Language; }
            set { _Language = value; }
        }

        public string Prefix
        {
            get { return _Prefix; }
            set { _Prefix = value; }
        }

        public string Program 
        {
            get { return _Program; }
            set { _Program = value; }
        }                    

        public string TypePrefix
        {
            get { return _TypePrefix; }
            set { _TypePrefix = value; }
        }

        public string ResourceForm
        {
            get { return _ResourceForm; }
        }


        private RC CreateBody(string Namespace)
        {
            int BaseIndent;
            string Body = string.Empty;
            string CodeCatch;
            string CodeComments;
            string CodeInternal;
            string CodeInternal2;
            string CodeTry;
            string FormName;
            BuildCode bc;
            BuildCode.Parameter Param;
            List<BuildCode.Parameter> lisParam;
            List<BuildCode.Parameter> lisParamCommon;
            RC rc = new RC();
            //BuildCode.SwitchCase SW;
            //List<BuildCode.SwitchCase> lisSW;
            BuildCode.IfArgument IfArg;
            List<BuildCode.IfArgument> lisIfArg;


            try
            {

                FormName = "frm" + _Name;
                bc = new BuildCode(_Language);
                bc.Indent = 4;

                if (_Language == LanguageType.CSharp)
                    BaseIndent = 4;
                else
                    BaseIndent = 0;



                //Form level variables
                Body += new string(' ', 4 + BaseIndent) + bc.DeclareVariable("_dirty", BuildCode.Scope.Private, BuildCode.VariableTypes.Boolean) + Environment.NewLine;
                Body += new string(' ', 4 + BaseIndent) + bc.DeclareVariable("_filename", BuildCode.Scope.Private, BuildCode.VariableTypes.String) + Environment.NewLine;
                Body += new string(' ', 4 + BaseIndent) + bc.DeclareVariable("_filepath", BuildCode.Scope.Private, BuildCode.VariableTypes.String) + Environment.NewLine;
                Body += Environment.NewLine;
                Body += Environment.NewLine;
                

                
                //Constructor
                CodeInternal = new string(' ', 8 + BaseIndent) + "InitializeComponent()" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.AddEvent("tsiNew.Click", "tsiNew_Click") + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.AddEvent("tsiOpen.Click", "tsiOpen_Click") + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.AddEvent("tsiSave.Click", "tsiSave_Click") + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.AddEvent("tsiSaveAs.Click", "tsiSaveAs_Click") + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.AddEvent("tsiExit.Click", "tsiExit_Click") + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.AddEvent("tsiLink.Click", "tsiLink_Click") + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.AddEvent("txtData.TextChanged", "txtData_TextChanged") + bc.EOL();
                Body += bc.Constructor(4 + BaseIndent, FormName, BuildCode.Scope.Public, CodeInternal);
                Body += Environment.NewLine;
                Body += Environment.NewLine;


                //Form_Closing
                CodeComments = new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "Checks for changes." + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "</summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<remarks>Code generated by Code Gen</remarks>" + Environment.NewLine;

                CodeInternal = new string(' ', 8 + BaseIndent) + "e.Cancel = true" + bc.EOL();
                lisIfArg = new List<BuildCode.IfArgument>();
                IfArg = new BuildCode.IfArgument(true, "ChangesHandled()", BuildCode.Comparison.None, string.Empty, BuildCode.Conjunction.None);
                lisIfArg.Add(IfArg);
                CodeInternal = bc.If(8 + BaseIndent, lisIfArg, new string(' ', 4) + CodeInternal);

                lisParamCommon = new List<BuildCode.Parameter>();
                Param = new BuildCode.Parameter("sender", "System.Object", BuildCode.PassingType.In);
                lisParamCommon.Add(Param);
                Param = new BuildCode.Parameter("e", "FormClosingEventArgs", BuildCode.PassingType.In);
                lisParamCommon.Add(Param);

                Body += CodeComments + bc.Function(4 + BaseIndent, "Form_Closing", BuildCode.Scope.Private, lisParamCommon, string.Empty, CodeInternal);
                Body += Environment.NewLine;
                Body += Environment.NewLine;


                //Form_Load
                CodeComments = new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "Sets the dirty flag to clear." + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "</summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<remarks>Code generated by Code Gen</remarks>" + Environment.NewLine;

                CodeInternal = new string(' ', 8 + BaseIndent) + "_dirty = false" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + "_filepath = System.IO.Directory.GetCurrentDirectory()" + bc.EOL();
                Body += Environment.NewLine;
                CodeInternal += new string(' ', 8 + BaseIndent) + "SetTitle()" + bc.EOL();

                lisParamCommon = new List<BuildCode.Parameter>();
                Param = new BuildCode.Parameter("sender", "System.Object", BuildCode.PassingType.In);
                lisParamCommon.Add(Param);
                Param = new BuildCode.Parameter("e", "System.EventArgs", BuildCode.PassingType.In);
                lisParamCommon.Add(Param);

                Body += CodeComments + bc.Function(4 + BaseIndent, "Form_Load", BuildCode.Scope.Private, lisParamCommon, string.Empty, CodeInternal);
                Body += Environment.NewLine;
                Body += Environment.NewLine;


                //tsiNew_Click
                CodeComments = new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "Clears the data." + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "</summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<remarks>Code generated by Code Gen</remarks>" + Environment.NewLine;

                CodeInternal = new string(' ', 8 + BaseIndent) + "CreateNew()" + bc.EOL();
                lisIfArg = new List<BuildCode.IfArgument>();
                IfArg = new BuildCode.IfArgument(false, "ChangesHandled()", BuildCode.Comparison.None, string.Empty, BuildCode.Conjunction.None);
                lisIfArg.Add(IfArg);
                CodeInternal = bc.If(8 + BaseIndent, lisIfArg, new string(' ', 4) + CodeInternal);

                lisParamCommon = new List<BuildCode.Parameter>();
                Param = new BuildCode.Parameter("sender", "System.Object", BuildCode.PassingType.In);
                lisParamCommon.Add(Param);
                Param = new BuildCode.Parameter("e", "EventArgs", BuildCode.PassingType.In);
                lisParamCommon.Add(Param);

                Body += CodeComments + bc.Function(4 + BaseIndent, "tsiNew_Click", BuildCode.Scope.Protected, BuildCode.Modifyer.Virtual, lisParamCommon, string.Empty, CodeInternal);
                Body += Environment.NewLine;
                Body += Environment.NewLine;


                //tsiOpen_Click
                CodeComments = new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "Loads data." + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "</summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<remarks>Code generated by Code Gen</remarks>" + Environment.NewLine;

                CodeInternal = new string(' ', 8 + BaseIndent) + "LoadData()" + bc.EOL();
                lisIfArg = new List<BuildCode.IfArgument>();
                IfArg = new BuildCode.IfArgument(false, "ChangesHandled()", BuildCode.Comparison.None, string.Empty, BuildCode.Conjunction.None);
                lisIfArg.Add(IfArg);
                CodeInternal = bc.If(8 + BaseIndent, lisIfArg, new string(' ', 4) + CodeInternal);

                lisParamCommon = new List<BuildCode.Parameter>();
                Param = new BuildCode.Parameter("sender", "System.Object", BuildCode.PassingType.In);
                lisParamCommon.Add(Param);
                Param = new BuildCode.Parameter("e", "EventArgs", BuildCode.PassingType.In);
                lisParamCommon.Add(Param);

                Body += CodeComments + bc.Function(4 + BaseIndent, "tsiOpen_Click", BuildCode.Scope.Protected, BuildCode.Modifyer.Virtual, lisParamCommon, string.Empty, CodeInternal);
                Body += Environment.NewLine;
                Body += Environment.NewLine;


                //tsiSave_Click
                CodeComments = new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "Saves the data." + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "</summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<remarks>Code generated by Code Gen</remarks>" + Environment.NewLine;

                CodeInternal = new string(' ', 8 + BaseIndent) + "Save()" + bc.EOL();

                lisParamCommon = new List<BuildCode.Parameter>();
                Param = new BuildCode.Parameter("sender", "System.Object", BuildCode.PassingType.In);
                lisParamCommon.Add(Param);
                Param = new BuildCode.Parameter("e", "EventArgs", BuildCode.PassingType.In);
                lisParamCommon.Add(Param);

                Body += CodeComments + bc.Function(4 + BaseIndent, "tsiSave_Click", BuildCode.Scope.Protected, BuildCode.Modifyer.Virtual, lisParamCommon, string.Empty, CodeInternal);
                Body += Environment.NewLine;
                Body += Environment.NewLine;


                //tsiSaveAs_Click
                CodeComments = new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "Saves the data optionally changing the path and name." + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "</summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<remarks>Code generated by Code Gen</remarks>" + Environment.NewLine;

                CodeInternal = new string(' ', 8 + BaseIndent) + "SaveAs()" + bc.EOL();

                lisParamCommon = new List<BuildCode.Parameter>();
                Param = new BuildCode.Parameter("sender", "System.Object", BuildCode.PassingType.In);
                lisParamCommon.Add(Param);
                Param = new BuildCode.Parameter("e", "EventArgs", BuildCode.PassingType.In);
                lisParamCommon.Add(Param);

                Body += CodeComments + bc.Function(4 + BaseIndent, "tsiSaveAs_Click", BuildCode.Scope.Protected, BuildCode.Modifyer.Virtual, lisParamCommon, string.Empty, CodeInternal);
                Body += Environment.NewLine;
                Body += Environment.NewLine;


                //tsiExit_Click
                CodeComments = new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "Disposes the form." + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "</summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<remarks>Code generated by Code Gen</remarks>" + Environment.NewLine;

                CodeInternal = new string(' ', 8 + BaseIndent) + bc.Self() + ".Dispose()" + bc.EOL();

                lisParamCommon = new List<BuildCode.Parameter>();
                Param = new BuildCode.Parameter("sender", "System.Object", BuildCode.PassingType.In);
                lisParamCommon.Add(Param);
                Param = new BuildCode.Parameter("e", "EventArgs", BuildCode.PassingType.In);
                lisParamCommon.Add(Param);

                Body += CodeComments + bc.Function(4 + BaseIndent, "tsiExit_Click", BuildCode.Scope.Protected, BuildCode.Modifyer.Virtual, lisParamCommon, string.Empty, CodeInternal);
                Body += Environment.NewLine;
                Body += Environment.NewLine;


                //tsiLink_Click
                CodeComments = new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "Disposes the form." + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "</summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<remarks>Code generated by Code Gen</remarks>" + Environment.NewLine;

                CodeInternal = new string(' ', 8 + BaseIndent) + "Process.Start(" + bc.EmbeddedQuote() + "https://marvelous-software.github.io/" + bc.EmbeddedQuote() + ")" + bc.EOL();

                lisParamCommon = new List<BuildCode.Parameter>();
                Param = new BuildCode.Parameter("sender", "System.Object", BuildCode.PassingType.In);
                lisParamCommon.Add(Param);
                Param = new BuildCode.Parameter("e", "EventArgs", BuildCode.PassingType.In);
                lisParamCommon.Add(Param);

                Body += CodeComments + bc.Function(4 + BaseIndent, "tsiLink_Click", BuildCode.Scope.Protected, BuildCode.Modifyer.Virtual, lisParamCommon, string.Empty, CodeInternal);
                Body += Environment.NewLine;
                Body += Environment.NewLine;


                //txtData_TextChanged
                CodeComments = new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "Sets. the dirty flag." + _Name + " or creates a new one if it is not found." + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "</summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<remarks>Code generated by Code Gen</remarks>" + Environment.NewLine;

                //Uses the same params as Prior event
                Body += CodeComments + bc.Function(4 + BaseIndent, "txtData_TextChanged", BuildCode.Scope.Protected, BuildCode.Modifyer.Virtual, lisParamCommon, string.Empty, new string(' ', 8 + BaseIndent) + "_dirty = true" + bc.EOL());
                Body += Environment.NewLine;
                Body += Environment.NewLine;


                //Changes Handled
                CodeComments = new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "Checks to discard changes, if any." + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "</summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<remarks>Code generated by Code Gen</remarks>" + Environment.NewLine;

                //CodeInternal = new string(' ', 8 + BaseIndent) + bc.DeclareVariable("OKtoProcede", BuildCode.VariableTypes.Boolean, "false");
                CodeInternal = new string(' ', 8 + BaseIndent) + bc.DeclareVariable("DR", "DialogResult") + Environment.NewLine;
                CodeInternal += Environment.NewLine;
                CodeInternal += Environment.NewLine;
                //System.Windows.Forms.DialogResult dr = System.Windows.Forms.MessageBox.Show("!!!", "TTT", System.Windows.Forms.MessageBoxButtons.YesNoCancel);

                CodeInternal2 = new string(' ', BaseIndent) + "DR = MessageBox.Show(" + bc.EmbeddedQuote() + "Do you wish to discard any changes?" + bc.EmbeddedQuote() + ", " + bc.EmbeddedQuote() + "Pending changes" + bc.EmbeddedQuote() + ", System.Windows.Forms.MessageBoxButtons.YesNoCancel)" + bc.EOL();
                lisIfArg = new List<BuildCode.IfArgument>();
                IfArg = new BuildCode.IfArgument(false, "DR", BuildCode.Comparison.Equal, "DialogResult.Yes", BuildCode.Conjunction.None);
                lisIfArg.Add(IfArg);
                CodeInternal2 += bc.If(12 + BaseIndent, lisIfArg, new string(' ', 16 + BaseIndent) + "_dirty = false" + bc.EOL());
                lisIfArg = new List<BuildCode.IfArgument>();
                IfArg = new BuildCode.IfArgument(false, "_dirty", BuildCode.Comparison.None, string.Empty, BuildCode.Conjunction.None);
                lisIfArg.Add(IfArg);
                CodeInternal += bc.If(8 + BaseIndent, lisIfArg, new string(' ', 8 + BaseIndent) + CodeInternal2);
                CodeInternal += Environment.NewLine;
                CodeInternal += new string(' ', 8 + BaseIndent) + "return " + bc.Not() + "_dirty" + bc.EOL();
                CodeInternal += Environment.NewLine;

                Body += CodeComments + bc.Function(4 + BaseIndent, "ChangesHandled", BuildCode.Scope.Protected, BuildCode.Modifyer.Virtual, null, BuildCode.VariableTypes.Boolean, CodeInternal);
                Body += Environment.NewLine;
                Body += Environment.NewLine;


                //Create New
                CodeComments = new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "Clears the form and amd flag." + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "</summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<remarks>Code generated by Code Gen</remarks>" + Environment.NewLine;

                CodeInternal = new string(' ', 8 + BaseIndent) + "txtData.Text = string.Empty" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + "_dirty = false" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + "_filename = string.Empty" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + "SetTitle()" + bc.EOL();

                Body += CodeComments + bc.Function(4 + BaseIndent, "CreateNew", BuildCode.Scope.Protected, BuildCode.Modifyer.Virtual, null, string.Empty, CodeInternal);
                Body += Environment.NewLine;
                Body += Environment.NewLine;


                //Load Data
                CodeComments = new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "Reads the data from a file." + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "</summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<remarks>Code generated by Code Gen</remarks>" + Environment.NewLine;

                CodeInternal = new string(' ', 8 + BaseIndent) + bc.DeclareVariable("OFD", "OpenFileDialog") + Environment.NewLine;
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.DeclareVariable("DR", "DialogResult") + Environment.NewLine;
                CodeInternal += Environment.NewLine;
                CodeInternal += Environment.NewLine;
                CodeTry = "OFD = new OpenFileDialog()" + bc.EOL();
                CodeTry += new string(' ', 12 + BaseIndent) + "OFD.Filter = " + bc.EmbeddedQuote() + "Text Files (*.txt)|*.txt|All Files|*.*" + bc.EmbeddedQuote() + bc.EOL();
                CodeTry += new string(' ', 12 + BaseIndent) + "OFD.FilterIndex = 1" + bc.EOL();
                CodeTry += new string(' ', 12 + BaseIndent) + "OFD.InitialDirectory = " + bc.EmbeddedQuote() + "_filepath" + bc.EmbeddedQuote() + bc.EOL();
                CodeTry += new string(' ', 12 + BaseIndent) + "DR = OFD.ShowDialog()" + bc.EOL();
                CodeTry += Environment.NewLine;
                CodeInternal2 = "txtData.Text = File.ReadAllText(OFD.FileName)" + bc.EOL();
                CodeInternal2 += new string(' ', 16 + BaseIndent) + "_dirty = false" + bc.EOL();
                CodeInternal2 += new string(' ', 16 + BaseIndent) + "SetTitle(System.IO.Path.GetDirectoryName(OFD.FileName), Path.GetFileName(OFD.FileName))" + bc.EOL();
                lisIfArg = new List<BuildCode.IfArgument>();
                IfArg = new BuildCode.IfArgument(false, "DR", BuildCode.Comparison.Equal, "DialogResult.OK", BuildCode.Conjunction.None);
                lisIfArg.Add(IfArg);
                CodeTry += bc.If(12 + BaseIndent, lisIfArg, new string(' ', 16 + BaseIndent) + CodeInternal2);
                CodeCatch = "MessageBox.Show(ex.ToString())" + bc.EOL();
                CodeInternal += bc.TryCatch(8 + BaseIndent, CodeTry, CodeCatch);

                Body += CodeComments + bc.Function(4 + BaseIndent, "LoadData", BuildCode.Scope.Protected, BuildCode.Modifyer.Virtual, null, string.Empty, CodeInternal);
                Body += Environment.NewLine;
                Body += Environment.NewLine;


                //Save
                CodeComments = new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "Saves the data if associated with a file name.." + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "</summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<remarks>Code generated by Code Gen</remarks>" + Environment.NewLine;

                CodeInternal = new string(' ', 12 + BaseIndent) + "File.WriteAllText(_filepath + " + bc.EmbeddedQuote() + bc.PathBackslash() + bc.EmbeddedQuote() + " + _filename, txtData.Text)" + bc.EOL();
                CodeInternal += new string(' ', 12 + BaseIndent) + "_dirty = false" + bc.EOL();

                lisIfArg = new List<BuildCode.IfArgument>();
                IfArg = new BuildCode.IfArgument(false, "_filename", BuildCode.Comparison.NotEqual, "string.Empty", BuildCode.Conjunction.None);
                lisIfArg.Add(IfArg);
                CodeInternal = bc.If(8 + BaseIndent, lisIfArg, CodeInternal);

                Body += CodeComments + bc.Function(4 + BaseIndent, "Save", BuildCode.Scope.Protected, BuildCode.Modifyer.Virtual, null, string.Empty, CodeInternal);
                Body += Environment.NewLine;
                Body += Environment.NewLine;


                //SaveAs
                CodeComments = new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "Saves the data allowing choosing or changing the file name and path." + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "</summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<remarks>Code generated by Code Gen</remarks>" + Environment.NewLine;


                CodeInternal = new string(' ', 8 + BaseIndent) + bc.DeclareVariable("SFD", "SaveFileDialog") + Environment.NewLine;
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.DeclareVariable("DR", "DialogResult") + Environment.NewLine;
                CodeInternal += Environment.NewLine;
                CodeInternal += Environment.NewLine;
                CodeTry = "SFD = new SaveFileDialog()" + bc.EOL();
                CodeTry += new string(' ', 12 + BaseIndent) + "SFD.FileName = _filepath + " + bc.EmbeddedQuote() + bc.PathBackslash() + bc.EmbeddedQuote() + " + _filename" + bc.EOL();
                CodeTry += new string(' ', 12 + BaseIndent) + "DR = SFD.ShowDialog()" + bc.EOL();
                CodeTry += Environment.NewLine;
                lisIfArg = new List<BuildCode.IfArgument>();
                IfArg = new BuildCode.IfArgument(false, "DR", BuildCode.Comparison.Equal, "DialogResult.OK", BuildCode.Conjunction.None);
                lisIfArg.Add(IfArg);
                //CodeTry += bc.If(12 + BaseIndent, lisIfArg, new string(' ', 12 + BaseIndent) + "File.WriteAllText(SFD.FileName, txtData.Text)");
                CodeInternal2 = "SetTitle(System.IO.Path.GetDirectoryName(SFD.FileName), Path.GetFileName(SFD.FileName))" + bc.EOL();
                CodeInternal2 += new string(' ', 16 + BaseIndent) + "Save()" + bc.EOL();
                CodeTry += bc.If(12 + BaseIndent, lisIfArg, new string(' ', 16 + BaseIndent) + CodeInternal2 + bc.EOL());
                CodeCatch = "MessageBox.Show(ex.ToString())" + bc.EOL();
                CodeInternal += bc.TryCatch(8 + BaseIndent, CodeTry, CodeCatch);

                Body += CodeComments + bc.Function(4 + BaseIndent, "SaveAs", BuildCode.Scope.Protected, BuildCode.Modifyer.Virtual, null, string.Empty, CodeInternal);
                Body += Environment.NewLine;
                Body += Environment.NewLine;


                //SetTitle
                CodeComments = new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<summary>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "Displays the file path and name in the title bar if it has been set." + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<param name=\"filepath\">Path to the file, or the app path if not set.</param>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<param name=\"filename\">File name if the data has been saved or loaded.</param>" + Environment.NewLine;
                CodeComments += new string(' ', 4 + BaseIndent) + bc.XMLComments() + "<remarks>Code generated by Code Gen</remarks>" + Environment.NewLine;

                lisIfArg = new List<BuildCode.IfArgument>();
                IfArg = new BuildCode.IfArgument(false, "filename", BuildCode.Comparison.Equal, "string.Empty", BuildCode.Conjunction.None);
                lisIfArg.Add(IfArg);
                CodeInternal = bc.If(8 + BaseIndent, lisIfArg, new string(' ', 12 + BaseIndent) + "SetTitle()" + bc.EOL());
                lisIfArg = new List<BuildCode.IfArgument>();
                IfArg = new BuildCode.IfArgument(true, "filename", BuildCode.Comparison.Equal, "string.Empty", BuildCode.Conjunction.None);
                lisIfArg.Add(IfArg);
                CodeInternal = new string(' ', 12 + BaseIndent) + bc.Self() + ".Text = " + bc.EmbeddedQuote() + "NotepadDemo - " + bc.EmbeddedQuote() + " + filepath + " + bc.EmbeddedQuote() + bc.PathBackslash() + bc.EmbeddedQuote() + " + filename" + bc.EOL();
                CodeInternal += new string(' ', 12 + BaseIndent) + "_filepath = filepath" + bc.EOL();
                CodeInternal += new string(' ', 12 + BaseIndent) + "_filename = filename" + bc.EOL();
                CodeInternal = bc.If(8 + BaseIndent, lisIfArg, CodeInternal + bc.EOL());

                lisParam = new List<BuildCode.Parameter>();
                Param = new BuildCode.Parameter("filepath", "string", BuildCode.PassingType.In);
                lisParam.Add(Param);
                Param = new BuildCode.Parameter("filename", "string", BuildCode.PassingType.In);
                lisParam.Add(Param);
                
                Body += CodeComments + bc.Function(4 + BaseIndent, "SetTitle", BuildCode.Scope.Public, BuildCode.Modifyer.Overloads, lisParam, string.Empty, CodeInternal);
                Body += Environment.NewLine;
                Body += bc.Function(4 + BaseIndent, "SetTitle", BuildCode.Scope.Private, BuildCode.Modifyer.Overloads, null, string.Empty, new string(' ', 8 + BaseIndent) + bc.Self() + ".Text = " + bc.EmbeddedQuote() + "NotepadDemo - UNSAVED" + bc.EmbeddedQuote() + bc.EOL());
                Body += Environment.NewLine;


                Body = bc.ClassDefinitionDesigner(BaseIndent, FormName, BuildCode.Scope.Public, false, Body, "Form", true);
                if (_Language != LanguageType.VB)
                    Body = bc.Namespace(Namespace, Body);
                Body = bc.AddLibrary("System.Windows.Forms") + bc.EOL() + Environment.NewLine + Environment.NewLine + Body;
                Body = bc.AddLibrary("System.IO") + bc.EOL() + Body;
                Body = bc.AddLibrary("System.Diagnostics") + bc.EOL() + Body;
                Body = bc.AddLibrary("System") + bc.EOL() + Body;

                _FileNameCode = _Name + Constant.ExtensionFile(_Language);


            }

            catch (System.Exception ex)
            {
                rc.AddError(ex.TargetSite.ReflectedType.Name + " - " + ex.TargetSite.Name + " - " + ex.Message);
#if DEBUG
                System.Windows.Forms.MessageBox.Show(ex.Message);
#endif
            }

            _Body = Body;
            return rc;

        }

        private RC CreateDesigner(string Namespace)
        {
            int BaseIndent;
            string BodyDesigner = string.Empty;
            string CodeInternal;
            string FormName;
            BuildCode bc;
            BuildCode.Parameter Param;
            List<BuildCode.Parameter> lisParam;
            BuildCode.IfArgument IfArg;
            List<BuildCode.IfArgument> lisIfArg;
            RC rc = new RC();
            List<string> lisMenuItems;


            try
            {

                rc = new RC();
                _NameSpace = Namespace;
                FormName = "frm" + _Name;
                bc = new BuildCode(_Language);
                bc.Indent = 4;

                if (_Language == LanguageType.CSharp)
                    BaseIndent = 4;
                else
                    BaseIndent = 0;



                //strBodyDesigner = "<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _" & Chr(13)
                //strBodyDesigner &= "Partial Class " & Constant.Hr & Constant.Root2 & "_" & mstrName & Chr(13)
                //strBodyDesigner &= "    Inherits IntuitiveForms.imsForm" & Chr(13)
                //strBodyDesigner &= Chr(13)
                //strBodyDesigner &= "    'Form overrides dispose to clean up the component list." & Chr(13)
                //strBodyDesigner &= "    <System.Diagnostics.DebuggerNonUserCode()> _" & Chr(13)
                //strBodyDesigner &= "    Inherits Protected Overrides Sub Dispose(ByVal disposing As Boolean)" & Chr(13)

                //Required variable
                BodyDesigner = Environment.NewLine;
                BodyDesigner += new string(' ', 4 + BaseIndent) + bc.Comment("Required by the Windows Form Designer") + Environment.NewLine;
                BodyDesigner += new string(' ', 4 + BaseIndent) + bc.VariableDefinition(BuildCode.Scope.Private, "components", "System.ComponentModel.IContainer", bc.Null()) + bc.EOL() + Environment.NewLine;

                //Dispose
                lisIfArg = new List<BuildCode.IfArgument>();
                IfArg = new BuildCode.IfArgument(false, "disposing", BuildCode.Comparison.None, string.Empty, BuildCode.Conjunction.AndShortCircuit);
                lisIfArg.Add(IfArg);
                IfArg = new BuildCode.IfArgument(true, "components", BuildCode.Comparison.Equal, bc.Null(), BuildCode.Conjunction.None);
                lisIfArg.Add(IfArg);
                CodeInternal = bc.If(8 + BaseIndent, lisIfArg, new string(' ', 12 + BaseIndent) + "components.Dispose()" + bc.EOL());
                CodeInternal += Environment.NewLine;
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.GetBase() + ".Dispose(disposing)" + bc.EOL();
                lisParam = new List<BuildCode.Parameter>();
                Param = new BuildCode.Parameter("disposing", bc.DataType(BuildCode.VariableTypes.Boolean), BuildCode.PassingType.In);
                lisParam.Add(Param);
                BodyDesigner += new string(' ', 4 + BaseIndent) + bc.Comment("Form overrides dispose to clean up the component list.");
                BodyDesigner += new string(' ', 4 + BaseIndent) + bc.Attribute("System.Diagnostics.DebuggerNonUserCode()");
                BodyDesigner += bc.Function(4 + BaseIndent, "Dispose", BuildCode.Scope.Protected, BuildCode.Modifyer.Override, lisParam, string.Empty, CodeInternal);
                BodyDesigner += Environment.NewLine;
                BodyDesigner += Environment.NewLine;

                //InitializeComponent
                BodyDesigner += new string(' ', 4 + BaseIndent) + bc.Comment("NOTE: The following procedure is required by the Windows Form Designer");
                BodyDesigner += new string(' ', 4 + BaseIndent) + bc.Comment("It can be modified using the Windows Form Designer.");
                BodyDesigner += new string(' ', 4 + BaseIndent) + bc.Comment("Do not modify it using the code editor.");
                BodyDesigner += new string(' ', 4 + BaseIndent) + bc.Attribute("System.Diagnostics.DebuggerStepThrough()");
                CodeInternal = new string(' ', 8 + BaseIndent) + bc.Self() + ".components = new System.ComponentModel.Container()" + bc.EOL() + Environment.NewLine;
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.DeclareVariable("resources", "System.ComponentModel.ComponentResourceManager", "new System.ComponentModel.ComponentResourceManager(" + bc.Type(FormName) + ")") + Environment.NewLine;

                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".txtData = new System.Windows.Forms.TextBox()" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".mnuMenu = new System.Windows.Forms.MenuStrip()" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiFile = new System.Windows.Forms.ToolStripMenuItem()" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiAbout = new System.Windows.Forms.ToolStripMenuItem()" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiNew = new System.Windows.Forms.ToolStripMenuItem()" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiOpen = new System.Windows.Forms.ToolStripMenuItem()" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiSave = new System.Windows.Forms.ToolStripMenuItem()" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiSaveAs = new System.Windows.Forms.ToolStripMenuItem()" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiSeparator = new System.Windows.Forms.ToolStripMenuItem()" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiExit = new System.Windows.Forms.ToolStripMenuItem()" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiLink = new System.Windows.Forms.ToolStripMenuItem()" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".mnuMenu.SuspendLayout()" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".SuspendLayout()" + bc.EOL();
                CodeInternal += Environment.NewLine;
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment("txtData");
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".txtData.Dock = System.Windows.Forms.DockStyle.Fill" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".txtData.Location = new System.Drawing.Point(0, 0)" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".txtData.Multiline = true" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".txtData.Name = " + bc.EmbeddedQuote() + "txtData" + bc.EmbeddedQuote() + bc.EOL();
                //CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".txtData.Size = new System.Drawing.Size(500, 500)" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".txtData.TabIndex = 0" + bc.EOL();

                lisMenuItems = new List<string>();
                lisMenuItems.Add(bc.Self() + ".tsiFile");
                lisMenuItems.Add(bc.Self() + ".tsiAbout");
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment("mnuMenu");
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".mnuMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem" + bc.ArrayInitialize(lisMenuItems) + ")" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".mnuMenu.Location = new System.Drawing.Point(0, 0)" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".mnuMenu.Name = " + bc.EmbeddedQuote() + "mnuMenu" + bc.EmbeddedQuote() + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".mnuMenu.Size = new System.Drawing.Size(500, 24)" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".mnuMenu.TabIndex = 1" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".mnuMenu.Text = " + bc.EmbeddedQuote() + "Menu" + bc.EmbeddedQuote() + bc.EOL();

                lisMenuItems = new List<string>();
                lisMenuItems.Add(bc.Self() + ".tsiNew");
                lisMenuItems.Add(bc.Self() + ".tsiOpen");
                lisMenuItems.Add(bc.Self() + ".tsiSave");
                lisMenuItems.Add(bc.Self() + ".tsiSaveAs");
                lisMenuItems.Add(bc.Self() + ".tsiSeparator");
                lisMenuItems.Add(bc.Self() + ".tsiExit");
                lisMenuItems.Add(bc.Self() + ".tsiLink");
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment("tsiFile");
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem" + bc.ArrayInitialize(lisMenuItems) + ")" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiFile.Name = " + bc.EmbeddedQuote() + "tsiFile" + bc.EmbeddedQuote() + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiFile.Size = new System.Drawing.Size(37, 20)" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiFile.Text = " + bc.EmbeddedQuote() + "&File" + bc.EmbeddedQuote() + bc.EOL();

                lisMenuItems = new List<string>();
                lisMenuItems.Add(bc.Self() + ".tsiLink");
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment("tsiAbout");
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiAbout.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem" + bc.ArrayInitialize(lisMenuItems) + ")" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiAbout.Name = " + bc.EmbeddedQuote() + "tsiAbout" + bc.EmbeddedQuote() + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiAbout.Size = new System.Drawing.Size(52, 20)" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiAbout.Text = " + bc.EmbeddedQuote() + "&About" + bc.EmbeddedQuote() + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment("tsiNew");
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiNew.Name = " + bc.EmbeddedQuote() + "tsiNew" + bc.EmbeddedQuote() + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiNew.Size = new System.Drawing.Size(152, 22)" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiNew.Text = " + bc.EmbeddedQuote() + "&New" + bc.EmbeddedQuote() + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment("tsiOpen");
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiOpen.Name = " + bc.EmbeddedQuote() + "tsiOpen" + bc.EmbeddedQuote() + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiOpen.Size = new System.Drawing.Size(152, 22)" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiOpen.Text = " + bc.EmbeddedQuote() + "&Open..." + bc.EmbeddedQuote() + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment("tsiSave");
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiSave.Name = " + bc.EmbeddedQuote() + "tsiSave" + bc.EmbeddedQuote() + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiSave.Size = new System.Drawing.Size(152, 22)" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiSave.Text = " + bc.EmbeddedQuote() + "&Save" + bc.EmbeddedQuote() + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment("tsiSaveAs");
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiSaveAs.Name = " + bc.EmbeddedQuote() + "tsiSaveAs" + bc.EmbeddedQuote() + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiSaveAs.Size = new System.Drawing.Size(152, 22)" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiSaveAs.Text = " + bc.EmbeddedQuote() + "&Save As..." + bc.EmbeddedQuote() + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment("tsiSeparator");
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiSeparator.Name = " + bc.EmbeddedQuote() + "tsiSeparator" + bc.EmbeddedQuote() + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiSeparator.Size = new System.Drawing.Size(152, 22)" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment("tsiExit");
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiExit.Name = " + bc.EmbeddedQuote() + "tsiExit" + bc.EmbeddedQuote() + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiExit.Size = new System.Drawing.Size(152, 22)" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiExit.Text = " + bc.EmbeddedQuote() + "&Exit" + bc.EmbeddedQuote() + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment("tsiLink");
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiLink.Name = " + bc.EmbeddedQuote() + "tsiExit" + bc.EmbeddedQuote() + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiLink.Size = new System.Drawing.Size(152, 22)" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".tsiLink.Text = " + bc.EmbeddedQuote() + "https://marvelous-software.github.io/" + bc.EmbeddedQuote() + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(FormName);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Comment(string.Empty);
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F)" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".BackColor = System.Drawing.Color.FromArgb(" + bc.DesignerCast(bc.DesignerCast("247", bc.DataType(BuildCode.VariableTypes.Byte)), bc.DataType(BuildCode.VariableTypes.Integer)) + ", " + bc.DesignerCast(bc.DesignerCast("243", bc.DataType(BuildCode.VariableTypes.Byte)), bc.DataType(BuildCode.VariableTypes.Integer)) + ", " + bc.DesignerCast(bc.DesignerCast("247", bc.DataType(BuildCode.VariableTypes.Byte)), bc.DataType(BuildCode.VariableTypes.Integer)) + ")" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".ClientSize = new System.Drawing.Size(500, 500)" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".Controls.Add(" + bc.Self() + ".txtData)" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".Controls.Add(" + bc.Self() + ".mnuMenu)" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".Name = " + bc.EmbeddedQuote() + FormName + bc.EmbeddedQuote() + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".Text = " + bc.EmbeddedQuote() + "Notepad Demo" + bc.EmbeddedQuote() + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".WindowState = System.Windows.Forms.FormWindowState.Normal" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".mnuMenu.ResumeLayout(false)" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".mnuMenu.PerformLayout()" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".ResumeLayout(false)" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + bc.Self() + ".PerformLayout()" + bc.EOL();
                CodeInternal += Environment.NewLine;
                BodyDesigner += bc.Function(4 + BaseIndent, "InitializeComponent", BuildCode.Scope.Private, BuildCode.VariableTypes.Null, CodeInternal);
                BodyDesigner += Environment.NewLine;
                BodyDesigner += Environment.NewLine;

                BodyDesigner += new string(' ', 4 + BaseIndent) + bc.DesignerDeclareControl("txtData", "System.Windows.Forms.TextBox") + bc.EOL();
                BodyDesigner += new string(' ', 4 + BaseIndent) + bc.DesignerDeclareControl("mnuMenu", "System.Windows.Forms.MenuStrip") + bc.EOL();
                BodyDesigner += new string(' ', 4 + BaseIndent) + bc.DesignerDeclareControl("tsiFile", "System.Windows.Forms.ToolStripMenuItem") + bc.EOL();
                BodyDesigner += new string(' ', 4 + BaseIndent) + bc.DesignerDeclareControl("tsiAbout", "System.Windows.Forms.ToolStripMenuItem") + bc.EOL();
                BodyDesigner += new string(' ', 4 + BaseIndent) + bc.DesignerDeclareControl("tsiNew", "System.Windows.Forms.ToolStripMenuItem") + bc.EOL();
                BodyDesigner += new string(' ', 4 + BaseIndent) + bc.DesignerDeclareControl("tsiOpen", "System.Windows.Forms.ToolStripMenuItem") + bc.EOL();
                BodyDesigner += new string(' ', 4 + BaseIndent) + bc.DesignerDeclareControl("tsiSave", "System.Windows.Forms.ToolStripMenuItem") + bc.EOL();
                BodyDesigner += new string(' ', 4 + BaseIndent) + bc.DesignerDeclareControl("tsiSaveAs", "System.Windows.Forms.ToolStripMenuItem") + bc.EOL();
                BodyDesigner += new string(' ', 4 + BaseIndent) + bc.DesignerDeclareControl("tsiSeparator", "System.Windows.Forms.ToolStripMenuItem") + bc.EOL();
                BodyDesigner += new string(' ', 4 + BaseIndent) + bc.DesignerDeclareControl("tsiExit", "System.Windows.Forms.ToolStripMenuItem") + bc.EOL();
                BodyDesigner += new string(' ', 4 + BaseIndent) + bc.DesignerDeclareControl("tsiLink", "System.Windows.Forms.ToolStripMenuItem") + bc.EOL();
                BodyDesigner = bc.ClassDefinitionDesigner(BaseIndent, FormName, BuildCode.Scope.Public, false, BodyDesigner, string.Empty, true);
                if (_Language != LanguageType.VB)
                    BodyDesigner = bc.Namespace(Namespace, BodyDesigner);

                _FileNameDesigner = _Name + ".Desinger" + Constant.ExtensionFile(_Language);

            }

            catch (System.Exception ex)
            {
                rc.AddError(ex.TargetSite.ReflectedType.Name + " - " + ex.TargetSite.Name + " - " + ex.Message);
            }
            //#If DEBUG Then
            //            MsgBox(ex.Message)
            //#End If

            _BodyDesigner = BodyDesigner;
            return rc;

        }

        private RC CreateProgram(string Namespace)
        {
            int BaseIndent;
            string Program = string.Empty;
            string CodeInternal = string.Empty;
            string FormName;
            BuildCode bc;
            RC rc = new RC();


            try
            {

                FormName = "frm" + _Name;
                bc = new BuildCode(_Language);
                bc.Indent = 4;

                if (_Language == LanguageType.CSharp)
                    BaseIndent = 4;
                else
                    BaseIndent = 0;


                //Main
                CodeInternal = new string(' ', 8 + BaseIndent) + "Application.EnableVisualStyles()" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + "Application.SetCompatibleTextRenderingDefault(false)" + bc.EOL();
                CodeInternal += new string(' ', 8 + BaseIndent) + "Application.Run(new " + FormName + "())" + bc.EOL();
                Program = new string(' ', 4 + BaseIndent) + bc.Attribute("STAThread");
                if (_Language == LanguageType.CSharp)
                    Program += bc.Function(4 + BaseIndent, "Main", BuildCode.Scope.None, BuildCode.Modifyer.Static, null, string.Empty, CodeInternal);
                else
                Program += bc.Function(4 + BaseIndent, "Main", BuildCode.Scope.None, BuildCode.Modifyer.None, null, string.Empty, CodeInternal);
                Program += Environment.NewLine;
                Program += Environment.NewLine;


                Program = bc.ClassDefinitionDesigner(BaseIndent, "Program", BuildCode.Scope.None, true, Program, string.Empty, false);
                if (_Language != LanguageType.VB)
                    Program = bc.Namespace(Namespace, Program);
                Program = bc.AddLibrary("System.Windows.Forms") + bc.EOL() + Environment.NewLine + Environment.NewLine + Program;
                Program = bc.AddLibrary("System.Threading") + bc.EOL() + Program;
                Program = bc.AddLibrary("System") + bc.EOL() + Program;

                _FileNameProgram = "Program" + Constant.ExtensionFile(_Language);

            }

            catch (System.Exception ex)
            {
                rc.AddError(ex.TargetSite.ReflectedType.Name + " - " + ex.TargetSite.Name + " - " + ex.Message);
#if DEBUG
                System.Windows.Forms.MessageBox.Show(ex.Message);
#endif
            }

            _Program = Program;
            return rc;

        }

        private RC CreateRes()//string Path, string Domain
        {
            string Data = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine +
"<root>" + Environment.NewLine +
"  <xsd:schema id=\"root\" xmlns=\"\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">" + Environment.NewLine +
"    <xsd:import namespace=\"http://www.w3.org/XML/1998/namespace\" />" + Environment.NewLine +
"    <xsd:element name=\"root\" msdata:IsDataSet=\"true\">" + Environment.NewLine +
"      <xsd:complexType>" + Environment.NewLine +
"        <xsd:choice maxOccurs=\"unbounded\">" + Environment.NewLine +
"          <xsd:element name=\"metadata\">" + Environment.NewLine +
"            <xsd:complexType>" + Environment.NewLine +
"              <xsd:sequence>" + Environment.NewLine +
"                <xsd:element name=\"value\" type=\"xsd:string\" minOccurs=\"0\" />" + Environment.NewLine +
"              </xsd:sequence>" + Environment.NewLine +
"              <xsd:attribute name=\"name\" use=\"required\" type=\"xsd:string\" />" + Environment.NewLine +
"              <xsd:attribute name=\"type\" type=\"xsd:string\" />" + Environment.NewLine +
"              <xsd:attribute name=\"mimetype\" type=\"xsd:string\" />" + Environment.NewLine +
"              <xsd:attribute ref=\"xml:space\" />" + Environment.NewLine +
"            </xsd:complexType>" + Environment.NewLine +
"          </xsd:element>" + Environment.NewLine +
"          <xsd:element name=\"assembly\">" + Environment.NewLine +
"            <xsd:complexType>" + Environment.NewLine +
"              <xsd:attribute name=\"alias\" type=\"xsd:string\" />" + Environment.NewLine +
"              <xsd:attribute name=\"name\" type=\"xsd:string\" />" + Environment.NewLine +
"            </xsd:complexType>" + Environment.NewLine +
"          </xsd:element>" + Environment.NewLine +
"          <xsd:element name=\"data\">" + Environment.NewLine +
"            <xsd:complexType>" + Environment.NewLine +
"              <xsd:sequence>" + Environment.NewLine +
"                <xsd:element name=\"value\" type=\"xsd:string\" minOccurs=\"0\" msdata:Ordinal=\"1\" />" + Environment.NewLine +
"                <xsd:element name=\"comment\" type=\"xsd:string\" minOccurs=\"0\" msdata:Ordinal=\"2\" />" + Environment.NewLine +
"              </xsd:sequence>" + Environment.NewLine +
"              <xsd:attribute name=\"name\" type=\"xsd:string\" use=\"required\" msdata:Ordinal=\"1\" />" + Environment.NewLine +
"              <xsd:attribute name=\"type\" type=\"xsd:string\" msdata:Ordinal=\"3\" />" + Environment.NewLine +
"              <xsd:attribute name=\"mimetype\" type=\"xsd:string\" msdata:Ordinal=\"4\" />" + Environment.NewLine +
"              <xsd:attribute ref=\"xml:space\" />" + Environment.NewLine +
"            </xsd:complexType>" + Environment.NewLine +
"          </xsd:element>" + Environment.NewLine +
"          <xsd:element name=\"resheader\">" + Environment.NewLine +
"            <xsd:complexType>" + Environment.NewLine +
"              <xsd:sequence>" + Environment.NewLine +
"                <xsd:element name=\"value\" type=\"xsd:string\" minOccurs=\"0\" msdata:Ordinal=\"1\" />" + Environment.NewLine +
"              </xsd:sequence>" + Environment.NewLine +
"              <xsd:attribute name=\"name\" type=\"xsd:string\" use=\"required\" />" + Environment.NewLine +
"            </xsd:complexType>" + Environment.NewLine +
"          </xsd:element>" + Environment.NewLine +
"        </xsd:choice>" + Environment.NewLine +
"      </xsd:complexType>" + Environment.NewLine +
"    </xsd:element>" + Environment.NewLine +
"  </xsd:schema>" + Environment.NewLine +
"  <resheader name=\"resmimetype\">" + Environment.NewLine +
"    <value>text/microsoft-resx</value>" + Environment.NewLine +
"  </resheader>" + Environment.NewLine +
"  <resheader name=\"version\">" + Environment.NewLine +
"    <value>2.0</value>" + Environment.NewLine +
"  </resheader>" + Environment.NewLine +
"  <resheader name=\"reader\">" + Environment.NewLine +
"    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>" + Environment.NewLine +
"  </resheader>" + Environment.NewLine +
"  <resheader name=\"writer\">" + Environment.NewLine +
"    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>" + Environment.NewLine +
"  </resheader>" + Environment.NewLine +
"</root>";


            RC rc = new RC();
            //using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(Path + "\\" + Domain + ".resx"))
            //    outputFile.Write(Data);
            _ResourceForm = Data;
            _FileNameResourceForm = _Name + Constant.ExtensionResource;
            return rc;

        }

        public RC Create(string Namespace)
        {
            RC rc;

            rc = CreateBody(Namespace);
            rc.Merge(CreateDesigner(Namespace));
            rc.Merge(CreateProgram(Namespace));
            rc.Merge(CreateRes());
            return rc;

        }
    }
}
