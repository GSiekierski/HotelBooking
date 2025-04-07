using HotelBookingAPI.Models;

namespace HotelBookingAPI.Auth
{
    public class AuthService
    {
        private readonly List<User> _users = new();
        private int _nextId = 1;

        public User? Register(string username, string password)
        {
            if (_users.Any(u => u.Username == username))
                return null;

            var user = new User
            {
                Id = _nextId++,
                Username = username,
                Password = password // potem zahashujemy
            };

            _users.Add(user);
            return user;
        }

        public User? Authenticate(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
