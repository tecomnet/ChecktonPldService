using ChecktonPld.DOM.Enums;
using ChecktonPld.DOM.Modelos;

namespace ChecktonPld.Funcionalidad.Functionality;

public interface IValidacionCurpFacade
{
    public Task<ValidacionCurp> ValidarCurpAsync(string nombre, string primerApellido, string segundoApellido, DateOnly fechaNacimiento, Genero genero, string estado, string nombreServicioCliente, Guid creationUser, string? testCase = null);
    public Task<ValidacionCurp> ObtenerValidacionCurpAsync(string curp);
}