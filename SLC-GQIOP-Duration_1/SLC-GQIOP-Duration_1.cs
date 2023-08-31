using System;
using Skyline.DataMiner.Analytics.GenericInterface;

[GQIMetaData(Name = "Calculate Duration")]
public class MyCustomOperator : IGQIColumnOperator, IGQIRowOperator, IGQIInputArguments
{
    private readonly GQIDoubleColumn _durationColumn = new GQIDoubleColumn("Duration");

    private readonly GQIColumnDropdownArgument _startColumnArg = new GQIColumnDropdownArgument("Start") { IsRequired = true, Types = new GQIColumnType[] { GQIColumnType.DateTime } };
    private readonly GQIColumnDropdownArgument _endColumnArg = new GQIColumnDropdownArgument("End") { IsRequired = true, Types = new GQIColumnType[] { GQIColumnType.DateTime } };

    private GQIColumn _startColumn;
    private GQIColumn _endColumn;

    public GQIArgument[] GetInputArguments()
    {
        return new GQIArgument[] { _startColumnArg, _endColumnArg };
    }

    public OnArgumentsProcessedOutputArgs OnArgumentsProcessed(OnArgumentsProcessedInputArgs args)
    {
        _startColumn = args.GetArgumentValue(_startColumnArg);
        _endColumn = args.GetArgumentValue(_endColumnArg);
        return new OnArgumentsProcessedOutputArgs();
    }

    public void HandleRow(GQIEditableRow row)
    {
        try
        {
            DateTime start;
            DateTime end;

            row.TryGetValue(_startColumn, out start);
            row.TryGetValue(_endColumn, out end);

            double duration = (end - start).TotalHours;
            row.SetValue(_durationColumn, duration, duration.ToString("0.##") + " h");
        }
        catch (Exception)
        {
            // Catch empty cells
        }
    }

    public void HandleColumns(GQIEditableHeader header)
    {
        header.AddColumns(_durationColumn);
    }
}