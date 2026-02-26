using System.Text.Json;

public class JsonScheduleRepository : IScheduleRepository
{
  
    private readonly string _path;
    private readonly JsonSerializerOptions _opts = new() { PropertyNameCaseInsensitive = true };

    public JsonScheduleRepository(string path)
    {
        _path = path;
        Console.WriteLine($"Путь к файлу: {Path.GetFullPath(_path)}");
        Console.WriteLine($"Файл существует: {File.Exists(_path)}");
        if (!File.Exists(_path))
        {
            var sample = new ScheduleFile
            {
                Groups = new List<GroupSchedule>
{
new GroupSchedule
{
Group = "9A",
Days = new List<DaySchedule>
{
new DaySchedule
{
    Day = "Monday",
    Lessons = new List<Lesson>
    {
        new Lesson("09:00","Math","Ivanov"),
        new Lesson("10:00","History","Sizov"),
        new Lesson("11:00","Geography","Kuznetsova"),
        new Lesson("12:00","Russian","Belyayeva"),
        new Lesson("13:00","Chemistry","Goncharova"),
    }
},
new DaySchedule 
{ 
    Day = "Tuesday", Lessons = new List<Lesson> 
    { 
        new Lesson("09:00","Physics","Petrova"),
        new Lesson("10:00","Physical Education","Nikolaev"),
        new Lesson("11:00","Biology","Markova"),
        new Lesson("12:00","Math","Ivanov"),
        new Lesson("13:00","Russian","Belyayeva"),
    } 
},
new DaySchedule 
{ 
    Day = "Wednesday", 
    Lessons = new List<Lesson>
    { 
        new Lesson("09:00", "English", "Voronova"),
        new Lesson("10:00","History","Sizov"),
        new Lesson("11:00","Math","Ivanov"),
        new Lesson("12:00", "Literature", "Belyayeva"),
        new Lesson("13:00", "Russian", "Belyayeva")
    } 
},
new DaySchedule 
{ 
    Day = "Thursday", Lessons = new List<Lesson>() 
    { 
        new Lesson("09:00", "Russian", "Belyayeva"),
        new Lesson("10:00","Biology","Markova"),
        new Lesson("11:00", "English", "Voronova"),
        new Lesson("12:00","Physics","Petrova"),
        new Lesson("13:00", "Literature", "Belyayeva"),
    }
},
new DaySchedule 
{ 
    Day = "Friday", Lessons = new List<Lesson>() 
    { 
        new Lesson("09:00", "Literature", "Belyayeva"),
        new Lesson("10:00","Math","Ivanov"),
        new Lesson("11:00","Physical Education","Nikolaev"),
        new Lesson("12:00","History","Sizov"),
        new Lesson("13:00","Chemistry","Goncharova"),
    }
}
}
}
}
            };
            File.WriteAllText(_path, JsonSerializer.Serialize(sample, new JsonSerializerOptions { WriteIndented = true }));
        }
    }


    public ScheduleFile Load()
    {
        using var s = File.OpenRead(_path);
        return JsonSerializer.Deserialize<ScheduleFile>(s, _opts) ?? new ScheduleFile();
    }
}