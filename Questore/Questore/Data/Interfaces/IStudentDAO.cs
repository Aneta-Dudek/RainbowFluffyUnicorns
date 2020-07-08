using System.Collections.Generic;
using Questore.Models;

namespace Questore.Persistence
{
    public interface IStudentDAO
    {
        IEnumerable<Student> GetStudents();
        Student GetStudent(int id);
        void AddStudent(Student student);
        void UpdateStudent(int id, Student updatedStudent);
        void DeleteStudent(int id);
    }
}