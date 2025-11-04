using ChecktonPld.DOM.Enums;
using ChecktonPld.DOM.Modelos;

namespace ChecktonPld.Funcionalidad.Functionality;

public interface IValidacionCurpFacade
{
    public Task<ValidacionCurp> ValidarChecktonPldAsync(string nombre, string primerApellido, string segundoApellido, DateTime fechaNacimiento, Genero genero, string estado, string nombreServicioCliente);
}