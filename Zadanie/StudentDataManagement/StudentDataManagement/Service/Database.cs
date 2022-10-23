using StudentDataManagement.Model;

namespace StudentDataManagement.Service
{
    enum StudentColumns
    {
        FirstName=0,
        LastName=1,
        IndexNumber = 2,
        DateBirth=3,
        StudiesName=4,
        StudiesMode=5,
        Email=6,
        FathersName=7,
        MothersName=8
    }
    public class Database
    {
        private const string DbPath = @"Data\studenci.csv";
        private static StreamReader GetDatabaseStreamReader()
        {
            try
            {
                FileInfo fileInfo = new(DbPath);
                StreamReader stream = new(fileInfo.OpenRead());
                return stream;
            }
            catch (DirectoryNotFoundException e)
            {
                throw new ArgumentException("Baza danych nie została znaleziona: " + e.Message);
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException("Podany plik nie istnieje" + e.Message);
            }

        }

        private static string[] ValidateStudentData(string line)
        {
            string[] arr = line.Split(',');
            string[] fieldsName = {StudentColumns.FirstName.ToString(),
                               StudentColumns.LastName.ToString(),
                               StudentColumns.IndexNumber.ToString(),
                               StudentColumns.DateBirth.ToString(),
                               StudentColumns.StudiesName.ToString(),
                               StudentColumns.StudiesMode.ToString(),
                               StudentColumns.Email.ToString(),
                               StudentColumns.FathersName.ToString(),
                               StudentColumns.MothersName.ToString()};

            if (arr.Length != 9)
            {
                throw new IncorrectStudentDataException($"Nieprawidłowe dane u studenta: '{line}'");
            }

            for (int i = 0; i < arr.Length; i++)
            {
                if (String.IsNullOrEmpty(arr[i]))
                {
                    throw new IncorrectStudentDataException($"Brakujące dane u studenta w polu {fieldsName[i]}: '{line}'");

                }
            }

            return arr;
        }

        private static Student ParseStudent(string line)
        {
            string[] arr = ValidateStudentData(line);

            Studies studies = new(arr[(int)StudentColumns.StudiesName], arr[(int)StudentColumns.StudiesMode]);
            Student st = new(arr[(int)StudentColumns.FirstName],
                             arr[(int)StudentColumns.LastName],
                             arr[(int)StudentColumns.IndexNumber],
                             arr[(int)StudentColumns.DateBirth],
                             studies,
                             arr[(int)StudentColumns.Email],
                             arr[(int)StudentColumns.FathersName],
                             arr[(int)StudentColumns.MothersName]);

            return st;
        }

        private static string ConvertStudentToLine(Student st)
        {
            string[] arr = new string[9];
            arr[0] = st.FName;
            arr[1] = st.LName;
            arr[2] = st.IndexNumber;
            arr[3] = st.DateBirth;
            arr[4] = st.Studies.Name;
            arr[5] = st.Studies.Mode;
            arr[6] = st.Email;
            arr[7] = st.FathersName;
            arr[8] = st.MothersName;
            var newLine = String.Join(",", arr);
            return newLine;
        }
        public static List<Student> GetStudents()
        {
            List<Student> students = new();

            using(StreamReader reader = GetDatabaseStreamReader())
            {
                string? line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        Student student = ParseStudent(line);
                        if(!students.Contains(student))
                        {
                            students.Add(student);
                        }  
                    } catch(IncorrectStudentDataException e)
                    {
                        Console.WriteLine(e.Message);
                        continue;
                    }                     
                }
            }

            return students;
        }

        public static Student? GetStudent(string indexNum)
        {
            using(StreamReader reader = GetDatabaseStreamReader())
            {
                string? line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    Student student = ParseStudent(line);
                    if(student.IndexNumber == indexNum)
                    {
                        return student;
                    }
                }
            }

            return null;
        }

        public static Student? UpdateStudent(string indexNum, Student st)
        {
            DeleteStudent(indexNum);
            CreateStudent(st);

            return st;
        }

        public static Student? CreateStudent(Student st)
        {
            foreach(Student student in GetStudents())
            {
                if(student.IndexNumber==st.IndexNumber)
                {
                    throw new IncorrectStudentDataException($"Student o podanym indeksie istnieje już w bazie: '{st.IndexNumber}'");
                }
            }                                  

            List<string> lines = new List<string>();
            lines.Add(ConvertStudentToLine(st));

;            File.AppendAllLines(@"Data\studenci.csv", lines);

            return st;
        }

        public static void DeleteStudent(string indexNumber)
        {
            if(GetStudent(indexNumber)==null)
            {
                throw new IncorrectStudentDataException($"Nie ma w bazie studenta o podanym indeksie: '{indexNumber}'");
            }

            List<string> lines = new List<string>();
            using (StreamReader reader = GetDatabaseStreamReader()) 
            {
                string? line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    Student currentStudent = ParseStudent(line);
                    if(!(currentStudent.IndexNumber == indexNumber))
                    {
                        lines.Add(line);
                    }
                }
            }

            File.WriteAllLines(@"Data\studenci.csv", lines);
        }
    }
}
