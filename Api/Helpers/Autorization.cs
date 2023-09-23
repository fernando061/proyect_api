namespace Api.Helpers;
public class Autorization
{
    public enum Roles
    {
        Administrador,
        Gerente,
        Empleado
    }

    public const Roles rol_predeterminado = Roles.Empleado;
}

