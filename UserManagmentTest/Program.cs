namespace G04_UserManagment1
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Unregister");
                Console.WriteLine("4. Exit");
                string? option = Console.ReadLine();

                if (option == null)
                {
                    Console.WriteLine("Invalid option. Please try again.");
                    continue;
                }

                switch (option)
                {
                    case "1":
                        RegisterUser();
                        break;
                    case "2":
                        LoginUser();
                        break;
                    case "3":
                        UnregisterUser();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void RegisterUser()
        {
            Console.Write("Enter username: ");
            string? username = Console.ReadLine();
            Console.Write("Enter password: ");
            string? password = Console.ReadLine();

            if (username == null || password == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Username and password cannot be null.");
                Console.ResetColor();
                return;
            }

            User user = new User(username, password);
            Result result = UserManager.Register(user);

            if (result == Result.Success)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("User registered successfully");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"User registration failed: {result}");
            }
            Console.ResetColor();
        }

        static void LoginUser()
        {
            Console.Write("Enter username: ");
            string? username = Console.ReadLine();
            Console.Write("Enter password: ");
            string? password = Console.ReadLine();

            if (username == null || password == null)
            {
                Console.WriteLine("Username and password cannot be null.");
                return;
            }

            bool success = UserManager.Login(username, password);
            Console.WriteLine(success ? "User logged in successfully" : "User login failed");
        }

        static void UnregisterUser()
        {
            Console.Write("Enter username: ");
            string? username = Console.ReadLine();

            if (username == null)
            {
                Console.WriteLine("Username cannot be null.");
                return;
            }

            bool success = UserManager.UnRegister(username);
            Console.WriteLine(success ? "User unregistered successfully" : "User unregistration failed");
        }
    }

    class User
    {
        public User(string username, string password)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));

            if (username.Length < 8 || password.Length < 8)
            {
                throw new ArgumentException("Username and Password must be at least 8 characters long");
            }
        }

        public string Username { get; private init; }
        public string Password { get; private set; }
    }

    static class UserManager
    {
        private static User?[] _users = new User?[10];

        public static Result Register(User? user)
        {
            if (user == null)
            {
                return Result.UserIsNull;
            }

            if (!IsUsernameUnique(user.Username))
            {
                return Result.UsernameIsNotUnique;
            }

            if (IsArrayFull())
            {
                Array.Resize(ref _users, _users.Length + 10);
            }

            _users[GetFirstEmptyIndex()] = user;
            return Result.Success;
        }

        public static bool Login(string username, string password)
        {
            return GetUserByUsername(username)?.Password == password;
        }

        public static bool UnRegister(string username)
        {
            int index = GetIndexByUsername(username);
            if (index != -1)
            {
                _users[index] = null;
                return true;
            }

            return false;
        }

        private static int GetIndexByUsername(string username)
        {
            for (int i = 0; i < _users.Length; i++)
            {
                if (_users[i]?.Username == username)
                {
                    return i;
                }
            }

            return -1;
        }

        private static User? GetUserByUsername(string username)
        {
            int index = GetIndexByUsername(username);
            return index != -1 ? _users[index] : null;
        }

        private static bool IsUsernameUnique(string username)
        {
            return GetIndexByUsername(username) == -1;
        }

        private static int GetFirstEmptyIndex()
        {
            for (int i = 0; i < _users.Length; i++)
            {
                if (_users[i] == null)
                {
                    return i;
                }
            }

            return -1;
        }

        private static bool IsArrayFull()
        {
            return GetFirstEmptyIndex() == -1;
        }
    }

    enum Result
    {
        Success = 0,
        UserIsNull = 1,
        UsernameIsNotUnique = 2,
        ArrayIsFull = 3
    }
}
