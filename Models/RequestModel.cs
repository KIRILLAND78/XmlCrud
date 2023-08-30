namespace WebApplication2.Models;

public class RequestModel
{
    public int ReportId { get; set; }
    public string? DataBase { get; set; }
    public string? Server { get; set; }
    public string? UserName { get; set; }
    public string? UserPassword { get; set; }
    public int? DataSourceType { get; set; }
    public string? FileQueryColName { get; set; }
    public string? FileQueryOutputColName { get; set; }
    public string? FileQueryTempPath { get; set; }
    public bool? UsePivotData { get; set; }
    public string? PivotCol { get; set; }
    public string? PivotCols { get; set; }
    public string? PivotRows { get; set; }
    public string? PivotData { get; set; }
    public string? DefaultComplex { get; set; }
}
