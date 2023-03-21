﻿namespace University.Models;

public class Student
{
    public string Name { get; set; }

    public string Surname { get; set; }
    
    public Study Study { get; set; }
    
    public int Index { get; set; }
    
    public DateTime BirthDate { get; set; }
    
    public string Email { get; set; }
    
    public string MotherName { get; set; }
    
    public string FatherName { get; set; }
}