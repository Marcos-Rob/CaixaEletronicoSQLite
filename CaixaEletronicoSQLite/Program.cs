bool sendoUsado = true;

while (sendoUsado)
{

    Console.WriteLine(
@"----- Banco Marcos -----

    1 - Criar Conta
    2 - Consultar Saldo de uma conta
    3 - Consultar Histórico de uma conta
    4 - Sacar de uma conta
    5 - Depositar em uma conta
    6 - Transferir de uma conta para outra
    0 - Sair

    Escolha uma opção: ");
}

string escolha = Console.ReadLine();

switch (escolha)
{
    case "1":
        break;

    case "2":
        break;

    case "3":
        break;

    case "4":
        break;

    case "5":
        break;

    case "6":
        break;

    case "0":
        sendoUsado = false;
        break;

    default:
        Console.WriteLine("Opção inválida. Tente novamente.");
        break;
}