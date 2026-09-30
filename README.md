# Sistema de Hotelaria

Sistema de reservas de suítes em console, feito como desafio do curso da DIO.
C# / .NET 8, sem banco de dados — os dados vivem em memória durante a execução.

## O que o sistema faz

Menu de sete operações: cadastrar suíte, listar disponíveis, remover suíte,
check-in, listar reservas ativas, checkout e cadastro de cliente avulso.

O fluxo é o de hotelaria real: a suíte só entra no checkbox de disponíveis quando é
liberada no checkout, e reservar uma suíte já ocupada é recusado.

## O que treina

**Validação no setter, não no método.** Cada propriedade valida a si mesma:

```csharp
public Suite SuiteReservada
{
    get => _suiteReservada;
    set
    {
        if (value is null)
            throw new ArgumentException("Suíte não pode ser nula.");

        if (!value.Disponivel)
            throw new InvalidOperationException("Suíte indisponível.");

        value.Disponivel = false;   // reserva ocupa a suíte
        _suiteReservada = value;
    }
}
```

O efeito: **não existe caminho no código que crie uma reserva inválida**, porque
toda reserva passa por aqui. Se amanhã alguém chamar `SuiteReservada` de outro lugar,
a regra continua valendo. Com validação no método, cada ponto de entrada precisaria
lembrar de validar.

**Campo privado atrás da propriedade.** `_suiteReservada` existe para evitar recursão
infinita: o setter altera o valor, o getter lê. Usar a própria propriedade dentro do
setter realimentaria a atribuição.

**Nullable reference types ligados.** `Chekout` é `DateTime?` porque é `null` até o
check-out acontecer — a diferença entre "não checked out" e "check-out não existe" é
que o primeiro é um estado válido da reserva.

**`Guid` no lugar de contador.** A reserva ganha `Guid.NewGuid().ToString()[..8]`, um
identificador curto sem colisão e sem estado global de sequência.

**Namespace por módulo.** `lib.suite`, `lib.pessoa`, `lib.gerenciadordequartos`,
`lib.gerenciadordereservas` — a organização segue o domínio, não o tipo de dado.

## Dependência

`Cpf.Cnpj` (1.0.2) para validação de CPF no cadastro de hóspedes.

## Como rodar

```bash
dotnet run --project src
```

Requer .NET 8.

## Estrutura

```
src/
├── Program.cs                    menu e despacho de comandos
├── 9_ReservasSuite.csproj
└── lib/
    ├── Pessoa.cs                 hóspede, com validação de CPF
    ├── Suite.cs                  suíte, com capacidade e diária validadas
    ├── GerenciadorDeQuartos.cs    cadastro e listagem de suítes
    └── GerenciadorDeReservas.cs   check-in, check-out e estado da reserva
```

## Licença

MIT
