using WADemo.Core;
using WADemo.Core.Interfaces;
using WeatherAlmanac.Core;

namespace WADemo.DAL;

public class CsvRecordRepository : IRecordRepository
{
  private readonly string _fileName;
  private readonly List<WeatherRecord> _records;
  private readonly ILogger _logger;
  public CsvRecordRepository(string fileName, ILogger logger)
  {
    _logger = logger;
    _records = new List<WeatherRecord>();
    _fileName = fileName;
    Init();
  }

  public Result<List<WeatherRecord>> Index()
  {
    return new Result<List<WeatherRecord>> {IsSuccess = true, Data = _records};
  }

  public Result<WeatherRecord> Add(WeatherRecord record)
  {
    _records.Add(record);
    SaveAllRecords2File();
    return new Result<WeatherRecord> {IsSuccess = true, Data = record};
  }

  public Result<WeatherRecord> Update(WeatherRecord record2Update)
  {
    var result = new Result<WeatherRecord>();

    for (var i = 0; i < _records.Count; i++)
    {
      if (_records[i].Date != record2Update.Date)
      {
        continue;
      }

      _records[i] = record2Update;
      SaveAllRecords2File();
      result.IsSuccess = true;
      result.Data = record2Update;
      return result;
    }

    result.IsSuccess = false;
    result.Message = "Record not found";
    return result;
  }

  public Result<WeatherRecord> Delete(DateOnly date)
  {
    var result = new Result<WeatherRecord>();

    foreach (var record in _records.Where(record => record.Date == date))
    {
      _records.Remove(record);
      SaveAllRecords2File();
      result.IsSuccess = true;
      result.Data = record;
      return result;
    }

    result.IsSuccess = false;
    result.Message = "Record not found";
    return result;
  }

  private void Init()
  {
    if (!File.Exists(_fileName))
    {
      File.Create(_fileName).Close();
      return;
    }

    using var sr = new StreamReader(_fileName);
    string? row;

    // Skip the header line
    sr.ReadLine();

    while ((row = sr.ReadLine()) != null)
    {
      _records.Add(Deserialize(row));
    }
  }

  private static WeatherRecord Deserialize(string row)
  {
    var record = new WeatherRecord();
    var values = row.Split(',');
    record.Date = DateOnly.Parse(values[0]);
    record.HighTemp = int.Parse(values[1]);
    record.LowTemp = int.Parse(values[2]);
    record.Humidity = int.Parse(values[3]);

    // Keep the Description at the end so that extra commas won't matter. Skip first 4 and join the rest.
    record.Description = String.Join(", ", values.Skip(4));

    return record;
  }

  private void SaveAllRecords2File()
  {
    using var sw = new StreamWriter(_fileName);

    // Write the header line
    sw.WriteLine("Date,HighTemp,LowTemp,Humidity,Description");

    foreach (var record in _records)
    {
      sw.WriteLine(
        $"{record.Date},{record.HighTemp},{record.LowTemp},{record.Humidity},{record.Description}");
    }
  }
}
