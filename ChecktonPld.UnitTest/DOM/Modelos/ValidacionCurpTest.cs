using ChecktonPld.DOM.Enums;
using ChecktonPld.DOM.Errors;
using ChecktonPld.DOM.Modelos;
using Xunit.Sdk;

namespace ChecktonPld.UnitTest.DOM.Modelos;

public class ValidacionCurpTest : UnitTestTemplate
{
    [Theory]
    // CASOS DE ÉXITO
    [InlineData("1. OK: Full Valid",
        "Juan", "Pérez", "Gómez", "1990-05-15", Genero.Masculino, 
        "Ciudad de México", "PEGJ900515HDFRMN01", TipoCheckton.Curp, 
        "Servicio Cliente", "val123", true, true, new string[] { })]
    
    [InlineData("2. OK: Min length names",
        "A", "B", "C", "2000-01-01", Genero.Femenino, 
        "E", "ABCD000101MDFXYZ01", TipoCheckton.Curp, 
        "S", "v1", false, true, new string[] { })]
    
    [InlineData("3. OK: Max length names",
        "Nombre muy largo que tiene 100 caracteres. Un total de cien caracteres para el nombre.",
        "Apellido muy largo que tiene 100 caracteres. Un total de cien caracteres para el apellido.",
        "Segundo muy largo que tiene 100 caracteres. Un total de cien caracteres para el segundo.",
        "1985-12-31", Genero.Masculino,
        "Estado muy largo que tiene 100 caracteres. Un total de cien caracteres para el estado.",
        "ABCD851231HDFXYZ01", TipoCheckton.Curp,
        "Servicio muy largo que tiene 100 caracteres. Un total de cien caracteres para el servicio.",
        "ValidationId muy largo que tiene 100 caracteres. Un total de cien caracteres para el validation.",
        true, true, new string[] { })]

    // CASOS DE ERROR PARA NOMBRE (string, min 1, max 100)
    [InlineData("4. ERROR: Nombre null",
        null, "Pérez", "Gómez", "1990-05-15", Genero.Masculino,
        "Ciudad de México", "PEGJ900515HDFRMN01", TipoCheckton.Curp,
        "Servicio Cliente", "val123", true, false, new string[] { "PROPERTY-VALIDATION-REQUIRED-ERROR" })]
    
    [InlineData("5. ERROR: Nombre empty",
        "", "Pérez", "Gómez", "1990-05-15", Genero.Masculino,
        "Ciudad de México", "PEGJ900515HDFRMN01", TipoCheckton.Curp,
        "Servicio Cliente", "val123", true, false, new string[] { "PROPERTY-VALIDATION-REQUIRED-ERROR" })]
    
    [InlineData("6. ERROR: Nombre too long",
        "Este nombre es demasiado largo y tiene más de 100 caracteres. Superamos el límite de cien caracteres por poco y esto debería fallar en la validación.",
        "Pérez", "Gómez", "1990-05-15", Genero.Masculino,
        "Ciudad de México", "PEGJ900515HDFRMN01", TipoCheckton.Curp,
        "Servicio Cliente", "val123", true, false, new string[] { "PROPERTY-VALIDATION-LENGTH-INVALID" })]

    // CASOS DE ERROR PARA PRIMER APELLIDO
    [InlineData("7. ERROR: PrimerApellido null",
        "Juan", null, "Gómez", "1990-05-15", Genero.Masculino,
        "Ciudad de México", "PEGJ900515HDFRMN01", TipoCheckton.Curp,
        "Servicio Cliente", "val123", true, false, new string[] { "PROPERTY-VALIDATION-REQUIRED-ERROR" })]
    
    [InlineData("8. ERROR: PrimerApellido empty",
        "Juan", "", "Gómez", "1990-05-15", Genero.Masculino,
        "Ciudad de México", "PEGJ900515HDFRMN01", TipoCheckton.Curp,
        "Servicio Cliente", "val123", true, false, new string[] { "PROPERTY-VALIDATION-REQUIRED-ERROR" })]

    // CASOS DE ERROR PARA SEGUNDO APELLIDO
    [InlineData("9. ERROR: SegundoApellido null",
        "Juan", "Pérez", null, "1990-05-15", Genero.Masculino,
        "Ciudad de México", "PEGJ900515HDFRMN01", TipoCheckton.Curp,
        "Servicio Cliente", "val123", true, false, new string[] { "PROPERTY-VALIDATION-REQUIRED-ERROR" })]
    
    [InlineData("10. ERROR: SegundoApellido empty",
        "Juan", "Pérez", "", "1990-05-15", Genero.Masculino,
        "Ciudad de México", "PEGJ900515HDFRMN01", TipoCheckton.Curp,
        "Servicio Cliente", "val123", true, false, new string[] { "PROPERTY-VALIDATION-REQUIRED-ERROR" })]

    // CASOS DE ERROR PARA NOMBRE ESTADO
    [InlineData("12. ERROR: NombreEstado null",
        "Juan", "Pérez", "Gómez", "1990-05-15", Genero.Masculino,
        null, "PEGJ900515HDFRMN01", TipoCheckton.Curp,
        "Servicio Cliente", "val123", true, false, new string[] { "PROPERTY-VALIDATION-REQUIRED-ERROR" })]
    
    [InlineData("13. ERROR: NombreEstado too long",
        "Juan", "Pérez", "Gómez", "1990-05-15", Genero.Masculino,
        "Este estado es demasiado largo y tiene más de 100 caracteres. Superamos el límite de cien caracteres por poco y esto debería fallar.",
        "PEGJ900515HDFRMN01", TipoCheckton.Curp,
        "Servicio Cliente", "val123", true, false, new string[] { "PROPERTY-VALIDATION-LENGTH-INVALID" })]

    // CASOS DE ERROR PARA CURP GENERADA
    [InlineData("14. ERROR: CurpGenerada null",
        "Juan", "Pérez", "Gómez", "1990-05-15", Genero.Masculino,
        "Ciudad de México", null, TipoCheckton.Curp,
        "Servicio Cliente", "val123", true, false, new string[] { "PROPERTY-VALIDATION-REQUIRED-ERROR" })]
    
    [InlineData("15. ERROR: CurpGenerada too short",
        "Juan", "Pérez", "Gómez", "1990-05-15", Genero.Masculino,
        "Ciudad de México", "ABC", TipoCheckton.Curp,
        "Servicio Cliente", "val123", true, false, new string[] { "PROPERTY-VALIDATION-LENGTH-INVALID" })]
    
    [InlineData("16. ERROR: CurpGenerada too long",
        "Juan", "Pérez", "Gómez", "1990-05-15", Genero.Masculino,
        "Ciudad de México", "PEGJ900515HDFRMN01EXTRA", TipoCheckton.Curp,
        "Servicio Cliente", "val123", true, false, new string[] { "PROPERTY-VALIDATION-LENGTH-INVALID" })]
    
    // CASOS DE ERROR PARA NOMBRE SERVICIO CLIENTE
    [InlineData("17. ERROR: NombreServicioCliente null",
        "Juan", "Pérez", "Gómez", "1990-05-15", Genero.Masculino,
        "Ciudad de México", "PEGJ900515HDFRMN01", TipoCheckton.Curp,
        null, "val123", true, false, new string[] { "PROPERTY-VALIDATION-REQUIRED-ERROR" })]
    
    [InlineData("18. ERROR: NombreServicioCliente too long",
        "Juan", "Pérez", "Gómez", "1990-05-15", Genero.Masculino,
        "Ciudad de México", "PEGJ900515HDFRMN01", TipoCheckton.Curp,
        "Servicio cliente muy largo que tiene más de 100 caracteres. Superamos el límite de cien caracteres por poco y esto debería fallar.",
        "val123", true, false, new string[] { "PROPERTY-VALIDATION-LENGTH-INVALID" })]

    // CASOS DE ERROR PARA VALIDATION ID
    [InlineData("19. ERROR: ValidationId null",
        "Juan", "Pérez", "Gómez", "1990-05-15", Genero.Masculino,
        "Ciudad de México", "PEGJ900515HDFRMN01", TipoCheckton.Curp,
        "Servicio Cliente", null, true, false, new string[] { "PROPERTY-VALIDATION-REQUIRED-ERROR" })]
    
    [InlineData("20. ERROR: ValidationId too long",
        "Juan", "Pérez", "Gómez", "1990-05-15", Genero.Masculino,
        "Ciudad de México", "PEGJ900515HDFRMN01", TipoCheckton.Curp,
        "Servicio Cliente", 
        "ValidationId muy largo que tiene más de 100 caracteres. Superamos el límite de cien caracteres por poco y esto debería fallar en la validación del sistema.",
        true, false, new string[] { "PROPERTY-VALIDATION-LENGTH-INVALID" })]
    
    // CASO DE ERROR MÚLTIPLE
    [InlineData("21. ERROR: Multiple errors",
        null, "", null, "0001-01-01", default(Genero),
        null, "ABC", default(TipoCheckton),
        null, null, default(bool), false, new string[] { 
            "PROPERTY-VALIDATION-REQUIRED-ERROR", // Nombre
            "PROPERTY-VALIDATION-REQUIRED-ERROR", // PrimerApellido
            "PROPERTY-VALIDATION-REQUIRED-ERROR", // SegundoApellido
            "PROPERTY-VALIDATION-REQUIRED-ERROR", // FechaNacimiento
            "PROPERTY-VALIDATION-REQUIRED-ERROR", // Genero
            "PROPERTY-VALIDATION-REQUIRED-ERROR", // NombreEstado
            "PROPERTY-VALIDATION-LENGTH-INVALID", // CurpGenerada (length)
            "PROPERTY-VALIDATION-REQUIRED-ERROR", // TipoCheckton
            "PROPERTY-VALIDATION-REQUIRED-ERROR", // NombreServicioCliente
            "PROPERTY-VALIDATION-REQUIRED-ERROR", // ValidationId
            "PROPERTY-VALIDATION-REQUIRED-ERROR"  // Success
        })]
    public void ValidacionCurpConstructorTest(
        // Case name
        string caseName,
        // Test data
        string nombre,
        string primerApellido,
        string segundoApellido,
        string fechaNacimientoStr,
        Genero genero,
        string nombreEstado,
        string curpGenerada,
        TipoCheckton tipoCheckton,
        string nombreServicioCliente,
        string validationId,
        bool success,
        // Result
        bool expectedSuccess,
        string[]? expectedErrors = null
    )
    {
        try
        {
            // Convertir string a DateOnly
            DateOnly fechaNacimiento = DateOnly.Parse(fechaNacimientoStr);

            // Crea la ValidacionCurp
            var validacion = new ValidacionCurp(
                nombre: nombre,
                primerApellido: primerApellido,
                segundoApellido: segundoApellido,
                fechaNacimiento: fechaNacimiento,
                genero: genero,
                nombreEstado: nombreEstado,
                curpGenerada: curpGenerada,
                tipoCheckton: tipoCheckton,
                nombreServicioCliente: nombreServicioCliente,
                validationId: validationId,
                success: success,
                creationUser: Guid.NewGuid(),
                testCase: caseName);

            // Check the properties (solo si es éxito)
            Assert.True(validacion.Nombre == nombre, $"Nombre no es correcto. Actual: {validacion.Nombre}");
            Assert.True(validacion.PrimerApellido == primerApellido, $"PrimerApellido no es correcto. Actual: {validacion.PrimerApellido}");
            Assert.True(validacion.SegundoApellido == segundoApellido, $"SegundoApellido no es correcto. Actual: {validacion.SegundoApellido}");
            Assert.True(validacion.FechaNacimiento == fechaNacimiento, $"FechaNacimiento no es correcto. Actual: {validacion.FechaNacimiento}");
            Assert.True(validacion.Genero == genero, $"Genero no es correcto. Actual: {validacion.Genero}");
            Assert.True(validacion.NombreEstado == nombreEstado, $"NombreEstado no es correcto. Actual: {validacion.NombreEstado}");
            Assert.True(validacion.CurpGenerada == curpGenerada, $"CurpGenerada no es correcto. Actual: {validacion.CurpGenerada}");
            Assert.True(validacion.TipoCheckton == tipoCheckton, $"TipoCheckton no es correcto. Actual: {validacion.TipoCheckton}");
            Assert.True(validacion.NombreServicioCliente == nombreServicioCliente, $"NombreServicioCliente no es correcto. Actual: {validacion.NombreServicioCliente}");
            Assert.True(validacion.ValidationId == validationId, $"ValidationId no es correcto. Actual: {validacion.ValidationId}");
            Assert.True(validacion.Success == success, $"Success no es correcto. Actual: {validacion.Success}");

            // Assert success
            Assert.True(expectedSuccess, "Should not reach on failures.");
        }
        // Catch the managed errors and check them with the expected ones in the case of failures
        catch (EMGeneralAggregateException exception)
        {
            // Treat the raised error
            CatchErrors(caseName: caseName, success: expectedSuccess, expectedErrors: expectedErrors, exception: exception);
        }
        // Catch any non managed errors and display them to understand the root cause
        catch (Exception exception) when (exception is not EMGeneralAggregateException &&
                                             exception is not TrueException && exception is not FalseException)
        {
            // Should not reach for unmanaged errors
            Assert.Fail($"Uncaught exception. {exception.Message}");
        }
    }
}