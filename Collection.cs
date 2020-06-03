using System;
using System.Collections.Generic;
using System.Text;
using ClassLibrary;


namespace lab14
{
    
    class Collection
    {
        
        public List<Person> City { get; set; }
        public List<Person> Department { get; set; }

        public Collection(int length)
        {

            City = new List<Person>();
            Department = new List<Person>();
            
            for (int i = 0; i < length; i++)
            {
                if (i / 2 == 0)
                {
                    Schooler child = new Schooler();
                    City.Add(child);
                }
                else
                {
                    Person p = new Person();
                    City.Add(p);
                }

                if (i / 2 == 0)
                {
                    ExramuralStudent exSt = new ExramuralStudent();
                    Department.Add(exSt);
                }
                else
                {
                    Student st = new Student();
                    Department.Add(st);
                }

            }

        }
        public void ShowCity()
        {
            foreach(Person p in City)
            {
                Console.WriteLine(p);
            }
           
        }
        public void ShowDepartment()
        {
            foreach (Person p in Department)
            {
                Console.WriteLine(p);
            }
        }
        public void ShowAll()
        {
            ShowCity();
            ShowDepartment();
        }

    }
}
