using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Text;
using System.Text.RegularExpressions;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.UserDefined,
IsByteOrdered = true,
IsFixedLength = false,
MaxByteSize = 11)]
public struct PhoneNumber : INullable, IBinarySerialize
{
    public override string ToString()
    {
        return this.number;
    }

    public bool IsNull
    {
        get
        {
            if (this.number == "")
                return true;
            else
                return false;
        }
    }

    public static PhoneNumber Null
    {
        get
        {
            PhoneNumber h = new PhoneNumber();
            h.number = "";
            return h;
        }
    }

    public static PhoneNumber Parse(SqlString s)
    {
        if (s.IsNull)
            return Null;
        PhoneNumber u = new PhoneNumber();

        //Call the Number property for assigning the value
        u.Number = s;
        return u;
    }

    // Public mutator for the number
    public SqlString Number
    {
        get
        {
            return new SqlString(this.number);
        }
        set
        {
            //If null, don't process any further
            if (value == "")
            {
                this.number = "";
                return;
            }

            //Match groups of 1 or more digits
            Regex regex = new Regex("[0-9]*");
            MatchCollection matches = regex.Matches((string)value);

            StringBuilder result = new StringBuilder();

            foreach (Match match in matches)
            {
                result.Append(match.Value);
            }

            if (result.Length == 10)
                this.number = result.ToString();
            else
                throw new ArgumentException("Phone numbers must be 10 digits.");
        }
    }

    // The phone number
    private string number;

    #region IBinarySerialize Members

    public void Read(System.IO.BinaryReader r)
    {
        this.number = r.ReadString();
    }

    public void Write(System.IO.BinaryWriter w)
    {
        w.Write(number);
    }

    #endregion
}