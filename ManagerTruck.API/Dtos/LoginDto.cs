namespace ManagerTruck.Dtos
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }

    public class CreateUserDto
    {
        public string Nome { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
