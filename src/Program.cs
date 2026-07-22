using lib.pessoa;
using lib.suite;
using lib.gerenciadordequartos;
using lib.gerenciadordereservas;

class Program
{
    static void Main()
    {
        var gerenciadorQuartos = new GerenciadorDeQuartos();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════╗");
            Console.WriteLine("║     SISTEMA DE HOTELARIA         ║");
            Console.WriteLine("╚═══════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("1. Cadastrar Suite");
            Console.WriteLine("2. Listar Suites Disponíveis");
            Console.WriteLine("3. Remover Suite");
            Console.WriteLine("4. Realizar Check-in");
            Console.WriteLine("5. Listar Reservas Ativas");
            Console.WriteLine("6. Realizar Checkout");
            Console.WriteLine("7. Cadastrar Cliente (avulso)");
            Console.WriteLine("0. Sair");
            Console.WriteLine();
            Console.Write("Escolha uma opção: ");

            string? opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    gerenciadorQuartos.AdicionarSuite();
                    break;

                case "2":
                    GerenciadorDeQuartos.ListarSuitesDisponiveis();
                    Pausar();
                    break;

                case "3":
                    gerenciadorQuartos.RemoverSuite();
                    Pausar();
                    break;

                case "4":
                    GerenciadorDeReservas.RealizarCheckin();
                    Pausar();
                    break;

                case "5":
                    GerenciadorDeReservas.ListarReservasAtivas();
                    Pausar();
                    break;

                case "6":
                    GerenciadorDeReservas.RealizarCheckout();
                    Pausar();
                    break;

                case "7":
                    CadastrarClienteAvulso();
                    break;

                case "0":
                    Console.Clear();
                    Console.WriteLine("Saindo...");
                    Thread.Sleep(1000);
                    return;

                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    Pausar();
                    break;
            }
        }
    }

    static void Pausar()
    {
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    static void CadastrarClienteAvulso()
    {
        Console.Clear();
        Console.WriteLine("=== CADASTRO DE CLIENTE AVULSO ===");
        
        try
        {
            var cliente = Pessoa.RetornarCliente();
            if (cliente != null)
            {
                Console.WriteLine("\n✅ Cliente cadastrado com sucesso!");
                Console.WriteLine(cliente);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
        
        Pausar();
    }
}