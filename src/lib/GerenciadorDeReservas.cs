using lib.pessoa;
using lib.suite;
using lib.gerenciadordequartos;

namespace lib.gerenciadordereservas
{
    public class GerenciadorDeReservas
    {
        public GerenciadorDeReservas(Suite suite, List<Pessoa> hospedes)
        {
            {
                // Gera ID aleatório (8 caracteres)
                Id = Guid.NewGuid().ToString()[..8].ToUpper();
                
                // Chama os setters com validação
                SuiteReservada = suite;
                PessoasReserva = hospedes;
                Chekin = DateTime.Now;
                Chekout = null; // Checkout não realizado ainda
            }
        }
        public static List<GerenciadorDeReservas> ReservasAtivas = [];

        public string Id { get; set; }
        private Suite _suiteReservada;

        public Suite SuiteReservada
        {
            get => _suiteReservada;
            set
            {
                if (value == null)
                    throw new ArgumentException("Suite não pode ser nula.");

                if (!value.Disponivel)
                    throw new InvalidOperationException("Erro: Suite indisponível. Não é possível reservar.");
                value.Disponivel = false;
                _suiteReservada = value;
            }
        }
        public List<Pessoa> PessoasReserva { get; set; }
        public DateTime Chekin { get; set; }
        public DateTime? Chekout { get; set; }

        public override string ToString()
        {
            return $"ID da reserva: {Id}\n" +
                $"Suite: {SuiteReservada?.DescricaoQuarto ?? "N/A"}\n" +
                $"Hóspedes: {PessoasReserva?.Count ?? 0}\n" +
                $"Check-in: {Chekin:dd/MM/yyyy HH:mm}\n" +
                $"Check-out: {(Chekout.HasValue ? Chekout.Value.ToString("dd/MM/yyyy HH:mm") : "Não realizado")}";
        }

        public static void RealizarCheckin()
        {
            try
            {
                // 1. Lista suites disponíveis
                GerenciadorDeQuartos.ListarSuitesDisponiveis();

                Console.Write("Digite a descrição da suite desejada: ");
                string? descricao = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(descricao))
                {
                    Console.WriteLine("Erro: descrição vazia.");
                    return;
                }

                // 2. Busca a suite
                Suite? suiteEscolhida = null;
                foreach (var suite in GerenciadorDeQuartos.SuitesHotel)
                {
                    if (suite.DescricaoQuarto.Equals(descricao, StringComparison.OrdinalIgnoreCase))
                    {
                        suiteEscolhida = suite;
                        break;
                    }
                }

                if (suiteEscolhida == null || !suiteEscolhida.Disponivel)
                {
                    Console.WriteLine("Erro: suite não encontrada ou indisponível.");
                    return;
                }

                // 3. Valida quantidade de hóspedes
                Console.Write("Digite a quantidade de pessoas a serem hospedadas: ");
                if (!int.TryParse(Console.ReadLine(), out int quantidade) || quantidade <= 0)
                {
                    Console.WriteLine("Erro: quantidade inválida.");
                    return;
                }

                if (quantidade > suiteEscolhida.Capacidade)
                {
                    Console.WriteLine($"Erro: capacidade máxima da suite é {suiteEscolhida.Capacidade} pessoas.");
                    return;
                }

                // 4. Cadastra os hóspedes
                List<Pessoa> pessoasHospedadas = [];
                Console.WriteLine("\n--- CADASTRO DE HÓSPEDES ---");

                for (int i = 0; i < quantidade; i++)
                {
                    Console.WriteLine($"\n--- Hóspede {i + 1} ---");
                    var cliente = Pessoa.RetornarCliente();

                    if (cliente == null)
                    {
                        Console.WriteLine("Cadastro cancelado pelo usuário.");
                        return;
                    }

                    // 🔥 VALIDA SE O CPF JÁ ESTÁ NA LISTA
                    if (pessoasHospedadas.Any(p => p.CpfCliente == cliente.CpfCliente))
                    {
                        Console.WriteLine($"⚠️ CPF {cliente.CpfCliente} já cadastrado neste check-in.");
                        Console.WriteLine("Por favor, cadastre um hóspede com CPF diferente.");
                        i--; // Volta uma posição para tentar novamente
                        continue;
                    }

                    pessoasHospedadas.Add(cliente);
                    Console.WriteLine($"✅ {cliente.NomeCompleto} adicionado com sucesso!");
                }

                // 5. Cria a reserva
                GerenciadorDeReservas reserva = new(suiteEscolhida, pessoasHospedadas);
                ReservasAtivas.Add(reserva);

                Console.WriteLine("\n✅ Check-in realizado com sucesso!");
                Console.WriteLine(reserva);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
            }
        }

        public static void ListarReservasAtivas()
        {
            if (ReservasAtivas.Count == 0)
            {
                Console.WriteLine("Nenhuma reserva ativa.");
                return;
            }

            Console.WriteLine("\n=== RESERVAS ATIVAS ===");
            foreach (var reserva in ReservasAtivas)
            {
                Console.WriteLine($"ID: {reserva.Id}");
                Console.WriteLine($"Suite: {reserva.SuiteReservada?.DescricaoQuarto ?? "N/A"}");
                Console.WriteLine($"Hóspedes: {reserva.PessoasReserva?.Count ?? 0}");
                Console.WriteLine($"Check-in: {reserva.Chekin:dd/MM/yyyy HH:mm}");
                Console.WriteLine("---");
            }
        }

        public static void RealizarCheckout()
        {
            // 1. Lista as reservas ativas
            ListarReservasAtivas();

            if (ReservasAtivas.Count == 0)
                return;

            // 2. Pede o ID da reserva
            Console.Write("\nDigite o ID da reserva para realizar o checkout: ");
            string? idReserva = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(idReserva))
            {
                Console.WriteLine("Erro: ID inválido.");
                return;
            }

            // 3. Busca a reserva pelo ID
            GerenciadorDeReservas? reservaEscolhida = null;
            foreach (var reserva in ReservasAtivas)
            {
                if (reserva.Id.Equals(idReserva.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    reservaEscolhida = reserva;
                    break;
                }
            }

            if (reservaEscolhida == null)
            {
                Console.WriteLine("Erro: reserva não encontrada.");
                return;
            }

            // 4. Confirmação
            Console.WriteLine($"\nDeseja realizar checkout da reserva?");
            Console.WriteLine($"Suite: {reservaEscolhida.SuiteReservada?.DescricaoQuarto}");
            Console.WriteLine($"Hóspedes: {reservaEscolhida.PessoasReserva?.Count}");
            Console.Write("Confirmar? (s/n): ");
            string confirmar = Console.ReadLine()?.ToLower() ?? "";

            if (confirmar != "s")
            {
                Console.WriteLine("Operação cancelada.");
                return;
            }

            // 5. Realiza o checkout
            reservaEscolhida.SuiteReservada.Disponivel = true;
            reservaEscolhida.Chekout = DateTime.Now;

            // Calcula diárias
            int dias = (int)Math.Ceiling((reservaEscolhida.Chekout.Value - reservaEscolhida.Chekin).TotalDays);
            if (dias == 0) dias = 1;
            decimal total = dias * reservaEscolhida.SuiteReservada.ValorDiaria;

            Console.WriteLine($"\n✅ Checkout realizado com sucesso!");
            Console.WriteLine($"   Suite: {reservaEscolhida.SuiteReservada.DescricaoQuarto}");
            Console.WriteLine($"   Check-in: {reservaEscolhida.Chekin:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"   Check-out: {reservaEscolhida.Chekout.Value:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"   Diárias: {dias}");
            Console.WriteLine($"   Total: {total:C}");

            // Remove da lista de reservas ativas
            ReservasAtivas.Remove(reservaEscolhida);
        } 
    }
} 