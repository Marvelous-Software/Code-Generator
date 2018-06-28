using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGen_Demo
{
    public class BuildCode
    {

        public enum CastType
        {
            Assured,    //We know the cast will succede
            Object,     //Inheritance type cast (useful only for VB)
            Unknown,    //May not work but we do not want an exception
        }

        public enum Conjunction
        {
            None,
            And,
            Or,
            AndShortCircuit,
            OrShortCircuit
        }

        public enum Comparison
        {
            None,
            Equal,
            NotEqual,
            GreaterThan,
            LessThan,
            GreaterThanOrEqual,
            LessThanOrEqual,
            Is,
        }

        [Flags]
        public enum Modifyer
        {
            None = 0x00,
            Abstract = 0x01,
            Overloads = 0x02,
            Override = 0x04,
            Sealed = 0x08,
            Static = 0x10,
            Virtual = 0x20,
        }

        public enum PassingType
        {
            In,
            Out,
            Reference,
        }

        public enum Scope
        {
            None,
            Friend,
            Private,
            Protected,
            Public,
        }

        public enum VariableTypes
        {
            Boolean,
            Byte,
            Decimal,
            Double,
            Float,
            Integer,
            Long,
            Null,
            Object,
            String,
            Other,
        }

        public class Enumerator
        {
            public string Name;
            public string Attribute;
            public string AttributeValue;
        }

        public class IfArgument
        {
            public bool Not;
            public string Expression;
            public Comparison Test;
            public string Value;
            public Conjunction NextExpression;

            public IfArgument(bool NotIn, string ExpressionIn, Comparison TestIn, string ValueIn, Conjunction NextExpressionIn)
            {
                Not = NotIn;
                Expression = ExpressionIn;
                Test = TestIn;
                Value = ValueIn;
                NextExpression = NextExpressionIn;
            }
        }

        public class Parameter
        {
            public string Name;
            public string Type;
            public PassingType PassBy;

            public Parameter(string NameIn, string TypeIn, PassingType PassByIn)
            {
                Name = NameIn;
                Type = TypeIn;
                PassBy = PassByIn;
            }
        }

        public class SwitchCase
        {
            public string Case;
            public string Code;

            public SwitchCase(string CaseIn, string CodeIn)
            {
                Case = CaseIn;
                Code = CodeIn;
            }
        }

        private int _Indent;
        private LanguageType _Language;


        private BuildCode()
        {
            //Disabled
        }

        public BuildCode(LanguageType which)
        {
            _Language = which;
        }


        public int Indent
        {
            get { return _Indent; }
            set { _Indent = value; }
        }

        /// <summary>
        /// *
        /// </summary>
        /// <param name="Indent"></param>
        /// <param name="ClassName"></param>
        /// <param name="Modifyer"></param>
        /// <param name="Shared"></param>
        /// <param name="Contents"></param>
        /// <param name="Parent"></param>
        /// <param name="Partial"></param>
        /// <returns></returns>
        private string ClassDefinitionWork(int Indent, string ClassName, Scope Modifyer, bool Shared, string Contents, string Parent, bool Partial)
        {
            string code = string.Empty;
            string modifyer = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = new string(' ', Indent);
                    if (Modifyer != Scope.None)
                        code += Modifyer.ToString().ToLower() + " ";
                    if (Shared)
                        code += "static ";
                    if (Partial)
                        code += "partial ";
                    code += "class " + ClassName;
                    if (Parent != string.Empty)
                        code += " : " + Parent;

                    //code += Environment.NewLine + new string(' ', Indent + _Indent) + "{" + Environment.NewLine + Contents + new string(' ', Indent + _Indent) + "}";
                    code += Environment.NewLine + new string(' ', Indent) + "{" + Environment.NewLine + Contents + new string(' ', Indent) + "}";
                    break;
                case LanguageType.VB:
                    code = new string(' ', Indent);
                    if (Modifyer != Scope.None)
                        code += Modifyer.ToString().ToLower() + " ";
                    if (Partial)
                        code += "Partial ";
                    if (Shared)
                        code += "Module " + ClassName;
                    else
                        code += "Class " + ClassName;
                    if (Parent != string.Empty)
                        code += Environment.NewLine + new string(' ', Indent + _Indent) + "Inherits " + Parent;

                    code += Environment.NewLine + Environment.NewLine + Contents + Environment.NewLine + new string(' ', Indent);
                    if (Shared)
                        code += "End Module";
                    else
                        code += "End Class";
                    break;
            }
            return code;
        }

        /// <summary>
        /// Used inside of functions.  Appends EOL.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Constant"></param>
        /// <param name="VarScope"></param>
        /// <param name="vt"></param>
        /// <param name="Default"></param>
        /// <returns></returns>
        private string DeclareVariableWork(string Name, bool Constant, Scope VarScope, string vt, string Default)
        {
            string code = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    if (VarScope != Scope.None)
                        code = VarScope.ToString().ToLower() + " ";
                    if (Constant)
                        code += "const ";
                    code += vt + " " + Name;
                    if (Default != string.Empty)
                        code += " = " + Default;
                    code += ";";
                    break;
                case LanguageType.VB:
                    if (VarScope != Scope.None)
                        code = VarScope.ToString() + " ";
                    else
                        if (!Constant)
                            code = "Dim ";
                    if (Constant)
                        code += "Const ";
                    code += Name + " As " + vt;
                    if (Default != string.Empty)
                        code += " = " + Default;
                    break;
            }

            return code;
        }

        /// <summary>
        /// *
        /// </summary>
        /// <param name="Indent"></param>
        /// <param name="Name"></param>
        /// <param name="AccessModifyer"></param>
        /// <param name="OptionalModifyer"></param>
        /// <param name="Params"></param>
        /// <param name="vt"></param>
        /// <param name="Contents"></param>
        /// <returns></returns>
        private string FunctionWork(int Indent, string Name, Scope AccessModifyer, Modifyer OptionalModifyer, List<Parameter> Params, string vt, string Contents)
        {
            string code = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = new string(' ', Indent);
                    if (AccessModifyer != Scope.None)
                        code += AccessModifyer.ToString().ToLower();
                    if ((OptionalModifyer & Modifyer.Abstract) == Modifyer.Abstract)
                        code += " abstract";
                    if ((OptionalModifyer & Modifyer.Override) == Modifyer.Override)
                        code += " override";
                    if ((OptionalModifyer & Modifyer.Sealed) == Modifyer.Sealed)
                        code += " sealed";
                    if ((OptionalModifyer & Modifyer.Static) == Modifyer.Static)
                        code += " static";
                    if ((OptionalModifyer & Modifyer.Virtual) == Modifyer.Virtual)
                        code += " virtual";
                    if (vt != string.Empty)
                        code += " " + vt;
                    else
                        code += " " + "void";
                    code += " " + Name + "(";

                    if (Params != null)
                    {
                        foreach (Parameter Param in Params)
                            code += ParameterDefinition(Param) + ", ";
                        code = code.Substring(0, code.Length - 2);
                    }
                    code += ")" + Environment.NewLine + new string(' ', Indent) + "{" + Environment.NewLine + Contents + new string(' ', Indent) + "}";
                    break;
                case LanguageType.VB:
                    code = new string(' ', Indent);
                    if (AccessModifyer != Scope.None)
                        code += AccessModifyer.ToString();
                    if ((OptionalModifyer & Modifyer.Abstract) == Modifyer.Abstract)
                        code += " Abstract";
                    if ((OptionalModifyer & Modifyer.Overloads) == Modifyer.Overloads)
                        code += " Overloads";
                    if ((OptionalModifyer & Modifyer.Override) == Modifyer.Override)
                        code += " Overrides";
                    if ((OptionalModifyer & Modifyer.Sealed) == Modifyer.Sealed)
                        code += " Notoverridable";
                    if ((OptionalModifyer & Modifyer.Static) == Modifyer.Static)
                        code += " Shared";
                    if ((OptionalModifyer & Modifyer.Virtual) == Modifyer.Virtual)
                        code += " Overridable";
                    if (vt != string.Empty)
                        code += " Function";
                    else
                        code += " Sub";
                    code += " " + Name + "(";

                    if (Params != null)
                    {
                        foreach (Parameter Param in Params)
                            code += ParameterDefinition(Param) + ", ";
                        code = code.Substring(0, code.Length - 2);
                    }
                    code += ")";
                    if (vt != string.Empty)
                    {
                        code += " As " + vt;
                        code += Environment.NewLine + Environment.NewLine + Contents + Environment.NewLine + new string(' ', Indent) + "End Function";
                    }
                    else
                        code += Environment.NewLine + Environment.NewLine + Contents + Environment.NewLine + new string(' ', Indent) + "End Sub";
                    break;
            }

            return code;
        }

        private string ParameterDefinitionWork(string Name, string vt, PassingType By)
        {
            string code = string.Empty;
            string PassedBy = string.Empty;

            switch (_Language)
            {
                case LanguageType.CSharp:
                    switch (By)
                    {
                        case PassingType.Out:
                            PassedBy = "out ";
                            break;
                        case PassingType.Reference:
                            PassedBy = "ref ";
                            break;
                    }
                    code = PassedBy + vt + " " + Name;
                    break;
                case LanguageType.VB:
                    switch (By)
                    {
                        case PassingType.Out:
                        case PassingType.Reference:
                            PassedBy = "Byref ";
                            break;
                        case PassingType.In:
                            PassedBy = "Byval ";
                            break;
                    }
                    code = PassedBy + Name + " As " + vt;
                    break;
            }

            return code;
        }


        public string AddEvent(string ObjectEvent, string Handler)
        {
            string code = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = ObjectEvent + " += " + Handler;
                    break;
                case LanguageType.VB:
                    code = "AddHandler " + ObjectEvent + ", AddressOf " + Handler;
                    break;
            }

            return code;
        }

        public string AddLibrary(string Library)
        {
            string code = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = "using " + Library;
                    break;
                case LanguageType.VB:
                    code = "Imports " + Library;
                    break;
            }

            return code;
        }

        public string AssemblyAttribute(string Attr, string Value)
        {
            string code = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = "[assembly: " + Attr + "(" + Value + ")]";
                    break;
                case LanguageType.VB:
                    code = "<Assembly: " + Attr + "(" + Value + ")>";
                    break;
            }

            code += Environment.NewLine;
            return code;
        }

        public string ArrayEmpty()
        {
            string code = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = "[]";
                    break;
                case LanguageType.VB:
                    code = "()";
                    break;
            }

            return code;
        }

        public string ArrayInitialize(List<string> lisValues)
        {
            string code = string.Empty;
            string values = string.Empty;


            foreach (string singlevalue in lisValues)
                values += singlevalue + ", ";
            if (values.Length > 1)
                values = values.Substring(0, values.Length - 2);

            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = "[] {" + values + "}";
                    break;
                case LanguageType.VB:
                    code = "() {" + values + "}";
                    break;
            }

            return code;
        }

        public string Attribute(string Attr)
        {
            string code = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = "[" + Attr + "]";
                    break;
                case LanguageType.VB:
                    code = "<" + Attr + "> _";
                    break;
            }

            code += Environment.NewLine;
            return code;
        }

        public string Cast(string Castor, string CastTo, CastType How)
        {
            string code = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    switch (How)
                    {
                        case CastType.Assured:
                        case CastType.Object:
                            code = "(" + CastTo + ")" + Castor;
                            break;
                        case CastType.Unknown:
                            code = Castor + " As " + CastTo;
                            break;
                    }
                    break;
                case LanguageType.VB:
                    switch (How)
                    {
                        case CastType.Assured:
                            code = "Ctype(" + Castor + ", " + CastTo + ")";
                            break;
                        case CastType.Object:
                            code = "Directcast(" + Castor + ", " + CastTo + ")";
                            break;
                        case CastType.Unknown:
                            code = "Trycast(" + Castor + ", " + CastTo + ")";
                            break;
                    }
                    break;
            }
            return code;
        }

        public string ClassDefinition(int Indent, string ClassName, Scope Modifyer, string Contents)
        {
            return ClassDefinition(Indent, ClassName, Modifyer, false, Contents, string.Empty);
        }
        public string ClassDefinition(int Indent, string ClassName, Scope Modifyer, bool Shared, string Contents, string Parent)
        {
            return ClassDefinitionWork(Indent, ClassName, Modifyer, Shared, Contents, Parent, false);
        }

        public string ClassDefinitionDesigner(int Indent, string ClassName, Scope Modifyer, bool Shared, string Contents, string Parent, bool Partial)
        {
            return ClassDefinitionWork(Indent, ClassName, Modifyer, Shared, Contents, Parent, Partial);
        }

        public string Constructor(int Indent, string ClassName, Scope Modifyer, string Contents)
        {
            return Constructor(Indent, ClassName, Modifyer, null, Contents, null);
        }
        public string Constructor(int Indent, string ClassName, Scope Modifyer, List<Parameter> Params, string Contents)
        {
            return Constructor(Indent, ClassName, Modifyer, Params, Contents, null);
        }
        public string Constructor(int Indent, string ClassName, Scope Modifyer, List<Parameter> Params, string Contents, List<Parameter> BaseParams)
        {
            string code = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = new string(' ', Indent) + Modifyer.ToString().ToLower();
                    code += " " + ClassName + "(";

                    if (Params != null)
                    {
                        foreach (Parameter Param in Params)
                            code += ParameterDefinition(Param) + ", ";
                        code = code.Substring(0, code.Length - 2);
                    }
                    code += ")";

                    if (BaseParams != null)
                    {
                        code += " : base(";
                        foreach (Parameter Param in BaseParams)
                            code += Param.Name + ", ";
                        code = code.Substring(0, code.Length - 2);
                        code += ")";
                    }
                    if (Contents != string.Empty)
                        code += Environment.NewLine + new string(' ', Indent) + "{" + Environment.NewLine + Contents + new string(' ', Indent) + "}";
                    else
                        code += Environment.NewLine + new string(' ', Indent) + "{" + Environment.NewLine + new string(' ', Indent) + "}";
                    break;
                case LanguageType.VB:
                    code = new string(' ', Indent) + Modifyer.ToString();
                    code += " Sub New(";

                    if (Params != null)
                    {
                        foreach (Parameter Param in Params)
                            code += ParameterDefinition(Param) + ", ";
                        code = code.Substring(0, code.Length - 2);
                    }
                    code += ")";
                    if (BaseParams != null)
                    {
                        code += Environment.NewLine + Environment.NewLine + new string(' ', Indent) + "    Mybase.New(";
                        foreach (Parameter Param in BaseParams)
                            code += Param.Name + ", ";
                        code = code.Substring(0, code.Length - 2);
                        code += ")" + Environment.NewLine;
                    }
                    if (Contents != string.Empty)
                        code += Environment.NewLine + Contents + Environment.NewLine + new string(' ', Indent) + "End Sub";
                    else
                        code += Environment.NewLine + new string(' ', Indent) + "End Sub";
                    break;
            }

            return code;
        }

        public string Comment(string Contents)
        {
            string code = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = "//";
                    break;
                case LanguageType.VB:
                    code = "'";
                    break;
            }

            return code + Contents + Environment.NewLine;
        }

        public string DataType(string vt)
        {
            return vt;
        }
        public string DataType(VariableTypes vt)
        {
            string code = string.Empty;

            switch (_Language)
            {
                case LanguageType.CSharp:
                    switch (vt)
                    {
                        case VariableTypes.Boolean:
                            code = "bool";
                            break;
                        case VariableTypes.Byte:
                            code = "byte";
                            break;
                        case VariableTypes.Decimal:
                            code = "decimal";
                            break;
                        case VariableTypes.Double:
                            code = "double";
                            break;
                        case VariableTypes.Float:
                            code = "float";
                            break;
                        case VariableTypes.Integer:
                            code = "int";
                            break;
                        case VariableTypes.Long:
                            code = "long";
                            break;
                        case VariableTypes.Object:
                            code = "object";
                            break;
                        case VariableTypes.String:
                            code = "string";
                            break;
                        //case VariableTypes.Other:
                        //    code = Other;
                        //    break;
                    }
                    break;
                case LanguageType.VB:
                    switch (vt)
                    {
                        case VariableTypes.Boolean:
                            code = "Boolean";
                            break;
                        case VariableTypes.Byte:
                            code = "Byte";
                            break;
                        case VariableTypes.Decimal:
                            code = "Decimal";
                            break;
                        case VariableTypes.Double:
                            code = "Double";
                            break;
                        case VariableTypes.Float:
                            code = "Single";
                            break;
                        case VariableTypes.Integer:
                            code = "Integer";
                            break;
                        case VariableTypes.Long:
                            code = "Long";
                            break;
                        case VariableTypes.Object:
                            code = "Object";
                            break;
                        case VariableTypes.String:
                            code = "String";
                            break;
                        //case VariableTypes.Other:
                        //    code = Other;
                        //    break;
                    }
                    break;
            }

            return code;
        }
        public string DataTypeIdentifyers(VariableTypes vt)
        {
            string code = string.Empty;

            switch (_Language)
            {
                case LanguageType.CSharp:
                    switch (vt)
                    {
                        case VariableTypes.Decimal:
                            code = "M";
                            break;
                        case VariableTypes.Double:
                            code = "D";
                            break;
                        case VariableTypes.Float:
                            code = "F";
                            break;
                        case VariableTypes.Integer:
                            code = "I";
                            break;
                        case VariableTypes.Long:
                            code = "L";
                            break;
                    }
                    break;
                case LanguageType.VB:
                    switch (vt)
                    {
                        case VariableTypes.Decimal:
                            code = "D";
                            break;
                        case VariableTypes.Double:
                            code = "R";
                            break;
                        case VariableTypes.Float:
                            code = "F";
                            break;
                        case VariableTypes.Integer:
                            code = "I";
                            break;
                        case VariableTypes.Long:
                            code = "L";
                            break;
                    }
                    break;
            }

            return code;
        }

        public string DeclareConstant(string Name, VariableTypes vt, string Value)
        {
            return DeclareVariableWork(Name, true, Scope.None, DataType(vt), Value);
        }
        public string DeclareConstant(string Name, Scope VarScope, VariableTypes vt, string Value)
        {
            return DeclareVariableWork(Name, true, VarScope, DataType(vt), Value);
        }

        public string DeclareVariable(string Name, VariableTypes vt)
        {
            return DeclareVariableWork(Name, false, Scope.None, DataType(vt), string.Empty);
        }
        public string DeclareVariable(string Name, string vt)
        {
            return DeclareVariableWork(Name, false, Scope.None, vt, string.Empty);
        }
        public string DeclareVariable(string Name, Scope VarScope, VariableTypes vt)
        {
            return DeclareVariableWork(Name, false, VarScope, DataType(vt), string.Empty);
        }
        public string DeclareVariable(string Name, Scope VarScope, string vt)
        {
            return DeclareVariableWork(Name, false, VarScope, vt, string.Empty);
        }
        public string DeclareVariable(string Name, VariableTypes vt, string Default)
        {
            return DeclareVariableWork(Name, false, Scope.None, DataType(vt), Default);
        }
        public string DeclareVariable(string Name, string vt, string Default)
        {
            return DeclareVariableWork(Name, false, Scope.None, vt, Default);
        }

        public string DesignerCast(string obj, string dest)
        {
            string code = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = "((" + dest + ")(" + obj + "))";
                    break;
                case LanguageType.VB:
                    code = "Ctype(" + obj + ", " + dest + ")";
                    break;
            }

            return code;
        }

        public string DesignerDeclareControl(string Name, string Control)
        {
            string code = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = "public " + Control + " " + Name;
                    break;
                case LanguageType.VB:
                    code = "Friend Withevents " + Name + " As " + Control;
                    break;
            }

            return code;
        }

        public string EmbeddedQuote()
        {
            return EmbeddedQuote(1);
        }
        public string EmbeddedQuote(int quantity)
        {
            string code = string.Empty;
            string quote = string.Empty;
            int r;

            if (quantity > 0)
            {
                switch (_Language)
                {
                    case LanguageType.CSharp:
                        quote = "\"";
                        break;
                    case LanguageType.VB:
                        quote = "\"";
                        break;
                }
                //string constructor with number?
                for (r = 0; r < quantity; r++)
                    code += quote;
            }

            return code;
        }

        public string Enumeration(int Indent, string Name, Scope AccessModifyer, List<Enumerator> Enums)//, string vt)
        {
            string code = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = new string(' ', Indent) + AccessModifyer.ToString().ToLower() + " enum " + Name + Environment.NewLine;
                    code += new string(' ', Indent) + "{" + Environment.NewLine;

                    foreach (Enumerator E in Enums)
                    {
                        if (E.Attribute != string.Empty)
                        {
                            code += new string(' ', Indent + _Indent) + "[" + E.Attribute;
                            if (E.AttributeValue != string.Empty)
                                code += "(" + E.AttributeValue + ")";
                            code += "]" + Environment.NewLine;
                        }
                        code += new string(' ', Indent + _Indent) + E.Name + "," + Environment.NewLine;
                    }
                    code += new string(' ', Indent) + "}" + Environment.NewLine;
                    break;
                case LanguageType.VB:
                    code = new string(' ', Indent) + AccessModifyer.ToString() + " Enum " + Name + Environment.NewLine;

                    foreach (Enumerator E in Enums)
                    {
                        if (E.Attribute != string.Empty)
                        {
                            code += new string(' ', Indent + _Indent) + "<" + E.Attribute;
                            if (E.AttributeValue != string.Empty)
                                code += "(" + E.AttributeValue + ")";
                            code += "> _" + Environment.NewLine;
                        }
                        code += new string(' ', Indent + _Indent) + E.Name + Environment.NewLine;
                    }
                    code += new string(' ', Indent) + "End Enum" + Environment.NewLine;
                    break;
            }

            return code;
        }

        public string EOL()
        {
            string code = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = ";";
                    break;
                case LanguageType.VB:
                    code = "";
                    break;
            }

            return code + Environment.NewLine;
        }

        public string Function(int Indent, string Name, Scope AccessModifyer, VariableTypes vt, string Contents)
        {
            return FunctionWork(Indent, Name, AccessModifyer, Modifyer.None, null, DataType(vt), Contents);
        }
        public string Function(int Indent, string Name, Scope AccessModifyer, string vt, string Contents)
        {
            return FunctionWork(Indent, Name, AccessModifyer, Modifyer.None, null, vt, Contents);
        }
        public string Function(int Indent, string Name, Scope AccessModifyer, List<Parameter> Params, VariableTypes vt, string Contents)
        {
            return FunctionWork(Indent, Name, AccessModifyer, Modifyer.None, Params, DataType(vt), Contents);
        }
        public string Function(int Indent, string Name, Scope AccessModifyer, List<Parameter> Params, string vt, string Contents)
        {
            return FunctionWork(Indent, Name, AccessModifyer, Modifyer.None, Params, vt, Contents);
        }
        public string Function(int Indent, string Name, Scope AccessModifyer, Modifyer OptionalModifyer, List<Parameter> Params, VariableTypes vt, string Contents)
        {
            return FunctionWork(Indent, Name, AccessModifyer, OptionalModifyer, Params, DataType(vt), Contents);
        }
        public string Function(int Indent, string Name, Scope AccessModifyer, Modifyer OptionalModifyer, List<Parameter> Params, string vt, string Contents)
        {
            return FunctionWork(Indent, Name, AccessModifyer, OptionalModifyer, Params, vt, Contents);
        }

        public string GetBase()
        {
            string code = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = "base";
                    break;
                case LanguageType.VB:
                    code = "Mybase";
                    break;
            }
            return code;
        }
        public string GetComparison(Comparison Which)
        {
            string code = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    switch (Which)
                    {
                        case Comparison.Equal:
                            code = "==";
                            break;
                        case Comparison.GreaterThan:
                            code = ">";
                            break;
                        case Comparison.GreaterThanOrEqual:
                            code = ">=";
                            break;
                        case Comparison.LessThan:
                            code = "<";
                            break;
                        case Comparison.LessThanOrEqual:
                            code = "<=";
                            break;
                        case Comparison.NotEqual:
                            code = "!=";
                            break;
                        case Comparison.Is:
                            code = "is";
                            break;
                    }
                    break;
                case LanguageType.VB:
                    switch (Which)
                    {
                        case Comparison.Equal:
                            code = "=";
                            break;
                        case Comparison.GreaterThan:
                            code = ">";
                            break;
                        case Comparison.GreaterThanOrEqual:
                            code = ">=";
                            break;
                        case Comparison.LessThan:
                            code = "<";
                            break;
                        case Comparison.LessThanOrEqual:
                            code = "<=";
                            break;
                        case Comparison.NotEqual:
                            code = "<>";
                            break;
                        case Comparison.Is:
                            code = "Typeof {0} Is";
                            break;
                    }
                    break;
            }
            return code;
        }

        public string GetConjunction(Conjunction Which)
        {
            string code = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    switch (Which)
                    {
                        case Conjunction.And:
                            code = "&";
                            break;
                        case Conjunction.AndShortCircuit:
                            code = "&&";
                            break;
                        case Conjunction.Or:
                            code = "|";
                            break;
                        case Conjunction.OrShortCircuit:
                            code = "||";
                            break;
                    }
                    break;
                case LanguageType.VB:
                    switch (Which)
                    {
                        case Conjunction.And:
                            code = "And";
                            break;
                        case Conjunction.AndShortCircuit:
                            code = "Andalso";
                            break;
                        case Conjunction.Or:
                            code = "Or";
                            break;
                        case Conjunction.OrShortCircuit:
                            code = "Orelse";
                            break;
                    }
                    break;
            }

            return code;
        }


        public string If(int Indent, List<IfArgument> Arguments, string Contents)
        {
            string code = string.Empty;
            string end = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = new string(' ', Indent) + "if (";

                    foreach (IfArgument Arg in Arguments)
                    {
                        if (Arg.Not)
                            code += "!(";
                        code += Arg.Expression;
                        if (Arg.Test != Comparison.None)
                        {
                            code += " " + GetComparison(Arg.Test);
                            if (Arg.Value != string.Empty)
                                code += " " + Arg.Value;
                        }
                        if (Arg.Not)
                            code += ")";
                        code += " ";
                        if (Arg.NextExpression != Conjunction.None)
                            code += GetConjunction(Arg.NextExpression) + " ";
                    }

                    //-1 to remove the last space
                    code = code.Substring(0, code.Length - 1) + ")" + Environment.NewLine;
                    code += new string(' ', Indent) + "{" + Environment.NewLine;
                    code += Contents;
                    code += new string(' ', Indent) + "}" + Environment.NewLine;
                    break;

                case LanguageType.VB:
                    code = new string(' ', Indent) + "If ";

                    foreach (IfArgument Arg in Arguments)
                    {
                        //null is a special case
                        if (Arg.Value != "Nothing")
                        {
                            if (Arg.Not)
                                code += "Not (";
                            if (Arg.Test != Comparison.None)
                                if (Arg.Test != Comparison.Is)
                                    code += Arg.Expression + " " + GetComparison(Arg.Test) + " " + Arg.Value + " ";
                                else
                                    code += string.Format(GetComparison(Arg.Test), Arg.Expression) + " " + Arg.Value + " ";
                            else
                                code += Arg.Expression + "  ";
                            if (Arg.Not)
                                code += ") ";
                        }
                        else
                        {
                            code += Arg.Expression + " ";
                            //if ((Arg.Not & Arg.Test == Comparison.Equal) | (Arg.Not & Arg.Test == Comparison.Is) | Arg.Test == Comparison.NotEqual)
                            if ((Arg.Not & Arg.Test == Comparison.Equal) | Arg.Test == Comparison.NotEqual)
                                code += "Isnot Nothing ";
                            else
                                code += "Is Nothing ";
                        }
                        if (Arg.NextExpression != Conjunction.None)
                            code += GetConjunction(Arg.NextExpression) + " ";
                    }

                    code += "Then" + Environment.NewLine;
                    code += Contents;
                    code += new string(' ', Indent) + "End If" + Environment.NewLine;
                    break;

            }

            return code;
        }

        //public string AddLibrary(string Library)
        //{
        //    string code = string.Empty;


        //    return code;
        //}

        public string Namespace(string Name, string Contents)
        {
            string code = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = "namespace " + Name + Environment.NewLine;
                    code += "{" + Environment.NewLine + Contents + Environment.NewLine + "}";
                    break;
                case LanguageType.VB:
                    code = "Namespace " + Name + Environment.NewLine;
                    code += Environment.NewLine + Contents + Environment.NewLine + Environment.NewLine + "End Namespace";
                    break;
            }
            return code;
        }

        public string Not()
        {
            string code = string.Empty;

            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = "!";
                    break;
                case LanguageType.VB:
                    code = "Not ";
                    break;
            }

            return code;
        }
        public string Null()
        {
            string code = string.Empty;

            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = "null";
                    break;
                case LanguageType.VB:
                    code = "Nothing";
                    break;
            }

            return code;
        }

        /// <summary>
        /// *
        /// </summary>
        /// <param name="Contents"></param>
        /// <returns></returns>
        public string PathBackslash()
        {
            string code = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = "\\\\";
                    break;
                case LanguageType.VB:
                    code = "\\";
                    break;
            }

            return code;
        }

        public string ParameterDefinition(Parameter Param)
        {
            return ParameterDefinitionWork(Param.Name, Param.Type, Param.PassBy);
        }
        public string ParameterDefinition(string Name, VariableTypes vt, PassingType By)
        {
            return ParameterDefinitionWork(Name, DataType(vt), By);
        }
        public string ParameterDefinition(string Name, string vt, PassingType By)
        {
            return ParameterDefinitionWork(Name, vt, By);
            //string code = string.Empty;
            //string PassedBy = string.Empty;

            //switch (_Language)
            //{
            //    case LanguageType.CSharp:
            //        if (By == PassingType.Reference)
            //            PassedBy = "ref ";
            //        code = PassedBy + vt + " " + Name;
            //        break;
            //    case LanguageType.VB:
            //        switch (By)
            //        {
            //            case PassingType.Reference:
            //                PassedBy = "Byref ";
            //                break;
            //            case PassingType.Value:
            //                PassedBy = "Byval ";
            //                break;
            //        }
            //        code = PassedBy + Name + " As " + vt;
            //        break;
            //}

            //return code;
        }

        public string ParameterPassing(string Name, PassingType By)
        {
            string code = string.Empty;
            string PassedBy = string.Empty;

            switch (_Language)
            {
                case LanguageType.CSharp:
                    switch (By)
                    {
                        case PassingType.Out:
                            PassedBy = "out ";
                            break;
                        case PassingType.Reference:
                            PassedBy = "ref ";
                            break;
                    }
                    code = PassedBy + Name;
                    break;
                case LanguageType.VB:
                    code = Name;
                    break;
            }

            return code;
        }

        public string Self()
        {
            string code = string.Empty;

            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = "this";
                    break;
                case LanguageType.VB:
                    code = "Me";
                    break;
            }

            return code;
        }

        public string Switch(int Indent, string Expression, List<SwitchCase> lisCases)
        {
            string begin = string.Empty;
            string breakKeyword = string.Empty;
            string caseSuffix = string.Empty;
            string code = string.Empty;
            string end = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    begin = new string(' ', Indent) + "switch (" + Expression + ")" + Environment.NewLine;
                    begin += new string(' ', Indent) + "{" + Environment.NewLine;
                    breakKeyword = new string(' ', Indent + _Indent * 2) + "break;" + Environment.NewLine;
                    caseSuffix = ":";
                    end = new string(' ', Indent) + "}" + Environment.NewLine;
                    break;
                case LanguageType.VB:
                    begin = new string(' ', Indent) + "Select Case " + Expression + Environment.NewLine + Environment.NewLine;
                    end = new string(' ', Indent) + "End Select" + Environment.NewLine;
                    break;
            }

            code = begin;
            foreach (SwitchCase sw in lisCases)
            {
                code += new string(' ', Indent + _Indent) + "case " + sw.Case + caseSuffix + Environment.NewLine;
                if (sw.Code != string.Empty)
                    code += new string(' ', Indent + _Indent * 2) + sw.Code + breakKeyword;
            }
            code += end;

            return code;
        }

        public string TryCatch(int Indent, string Try, string Catch)
        {
            return TryCatch(Indent, Try, Catch, string.Empty);
        }
        public string TryCatch(int Indent, string Try, string Catch, string Finally)
        {
            string code = string.Empty;
            string indent;


            indent = new string(' ', Indent);
            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = indent + "try" + Environment.NewLine;
                    code += indent + "{" + Environment.NewLine;
                    code += indent + new string(' ', _Indent) + Try;
                    code += indent + "}" + Environment.NewLine + Environment.NewLine;
                    if (Catch != string.Empty)
                    {
                        code += indent + "catch (System.Exception ex)" + Environment.NewLine;
                        code += indent + "{" + Environment.NewLine;
                        code += indent + new string(' ', _Indent) + Catch;
                        code += indent + "}" + Environment.NewLine + Environment.NewLine;
                    }
                    if (Finally != string.Empty)
                    {
                        code += indent + "finally" + Environment.NewLine;
                        code += indent + "{" + Environment.NewLine;
                        code += indent + new string(' ', _Indent) + Finally;
                        code += indent + "}" + Environment.NewLine + Environment.NewLine;
                    }
                    break;
                case LanguageType.VB:
                    code = indent + "Try" + Environment.NewLine;
                    code += Environment.NewLine;
                    code += indent + new string(' ', _Indent) + Try;
                    code += Environment.NewLine;
                    code += Environment.NewLine;
                    if (Catch != string.Empty)
                    {
                        code += indent + "Catch ex As System.Exception" + Environment.NewLine;
                        code += Environment.NewLine;
                        code += indent + new string(' ', _Indent) + Catch;
                        code += Environment.NewLine;
                        code += Environment.NewLine;
                    }
                    if (Finally != string.Empty)
                    {
                        code += indent + "Finally" + Environment.NewLine;
                        code += Environment.NewLine;
                        code += indent + new string(' ', _Indent) + Finally;
                        code += Environment.NewLine;
                    }
                        code += indent + "End Try";
                        code += Environment.NewLine;
                    break;
            }

            return code;
        }

        public string Type(string Expression)
        {
            string code = string.Empty;


            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = "typeof(" + Expression + ")";
                    break;
                case LanguageType.VB:
                    code = "Gettype(" + Expression + ")";
                    break;
            }

            return code;
        }

        public string VariableDefinition(string Name, VariableTypes vt)
        {
            return VariableDefinition(Scope.Private, Name, DataType(vt), string.Empty);
        }
        public string VariableDefinition(string Name, string vt)
        {
            return VariableDefinition(Scope.Private, Name, vt, string.Empty);
        }
        public string VariableDefinition(string Name, VariableTypes vt, string InitialValue)
        {
            return VariableDefinition(Scope.Private, Name, DataType(vt), InitialValue);
        }
        /// <summary>
        /// Used at class level.  Does NOT append an EOL.
        /// </summary>
        /// <param name="Access"></param>
        /// <param name="Name"></param>
        /// <param name="vt"></param>
        /// <param name="InitialValue"></param>
        /// <returns></returns>
        public string VariableDefinition(Scope Access, string Name, string vt, string InitialValue)
        {
            string code = string.Empty;


            if (InitialValue != string.Empty)
                InitialValue = " = " + InitialValue;

            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = Access.ToString().ToLower() + " " + vt + " " + Name + InitialValue;
                    break;
                case LanguageType.VB:
                    code = Access.ToString() + " " + Name + " As " + vt + InitialValue;
                    break;
            }

            return code;
        }

        public string XMLComments()
        {
            string code = string.Empty;

            switch (_Language)
            {
                case LanguageType.CSharp:
                    code = "///";
                    break;
                case LanguageType.VB:
                    code = "'''";
                    break;
            }

            return code;
        }
    }
}
