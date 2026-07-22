using lib.suite;
namespace lib.gerenciadordequartos
{
    public class GerenciadorDeQuartos
    {
        public static List<Suite> SuitesHotel = new List<Suite>();

        public void AdicionarSuite()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Digite a descrição do quarto");
                    string descricao = Console.ReadLine();

                    Console.WriteLine("Digite a capacidade do quarto");
                    int capacacidade = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Digite o preço da diaria");
                    decimal preco = Convert.ToDecimal(Console.ReadLine());

                    Suite s = new(descricao, capacacidade, preco);
                    SuitesHotel.Add(s);
                    break;

                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine($"Erro: informações inválidas {ex.Message}");
                    Console.WriteLine("Deseja continuar? (s/n)");
                    string continuar = Console.ReadLine();
                    if (continuar.ToLower() is "s")
                    {
                        continue;
                    }
                    else if (continuar.ToLower() is "n")
                    {
                        Console.Clear();
                        Console.WriteLine("saindo");

                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Erro: dados inválidos");
                        continue;
                    }
                }
            }
        }

        public void RemoverSuite()
        {
            // Lista as suites
            foreach (Suite c in SuitesHotel)
            {
                Console.WriteLine($"Suite: {c.DescricaoQuarto}\nEsta ocupada: {(c.Disponivel ? "Não está ocupada" : "Está ocupada")}");
            }

            Console.WriteLine("Digite a Suite que deseja remover:");
            string? descricao = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(descricao))
            {
                Console.WriteLine("Erro: parâmetros inválidos");
                return;
            }

            // 🔥 Busca e remove
            for (int i = 0; i < SuitesHotel.Count; i++)
            {
                if (SuitesHotel[i].DescricaoQuarto.ToLower() == descricao.ToLower())
                {
                    Console.WriteLine($"Suite '{SuitesHotel[i].DescricaoQuarto}' removida com sucesso!");
                    SuitesHotel.RemoveAt(i);
                    return; // Sai do método
                }
            }

            // Se chegou aqui, não encontrou
            Console.WriteLine("Erro: descrição inválida - suite não encontrada.");
        }
        
        public static void ListarSuitesDisponiveis()
        {
            if (SuitesHotel.Count == 0)
            {
                Console.WriteLine("Não há suites cadastradas.");
                return;
            }

            bool encontrouDisponivel = false;

            foreach (Suite s in SuitesHotel)
            {
                if (s.Disponivel)
                {
                    Console.WriteLine(s);
                    encontrouDisponivel = true;
                }
            }

            if (!encontrouDisponivel)
            {
                Console.WriteLine("Não há suites disponíveis no momento.");
            }
        }
    }
}