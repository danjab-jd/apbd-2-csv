using System.Text.Encodings.Web;
using System.Text.Json;
using University.Models;

namespace University;

public class Program
{
    private static readonly string WorkingDir = Environment.CurrentDirectory;
    private static readonly string ProjectDir = Directory.GetParent(WorkingDir).Parent.Parent.FullName;
    private static int _numOfValidElements = 9;
    
    public static async Task Main(string[] args)
    {
        string path = args[0];
        string resultPath = args[1];
        string logsPath = @$"{ProjectDir}\Logs\logs.txt";

        FileInfo fileInfo = new(path);
        HashSet<Student> studentSet = new();

        await using StreamWriter streamWriter = new(logsPath);
        using StreamReader streamReader = new(fileInfo.OpenRead());

        string line = null;
        while ((line = await streamReader.ReadLineAsync()) != null)
        {
            string[] studentData = line.Split(",");

            if (studentData.Length != _numOfValidElements)
            {
                await AppendLine(streamWriter, "Niewystarczająca liczba elementów tablicy opisujących studenta!");
                continue;
            }
                    
            if (IsStudentDataValid(studentData))
            {
                Student student = CreateStudent(studentData);
                bool result = studentSet.Add(student);
                if (!result)
                {
                    await AppendLine(streamWriter, $"Duplikat studenta o indeksie {studentData[4]}");
                }
            }
            else
            {
                await AppendLine(streamWriter, $"Jedna z wartości opisujących studenta jest nieprawidłowa!");
            }
        }
        
        string json = ParseToJson(studentSet);
        await using StreamWriter resultStreamWriter = new(resultPath);
        await AppendLine(resultStreamWriter, json);
    }

    private static bool IsStudentDataValid(string[] elements)
    {
        foreach (string str in elements)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }
        }
        
        return true;
    }

    private static Student CreateStudent(string[] elements)
    {
        return new Student
        {
            Name = elements[0],
            Surname = elements[1],
            Study = new()
            {
                Name = elements[2],
                Mode = elements[3]
            },
            Index = int.Parse(elements[4]),
            BirthDate = DateTime.Parse(elements[5]),
            Email = elements[6],
            MotherName = elements[7],
            FatherName = elements[8]
        };
    }

    private static string ParseToJson(HashSet<Student> students)
    {
        JsonResult resToParse = new()
        {
            CreatedAt = DateTime.Now,
            Author = "Daniel Jabłoński",
            Students = students
        };

        JsonSerializerOptions jsonSerializerOptions = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        return JsonSerializer.Serialize(resToParse, jsonSerializerOptions);
    }

    private static async Task AppendLine(StreamWriter streamWriter, string line)
    {
        await streamWriter.WriteLineAsync(line);
    }
}