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