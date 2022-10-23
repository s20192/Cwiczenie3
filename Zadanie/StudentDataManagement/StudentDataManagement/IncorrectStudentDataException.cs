namespace StudentDataManagement
{
    public class IncorrectStudentDataException: Exception
    {
        public IncorrectStudentDataException() { }
        public IncorrectStudentDataException(string message) : base(message) { }
        public IncorrectStudentDataException(string message, Exception inner) : base(message, inner) { }
    }
}
