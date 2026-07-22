namespace lib.suite
{
    public class Suite
    {
        // 🔥 Campos privados para evitar recursão
        private string _descricaoQuarto;
        private int _capacidade;
        private decimal _valorDiaria;

        public Suite(string descricaoQuarto, int capacidade, decimal valorDiaria)
        {
            DescricaoQuarto = descricaoQuarto;
            Capacidade = capacidade;
            ValorDiaria = valorDiaria;
            Disponivel = true;
        }

        // ===== DESCRIÇÃO =====
        public string DescricaoQuarto
        {
            get => _descricaoQuarto;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Descrição não pode ser vazia.");

                if (value.Length < 3)
                    throw new ArgumentException("Descrição deve ter pelo menos 3 caracteres.");

                _descricaoQuarto = value.Trim();
            }
        }

        // ===== CAPACIDADE =====
        public int Capacidade
        {
            get => _capacidade;
            set
            {
                if (value < 1 || value > 5)
                    throw new ArgumentException("Capacidade deve ser entre 1 e 5.");

                _capacidade = value;
            }
        }

        // ===== VALOR DA DIÁRIA =====
        public decimal ValorDiaria
        {
            get => _valorDiaria;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Valor da diária deve ser maior que zero.");

                _valorDiaria = value;
            }
        }

        // ===== DISPONIBILIDADE =====
        public bool Disponivel { get; set; }

        // ===== ToString =====
        public override string ToString()
        {
            return $"Descrição: {DescricaoQuarto}\n" +
                   $"Capacidade: {Capacidade}\n" +
                   $"Valor da Diária: {ValorDiaria:C}\n" +
                   $"Disponibilidade: {(Disponivel ? "Suite Disponível" : "Suite Indisponível")}\n";
        }
    }
}