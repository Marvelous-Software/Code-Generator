using System;
using System.Text;

namespace CodeGen_Demo
{
    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class TypeDescription : Attribute
    {

        private string _Description;


        public string Description
        {
            get { return _Description; }
        }

        public TypeDescription(string Description)
        {
            _Description = Description;
        }

    }
}
