namespace GrillSystem.Authorization;

public static class Perfis
{
    public const string Administrador = "Administrador";
    public const string Gerente = "Gerente";
    public const string Operador = "Operador";
    public const string MestreProducao = "Mestre de Produção";
    public const string Financeiro = "Financeiro";
    public const string Vendedor = "Vendedor";
    public const string Comprador = "Comprador";

    public static readonly string[] Todos =
        [Administrador, Gerente, MestreProducao, Financeiro, Vendedor, Comprador];
}

public static class Politicas
{
    public const string GerenciarUsuarios = nameof(GerenciarUsuarios);
    public const string GerenciarSistema = nameof(GerenciarSistema);
}
