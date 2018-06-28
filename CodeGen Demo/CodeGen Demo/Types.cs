using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace CodeGen_Demo
{
    public enum LanguageType
    {
        CSharp,
        VB,
    }


    public class ControlInfo
    {
        private FieldInfo _parent;
        private int _left;
        private Point _location;
        private Size _size;
        private int _top;


        public ControlInfo(FieldInfo FI)
        {

            _parent = FI;
            
        }


        public int Left
        {
            get { return _left; }
            set 
            {
                _left = value;
                _location = new Point(_left, _location.Y);
            }
        }

        public Point Location
        {
            get { return _location; }
            set 
            { 
                _location = value;
                _left = _location.X;
                _top = _location.Y;
            }
        }

        public string Name
        {
            get
            {
                string controlname = string.Empty;


                if (!_parent.EmployeeID)
                switch (_parent.Type)
                {
                    case Enumerations.FieldTypes.bit:
                        controlname = "chk" + _parent.IdentifyerName;
                        break;
                    //case Enumerations.FieldTypes.date:
                    case Enumerations.FieldTypes.datetime:
                        //case Enumerations.FieldTypes.datetime2:
                        //case Enumerations.FieldTypes.smalldatetime:
                        controlname = "dtm" + _parent.IdentifyerName;
                        break;
                    default:
                        controlname = "txt" + _parent.IdentifyerName;
                        break;
                }
                else
                    controlname = "lbl" + _parent.IdentifyerName;

                return controlname;
            }
        }

        public Size Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public int Top
        {
            get { return _top; }
            set
            {
                _top = value;
                _location = new Point(_location.X, _top);
            }
        }

        public string Type
        {
            get {
                string type = string.Empty;


                if (!_parent.EmployeeID)
                    switch (_parent.Type)
                    {
                        case Enumerations.FieldTypes.bit:
                    type = "IntuitiveForms.imsCheckBox";
                            break;
                        //case Enumerations.FieldTypes.date:
                        case Enumerations.FieldTypes.datetime:
                            //case Enumerations.FieldTypes.datetime2:
                            //case Enumerations.FieldTypes.smalldatetime:
                            type = "IntuitiveForms.imsDateBox";
                            break;
                        default:
                            type = "IntuitiveForms.imsTextBox";
                            break;
                    }
                else
                    type = "IntuitiveForms.imsLabel";

                return type;
            }
        }

        public string Value
        {
            get {
                string value = string.Empty;


                if (!_parent.EmployeeID)
                    switch (_parent.Type)
                    {
                        case Enumerations.FieldTypes.bit:
                            value = "Checked";
                            break;
                        //case Enumerations.FieldTypes.date:
                        case Enumerations.FieldTypes.datetime:
                            //case Enumerations.FieldTypes.datetime2:
                            //case Enumerations.FieldTypes.smalldatetime:
                            value = "Text";
                            break;
                        default:
                            value = "Text";
                            break;
                    }
                else
                    value = "Text";

                return value;
            }
        }

    }

    public class FieldInfo
    {
        private ControlInfo _Control;
        private object _DefaultValue;
        private bool _EmployeeID;
        private string _Name;
        private string _IdentifyerName;
        private string _LabelCaption;
        private int _LabelLeft;
        private Point _LabelLocation;
        private Size _LabelSize;
        private int _LabelTop;
        private int _StatusIconLeft;
        private Point _StatusIconLocation;
        private Size _StatusIconSize;
        private int _StatusIconTop;
        private Enumerations.FieldTypes _Type;

        public bool LookupKey;
        public bool Nullable;
        public bool PKey;
        public int Precision;
        public string Size;


        public ControlInfo Control
        {
            get { return _Control; }
        }

        public object Default
        {
            get 
            { 
                if (_DefaultValue == null)
                    return string.Empty;
                else
                    return _DefaultValue;
            }
            set { _DefaultValue = value; }
        }

        public string DefaultValue
        {
            get
            {
                string DefaultReturned = string.Empty;


                switch (_Type)
                {
                    case Enumerations.FieldTypes.bigint:
                    case Enumerations.FieldTypes.@decimal:
                    case Enumerations.FieldTypes.@float:
                    case Enumerations.FieldTypes.money:
                    case Enumerations.FieldTypes.@int:
                    case Enumerations.FieldTypes.numeric:
                    case Enumerations.FieldTypes.smallmoney:
                    case Enumerations.FieldTypes.smallint:
                    case Enumerations.FieldTypes.tinyint:
                        DefaultReturned = _DefaultValue.ToString();
                        break;
                    case Enumerations.FieldTypes.@char:
                    case Enumerations.FieldTypes.nchar:
                    case Enumerations.FieldTypes.ntext:
                    case Enumerations.FieldTypes.nvarchar:
                    case Enumerations.FieldTypes.text:
                    case Enumerations.FieldTypes.varchar:
                    case Enumerations.FieldTypes.datetime:
                    case Enumerations.FieldTypes.uniqueidentifier:
                        DefaultReturned = "\"" + _DefaultValue + "\"";
                        break;
                    case Enumerations.FieldTypes.bit:
                        if (_DefaultValue != null && (_DefaultValue.ToString() == "1" || _DefaultValue.ToString().ToLower() == "true"))
                        DefaultReturned = "true";
                        else
                        DefaultReturned = "false";
                        break;
                }
                return DefaultReturned;
            }
        }

        public bool EmployeeID
        {
            get { return _EmployeeID; }
            set { _EmployeeID = value; }
        }

        public bool GiudKey
        {
            get { return PKey & (Type == Enumerations.FieldTypes.uniqueidentifier); }
        }

        public string IdentifyerName
        {
            get { return _IdentifyerName; }
        }

        public bool IsDate
        {
            //get { return (Type == Enumerations.FieldTypes.date || Type == Enumerations.FieldTypes.datetime || Type == Enumerations.FieldTypes.datetime2); }
            get { return (Type == Enumerations.FieldTypes.datetime); }
        }

        public string LabelCaption
        {
            get { return _LabelCaption; }
            set { _LabelCaption = value; }
        }

        public int LabelLeft
        {
            get { return _LabelLeft; }
            set
            {
                _LabelLeft = value;
                _LabelLocation = new Point(_LabelLeft, _LabelLocation.Y);
            }
        }

        public Point LabelLocation
        {
            get { return _LabelLocation; }
            set
            {
                _LabelLocation = value;
                _LabelLeft = _LabelLocation.X;
                _LabelTop = _LabelLocation.Y;
            }
        }

        public string LabelName
        {
            get { 
                if (!_EmployeeID)
                    return "lbl" + _IdentifyerName;
            else
                    return "lbl" + _IdentifyerName + "2";
            }
        }

        public Size LabelSize
        {
            get { return _LabelSize; }
            set { _LabelSize = value; }
        }

        public int LabelTop
        {
            get { return _LabelTop; }
            set
            {
                _LabelTop = value;
                _LabelLocation = new Point(_LabelLocation.X, _LabelTop);
            }
        }

        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                _IdentifyerName = string.Empty;
                foreach (char c in value)
                    if ((c > 64 & c < 93) || (c > 47 & c < 58) || (c > 96 & c < 123) || c == '_')
                        _IdentifyerName += c.ToString();

                // while (_IdentifyerName.Substring(1, 1) < "A" &;
            }
        }

        public int StatusIconLeft
        {
            get { return _StatusIconLeft; }
            set
            {
                _StatusIconLeft = value;
                _StatusIconLocation = new Point(_StatusIconLeft, _StatusIconLocation.Y);
            }
        }

        public Point StatusIconLocation
        {
            get { return _StatusIconLocation; }
            set
            {
                _StatusIconLocation = value;
                _StatusIconLeft = _StatusIconLocation.X;
                _StatusIconTop = _StatusIconLocation.Y;
            }
        }

        public string StatusIconName
        {
            get { return "sti" + _IdentifyerName; }
        }

        public Size StatusIconSize
        {
            get { return _StatusIconSize; }
            set { _StatusIconSize = value; }
        }

        public int StatusIconTop
        {
            get { return _StatusIconTop; }
            set
            {
                _StatusIconTop = value;
                _StatusIconLocation = new Point(_StatusIconLocation.X, _StatusIconTop);
            }
        }

        public Enumerations.FieldTypes Type
        {
            get { return _Type; }
        }

        private FieldInfo()
        {
        }

        public FieldInfo(string NameIn, Enumerations.FieldTypes TypeIn)
        {
            Name = NameIn;
            _LabelCaption = NameIn;
            _Type = TypeIn;
            _Control = new ControlInfo(this);
        }

        public bool CheckValue(object value)
        {
            bool b;
            DateTime dt;
            decimal d;
            float f;
            Guid g;
            Int16 i16;
            Int32 i32;
            Int64 i64;
            bool OK = false;
            byte y;


            switch (_Type)
            {
                case Enumerations.FieldTypes.bigint:
                    OK = Int64.TryParse(value.ToString(), out i64);
                    break;
                case Enumerations.FieldTypes.bit:
                    OK = bool.TryParse(value.ToString(), out b);
                    break;
                case Enumerations.FieldTypes.@char:
                case Enumerations.FieldTypes.nchar:
                case Enumerations.FieldTypes.ntext:
                case Enumerations.FieldTypes.nvarchar:
                case Enumerations.FieldTypes.text:
                case Enumerations.FieldTypes.varchar:
                    OK = value is string;
                    break;
                case Enumerations.FieldTypes.datetime:
                    OK = DateTime.TryParse(value.ToString(), out dt);
                    break;
                case Enumerations.FieldTypes.@decimal:
                case Enumerations.FieldTypes.money:
                case Enumerations.FieldTypes.numeric:
                case Enumerations.FieldTypes.smallmoney:
                    OK = decimal.TryParse(value.ToString(), out d);
                    break;
                case Enumerations.FieldTypes.@float:
                    OK = float.TryParse(value.ToString(), out f);
                    break;
                case Enumerations.FieldTypes.@int:
                    OK = Int32.TryParse(value.ToString(), out i32);
                    break;
                case Enumerations.FieldTypes.smallint:
                    OK = Int16.TryParse(value.ToString(), out i16);
                    break;
                case Enumerations.FieldTypes.tinyint:
                    OK = byte.TryParse(value.ToString(), out y);
                    break;
                case Enumerations.FieldTypes.uniqueidentifier:
                    OK = Guid.TryParse(value.ToString(), out g);
                    break;
            }

            return OK;
        }

        public override string ToString()
        {
            return _Name;
        }
    }

    public class FieldsList : Dictionary<string, FieldInfo>
    {
        private string _LookupKey;


        public FieldInfo EmployeeID
        {
            get
            {
                FieldInfo FI;
                FieldInfo EmployeeIDField = null;


                foreach (KeyValuePair<string, FieldInfo> KV in this)
                {
                    FI = KV.Value;
                    if (FI.EmployeeID)
                    {
                        EmployeeIDField = FI;
                        break;
                    }
                }

                return EmployeeIDField;
            }
        }

        public bool HasDefaults
        {
            get
            {
                FieldInfo FI;
                bool defaults = false;


                foreach (KeyValuePair<string, FieldInfo> KV in this)
                {
                    FI = KV.Value;
                    if (FI.Default.ToString() != string.Empty)
                    {
                        defaults = true;
                        break;
                    }
                }

                return defaults;
            }
        }

        public bool HasEmployeeID
        {
            get
            {
                FieldInfo FI;
                bool EmployeeID = false;


                foreach (KeyValuePair<string, FieldInfo> KV in this)
                {
                    FI = KV.Value;
                    if (FI.EmployeeID)
                    {
                        EmployeeID = true;
                        break;
                    }
                }

                return EmployeeID;
            }
        }

        public bool HasNonGuidPKey
        {
            get
            {
                FieldInfo FI;
                bool nonGuidKey = false;


                foreach (KeyValuePair<string, FieldInfo> KV in this)
                {
                    FI = KV.Value;
                    if (FI.PKey && FI.Type != Enumerations.FieldTypes.uniqueidentifier)
                    {
                        nonGuidKey = true;
                        break;
                    }
                }

                return nonGuidKey;
            }
        }

        public FieldInfo LookupField
        {
            get
            {
                FieldInfo FI;
                FieldInfo Lookup = null;

                foreach (KeyValuePair<string, FieldInfo> KV in this)
                {
                    FI = KV.Value;
                    if (FI.LookupKey)
                    {
                        Lookup = FI;
                        break;
                    }
                }

                return Lookup;
            }

        }

        public string LookupKey
        {
            get { return _LookupKey; }
            set
            {
                FieldInfo FI;


                _LookupKey = value;

                foreach (KeyValuePair<string, FieldInfo> KV in this)
                {
                    FI = KV.Value;
                    if (FI.Name != _LookupKey)
                        FI.LookupKey = false;
                    else
                        FI.LookupKey = true;
                }
            }

        }

        public List<FieldInfo> NonGuidKeyFields
        {
            get
            {
                FieldInfo FI;
                List<FieldInfo> NonGuidPKeys = new List<FieldInfo>();

                foreach (KeyValuePair<string, FieldInfo> KV in this)
                {
                    FI = KV.Value;
                    if (FI.PKey && FI.Type != Enumerations.FieldTypes.uniqueidentifier)
                        NonGuidPKeys.Add(FI);
                }

                return NonGuidPKeys;
            }
        }

        public List<FieldInfo> NonKeyFields
        {
            get
            {
                FieldInfo FI;
                List<FieldInfo> NonPKeys = new List<FieldInfo>();

                foreach (KeyValuePair<string, FieldInfo> KV in this)
                {
                    FI = KV.Value;
                    if (!FI.PKey)
                        NonPKeys.Add(FI);
                }

                return NonPKeys;
            }
        }

        public int PrimaryKeyCount
        {
            get
            {
                int count = 0;
                FieldInfo FI;


                foreach (KeyValuePair<string, FieldInfo> KV in this)
                {
                    FI = KV.Value;
                    if (FI.PKey)
                        count++;
                }

                return count;
            }
        }

        public List<FieldInfo> PrimaryKeys
        {
            get
            {
                FieldInfo FI;
                List<FieldInfo> PKeys = new List<FieldInfo>();

                foreach (KeyValuePair<string, FieldInfo> KV in this)
                {
                    FI = KV.Value;
                    if (FI.PKey)
                        PKeys.Add(FI);
                }

                return PKeys;
            }
        }

        public void SetSizeAndLocation()
        {
            FieldInfo FI;
            int width;
            int y = 20;


            foreach (KeyValuePair<string, FieldInfo> KV in this)
            {
                FI = KV.Value;
                if (!FI.GiudKey)
                {
                    if (FI.Size != string.Empty)
                        if (int.TryParse(FI.Size, out width))
                        {
                            if (width < 5)
                                width = 5;
                            if (width > 100)
                                width = 100;
                        }
                        else
                            width = 5;
                    else
                        if (FI.IsDate)
                            width = 25;
                        else
                            width = 15;

                    FI.Control.Location = new Point(165, y);
                    FI.Control.Size = new Size(5 * width, 20);

                    FI.LabelLocation = new Point(50, y);
                    FI.LabelSize = new Size(100, 20);

                    FI.StatusIconLocation = new Point(25, y);
                    FI.StatusIconSize = new Size(15, 15);

                    y += 25;
                }
            }
        }

        public void SetCaptions()
        {
            FieldInfo FI;


            foreach (KeyValuePair<string, FieldInfo> KV in this)
            {
                FI = KV.Value;
                FI.LabelCaption = FI.Name;
            }
        }
    }

    public class FindTypeField
    {
        public string Caption;
        public bool Description;
        public bool Display;
        public bool ID;
        public bool Key;
        public string Name;
    }

    public class FindType : List<FindTypeField>
    {
        public string Name;
        public string TableName;
    }

    public class FormInfo
    {
        public Control Find;
        public Label KeyInputLabel;
        public TextBox KeyInputTextBox;
        public Label KeyValueLabel1;
        public Label KeyValueLabel2;

        public FormInfo()
        {
            Find = new Control();
            KeyInputLabel = new Label();
            KeyInputTextBox = new TextBox();
            KeyValueLabel1 = new Label();
            KeyValueLabel2 = new Label();

            KeyInputLabel.Location = new Point(16, 10);
            KeyInputLabel.Size = new Size(109, 20);
            KeyInputTextBox.Location = new Point(156, 10);
            KeyInputTextBox.Size = new Size(172, 27);
            Find.Location = new Point(328, 10);
            Find.Size = new Size(20, 20);
            KeyValueLabel1.Location = new Point(56, 10);
            KeyValueLabel1.Size = new Size(123, 25);
            KeyValueLabel2.Location = new Point(196, 10);
            KeyValueLabel2.Size = new Size(832, 25);
            KeyValueLabel2.Text ="< Description >";

        }

    }

    public class RC
    {

        private List<string> _Errors;
        private string _LastError;
        private string _Results;
        private bool _Success;

        public List<string> Error
        {
            get { return _Errors; }
        }

        public string LastError
        {
            get { return _LastError; }
        }

        public string Results
        {
            get { return _Results; }
            set { _Results = value; }
        }

        public bool Success
        {
            get { return _Success; }
            set { _Success = value; }
        }


        public RC()
        {
            _Errors = new List<String>();
        }

        public void AddError(string Error)
        {
            _LastError = Error;
            _Errors.Add(Error);
        }

        public void Merge(RC rc)
        {
            if (_LastError == string.Empty)
                _LastError = rc.LastError;
            if (_Results != string.Empty)
            {
                if (rc.Results != string.Empty)
                    _Results = Environment.NewLine + rc.LastError;
            }
            else
                _Results = rc.Results;
            _Success = _Success | rc.Success;

            foreach (string Error in rc.Error)
                _Errors.Add(Error);
        }

    }
}
