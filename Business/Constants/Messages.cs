using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public static class Messages
    {
        public static string CustomerAdded = "Customer added succesfully";
        public static string CustomerDeleted = "Customer deleted succesfully";
        public static string CustomerUpdated= "Customer updated succesfully";
        public static string CustomerNotFound = "Customer Not Found";

        public static string ProjectAdded = "Project added succesfully";
        public static string ProjectDeleted = "Project deleted succesfully";
        public static string ProjectUpdated = "Project updated succesfully";
        public static string ProjectNotFound = "Project Not Found"; 

        public static string TaskAdded = "Task added succesfully";
        public static string TaskDeleted = "Task deleted succesfully";
        public static string TaskUpdated = "Task updated succesfully";
        public static string TaskNotFound = "Task Not Found";

        public static string UserNotFound = "User Not Found";
        public static string PasswordError = "Wrong Password";
        public static string SuccessfulLogin = "Successful Login";
        public static string SuccessfulRegistered = "Successful Registered";
        public static string UserAlreadyExist = "User Already Exist";
    }
}
