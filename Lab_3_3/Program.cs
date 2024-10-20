using System;
using System.Text;
using System.IO;

namespace Person1
{
    class Person
    {
        string surname;
        string initials;
        int birth_year;
        double pay;

        public Person()  
        {
            surname = "Anonimous";
            initials = "N/A";
            birth_year = 0;
            pay = 0.0;
        }

        public Person(string s)  
        {
            string[] parts = s.Split(' ');  

            if (parts.Length != 4)
                throw new FormatException("Невірний формат вхідного рядка");

            surname = parts[0];  
            initials = parts[1];  
            birth_year = Convert.ToInt32(parts[2]);  
            pay = Convert.ToDouble(parts[3]);  

            if (birth_year < 0 || pay < 0)
                throw new FormatException("Неприпустимі значення року народження або окладу");
        }

        public override string ToString()  
        {
            return string.Format("Прізвище: {0}, Ініціали: {1}, Рік народження: {2}, Оклад: {3:F2}",
                surname, initials, birth_year, pay);
        }

        public int Compare(string name)  
        {
            return string.Compare(this.surname, name, StringComparison.OrdinalIgnoreCase);
        }

        
        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }

        public string Initials
        {
            get { return initials; }
            set { initials = value; }
        }

        public int BirthYear
        {
            get { return birth_year; }
            set
            {
                if (value > 0) birth_year = value;
                else throw new FormatException("Невірний рік народження");
            }
        }

        public double Pay
        {
            get { return pay; }
            set
            {
                if (value > 0) pay = value;
                else throw new FormatException("Невірний оклад");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Person[] dbase = new Person[100];
            int n = 0;

            try
            {
                StreamReader f = new StreamReader("Persons.txt");  
                string s;
                int i = 0;

                while ((s = f.ReadLine()) != null)  
                {
                    dbase[i] = new Person(s);  
                    Console.WriteLine(dbase[i]);  
                    ++i;
                }

                n = i;
                f.Close();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Перевірте правильність імені і шляху до файлу!");
                return;
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Дуже великий файл!");
                return;
            }
            catch (FormatException e)
            {
                Console.WriteLine("Помилка формату: " + e.Message);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("Інша помилка: " + e.Message);
                return;
            }

            int n_pers = 0;
            double mean_pay = 0;
            Console.WriteLine("Введіть прізвище співробітника:");
            string name;

            while ((name = Console.ReadLine()) != "")  
            {
                bool not_found = true;

                for (int k = 0; k < n; ++k)
                {
                    Person pers = dbase[k];
                    if (pers.Compare(name) == 0)
                    {
                        Console.WriteLine(pers);  
                        ++n_pers;
                        mean_pay += pers.Pay;
                        not_found = false;
                    }
                }

                if (not_found)
                    Console.WriteLine("Такого співробітника немає.");

                Console.WriteLine("Введіть прізвище співробітника або натисніть Enter для завершення");
            }

            if (n_pers > 0)
                Console.WriteLine("Середній оклад: {0:F2}", mean_pay / n_pers);  

            Console.ReadKey();
        }
    }
}
