using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CodeGen_Demo
{
    public class Enumerations
    {

    public enum FieldTypes
    {
        Unknown,
        [TypeDescription("Exact Numeric")] 
        bigint,
        //[TypeDescription("Binary")] 
        //binary,
        [TypeDescription("Exact Numeric")] 
        bit,
        [TypeDescription("Character String")] 
        @char,
        //[TypeDescription("Date and Time")] 
        //date,
        //[TypeDescription("Date and Time")] 
        //datetimeoffset,
        [TypeDescription("Date and Time")] 
        datetime,
        //[TypeDescription("Date and Time")] 
        //datetime2,
        [TypeDescription("Exact Numeric")] 
        @decimal,
        [TypeDescription("Approximate Numeric")] 
        @float,
        //[TypeDescription("Binary")] 
        //image,
        [TypeDescription("Exact Numeric")] 
        @int,
        [TypeDescription("Exact Numeric")] 
        money,
        [TypeDescription("Unicode Character String")] 
        nchar,
        [TypeDescription("Unicode Character String")] 
        nvarchar,
        [TypeDescription("Unicode Character String")] 
        ntext,
        [TypeDescription("Exact Numeric")] 
        numeric,
        [TypeDescription("Approximate Numeric")] 
        real,
        //[TypeDescription("Date and Time")] 
        //smalldatetime,
        [TypeDescription("Exact Numeric")] 
        smallint,
        [TypeDescription("Exact Numeric")] 
        smallmoney,
        //[TypeDescription("Custom")] 
        //sqlvariant,
        [TypeDescription("Character String")] 
        text,
        //[TypeDescription("Date and Time")] 
        //time,
        [TypeDescription("Exact Numeric")] 
        tinyint,
        //[TypeDescription("System Numeric")] 
        //timestamp,
        //[TypeDescription("Binary")] 
        //varbinary,
        [TypeDescription("Character String")] 
        varchar,
        [TypeDescription("Binary")] 
        uniqueidentifier,
        [TypeDescription("Character String")] 
        xml,
    }


    public static string GetFieldTypeDescription(Enum e)
    {
        object[] Attributes;
        System.Reflection.MemberInfo[] MemberInfo;


        MemberInfo = typeof(Enumerations.FieldTypes).GetMember(e.ToString());
        Attributes = MemberInfo[0].GetCustomAttributes(typeof(TypeDescription), false);
        if (Attributes.GetUpperBound(0) > -1)
            return ((TypeDescription)Attributes[0]).Description;
        else
            return string.Empty;

    }

}
}
