using System.Collections.Generic;
using Questore.Data.Models;

namespace Questore.Data.Interfaces
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