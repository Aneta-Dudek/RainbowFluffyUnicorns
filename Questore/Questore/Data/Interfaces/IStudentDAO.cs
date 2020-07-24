using Questore.Data.Models;
using System.Collections.Generic;

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