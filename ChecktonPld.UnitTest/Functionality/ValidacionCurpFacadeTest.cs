using ChecktonPld.DOM.Enums;
using ChecktonPld.DOM.Errors;
using ChecktonPld.Funcionalidad.Functionality;
using ChecktonPld.UnitTest.Functionality.Configuration;
using Microsoft.EntityFrameworkCore;
using Xunit.Sdk;

namespace ChecktonPld.UnitTest.Functionality;

public class ValidacionCurpFacadeTest(SetupDataConfig setupConfig)
    : BaseFacadeTest<IValidacionCurpFacade>(setupConfig) 
{
    [Theory]
    // CASOS DE ÉXITO
    [InlineData("1. Successfully case, valid CURP generation and validation",
        "Enrique", "Escandon", "Cruz", "1989-04-30", Genero.Masculino, "Distrito Federal",
        "Servicio Cliente Test", true, new string[] { })]

    /*[InlineData("2. Successfully case, female gender and different state",
        "María", "García", "López", "1985-12-25", Genero.Femenino, "Jalisco",
        "Servicio Cliente Web", true, new string[] { })]

    [InlineData("3. Successfully case, minimum length names",
        "Ana", "L", "M", "2000-01-01", Genero.Femenino, "NL",
        "S", true, new string[] { })]*/

    public async Task ValidarCurpAsyncTest(
        string caseName,
        string nombre,
        string primerApellido,
        string segundoApellido,
        string fechaNacimientoStr,
        Genero genero,
        string estado,
        string nombreServicioCliente,
        bool success,
        string[] expectedErrors)
    {
        try
        {
            // Convertir string a DateOnly
            DateOnly fechaNacimiento = DateOnly.Parse(fechaNacimientoStr);
            // Llamar al método bajo prueba
            var validacionCurp = await Facade.ValidarCurpAsync(
                nombre: nombre,
                primerApellido: primerApellido,
                segundoApellido: segundoApellido,
                fechaNacimiento: fechaNacimiento,
                genero: genero,
                estado: estado,
                nombreServicioCliente: nombreServicioCliente,
                creationUser: SetupConfig.UserId,
                testCase: SetupConfig.TestCaseId);

            // ASSERT: Validar que se retornó el objeto
            Assert.NotNull(validacionCurp);

            // ASSERT: Validar propiedades del objeto retornado
            Assert.True(validacionCurp.Nombre == nombre,
                $"Nombre no coincide. Esperado: {nombre}, Actual: {validacionCurp.Nombre}");
            Assert.True(validacionCurp.PrimerApellido == primerApellido,
                $"PrimerApellido no coincide. Esperado: {primerApellido}, Actual: {validacionCurp.PrimerApellido}");
            Assert.True(validacionCurp.SegundoApellido == segundoApellido,
                $"SegundoApellido no coincide. Esperado: {segundoApellido}, Actual: {validacionCurp.SegundoApellido}");
            Assert.True(validacionCurp.FechaNacimiento == fechaNacimiento,
                $"FechaNacimiento no coincide. Esperado: {fechaNacimiento}, Actual: {validacionCurp.FechaNacimiento}");
            Assert.True(validacionCurp.Genero == genero,
                $"Genero no coincide. Esperado: {genero}, Actual: {validacionCurp.Genero}");
            Assert.True(validacionCurp.NombreEstado == estado,
                $"NombreEstado no coincide. Esperado: {estado}, Actual: {validacionCurp.NombreEstado}");
            Assert.True(validacionCurp.NombreServicioCliente == nombreServicioCliente,
                $"NombreServicioCliente no coincide. Esperado: {nombreServicioCliente}, Actual: {validacionCurp.NombreServicioCliente}");
            Assert.True(validacionCurp.CreationUser == SetupConfig.UserId,
                $"CreationUser no coincide. Esperado: {SetupConfig.UserId}, Actual: {validacionCurp.CreationUser}");

            // ASSERT: Validar que se generó la CURP
            Assert.False(string.IsNullOrEmpty(validacionCurp.CurpGenerada),
                "La CURP generada no debe ser nula o vacía");
            Assert.True(validacionCurp.CurpGenerada.Length == 18,
                $"La CURP generada debe tener 18 caracteres. Actual: {validacionCurp.CurpGenerada.Length}");

            // ASSERT: Validar datos guardados en base de datos
            var validacionFromDb = await Context.ValidacionCurp
                .AsNoTracking()
                .Where(v => v.ValidationId == validacionCurp.ValidationId)
                .SingleOrDefaultAsync();

            Assert.NotNull(validacionFromDb);
            Assert.True(validacionFromDb.Nombre == nombre,
                $"Nombre en BD no coincide. Esperado: {nombre}, Actual: {validacionFromDb.Nombre}");
            Assert.True(validacionFromDb.PrimerApellido == primerApellido,
                $"PrimerApellido en BD no coincide. Esperado: {primerApellido}, Actual: {validacionFromDb.PrimerApellido}");
            Assert.True(validacionFromDb.SegundoApellido == segundoApellido,
                $"SegundoApellido en BD no coincide. Esperado: {segundoApellido}, Actual: {validacionFromDb.SegundoApellido}");
            Assert.True(validacionFromDb.FechaNacimiento == fechaNacimiento,
                $"FechaNacimiento en BD no coincide. Esperado: {fechaNacimiento}, Actual: {validacionFromDb.FechaNacimiento}");
            Assert.True(validacionFromDb.Genero == genero,
                $"Genero en BD no coincide. Esperado: {genero}, Actual: {validacionFromDb.Genero}");
            Assert.True(validacionFromDb.NombreEstado == estado,
                $"NombreEstado en BD no coincide. Esperado: {estado}, Actual: {validacionFromDb.NombreEstado}");
            Assert.True(validacionFromDb.CurpGenerada == validacionCurp.CurpGenerada,
                $"CurpGenerada en BD no coincide. Esperado: {validacionCurp.CurpGenerada}, Actual: {validacionFromDb.CurpGenerada}");
            Assert.True(validacionFromDb.TipoCheckton == TipoCheckton.Curp,
                $"TipoCheckton en BD no coincide. Esperado: {TipoCheckton.Curp}, Actual: {validacionFromDb.TipoCheckton}");
            Assert.True(validacionFromDb.ValidationId == validacionCurp.ValidationId,
                $"ValidationId en BD no coincide. Esperado: {validacionCurp.ValidationId}, Actual: {validacionFromDb.ValidationId}");
            Assert.True(validacionFromDb.Success == validacionCurp.Success,
                $"Success en BD no coincide. Esperado: {validacionCurp.Success}, Actual: {validacionFromDb.Success}");
            Assert.True(validacionFromDb.CreationUser == SetupConfig.UserId,
                $"CreationUser en BD no coincide. Esperado: {SetupConfig.UserId}, Actual: {validacionFromDb.CreationUser}");

            // ASSERT: Test exitoso
            Assert.True(success, "El test debería ser exitoso");
        }
        // Catch the managed errors and check them with the expected ones in the case of failures
        catch (EMGeneralAggregateException exception)
        {
            // Treat the raised error
            CatchErrors(caseName: caseName, success: success, expectedErrors: expectedErrors, exception: exception);
        }
        // Catch any non managed errors and display them to understand the root cause
        catch (Exception exception) when (exception is not EMGeneralAggregateException &&
                                          exception is not TrueException && exception is not FalseException)
        {
            // Should not reach for unmanaged errors
            Assert.Fail($"Case {caseName}: Uncaught exception. {exception.Message}");
        }
    }
}