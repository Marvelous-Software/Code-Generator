//+---------------------------+---------------+-----------+----------------+
//|       Product name        |   Codename    | Version # | .NET Framework | 
//+---------------------------+---------------+-----------+----------------+
//| Visual Studio 4.0         | N/A           | 4.0.*     | N/A            |
//| Visual Studio 97          | Boston        | 5.0.*     | N/A            |
//| Visual Studio 6.0         | Aspen         | 6.0.*     | N/A            |
//| Visual Studio .NET (2002) | Rainier       | 7.0.*     | 1              |
//| Visual Studio .NET 2003   | Everett       | 7.1.*     | 1.1            |
//| Visual Studio 2005        | Whidbey       | 8.0.*     | 2.0, 3.0       |
//| Visual Studio 2008        | Orcas         | 9.0.*     | 2.0, 3.0, 3.5  | 9.0.21022
//| Visual Studio 2010        | Dev10/Rosario | 10.0.*    | 2.0 – 4.0      |
//| Visual Studio 2012        | Dev11         | 11.0.*    | 2.0 – 4.5.2    |
//| Visual Studio 2013        | Dev12         | 12.0.*    | 2.0 – 4.5.2    |
//| Visual Studio 2015        | Dev14         | 14.0.*    | 2.0 – 4.6      |
//+---------------------------+---------------+-----------+----------------+

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace CodeGen_Demo
{
    public class BuildFiles
    {
        private const char _Tab = (char)9;


        public enum CodeFileType
        {
            Standard,
            Form,
            Program,
            Resource,
        }

        public enum VS_Version
        {
            Unknown,
            v2005,
            v2008,
            v2010,
            v2012,
            v2013,
            v2015,
            v2017,
        }


        public class CodeFile
        {
            public string Name;
            public CodeFileType Type;
            public string Dependant;


            public string Designer()
            {
                if (Type == CodeFileType.Form)
                    return Name.Substring(0, Name.LastIndexOf(".")) + ".Desinger" + Extension();
                else
                    return string.Empty;
            }

            public string Extension()
            {
                return Name.Substring(Name.LastIndexOf("."));
            }

        }

        public class Reference
        {
            public string Name;
            public bool CopyLocal;
            public bool SpecificVersion;
            public string Version;
            public string Culture;
            public string ProcessorArchitecture;

            public Reference()
            {
                Version = "0.0.0.0";
                Culture = "neutral";
                ProcessorArchitecture = "MSIL";
            }

            public string FullName
            {
                get { return Name + ", Version=" + Version + ", culture=" + Culture + ", processorArchitecture=" + ProcessorArchitecture; }
            }
        }

        private bool _CreateResources;
        private List<CodeFile> _Files;
        private List<Reference> _References;
        private Guid _ProjectID;
        private LanguageType _Language;
        private string _AssemblyInfoFile;
        private string _DevelopmentDirectory;
        private string _Name;
        private string _Path;
        private string _Prefix;
        private string _ProjectFile;
        private string _SolutionFile;
        private string _TypePrefix;
        private VS_Version _Version;


        public string AssemblyInfoFile
        {
            get { return _AssemblyInfoFile; }
            set { _AssemblyInfoFile = value; }
        }

        public bool CreateResources
        {
            get { return _CreateResources; }
            set { _CreateResources = value; }
        }

        public string DevelopmentDirectory
        {
            get { return _DevelopmentDirectory; }
            set { _DevelopmentDirectory = value; }
        }

        public LanguageType Language
        {
            get { return _Language; }
            set { _Language = value; }
        }

        public string Path
        {
            get { return _Path; }
            set { _Path = value; }
        }

        public string Prefix
        {
            get { return _Prefix; }
            set { _Prefix = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string ProjectFile
        {
            get { return _ProjectFile; }
        }

        public string SolutionFile
        {
            get { return _SolutionFile; }
        }

        public static string SolutionFileExtension
        {
            get { return Constant.ExtensionSolution; }
        }

        public string TypePrefix
        {
            get { return _TypePrefix; }
            set { _TypePrefix = value; }
        }


        private BuildFiles()
        {
            //Hidden
        }

        public BuildFiles(VS_Version Version)
        {
            _Version = Version;
            BuildFilesCommon();
        }
        public BuildFiles(VS_Version Version, string Name)
        {
            _Name = Name;
            _Version = Version;
            BuildFilesCommon();
        }
        private void BuildFilesCommon()
        {
            _Files = new List<CodeFile>();
            _References = new List<Reference>();
        }

        public void AddFile(string File)
        {
            CodeFile NewFile = new CodeFile();
            NewFile.Name = File;
            NewFile.Type = CodeFileType.Standard;
            _Files.Add(NewFile);
        }

        public void AddFile(string File, CodeFileType Which)
        {
            CodeFile NewFile = new CodeFile();
            NewFile.Name = File;
            NewFile.Type = Which;
            _Files.Add(NewFile);
        }

        public void AddFile(string File, CodeFileType Which, string Dependant)
        {
            CodeFile NewFile = new CodeFile();
            NewFile.Name = File;
            NewFile.Type = Which;
            NewFile.Dependant = Dependant;
            _Files.Add(NewFile);
        }

        public Reference AddReference(string Name)
        {
            Reference NewReference = new Reference();
            NewReference.Name = Name;
            _References.Add(NewReference);
            return NewReference;
        }

        public void BuildSolutionFile()
        {
            StreamWriter SolutionFile;
            string Version = string.Empty;
            string VersionFormat = string.Empty;


            switch (_Version)
            {
                case VS_Version.v2005:
                    Version = "# Visual Studio 2005";
                    VersionFormat = "Microsoft Visual Studio Solution File, Format Version 9.00";
                    break;
                case VS_Version.v2008:
                    switch (_Language)
                    {
                        case LanguageType.CSharp:
                            Version = "# Visual Studio 2008";
                            break;
                        case LanguageType.VB:
                            Version = "# Visual Studio 2008";
                            break;
                    }
                    VersionFormat = "ï»¿" + Environment.NewLine + "Microsoft Visual Studio Solution File, Format Version 10.00";
                    VersionFormat = "Microsoft Visual Studio Solution File, Format Version 10.00";
                    break;
                case VS_Version.v2010:
                    Version = "# Visual Studio 2010";
                    VersionFormat = "Microsoft Visual Studio Solution File, Format Version 11.00";
                    break;
                case VS_Version.v2012:
                    Version = "# Visual Studio 2012";
                    VersionFormat = "Microsoft Visual Studio Solution File, Format Version 12.00";
                    break;
            }

            if (Version != string.Empty)
            {
                //SolutionFile = New StreamWriter(mstrSolutionPath & "\" & mstrName & mconExtension)
                _ProjectID = Guid.NewGuid();
                _SolutionFile = _Path + "\\" + _Name + Constant.ExtensionSolution;
                SolutionFile = new StreamWriter(_SolutionFile);
                SolutionFile.WriteLine(VersionFormat);
                SolutionFile.WriteLine(Version);
                //SolutionFile.WriteLine("Project(""" & mconLibraryGUID & """) = """ & constant.Prefix & constant.Root1 & "_" & mstrName & """, """ & strPath & Project.Extension & """, ""{" & guidUUID.ToString & "}""")
                SolutionFile.WriteLine("Project(\"{" + Guid.NewGuid().ToString() + "}\") = \"" + _Name + "\", \"" + _Name + Constant.ExtensionProject(_Language) + "\", \"{" + _ProjectID.ToString() + "}\"");
                SolutionFile.WriteLine("EndProject");
                SolutionFile.WriteLine("Global");
                SolutionFile.WriteLine(_Tab + "GlobalSection(SolutionConfigurationPlatforms) = preSolution");
                SolutionFile.WriteLine(_Tab + "" + _Tab + "Debug|Any CPU = Debug|Any CPU");
                SolutionFile.WriteLine(_Tab + "" + _Tab + "Release|Any CPU = Release|Any CPU");
                SolutionFile.WriteLine(_Tab + "EndGlobalSection");
                SolutionFile.WriteLine(_Tab + "GlobalSection(ProjectConfigurationPlatforms) = postSolution");
                SolutionFile.WriteLine(_Tab + "" + _Tab + "{" + _ProjectID.ToString() + "}.Debug|Any CPU.ActiveCfg = Debug|Any CPU");
                SolutionFile.WriteLine(_Tab + "" + _Tab + "{" + _ProjectID.ToString() + "}.Debug|Any CPU.Build.0 = Debug|Any CPU");
                SolutionFile.WriteLine(_Tab + "" + _Tab + "{" + _ProjectID.ToString() + "}.Release|Any CPU.ActiveCfg = Release|Any CPU");
                SolutionFile.WriteLine(_Tab + "" + _Tab + "{" + _ProjectID.ToString() + "}.Release|Any CPU.Build.0 = Release|Any CPU");
                SolutionFile.WriteLine(_Tab + "EndGlobalSection");
                SolutionFile.WriteLine(_Tab + "GlobalSection(SolutionProperties) = preSolution");
                SolutionFile.WriteLine(_Tab + "" + _Tab + "HideSolutionNode = False");
                SolutionFile.WriteLine(_Tab + "EndGlobalSection");
                SolutionFile.WriteLine("EndGlobal");
                SolutionFile.Close();
            }

        }

        public void BuildAssemblyInfo(string Name, string Prefix)
        {
            StreamWriter AssemblyInfoFile;
            BuildCode bc;
            string Code;


            bc = new BuildCode(_Language);

            Code = bc.AssemblyAttribute("AssemblyTitle", "\"" + Name + "\"");
            Code += bc.AssemblyAttribute("AssemblyDescription", "\"\"");
            Code += bc.AssemblyAttribute("AssemblyConfiguration", "\"\"");
            Code += bc.AssemblyAttribute("AssemblyCompany", "\"\"");
            Code += bc.AssemblyAttribute("AssemblyProduct", "\"" + Name + "\"");
            Code += bc.AssemblyAttribute("AssemblyCopyright", "\"Copyright ©  " + DateTime.Today.Year + "\"");
            Code += bc.AssemblyAttribute("AssemblyTrademark", "\"\"");
            Code += bc.AssemblyAttribute("AssemblyCulture", "\"\"");
            Code += bc.AssemblyAttribute("ComVisible", "false");
            Code += bc.AssemblyAttribute("AssemblyVersion", "\"1.0.0.0\"");
            Code += bc.AssemblyAttribute("AssemblyFileVersion", "\"1.0.0.0\"");
            Code += Environment.NewLine;
            Code = bc.AddLibrary("System.Runtime.InteropServices") + bc.EOL() + Environment.NewLine + Code;
            Code = bc.AddLibrary("System.Runtime.CompilerServices") + bc.EOL() + Code;
            Code = bc.AddLibrary("System.Reflection") + bc.EOL() + Code;

            //"[assembly: Guid("9aba4f90-d077-4a8d-93a5-c7bd52573794")]" + Environment.NewLine +

            _AssemblyInfoFile = _Path + "\\" + Constant.AuxilleryDirectory(_Language);
            if (!Directory.Exists(_AssemblyInfoFile))
                Directory.CreateDirectory(_AssemblyInfoFile);

            _AssemblyInfoFile += "\\AssemblyInfo" + Constant.ExtensionFile(_Language);
            AssemblyInfoFile = new StreamWriter(_AssemblyInfoFile);
            AssemblyInfoFile.WriteLine(Code);
            AssemblyInfoFile.Close();

        }

        public void BuildProjectFile(string Name)
        {
            XmlAttribute Attr;
            XmlDocument Doc;
            XmlDeclaration Dec;
            XmlElement Element;
            XmlElement ElementInner;
            string FrameworkVersion = string.Empty;
            XmlElement ItemGroup;
            string MSBuild = string.Empty;
            string ProductVersion = string.Empty;
            XmlElement Project;
            XmlElement PropertyGroup;
            string ToolsVersion = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    MSBuild = "CSharp";
                    break;
                case LanguageType.VB:
                    MSBuild = "VisualBasic";
                    break;
            }

            switch (_Version)
            {
                case VS_Version.v2005:
                    FrameworkVersion = "??";
                    ToolsVersion = "??";
                    ProductVersion = "??";
                    break;
                case VS_Version.v2008:
                    FrameworkVersion = "v3.5";
                    ToolsVersion = "3.5";
                    ProductVersion = "9.0.21022";
                    break;
                case VS_Version.v2010:
                    FrameworkVersion = "v4.0";
                    ToolsVersion = "4.0";
                    ProductVersion = "8.0.30703";
                    break;
            }

            //CodeName = Constant.Prefix & Constant.Root1 & "_" & strName
            Doc = new XmlDocument();
            Dec = Doc.CreateXmlDeclaration("1.0", "utf-8", null);
            Doc.AppendChild(Dec);
            Project = Doc.CreateElement("Project");
            Attr = Doc.CreateAttribute("ToolsVersion");
            Attr.InnerText = ToolsVersion;
            Project.Attributes.Append(Attr);
            Attr = Doc.CreateAttribute("xmlns");
            Attr.InnerText = "http://schemas.microsoft.com/developer/msbuild/2003";
            Project.Attributes.Append(Attr);
            //Property Group 
            PropertyGroup = Doc.CreateElement("PropertyGroup");
            Element = Doc.CreateElement("Configuration");
            Element.InnerText = "Debug";
            Attr = Doc.CreateAttribute("Condition");
            Attr.InnerText = " '$(Configuration)' == '' ";
            Element.Attributes.Append(Attr);
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("Platform");
            Element.InnerText = "AnyCPU";
            Attr = Doc.CreateAttribute("Condition");
            Attr.InnerText = " '$(Platform)' == '' ";
            Element.Attributes.Append(Attr);
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("ProductVersion");
            Element.InnerText = ProductVersion;//"9.0.21022" maybe VS 2008 pro;
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("SchemaVersion");
            Element.InnerText = "2.0";
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("ProjectGuid");
            Element.InnerText = _ProjectID.ToString();
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("OutputType");
            Element.InnerText = "WinExe";
            PropertyGroup.AppendChild(Element);
            if (_Language == LanguageType.CSharp)
            {
                Element = Doc.CreateElement("AppDesignerFolder");
                Element.InnerText = "Properties";
                PropertyGroup.AppendChild(Element);
            }
            Element = Doc.CreateElement("RootNamespace");
            Element.InnerText = Name;
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("AssemblyName");
            Element.InnerText = Name;
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("FileAlignment");
            Element.InnerText = "512";
            PropertyGroup.AppendChild(Element);
            if (_Language == LanguageType.VB)
            {
                Element = Doc.CreateElement("OptionExplicit");
                Element.InnerText = "On";
                PropertyGroup.AppendChild(Element);
                Element = Doc.CreateElement("OptionCompare");
                Element.InnerText = "Binary";
                PropertyGroup.AppendChild(Element);
                Element = Doc.CreateElement("OptionStrict");
                Element.InnerText = "On";
                PropertyGroup.AppendChild(Element);
                Element = Doc.CreateElement("OptionInfer");
                Element.InnerText = "On";
                PropertyGroup.AppendChild(Element);
            }
            Element = Doc.CreateElement("TargetFrameworkVersion");
            Element.InnerText = FrameworkVersion;
            PropertyGroup.AppendChild(Element);
            Project.AppendChild(PropertyGroup);

            //Property Group Debug
            PropertyGroup = Doc.CreateElement("PropertyGroup");
            Attr = Doc.CreateAttribute("Condition");
            Attr.InnerText = " '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ";
            PropertyGroup.Attributes.Append(Attr);
            Element = Doc.CreateElement("DebugSymbols");
            Element.InnerText = "true";
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("DebugType");
            Element.InnerText = "full";
            PropertyGroup.AppendChild(Element);
            if (_Language == LanguageType.CSharp)
            {
                Element = Doc.CreateElement("Optimize");
                Element.InnerText = "false";
                PropertyGroup.AppendChild(Element);
            }
            else
            {
                Element = Doc.CreateElement("DefineDebug");
                Element.InnerText = "true";
                PropertyGroup.AppendChild(Element);
                Element = Doc.CreateElement("DefineTrace");
                Element.InnerText = "true";
                PropertyGroup.AppendChild(Element);
            }
            Element = Doc.CreateElement("OutputPath");
            Element.InnerText = "bin\\Debug\\";
            PropertyGroup.AppendChild(Element);
            //Element = Doc.CreateElement("DocumentationFile");
            //Element.InnerText = Name + Constant.xmlExtenstion;
            //PropertyGroup.AppendChild(Element);
            if (_Language == LanguageType.CSharp)
            {
                Element = Doc.CreateElement("DefineConstants");
                Element.InnerText = "DEBUG;TRACE";
                PropertyGroup.AppendChild(Element);
                Element = Doc.CreateElement("ErrorReport");
                Element.InnerText = "prompt";
                PropertyGroup.AppendChild(Element);
                Element = Doc.CreateElement("WarningLevel");
                Element.InnerText = "4";
                PropertyGroup.AppendChild(Element);
            }
            else
            {
                Element = Doc.CreateElement("NoWarn");
                Element.InnerText = string.Empty;
                PropertyGroup.AppendChild(Element);
                Element = Doc.CreateElement("WarningsAsErrors");
                Element.InnerText = "41999,42016,42017,42018,42019,42020,42021,42022,42032,42036";
                PropertyGroup.AppendChild(Element);
            }
            Project.AppendChild(PropertyGroup);
            //Property Group Release
            PropertyGroup = Doc.CreateElement("PropertyGroup");
            Attr = Doc.CreateAttribute("Condition");
            Attr.InnerText = " '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ";
            PropertyGroup.Attributes.Append(Attr);
            Element = Doc.CreateElement("DebugType");
            Element.InnerText = "pdbonly";
            PropertyGroup.AppendChild(Element);
            if (_Language == LanguageType.VB)
            {
                Element = Doc.CreateElement("DefineDebug");
                Element.InnerText = "false";
                PropertyGroup.AppendChild(Element);
                Element = Doc.CreateElement("DefineTrace");
                Element.InnerText = "true";
                PropertyGroup.AppendChild(Element);
            }
            Element = Doc.CreateElement("Optimize");
            Element.InnerText = "true";
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("OutputPath");
            Element.InnerText = "bin\\Release\\";
            PropertyGroup.AppendChild(Element);
            //Element = Doc.CreateElement("DocumentationFile");
            //Element.InnerText = Name + Constant.xmlExtenstion;
            //PropertyGroup.AppendChild(Element);
            if (_Language == LanguageType.CSharp)
            {
                Element = Doc.CreateElement("DefineConstants");
                Element.InnerText = "TRACE";
                PropertyGroup.AppendChild(Element);
                Element = Doc.CreateElement("ErrorReport");
                Element.InnerText = "prompt";
                PropertyGroup.AppendChild(Element);
                Element = Doc.CreateElement("WarningLevel");
                Element.InnerText = "4";
                PropertyGroup.AppendChild(Element);
            }
            else
            {
                Element = Doc.CreateElement("NoWarn");
                Element.InnerText = string.Empty;
                PropertyGroup.AppendChild(Element);
                Element = Doc.CreateElement("WarningsAsErrors");
                Element.InnerText = "41999,42016,42017,42018,42019,42020,42021,42022,42032,42036";
                PropertyGroup.AppendChild(Element);
            }
            Project.AppendChild(PropertyGroup);

            //Program
            PropertyGroup = Doc.CreateElement("PropertyGroup");
            Element = Doc.CreateElement("StartupObject");
            Element.InnerText = Name + ".Program";
            PropertyGroup.AppendChild(Element);
            Project.AppendChild(PropertyGroup);

            //References  
            ItemGroup = Doc.CreateElement("ItemGroup");
            Element = Doc.CreateElement("Reference");
            Attr = Doc.CreateAttribute("Include");
            Attr.InnerText = "System";
            Element.Attributes.Append(Attr);
            ItemGroup.AppendChild(Element);
            Element = Doc.CreateElement("Reference");
            Attr = Doc.CreateAttribute("Include");
            Attr.InnerText = "System.Core";
            Element.Attributes.Append(Attr);
            ItemGroup.AppendChild(Element);
            Element = Doc.CreateElement("Reference");
            Attr = Doc.CreateAttribute("Include");
            Attr.InnerText = "System.Xml";
            Element.Attributes.Append(Attr);
            ItemGroup.AppendChild(Element);
            if (_Language == LanguageType.CSharp)
            {
                Element = Doc.CreateElement("Reference");
                Attr = Doc.CreateAttribute("Include");
                Attr.InnerText = "Microsoft.CSharp";
                Element.Attributes.Append(Attr);
                ItemGroup.AppendChild(Element);
            }

                Element = Doc.CreateElement("Reference");
                Attr = Doc.CreateAttribute("Include");
                Attr.InnerText = "System.Drawing";
                Element.Attributes.Append(Attr);
                ItemGroup.AppendChild(Element);
                Element = Doc.CreateElement("Reference");
                Attr = Doc.CreateAttribute("Include");
                Attr.InnerText = "System.Windows.Forms";
                Element.Attributes.Append(Attr);
                ItemGroup.AppendChild(Element);

            Project.AppendChild(ItemGroup);

            ItemGroup = Doc.CreateElement("ItemGroup");
            foreach (CodeFile File in _Files)
            {
                switch (File.Type)
                {
                    case CodeFileType.Standard:
                        {
                            Element = Doc.CreateElement("Compile");
                            Attr = Doc.CreateAttribute("Include");
                            Attr.InnerText = File.Name;
                            Element.Attributes.Append(Attr);
                            ItemGroup.AppendChild(Element);
                            break;
                        }
                    case CodeFileType.Program:
                        {
                            Element = Doc.CreateElement("Compile");
                            Attr = Doc.CreateAttribute("Include");
                            Attr.InnerText = File.Name;
                            Element.Attributes.Append(Attr);
                            ItemGroup.AppendChild(Element);
                            break;
                        }
                    case CodeFileType.Form:
                        Element = Doc.CreateElement("Compile");
                        Attr = Doc.CreateAttribute("Include");
                        Attr.InnerText = File.Name;
                        Element.Attributes.Append(Attr);
                        ElementInner = Doc.CreateElement("SubType");
                        ElementInner.InnerText = "Form";
                        Element.AppendChild(ElementInner);
                        ItemGroup.AppendChild(Element);

                        //Designer
                        Element = Doc.CreateElement("Compile");
                        Attr = Doc.CreateAttribute("Include");
                        Attr.InnerText = File.Designer();
                        Element.Attributes.Append(Attr);
                        //ElementInner = Doc.CreateElement("SubType");
                        //ElementInner.InnerText = "Code";
                        //Element.AppendChild(ElementInner);
                        ElementInner = Doc.CreateElement("DependentUpon");
                        ElementInner.InnerText = File.Name;
                        Element.AppendChild(ElementInner);
                        ItemGroup.AppendChild(Element);

                        //AssemblyInfo
                        Element = Doc.CreateElement("Compile");
                        Attr = Doc.CreateAttribute("Include");
                        Attr.InnerText = Constant.AuxilleryDirectory(_Language) + "\\AssemblyInfo" + Constant.ExtensionFile(_Language);
                        Element.Attributes.Append(Attr);
                        ItemGroup.AppendChild(Element);
                        break;

                    case CodeFileType.Resource:
                        if ((int)_Version < (int)VS_Version.v2010)
                        {
                            Project.AppendChild(ItemGroup);
                            ItemGroup = Doc.CreateElement("ItemGroup");
                        }
                        Element = Doc.CreateElement("EmbeddedResource");
                        Attr = Doc.CreateAttribute("Include");
                        Attr.InnerText = File.Name;
                        Element.Attributes.Append(Attr);
                        ElementInner = Doc.CreateElement("DependentUpon");
                        ElementInner.InnerText = File.Dependant;
                        Element.AppendChild(ElementInner);
                        ItemGroup.AppendChild(Element);
                        break;

                    //<ItemGroup>
                    //  <None Include="Resources\StatusIcon.bmp" />
                    //</ItemGroup>

                }
                //ItemGroup.AppendChild(Element);

            }
            Project.AppendChild(ItemGroup);

            Element = Doc.CreateElement("Import");
            Attr = Doc.CreateAttribute("Project");
            Attr.InnerText = "$(MSBuildToolsPath)\\Microsoft." + MSBuild + ".targets";
            Element.Attributes.Append(Attr);
            Project.AppendChild(Element);

            Doc.AppendChild(Project);

            _ProjectFile = _Name + Constant.ExtensionProject(_Language);
            Doc.Save(Path + "\\" + _ProjectFile);
        }

        public void BuildProjectFileCS2010(string Name, string Prefix)
        {
            XmlAttribute Attr;
            XmlDocument Doc;
            XmlDeclaration Dec;
            XmlElement Element;
            XmlElement ElementInner;
            XmlElement ItemGroup;
            XmlElement Project;
            XmlElement PropertyGroup;



            //CodeName = Constant.Prefix & Constant.Root1 & "_" & strName
            Doc = new XmlDocument();
            Dec = Doc.CreateXmlDeclaration("1.0", "utf-8", null);
            Doc.AppendChild(Dec);
            Project = Doc.CreateElement("Project");
            Attr = Doc.CreateAttribute("ToolsVersion");
            Attr.InnerText = "4.0";
            Project.Attributes.Append(Attr);
            Attr = Doc.CreateAttribute("DefaultTargets");
            Attr.InnerText = "Build";
            Project.Attributes.Append(Attr);
            Attr = Doc.CreateAttribute("xmlns");
            Attr.InnerText = "http://schemas.microsoft.com/developer/msbuild/2003";
            Project.Attributes.Append(Attr);
            //Property Group 
            PropertyGroup = Doc.CreateElement("PropertyGroup");
            Element = Doc.CreateElement("Configuration");
            Element.InnerText = "Debug";
            Attr = Doc.CreateAttribute("Condition");
            Attr.InnerText = " '$(Configuration)' == '' ";
            Element.Attributes.Append(Attr);
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("Platform");
            Element.InnerText = "AnyCPU";
            Attr = Doc.CreateAttribute("Condition");
            Attr.InnerText = " '$(Platform)' == '' ";
            Element.Attributes.Append(Attr);
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("ProductVersion");
            Element.InnerText = "8.0.30703";
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("SchemaVersion");
            Element.InnerText = "2.0";
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("ProjectGuid");
            Element.InnerText = _ProjectID.ToString();
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("OutputType");
            Element.InnerText = "WinExe";
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("RootNamespace");
            Element.InnerText = Name;
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("AssemblyName");
            Element.InnerText = Name;
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("TargetFrameworkVersion");
            Element.InnerText = "v4.0";
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("FileAlignment");
            Element.InnerText = "512";
            PropertyGroup.AppendChild(Element);
            Project.AppendChild(PropertyGroup);

            //Property Group Debug
            PropertyGroup = Doc.CreateElement("PropertyGroup");
            Attr = Doc.CreateAttribute("Condition");
            Attr.InnerText = " '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ";
            PropertyGroup.Attributes.Append(Attr);
            Element = Doc.CreateElement("DebugSymbols");
            Element.InnerText = "true";
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("DebugType");
            Element.InnerText = "full";
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("Optimize");
            Element.InnerText = "false";
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("OutputPath");
            Element.InnerText = _DevelopmentDirectory;
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("DefineConstants");
            Element.InnerText = "DEBUG;TRACE";
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("ErrorReport");
            Element.InnerText = "prompt";
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("WarningLevel");
            Element.InnerText = "4";
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("DocumentationFile");
            Element.InnerText = Name + Constant.xmlExtenstion;
            PropertyGroup.AppendChild(Element);
            Project.AppendChild(PropertyGroup);
            //Property Group Release
            PropertyGroup = Doc.CreateElement("PropertyGroup");
            Attr = Doc.CreateAttribute("Condition");
            Attr.InnerText = " '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ";
            PropertyGroup.Attributes.Append(Attr);
            Element = Doc.CreateElement("DebugType");
            Element.InnerText = "pdbonly";
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("Optimize");
            Element.InnerText = "true";
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("OutputPath");
            Element.InnerText = _DevelopmentDirectory;
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("DefineConstants");
            Element.InnerText = "TRACE";
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("ErrorReport");
            Element.InnerText = "prompt";
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("WarningLevel");
            Element.InnerText = "4";
            PropertyGroup.AppendChild(Element);
            Element = Doc.CreateElement("DocumentationFile");
            Element.InnerText = Name + Constant.xmlExtenstion;
            PropertyGroup.AppendChild(Element);
            Project.AppendChild(PropertyGroup);

            //References  
            ItemGroup = Doc.CreateElement("ItemGroup");
            Element = Doc.CreateElement("Reference");
            Attr = Doc.CreateAttribute("Include");
            Attr.InnerText = "System";
            Element.Attributes.Append(Attr);
            ItemGroup.AppendChild(Element);
            Element = Doc.CreateElement("Reference");
            Attr = Doc.CreateAttribute("Include");
            Attr.InnerText = "System.Core";
            Element.Attributes.Append(Attr);
            ItemGroup.AppendChild(Element);
            Element = Doc.CreateElement("Reference");
            Attr = Doc.CreateAttribute("Include");
            Attr.InnerText = "System.Xml";
            Element.Attributes.Append(Attr);
            ItemGroup.AppendChild(Element);

            Element = Doc.CreateElement("Reference");
                Attr = Doc.CreateAttribute("Include");
                Attr.InnerText = "System.Drawing";
                Element.Attributes.Append(Attr);
                ItemGroup.AppendChild(Element);
                Element = Doc.CreateElement("Reference");
                Attr = Doc.CreateAttribute("Include");
                Attr.InnerText = "System.Windows.Forms";
                Element.Attributes.Append(Attr);
                ItemGroup.AppendChild(Element);

            Project.AppendChild(ItemGroup);

            ItemGroup = Doc.CreateElement("ItemGroup");
            foreach (CodeFile File in _Files)
            {
                switch (_Version)
                {
                    case VS_Version.v2008:
                        {
                            Element = Doc.CreateElement("Compile");
                            Attr = Doc.CreateAttribute("Include");
                            Attr.InnerText = File.Name;
                            Element.Attributes.Append(Attr);
                            ItemGroup.AppendChild(Element);
                            break;
                        }
                    case VS_Version.v2010:
                        {
                            switch (File.Type)
                            {
                                case CodeFileType.Standard:
                                    {
                                        Element = Doc.CreateElement("Compile");
                                        Attr = Doc.CreateAttribute("Include");
                                        Attr.InnerText = File.Name;
                                        Element.Attributes.Append(Attr);
                                        ItemGroup.AppendChild(Element);
                                        break;
                                    }
                                case CodeFileType.Form:
                                    Element = Doc.CreateElement("Compile");
                                    Attr = Doc.CreateAttribute("Include");
                                    Attr.InnerText = File.Name;
                                    Element.Attributes.Append(Attr);
                                    ElementInner = Doc.CreateElement("SubType");
                                    ElementInner.InnerText = "Form";
                                    Element.AppendChild(ElementInner);
                                    ItemGroup.AppendChild(Element);

                                    //Designer
                                    Element = Doc.CreateElement("Compile");
                                    Attr = Doc.CreateAttribute("Include");
                                    Attr.InnerText = File.Designer();
                                    Element.Attributes.Append(Attr);
                                    ElementInner = Doc.CreateElement("DependentUpon");
                                    ElementInner.InnerText = File.Name;
                                    Element.AppendChild(ElementInner);
                                    ItemGroup.AppendChild(Element);

                                    //AssemblyInfo
                                    Element = Doc.CreateElement("Compile");
                                    Attr = Doc.CreateAttribute("Include");
                                    Attr.InnerText = "Properties\\AssemblyInfo.cs";
                                    Element.Attributes.Append(Attr);
                                    ItemGroup.AppendChild(Element);
                                    break;
                                case CodeFileType.Resource:
                                    Project.AppendChild(ItemGroup);//MAR *** Fix
                                    ItemGroup = Doc.CreateElement("ItemGroup");
                                    Element = Doc.CreateElement("EmbeddedResource");
                                    Attr = Doc.CreateAttribute("Include");
                                    Attr.InnerText = File.Name;
                                    Element.Attributes.Append(Attr);
                                    ElementInner = Doc.CreateElement("DependentUpon");
                                    ElementInner.InnerText = File.Dependant;
                                    Element.AppendChild(ElementInner);
                                    ItemGroup.AppendChild(Element);
                                    break;
                            }
                            //ItemGroup.AppendChild(Element);
                            break;
                        }
                }

            }
            Project.AppendChild(ItemGroup);

            Element = Doc.CreateElement("Import");
            Attr = Doc.CreateAttribute("Project");
            Attr.InnerText = "$(MSBuildToolsPath)\\Microsoft.CSharp.targets";
            Element.Attributes.Append(Attr);
            Project.AppendChild(Element);

            Doc.AppendChild(Project);

            _ProjectFile = _Name + Constant.ExtensionProject(_Language);
            Doc.Save(Path + "\\" + _ProjectFile);
        }


    }
}
