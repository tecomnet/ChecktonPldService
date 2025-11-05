using ChecktonPld.DOM;
using ChecktonPld.DOM.ApplicationDbContext;
using ChecktonPld.DOM.Enums;
using ChecktonPld.DOM.Errors;
using ChecktonPld.DOM.Modelos;
using ChecktonPld.Funcionalidad.Helper;
using ChecktonPld.Funcionalidad.ServiceClient;

namespace ChecktonPld.Funcionalidad.Functionality;

public class ValidacionCurpFacade(ServiceDbContext context, IChecktonPLDAPIServicesFacade checktonPldapiServicesFacade) : IValidacionCurpFacade
{
    public async Task<ValidacionCurp> ValidarCurpAsync(string nombre, string primerApellido, string segundoApellido, DateOnly fechaNacimiento, Genero genero, string estado, string nombreServicioCliente, Guid creationUser, string? testCase = null)
    {
        try
        {
            // Genera la curp con el helper
            var curpGenerada = GeneradorCURP.GenerarCURP(
                primerApellido: primerApellido,
                segundoApellido: segundoApellido,
                nombre: nombre,
                fechaNacimiento: fechaNacimiento.ToDateTime(TimeOnly.MinValue),
                sexo: genero.ToString(),
                entidadFederativa: estado);
            // LLamar a servicio de checkton
            var response = await checktonPldapiServicesFacade.ValidacionChecktonRenapo(curpGenerada);
            // Nueva validacion a guardar
            var validacionCurp = new ValidacionCurp(
                nombre: nombre,
                primerApellido: primerApellido,
                segundoApellido: segundoApellido,
                fechaNacimiento: fechaNacimiento,
                genero: genero,
                curpGenerada: curpGenerada,
                nombreEstado: estado,
                nombreServicioCliente: nombreServicioCliente,
                tipoCheckton: TipoCheckton.Curp,
                validationId: response.Validation_id,
                success: response.Success,
                creationUser: creationUser,
                testCase: testCase);
            // Guarda la validacion
            await context.AddAsync(validacionCurp);
            await context.SaveChangesAsync();
            return validacionCurp;
        }
        catch (Exception exception) when (exception is not EMGeneralAggregateException)
        {
            // Throw an aggregate exception
            throw GenericExceptionManager.GetAggregateException(
                serviceName: DomCommon.ServiceName,
                module: this.GetType().Name,
                exception: exception);
        }
    }

    public async Task<ValidacionCurp> ObtenerValidacionCurpAsync(string curp)
    {
        throw new NotImplementedException();
    }
}