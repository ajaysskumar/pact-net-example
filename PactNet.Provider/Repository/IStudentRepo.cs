using System;
using System.Collections.Generic;
using System.Linq;
using PactNet.Provider.Models;

namespace PactNet.Provider.Repository;

public interface IStudentRepo
{
    Student GetStudentById(string id);
    Student AddStudent(Student student);
}

public class StudentRepo : IStudentRepo
{
    private readonly List<Student> _students;

    public StudentRepo()
    {
        _students = new List<Student>();
        // Add one time dummy student list

        for (int i = 0; i < 10; i++)
        {
            _students.Add(new Student()
            {
                Id = Guid.NewGuid().ToString(),
                Address = $"{i} Block, 12 {1} Road, Delhi, India",
                Gender = DateTime.Now.Microsecond % 2 == 0? "Male": "Female",
                FirstName = $"FName {i}",
                LastName = "LName"
            });
        }
    }
    
    public Student GetStudentById(string id)
    {
        return _students.FirstOrDefault(x => x.Id == id);
    }

    public Student AddStudent(Student student)
    {
        _students.Add(student);
        return student;
    }
}