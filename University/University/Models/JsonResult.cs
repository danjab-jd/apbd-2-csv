namespace University.Models;

public class JsonResult
{
    public DateTime CreatedAt { get; set; }
    
    public string Author { get; set; }
    
    public HashSet<Student> Students { get; set; }
}