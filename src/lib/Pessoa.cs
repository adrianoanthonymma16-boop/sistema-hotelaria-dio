using CpfCnpjLibrary;
namespace lib.pessoa
{
    
    public class Pessoa
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        private string _cpf;
        public string CpfCliente
        {
            get => _cpf;

            set
            {
                if (string.IsNullOrWhiteSpace(value))

                {
                    throw new ArgumentException("CPF não pode ser vazio.");
                }

                string cpflimpo = Cpf.FormatarSemPontuacao(value);

                if (!Cpf.Validar(cpflimpo))
                {
                    throw new ArgumentException("CPF inválido.");
                }

                _cpf = cpflimpo;
            }
            
        }

        private string? _email;

        public string? Email
        {
            get => _email ?? "Usuário optou por não fornecer o Email";
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _email = null;
                    return;
                }

                if (!value.Contains("@"))
                {
                    throw new Exception("Erro: Email inválido (não contém '@').");
                }

                string[] partes = value.Split('@', 2);
                if (partes.Length != 2)
                {
                    throw new Exception("Erro: Email inválido (formato incorreto).");
                }

                string usuario = partes[0];
                string dominio = partes[1];

                if (usuario.Length < 4)
                {
                    throw new Exception("Erro: Email inválido (usuário deve ter pelo menos 4 caracteres).");
                }

                if (!dominio.Contains("."))
                {
                    throw new Exception("Erro: Email inválido (domínio sem '.').");
                }

                string[] partesDominio = dominio.Split('.');
                if (partesDominio.Length != 2)
                {
                    throw new Exception("Erro: Email inválido (domínio incorreto).");
                }

                string antesDoPonto = partesDominio[0];
                string depoisDoPonto = partesDominio[1];

                if (antesDoPonto.Length < 3 || depoisDoPonto.Length < 3)
                {
                    throw new Exception("Erro: Email inválido (domínio inválido).");
                }

                _email = value.Trim();
            }
        }

        public string NomeCompleto => $"{Nome} {Sobrenome}";

        public Pessoa(string nome, string sobrenome, string cpf, string? email = null)
        {
            Nome = nome;
            Sobrenome = sobrenome;
            CpfCliente = cpf;
            Email = email;
        }

        public override string ToString()
        {
            return $"Nome: {NomeCompleto}\n" +
                   $"CPF: {Cpf.FormatarComPontuacao(CpfCliente)}\n" +
                   $"Email: {_email ?? "Não informado"}";
        }
        public static Pessoa RetornarCliente()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("=== CADASTRO DE CLIENTE ===");

                    Console.Write("Digite o nome: ");
                    string? nome = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(nome))
                    {
                        Console.WriteLine("Erro: nome não pode ser vazio.");
                        continue;
                    }

                    Console.Write("Digite o sobrenome: ");
                    string? sobrenome = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(sobrenome))
                    {
                        Console.WriteLine("Erro: sobrenome não pode ser vazio.");
                        continue;
                    }

                    Console.Write("Digite o CPF: ");
                    string? cpf = Console.ReadLine();

                    Console.Write("Digite o email (opcional, Enter para pular): ");
                    string? email = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(email))
                    {
                        email = null;
                    }

                    var cliente = new Pessoa(nome, sobrenome, cpf, email);
                    Console.Clear();
                    Console.WriteLine($"✅ Cliente '{cliente.NomeCompleto}' cadastrado com sucesso!");
                    return cliente;
                }
                catch (ArgumentException ex) // Captura as exceções da classe
                {
                    Console.Clear();
                    Console.WriteLine($"Erro: {ex.Message}");
                    Console.Write("Deseja tentar novamente? (s/n): ");
                    string? opcao = Console.ReadLine();
                    if (opcao?.ToLower() != "s")
                    {
                        Console.WriteLine("Operação cancelada.");
                        return null!;
                    }
                }
            }
        }
    }
}