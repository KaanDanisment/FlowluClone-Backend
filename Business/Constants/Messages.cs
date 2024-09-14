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

        public static string AccessTokenCreted = "Access Token Created Successfully";

        public static string TeamUpdated = "Team Updated Succesfully";
        public static string TeamNotFound = "User don't have a Team";
        public static string TeamDeleted = "Team Deleted Successfuly";
        public static string ExistingUser = "This User already in this Team";
        public static string UserHaveATeam = "This User already have a Team";
        public static string UsersAdded = "User Added Team Successfully";
        public static string UserNotFoundTeam = "User is not in This Team";
        public static string UserRemoved = "User Removed from Team Successfully";
        public static string UserAlreadyInAnotherTeam = "This User Already In Another Team";
        public static string RoleNotFound = "Role Not Found";
    }
}
