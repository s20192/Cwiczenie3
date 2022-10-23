
namespace StudentDataManagement.Model
{
    public class Student
    {
        public string FName { get; }
        public string LName { get; }
        public string IndexNumber { get; }
        public string DateBirth { get; }
        public Model.Studies Studies { get; }
        public string Email { get; }
        public string FathersName { get; }
        public string MothersName { get; }

        public Student(string fName, string lName, string indexNumber, string dateBirth, Studies studies, string email, string fathersName, string mothersName)
        {
            IndexNumber = indexNumber;
            if(!ValidateIndex())
            {
                throw new ArgumentException($"Invalid index number of student: {fName} {lName}");
            }
            FName = fName;
            LName = lName;
            DateBirth = dateBirth;
            Email = email;
            MothersName = mothersName;
            FathersName = fathersName;
            Studies = studies;
        }
    
        public bool ValidateIndex()
        {
            if(!IndexNumber.StartsWith("s")) {
                return false;
            }

            for(int i=1; i<IndexNumber.Length; i++)
            {
                if(!Char.IsNumber(IndexNumber[i])) {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object? obj)
        {
            return obj is Student student &&
                   FName == student.FName &&
                   LName == student.LName &&
                   IndexNumber == student.IndexNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FName, LName, IndexNumber);
        }
    }
}
