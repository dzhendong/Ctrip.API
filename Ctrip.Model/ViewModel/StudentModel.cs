using System;

namespace Ctrip.Model
{
    public class StudentModel : StudentBaseModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }
    } 
}
