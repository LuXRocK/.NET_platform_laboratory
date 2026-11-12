namespace Lab4;

public class Students { 
    public string StudentName { get; set; } 
    public int GroupPoint { get; set; } 
    public int StudentId { get; set; } 
    public List<Students> GtStuRec() { 
        List<Students> stulist = new List<Students>(); 
        stulist.Add(new Students { StudentId = 1, StudentName = "A", GroupPoint = 800 }); 
        stulist.Add(new Students { StudentId = 2, StudentName = "B", GroupPoint = 458 }); 
        stulist.Add(new Students { StudentId = 3, StudentName = "C", GroupPoint = 900 }); 
        stulist.Add(new Students { StudentId = 4, StudentName = "D", GroupPoint = 900 }); 
        stulist.Add(new Students { StudentId = 5, StudentName = "E", GroupPoint = 458 }); 
        stulist.Add(new Students { StudentId = 6, StudentName = "F", GroupPoint = 700 }); 
        stulist.Add(new Students { StudentId = 7, StudentName = "G", GroupPoint = 750 }); 
        stulist.Add(new Students { StudentId = 8, StudentName = "H", GroupPoint = 700 }); 
        stulist.Add(new Students { StudentId = 9, StudentName = "I", GroupPoint = 597 }); 
        stulist.Add(new Students { StudentId = 10, StudentName = "J", GroupPoint = 750 }); 
        return stulist; 
    } 
}
public class HelperFunctions
{
    public static void PrintArray(IEnumerable<int> array)
    {
        foreach (int t in array)
        {
            Console.WriteLine(t.ToString());
        }
    }
    public static void PrintArray(IEnumerable<string> array)
    {
        foreach (string t in array)
        {
            Console.WriteLine(t.ToString());
        }
    }

    public static List<int> GetListIntFromUser()
    {
        List<int> numbers = new List<int>();
        while (true)
        {
            Console.WriteLine("Enter a number to append list. X to stop.");
            string? input = Console.ReadLine();
            if (!int.TryParse(input, out int result))
            {
                if (input.Length != 1)
                {
                    Console.WriteLine("If you want to stop enter X only!");
                }
                else if (input[0].ToString().ToUpper() == "X")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Enter X to stop!");
                }
            }
            else
            {
                numbers.Add(result);
                Console.WriteLine($"Added {result} to list.");
            }
        }
        Console.WriteLine("Final list:");
        PrintArray(numbers);
        return numbers;
    }

    public static int? SingleIntFromUser()
    {
        int? filter = null;
        while (true)
        {
            Console.WriteLine("Enter a value to filter the list.");
            string? value = Console.ReadLine();
            if (!int.TryParse(value, out int result))
            {
                Console.WriteLine("Enter a number!");
            }
            else
            {
                filter = result;
                break;
            }
        }
        return filter;
    }

    public static string? StringFromUser()
    {
        string? sentence = null;
        while(true)
        {
            Console.WriteLine("Enter a sentence");
            string? input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                sentence = input;
                break;
            }
            else
            {
                Console.WriteLine("You can't enter a empty string!");
            }
        }
        return sentence; 
    }

    public static bool AskYesNo(string question)
    {
        while (true)
        {
            Console.WriteLine($"{question} (Y/N)");
            string answer = (Console.ReadLine() ?? "").Trim().ToUpper();
            if (answer == "Y") return true;
            if (answer == "N") return false;
            Console.WriteLine("Answer only with 'Y' or 'N'");
        }
    }

    public static string[] StringArrayFromUser()
    {
        List<string> result = new List<string>();
        while (true)
        {
            Console.WriteLine("Enter strings for a string array. Enter 'X' to stop.");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Empty input ignored.");
                continue;
            }

            if (input.ToUpper() == "X" && AskYesNo("Do you want to stop?"))
            {
                if (AskYesNo("Do you want to add the 'X' to array?"))
                {
                    result.Add(input);
                }
                return result.ToArray();
            }

            result.Add(input);
            Console.WriteLine($"Added {input} to array");
        }
    }
}
public class Solution
{
    public static void Zad1()
    {
        Console.WriteLine("Zad1: divisible by 2:");
        int[] n1 = { 1, 3, -2, -4, -7, -3, -8, 12, 19, 6, 9, 10, 14 };
        Console.WriteLine("Original array:");
        HelperFunctions.PrintArray(n1);

        var divisibleByTwo = from num in n1
                                where num % 2 == 0
                                select num;
        int[] result = divisibleByTwo.ToArray();
        Console.WriteLine("Array after filter:");
        HelperFunctions.PrintArray(result);
    }

    public static void Zad2()
    {
        Console.WriteLine("Zad2: not divisible by 2:");
        int[] n1 = { 1, 3, -2, -4, -7, -3, -8, 12, 19, 6, 9, 10, 14 };
        Console.WriteLine("Original array:");
        HelperFunctions.PrintArray(n1);

        var divisibleByTwo = from num in n1
                                where num % 2 != 0
                                select num;
        int[] result = divisibleByTwo.ToArray();
        Console.WriteLine("Array after filter:");
        HelperFunctions.PrintArray(result);
    }

    public static void Zad3()
    {
        Console.WriteLine("Zad3: In range 0 < x < 12:");
        var arr1 = new[] { 3, -1, -3, 6, 9, 2, -7, 0, 8, 14, 13, 24, 12, 6, 5 };
        Console.WriteLine("Original array:");
        HelperFunctions.PrintArray(arr1);
            
        var inRange = from num in arr1
                        where num > 0 && num < 12
                        select num;
        int[] result = inRange.ToArray();
        Console.WriteLine("Array after filter:");
        HelperFunctions.PrintArray(result);
    }

    public static void Zad4()
    {
        Console.WriteLine("Zad4: Greater than 20 when squared:");
        var arr1 = new[] { 3, 9, 2, 8, 6, 5 };
        Console.WriteLine("Original array:");
        HelperFunctions.PrintArray(arr1);

        var inRange = from num in arr1
                        where num * num > 20
                        select num;
        int[] result = inRange.ToArray();
        Console.WriteLine("Array after filter:");
        HelperFunctions.PrintArray(result);
    }
    public static void Zad5()
    {
        Console.WriteLine("Zad5: All numbers and how often they occure:");
        var arr1 = new[] { 5, 9, 1, 2, 3, 7, 5, 6, 7, 3, 7, 6, 8, 5, 4, 9, 6, 2 };
        Console.WriteLine("Original array:");
        HelperFunctions.PrintArray(arr1);

        var orderCounts = from num in arr1
                            group num by num into g
                            select (Number: g.Key, Count: g.Count());
        Console.WriteLine("Numbers and their occurances:");
        foreach (var number in orderCounts)
        {
            Console.WriteLine($"Number: {number.Number}, count: {number.Count}");
        }
    }
    public static void Zad6()
    {
        Console.WriteLine("Zad6: All letters in a string and how often they occure:");
        var str = "abeddwkkecjjeksoiekcllkenndkwel";
        Console.WriteLine($"Original string: \n {str}");

        var orderCounts = from ltr in str
                            group ltr by ltr into g
                            select (Letter: g.Key, Count: g.Count());
        Console.WriteLine("Letters and their occurances:");
        foreach(var ltr in orderCounts)
        {
            Console.WriteLine($"Letter: {ltr.Letter}, count: {ltr.Count}");
        }
    }

    public static void Zad7()
    {
        Console.WriteLine("Zad7: Display all months:");
        string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        Console.WriteLine("Original array:");
        HelperFunctions.PrintArray(months);

        var selectedMonths = from month in months
                                select month;
        string[] result = selectedMonths.ToArray();
        Console.WriteLine("Array after filter:");
        HelperFunctions.PrintArray(result);
    }

    public static void Zad8()
    {
        Console.WriteLine("Zad8: Unique values, their frequency and sum of each unique value:");
        int[] nums = new int[] { 5, 1, 9, 2, 3, 7, 4, 5, 6, 8, 7, 6, 3, 4, 5, 2 };
        HelperFunctions.PrintArray(nums);
        Console.WriteLine("Original array:");

        var uniqueNums = from num in nums
                         group num by num into g
                         select (Number: g.Key, Count: g.Count(), Sum: g.Sum());
        Console.WriteLine("Number, their count and sum");
        foreach (var num in uniqueNums)
        {
            Console.WriteLine($"Number: {num.Number}, Count: {num.Count}, Sum: {num.Sum}");
        }
    }

    public static void Zad9()
    {
        Console.WriteLine("Zad9: Match city with user inputed first and last letter:");
        string[] cities = { "ROME", "LONDON", "NAIROBI", "CALIFORNIA", "ZURICH", "NEW DELHI", "AMSTERDAM", "ABU DHABI", "PARIS" };
        HelperFunctions.PrintArray(cities);
        Console.WriteLine("Enter first and last letter:");
        char ltr1, ltr2;
        while (true)
        {
            string? ltrs = Console.ReadLine();
            if (ltrs.Length != 2)
            {
                Console.WriteLine("Enter 2 letters only!");
            }
            else if (!char.IsLetter(ltrs[0]))
            {
                Console.WriteLine("Enter letters only!");
            }
            else if (!char.IsLetter(ltrs[1]))
            {
                Console.WriteLine("Enter letters only!");
            }
            else
            {
                ltr1 = ltrs[0];
                ltr2 = ltrs[1];
                break;
            }
        }
        var result = from city in cities
                     where city.StartsWith(ltr1.ToString().ToUpper()) && city.EndsWith(ltr2.ToString().ToUpper())
                     select city;
        string[] filteredCities = result.ToArray();
        if (filteredCities.Length > 0)
        {
            HelperFunctions.PrintArray(filteredCities);
        }
        else
        {
            Console.WriteLine("No matching results :(");
        }
    }

    public static void Zad10()
    {
        Console.WriteLine("Zad10: Create a list of integer numbers and print out only the ones greater than a given value:");
        List<int> numbers = HelperFunctions.GetListIntFromUser();
        int? filter = HelperFunctions.SingleIntFromUser();
        if(filter != null)
        {
            var filteredList = from num in numbers
                               where num > filter
                               select num;
            Console.WriteLine("Filtered list:");
            HelperFunctions.PrintArray(filteredList);
        }
    }
    public static void Zad11()
    {
        Console.WriteLine("Zad11: Create a list of integer numbers and print out N last values:");
        List<int> numbers = HelperFunctions.GetListIntFromUser();
        int? n = HelperFunctions.SingleIntFromUser();
        if(n != null)
        {
            Console.WriteLine($"Last {n.Value} numbers:");
            HelperFunctions.PrintArray(numbers.Skip(numbers.Count()-n.Value).Take(n.Value));
        }
    }

    public static void Zad12()
    {
        Console.WriteLine("Zad12: Create a list of integer numbers and print out N greatest values:");
        List<int> numbers = HelperFunctions.GetListIntFromUser();
        int? n = HelperFunctions.SingleIntFromUser();
        if(n != null)
        {
            Console.WriteLine($"{n.Value} greatest values:");
            numbers.Sort();
            numbers.Reverse();
            HelperFunctions.PrintArray(numbers.Take(n.Value));
        }
    }
    public static void Zad13()
    {
        Console.WriteLine("Zad13: Find words written in uppercase in a string");
        string? sentence = HelperFunctions.StringFromUser();
        var splitted = from s in sentence.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                       where s.ToUpper().Equals(s)
                       select s;
        HelperFunctions.PrintArray(splitted);
    }

    public static void Zad14()
    {
        Console.WriteLine("Zad14: Turn a array of strings into one string");
        string?[] words = HelperFunctions.StringArrayFromUser();
        Console.WriteLine($"Original array:");
        HelperFunctions.PrintArray(words);
        string joined = string.Join(",", words);
        Console.WriteLine($"Final string: \n {joined}");
    }

    public static void Zad15()
    {
        Console.WriteLine("Zad15: Find n students with highest scores.");
        int? n = HelperFunctions.SingleIntFromUser();

        Students students = new Students();
        List<Students> studentsList = students.GtStuRec();
        var bestStudents = from student in studentsList
                           orderby student.GroupPoint
                           select student;
        var result = bestStudents.Reverse().Take(n.Value);
        foreach( var student in result )
        {
            Console.WriteLine($"Student ID: {student.StudentId}, student name: {student.StudentName}, student score: {student.GroupPoint}");
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        int result;
        bool running = true;
        while (running)
        {
            Console.WriteLine($"Which task do yo want to see? (1-21, 22 - exit)\n Enter task number:");
            string? task_number = Console.ReadLine();
            if (!int.TryParse(task_number, out result))
            {
                Console.WriteLine("Enter a valid number!");
            }
            else if (result > 22 || result <= 0)
            {
                Console.WriteLine("Enter a number in range 1-21 or 22 to exit");
            }
            else
            {
                switch (result)
                {
                    case 1:
                        Solution.Zad1();
                        break;
                    case 2:
                        Solution.Zad2();
                        break;
                    case 3:
                        Solution.Zad3();
                        break;
                    case 4:
                        Solution.Zad4();
                        break;
                    case 5:
                        Solution.Zad5();
                        break;
                    case 6:
                        Solution.Zad6();
                        break;
                    case 7:
                        Solution.Zad7();
                        break;
                    case 8:
                        Solution.Zad8();
                        break;
                    case 9:
                        Solution.Zad9();
                        break;
                    case 10:
                        Solution.Zad10();
                        break;
                    case 11:
                        Solution.Zad11();
                        break;
                    case 12:
                        Solution.Zad12();
                        break;
                    case 13:
                        Solution.Zad13();
                        break;
                    case 14:
                        Solution.Zad14();
                        break;
                    case 15:
                        Solution.Zad15();
                        break;
                    case 16:
                        Console.WriteLine("Not yet implemented");
                        break;
                    case 17:
                        Console.WriteLine("Not yet implemented");
                        break;
                    case 18:
                        Console.WriteLine("Not yet implemented");
                        break;
                    case 19:
                        Console.WriteLine("Not yet implemented");
                        break;
                    case 20:
                        Console.WriteLine("Not yet implemented");
                        break;
                    case 21:
                        Console.WriteLine("Not yet implemented");
                        break;
                    case 22:
                        running = false;
                        break;
                }
            }
        }

        //Console.WriteLine("");
        
    }
}
