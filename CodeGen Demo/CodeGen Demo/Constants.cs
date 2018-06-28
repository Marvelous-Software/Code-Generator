using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGen_Demo
{
    class Constant
    {

        public const string Filter = "Filter";
        public const string NamePrefix = "CodeGen";
        public const string NameRoot = "NotePadDemo";
        public const string NoFilter = "None";
        public const string xmlExtenstion = ".xml";

        public const string ExtensionCompiled = ".dll";
        public const string ExtensionCS = ".cs";
        public const string ExtensionVB = ".vb";
        public const string ExtensionDesigner = ".Desinger";
        public const string ExtensionProjectCS = ".csproj";
        public const string ExtensionProjectVB = ".vbproj";
        public const string ExtensionResource = ".resx";
        public const string ExtensionSolution = ".sln";


        public static string AuxilleryDirectory(LanguageType which)
        {
                switch (which)
                {
                    case LanguageType.CSharp:
                        return "Properties";
                    case LanguageType.VB:
                        return "My Project";
                    default:
                        return string.Empty;
                }
        }
        
        public static string ExtensionFile(LanguageType which)
        {
                switch (which)
                {
                    case LanguageType.CSharp:
                        return ExtensionCS;
                    case LanguageType.VB:
                        return ExtensionVB;
                    default:
                        return string.Empty;
                }
        }

        public static string ExtensionProject(LanguageType which)
        {
                switch (which)
                {
                    case LanguageType.CSharp:
                        return ExtensionProjectCS;
                    case LanguageType.VB:
                        return ExtensionProjectVB;
                    default:
                        return string.Empty;
                }
        }

        public static string Generator(LanguageType which)
        {
                switch (which)
                {
                    case LanguageType.CSharp:
                        return "ResXFileCodeGenerator";
                    case LanguageType.VB:
                        return "VbMyResourcesResXFileCodeGenerator";
                    default:
                        return string.Empty;
                }
        }

    }
}
