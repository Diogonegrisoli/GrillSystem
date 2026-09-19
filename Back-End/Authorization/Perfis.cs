namespace GrillSystem.Authorization;

public static class Perfis
{
    public const string Administrador = "Administrador";
    public const string Gerente = "Gerente";
    public const string Operador = "Operador";

    public static readonly string[] Todos = [Administrador, Gerente, Operador];
}

public static class Politicas
{
    public const string GerenciarUsuarios = nameof(GerenciarUsuarios);
    public const string GerenciarSistema = nameof(GerenciarSistema);
}
