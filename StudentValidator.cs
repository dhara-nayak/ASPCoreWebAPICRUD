using ASPCoreWebAPICRUD.Models;
using FluentValidation;

namespace ASPCoreWebAPICRUD
{
    public class StudentValidator : AbstractValidator<Student>
    {
        public StudentValidator()
        {
            RuleFor(Student => Student.StudentName).NotEmpty().WithMessage("Student Name is Required");

            RuleFor(Student => Student.StudentGender).NotEmpty().WithMessage("Gender is required").MaximumLength(10).WithMessage("Gender must not exceed 10 characters");

            RuleFor(student => student.Age).InclusiveBetween(5, 100).WithMessage("Age must be between 1 and 100");

            RuleFor(student => student.Standard).NotEmpty().WithMessage("Standard is required");

            RuleFor(student => student.FatherName).NotEmpty().WithMessage("Father's Name is required").MaximumLength(20).WithMessage("Father's Name must not exceed 20 characters");

        }
    }   
}
