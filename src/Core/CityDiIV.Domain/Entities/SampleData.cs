using CityDiIV.Domain.Entities.Common;

namespace CityDiIV.Domain.Entities;

public class SampleData : EntityBase
{
    public int SampleInt { get; set; }
    public decimal SampleDecimal { get; set; }
    public string SampleString { get; set; }
    public bool SampleBool { get; set; }
}