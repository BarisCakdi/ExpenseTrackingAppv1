namespace ExpenseTrackingApp
{
    class Program
    {
        static string Quest(string quest)
        {
            Console.Write(quest);
            return Console.ReadLine();
        }

        static List<User> users = new List<User>();
        static User loggedUser = new User();
        static List<ExpenseTracking> expenseTrackings = new List<ExpenseTracking>();

        static void TxtSaveUsers()
        {
            using StreamWriter writer = new StreamWriter("users.txt");
            foreach (var user in users)
            {
                writer.WriteLine($"{user.UserName} | {user.Password}");
            }
        }


        static void TxtSaveExpenses(User user)
        {
            string fileName = $"{user.UserName}_expenses.txt"; //Dosya ismini giriş yapan kullanıcı adı olarak belirliyorum
            using StreamWriter writer = new StreamWriter(fileName);
            //LINQ, C# da veri koleksiyolarını sorgulamak için kullanılan bir araçtır.
            foreach (var expense in expenseTrackings.Where(e => e.UserName == user.UserName))//'Where' bir 'LINQ' methodudur. Burada giriş yapan kullanıcının yaptığı harcamaları kayıt altına almamızı sağlıyor. 
            {
                writer.WriteLine($"{expense.CategoryName} | {expense.ProductName} | {expense.Amount}");
            }
        }
        

        static void UserSave()
        {
            if (File.Exists("users.txt"))
            {
                using StreamReader reader = new StreamReader("users.txt");
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split('|');
                    users.Add(new User { UserName = parts[0].Trim(), Password = parts[1].Trim() });//Trim = kayıt edilen kullanıcı adı ve şifrede gereksiz boşlukları silmek için kullanıyoruz.
                }
            }
        }

        static void Opening(bool firstOpenin = false)//Girişi bir döngüye alıyorum. Döngü 'True' olana kadar döngüden çıkmıyor.
        {
            Console.Clear();
            if (firstOpenin)
            {
                Console.WriteLine("GİDER TAKİP SİSTEMİ");
                Console.WriteLine("1- Yeni kayıt oluştur");
                Console.WriteLine("2- Giriş yap");
                Console.WriteLine("3- Çıkış");
                char inputchoose = Console.ReadKey().KeyChar;
                switch (inputchoose)
                {
                    case '1':
                        UserAdd();
                        break;
                    case '2':
                        Start();
                        break;
                    case '3':
                        break;
                    default:
                        Console.WriteLine("Hatalı seçim!");
                        ReturnMenu();
                        break;
                }
            }
        }

        static void Menu()
        {
            Console.Clear();
            Console.WriteLine($"Gider takip uygulamasına Hoş geldin {loggedUser.UserName}");
            Console.WriteLine("=============================================================");
            Console.WriteLine("1- Gider ekle");
            Console.WriteLine("2- Rapor oluştur");
            Console.WriteLine("3- Çıkış yap");
            char inputchoose = Console.ReadKey().KeyChar;
            switch (inputchoose)
            {
                case '1':
                    ExpenseAdd();
                    break;
                case '2':
                    CreateReport();
                    break;
                case '3':
                    ReturnMenu();
                    break;
                default:
                    Console.WriteLine("Hatalı seçim!");
                    AgainExpense();
                    break;
            }
        }

        static void Start()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("Kullanıcı adı giriniz: ");
                var inputName = Console.ReadLine();
                Console.Write("Şifrenizi Giriniz: ");
                var inputPass = Console.ReadLine();
                Console.Clear();
                //LINQ, C# da veri koleksiyolarını sorgulamak için kullanılan bir araçtır.
                //FirsOrDefault bir LINQ operatörüdür. Sorguda ilk eşleleşen öğeyi veya varsayılan değeri döndürür. Hiç bir öğe eşleşmezse 'Null' döner.
                loggedUser = users.FirstOrDefault(u => u.UserName == inputName && u.Password == inputPass);//Kullanılan 'u' değişkeni her öğe için geçiçi bir değişkeni temsil eder.
                if (loggedUser != null)
                {
                    Console.WriteLine("Giriş başarılı! Hoş geldin " + loggedUser.UserName);
                    AgainExpense();
                }
                else
                {
                    Console.WriteLine("Kullanıcı adı veya şifre hatalı. Lütfen tekrar deneyin!");
                    ReturnMenu();
                }
                break;
            }
        }

        static void ReturnMenu()
        {
            Console.WriteLine("Ana menü için bir tuşa basınız.");
            Console.ReadKey();
            Opening(true);
        }

        static void AgainExpense()
        {
            Console.WriteLine("Devam etmek için bir tuşa basınız");
            Console.ReadKey();
            Menu();
        }

        static void UserAdd()
        {
            Console.Clear();
            User user = new User();
            user.UserName = Quest("Kullanıcı adı oluşturunuz: ");
            user.Password = Quest("Şifre oluşturunuz: ");
            Console.WriteLine("Kayıt oluşturulmuştur.");
            users.Add(user);
            TxtSaveUsers();
            ReturnMenu();
        }

        static void ExpenseAdd()
        {
            Console.Clear();
            ExpenseTracking expense = new ExpenseTracking();
            expense.UserName = loggedUser.UserName;
            expense.CategoryName = Quest("Katagori giriniz: ");
            expense.ProductName = Quest("Ürün ismi giriniz: ");
            expense.Amount = double.Parse(Quest("Fiyatı giriniz: "));
            expense.Time = DateTime.Now;
            Console.WriteLine("Kayıt oluşturulmuştur.");
            expenseTrackings.Add(expense);
            TxtSaveExpenses(loggedUser);
            AgainExpense();
        }

        static void CreateReport()
        {
            Console.Clear();
            Console.WriteLine("\nTüm kayıtlar");
            Console.WriteLine("========================");
            double totalAmount = 0;
            //LINQ, C# da veri koleksiyolarını sorgulamak için kullanılan bir araçtır.
            foreach (var expense in expenseTrackings.Where(e => e.UserName == loggedUser.UserName))//'Where' bir 'LINQ' methodudur. Burada giriş yapan kullanıcının yaptığı harcamaları görmemizi sağlıyor.
            {                                   //Kullanılan 'e' değişkeni her öğe için geçiçi bir değişkeni temsil eder.
                Console.WriteLine(expense.Time);
                Console.WriteLine("-----------------------------------");
                Console.WriteLine($"{expense.UserName} - {expense.CategoryName} - {expense.ProductName} - {expense.Amount}");
                Console.WriteLine("===================================");
                totalAmount += expense.Amount;
            }
            Console.WriteLine($"Yapılan toplam harcama: {totalAmount}");
            AgainExpense();
        }

        static void Main(string[] args)
        {
            UserSave();
            Opening(true);
        }
    }

    class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    class ExpenseTracking
    {
        public string UserName { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public double Amount { get; set; }
        public DateTime Time { get; set; }
    }
}
