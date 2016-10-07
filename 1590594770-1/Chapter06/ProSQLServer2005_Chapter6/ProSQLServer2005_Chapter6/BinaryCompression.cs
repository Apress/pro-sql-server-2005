using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.IO;
using System.IO.Compression;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlBytes BinaryCompress(SqlBytes inputStream)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (GZipStream x =
                new GZipStream(ms, CompressionMode.Compress, true))
            {
                byte[] inputBytes = (byte[])inputStream.Value;
                x.Write(inputBytes, 0, inputBytes.Length);
            }

            return (new SqlBytes(ms.ToArray()));
        }
    }


    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlBytes BinaryDecompress(SqlBytes inputBinary)
    {
        byte[] inputBytes = (byte[])inputBinary.Value;

        using (MemoryStream memStreamIn = new MemoryStream(inputBytes))
        {
            using (GZipStream s =
                new GZipStream(memStreamIn, CompressionMode.Decompress))
            {
                using (MemoryStream memStreamOut = new MemoryStream())
                {
                    for (int num = s.ReadByte(); num != -1; num = s.ReadByte())
                    {
                        memStreamOut.WriteByte((byte)num);
                    }

                    return (new SqlBytes(memStreamOut.ToArray()));
                }
            }
        }
    }

};

