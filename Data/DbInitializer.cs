using ContosoUniversity.Models;

namespace ContosoUniversity.Data
{
    public class DbInitializer
    {
        public static void Initialize(SchoolContext context) 
        { 
            //teeb kindlaks et andmebaas tehakse, või oleks olemas
            context.Database.EnsureCreated();

            //kui õpilaste tabelis juba on õpilasi, väljub funktsioonist
            if (context.Students.Any())
            {
                return;
            }

            //objekt õpilastega, mis lisatakse siis, kui õpilasi sisestatud ei ole.
            var students = new Student[] 
            {
                new Student{FirstMidName="Nipi",LastName="Tiri",EnrollmentDate=DateTime.Parse("2069-04-20")},
                new Student{FirstMidName="Meredith",LastName="Alonso",EnrollmentDate=DateTime.Parse("2002-09-01")},
                new Student{FirstMidName="Arturo",LastName="Anand",EnrollmentDate=DateTime.Parse("2003-09-01")},
                new Student{FirstMidName="Gytis",LastName="Barzdukas",EnrollmentDate=DateTime.Parse("2002-09-01")},
                new Student{FirstMidName="Yan",LastName="Li",EnrollmentDate=DateTime.Parse("2002-09-01")},
                new Student{FirstMidName="Peggy",LastName="Justice",EnrollmentDate=DateTime.Parse("2001-09-01")},
                new Student{FirstMidName="Laura",LastName="Norman",EnrollmentDate=DateTime.Parse("2003-09-01")},
                new Student{FirstMidName="Nino",LastName="Olivetto",EnrollmentDate=DateTime.Parse("2005-09-01")},
                new Student{FirstMidName="Carson",LastName="Alexander",EnrollmentDate=DateTime.Parse("2005-09-01")},
                new Student{FirstMidName="Trevor",LastName="Metsmaaker",EnrollmentDate=DateTime.Parse("2021-09-01")},
            };

            //iga õpilane lisatakse ükshaaval läbi foreach tsükli
            foreach (Student student in students)
            {
                context.Students.Add(student);
            }
            //ja andmebaasi muudatused salvestatakse
            context.SaveChanges();

            //eelnev struktuur, kuid kursustega: \/
            var courses = new Course[]
            {
                new Course{CourseID=1050,Title="Keemia",Credits=3},
                new Course{CourseID=4022,Title="Mikroökonoomika",Credits=3},
                new Course{CourseID=4041,Title="Mikroökonoomika",Credits=3},
                new Course{CourseID=1045,Title="Calculus",Credits=4},
                new Course{CourseID=3141,Title="Trigonomeetria",Credits=4},
                new Course{CourseID=2021,Title="Kompositsioon",Credits=3},
                new Course{CourseID=2042,Title="Kirjandus",Credits=4},
                new Course{CourseID=9001,Title="Arvutimängude Ajalugu",Credits=1}
            };
            foreach(Course course in courses)
            {
                context.Courses.Add(course);
            }
            context.SaveChanges();

            var enrollments = new Enrollment[]
            {
                new Enrollment{StudentID=1,CourseID=1050,Grade=Grade.A},
                new Enrollment{StudentID=1,CourseID=4022,Grade=Grade.C},
                new Enrollment{StudentID=1,CourseID=4041,Grade=Grade.B},

                new Enrollment{StudentID=2,CourseID=1045,Grade=Grade.B},
                new Enrollment{StudentID=2,CourseID=3141,Grade=Grade.F},
                new Enrollment{StudentID=2,CourseID=2021,Grade=Grade.F},

                new Enrollment{StudentID=3,CourseID=1050},

                new Enrollment{StudentID=4,CourseID=1050},
                new Enrollment{StudentID=4,CourseID=4022,Grade=Grade.F},

                new Enrollment{StudentID=5,CourseID=4041,Grade=Grade.C},

                new Enrollment{StudentID=6,CourseID=1045},

                new Enrollment{StudentID=7,CourseID=3141,Grade=Grade.A},

                new Enrollment{StudentID=10,CourseID=9001,Grade=Grade.A},
            };
            foreach (Enrollment enrollment in enrollments)
            {
                context.Enrollments.Add(enrollment);
            }
            context.SaveChanges();
        }

    }
}
