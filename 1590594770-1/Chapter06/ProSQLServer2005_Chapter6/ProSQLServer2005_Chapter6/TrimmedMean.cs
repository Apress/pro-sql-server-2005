using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(Format.Native)]
public struct TrimmedMean
{
    public void Init()
    {
        this.numValues = 0;
        this.totalValue = 0;
        this.minValue = SqlMoney.MaxValue;
        this.maxValue = SqlMoney.MinValue;
    }

    public void Accumulate(SqlMoney Value)
    {
        if (!Value.IsNull)
        {
            this.numValues++;
            this.totalValue += Value;
            if (Value < this.minValue)
                this.minValue = Value;
            if (Value > this.maxValue)
                this.maxValue = Value;
        }
    }

    public void Merge(TrimmedMean Group)
    {
        if (Group.numValues > 0)
        {
            this.numValues += Group.numValues;
            this.totalValue += Group.totalValue;
            if (Group.minValue < this.minValue)
                this.minValue = Group.minValue;
            if (Group.maxValue > this.maxValue)
                this.maxValue = Group.maxValue;
        }
    }

    public SqlMoney Terminate()
    {
        if (this.numValues < 3)
            return (SqlMoney.Null);
        else
        {
            this.numValues -= 2;
            this.totalValue -= this.minValue;
            this.totalValue -= this.maxValue;
            return (this.totalValue / this.numValues);
        }
    }

    private int numValues;
    private SqlMoney totalValue;
    private SqlMoney minValue;
    private SqlMoney maxValue;
}
