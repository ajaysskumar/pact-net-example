using System;
using System.Collections.Generic;
using System.Linq;

namespace PactNet.Provider.Repository;

public interface IStudentRepo
{
    Student GetStudentById(int id);
    void AddStudent(Student student);
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
                Id = 100 + i,
                Address = $"{i} Block, 12 {1} Road, Delhi, India",
                Gender = DateTime.Now.Microsecond % 2 == 0? "Male": "Female",
                FirstName = $"FName {i}",
                LastName = "LName"
            });
        }
    }
    
    public Student GetStudentById(int id)
    {
        return _students.FirstOrDefault(x => x.Id == id);
    }

    public void AddStudent(Student student)
    {
        _students.Add(student);
    }
}