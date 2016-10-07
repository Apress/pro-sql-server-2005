using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(
    Format.UserDefined,
    IsByteOrdered = false,
    IsFixedLength = false,
    MaxByteSize = 8000)]
public struct StringArray : INullable, IBinarySerialize
{
    public override string ToString()
    {
        // Replace the followng code with your code
        if (this.IsNull)
            return "";
        else
            return String.Join(",", (string[])this.arr.ToArray());
    }

    public bool IsNull
    {
        get
        {
            return (this.arr == null);
        }
    }

    public static StringArray Null
    {
        get
        {
            StringArray h = new StringArray();
            return h;
        }
    }

    public static StringArray Parse(SqlString s)
    {
        if (s.IsNull)
            return Null;

        StringArray u = new StringArray();

        string[] strings = ((string)s).Split(',');

        for (int i = 0; i < strings.Length; i++)
        {
            strings[i] = strings[i].Trim();
        }

        u.arr = new List<string>(strings.Length);

        foreach (string str in strings)
        {
            if (str != "")
                u.arr.Add(str);
        }

        return u;
    }

    public SqlInt32 Count
    {
        get
        {
            if (this.IsNull)
                return SqlInt32.Null;
            else
                return (SqlInt32)(this.arr.Count);
        }
    }

    public SqlString GetAt(int Index)
    {
        return (SqlString)(string)(this.arr[Index]);
    }

    public StringArray AddString(SqlString str)
    {
        if (this.IsNull)
            this.arr = new List<string>(1);

        this.arr.Add((string)str);

        return (this);
    }

    public StringArray RemoveAt(int Index)
    {
        this.arr.RemoveAt(Index);
        return this;
    }

    // The actual array
    private List<string> arr;

    #region IBinarySerialize Members

    public void Read(System.IO.BinaryReader r)
    {
        int count = r.ReadInt32();
        if (count > -1)
        {
            this.arr = new List<string>(count);

            for (int i = 0; i < count; i++)
            {
                this.arr.Add(r.ReadString());
            }
        }
    }

    public void Write(System.IO.BinaryWriter w)
    {
        if (this.IsNull)
        {
            w.Write(-1);
        }
        else
        {
            w.Write(this.arr.Count);

            foreach (string str in this.arr)
            {
                w.Write(str);
            }
        }
    }

    #endregion
}
