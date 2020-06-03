using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ClassLibrary;

namespace lab14
{
    class Program
    {
        public static Collection[] all2 = new Collection[3] { new Collection(4), new Collection(4), new Collection(4) };

        static void Main(string[] args)
        {
            Select();
            Count();
            Except();
            Aggregate();
            GroupBy();
        }

        //Найти во всех городах людей женского пола
        public static void Select()
        {

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Начальные коллекции: ");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (Collection c in all2)
            {
                c.ShowCity();
                Console.WriteLine("__________________");
            }

            //LINQ
            var females_linq = from coll in all2 
                               from person in coll.City
                               where person.Sex is "Женский"
                               select person.Name;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Результат запроса: ");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (string names in females_linq)
            {
                Console.WriteLine(names) ;
            }
            //Метод расширения

            var females_expansion = all2.SelectMany(col => col.City).Where(person => person.Sex == "Женский").Select(person => person.Name);

            Console.WriteLine($"\nСовпало: {females_linq.Count() == females_expansion.Count()}");
            Console.WriteLine("\nЧтобы продолжить нажмите любую клавишу...");
            Console.ReadKey();
            Console.Clear();

        }

        //Количество студентов, не сдавших хотя бы один экзамен.
        public static void Count()
        {

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Начальные коллекции: ");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (Collection c in all2)
            {
                c.ShowDepartment();
                Console.WriteLine("__________________");
            }

            //LINQ
            var deduct_linq = (from coll in all2
                               from person in coll.Department
                               where person is ExramuralStudent &&
                                            (((ExramuralStudent)person).ExamMark1 < 3 || ((ExramuralStudent)person).ExamMark2 < 3)
                               select person).Count();

            //Метод расширения
            var deduct_expansion = all2.SelectMany(coll => coll.Department)
               .Where(person => (person is ExramuralStudent) &&
                     (((ExramuralStudent)person).ExamMark1 < 3 || ((ExramuralStudent)person).ExamMark2 < 3))
               .Select(person => person).Count();


            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Количество студентов не сдавших хотя бы один экзамен = {deduct_linq}");
            Console.ForegroundColor = ConsoleColor.White;
            
            Console.WriteLine($"\nСовпало: {deduct_linq == deduct_expansion}");

            Console.WriteLine("\nЧтобы продолжить нажмите любую клавишу...");
            Console.ReadKey();
            Console.Clear();
        }
       
        //Студентов, у которых все экзамены сданы хорошо или отлично и их возраст больше 30
        public static void Except()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Начальные коллекции: ");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (Collection c in all2)
            {
                c.ShowDepartment();
                Console.WriteLine("__________________");
            }

            //LINQ
            var genius_linq = (from coll in all2 from person in coll.Department 
                               where person is ExramuralStudent &&  ((ExramuralStudent)person).ExamMark1 > 4 && ((ExramuralStudent)person).ExamMark2 > 4 
                               select person)
                              .Except(from coll in all2 from person in coll.Department 
                               where person is ExramuralStudent && (((ExramuralStudent)person).Age <= 30 ) select person);

            //Метод расширения

            var genius_expansion = (all2.SelectMany(coll => coll.Department)
                .Where(person => (person is ExramuralStudent) && (((ExramuralStudent)person).ExamMark1 > 4 && ((ExramuralStudent)person).ExamMark2 > 4))
                .Select(person => person)).Except(all2.SelectMany(coll => coll.Department)
                .Where(person => person is ExramuralStudent && ((ExramuralStudent)person).Age <= 30).Select(person => person));


            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Количество студентов, у которых все экзамены сданы на отлично и их возраст больше 30: {genius_linq.Count()}");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (Person person in genius_linq)
            {
                person.Show();
            }

            Console.WriteLine($"\nСовпало: {genius_expansion.Count() == genius_linq.Count()}");

            Console.WriteLine("\nЧтобы продолжить нажмите любую клавишу...");
            Console.ReadKey();
            Console.Clear();
        }

        //средняя оценка за первый экзамен
        public static void Aggregate()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Начальные коллекции: ");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (Collection c in all2)
            {
                c.ShowDepartment();
                Console.WriteLine("__________________");
            }

            //LINQ
            var linq = (from coll in all2 from person in coll.Department
                               where person is ExramuralStudent
                               select ((ExramuralStudent)person).ExamMark1).Sum()/
                               (from coll in all2 from person in coll.Department
                                where person is ExramuralStudent select person).Count();



            //Метод расширения
            var expansion = all2.SelectMany(coll => coll.Department).Where(person => person is ExramuralStudent)
                .Select( person => ((ExramuralStudent)person).ExamMark1)
                .Aggregate<int>((a, b) => a + b) /
                all2.SelectMany(coll => coll.Department).Where(person => person is ExramuralStudent)
                .Select (person => person).Count();
             
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Средняя оценка за первый экзамен = {linq}");
            Console.ForegroundColor = ConsoleColor.White;
            
            Console.WriteLine($"\nСовпало: {linq == expansion}");

            Console.WriteLine("\nЧтобы продолжить нажмите любую клавишу...");
            Console.ReadKey();
            Console.Clear();

        }

        //группировка по классу
        public static void GroupBy()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Начальные коллекции: ");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (Collection c in all2)
            {
                c.ShowCity();
                Console.WriteLine("__________________");
            }

            //LINQ
            var linq = from coll in all2 from person in coll.City where person is Schooler orderby ((Schooler)person).Year group person by ((Schooler)person).Year;
            
            foreach(var group in linq)
            {
                Console.WriteLine($"класс {group.Key}");
                foreach(var item in group)
                {
                    Console.WriteLine($"\t{item}");
                }
            }
            

            //Метод расширения
            var expansion = all2.SelectMany(coll => coll.Department).Where(person => person is Schooler)
                                .OrderBy(person => ((Schooler)person).Year).GroupBy(person => ((Schooler)person).Year);

            foreach (var group in expansion)
            {
                Console.WriteLine($"класс {group.Key}");
                foreach (var item in group)
                {
                    Console.WriteLine($"\t{item}");
                }
            }

            Console.WriteLine("\nЧтобы продолжить нажмите любую клавишу...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
