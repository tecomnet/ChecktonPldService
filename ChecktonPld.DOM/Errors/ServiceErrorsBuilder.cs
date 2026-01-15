namespace ChecktonPld.DOM.Errors;

public class ServiceErrorsBuilder
{
    // Almacena todos los errores de servicio por su código
    private readonly Dictionary<string, IServiceError> _errors = new();
    private static readonly Lazy<ServiceErrorsBuilder> _instance = new(() => new ServiceErrorsBuilder());

    public static ServiceErrorsBuilder Instance() => _instance.Value;
    public ServiceErrorsBuilder()
    {
        // 1. Carga inicial de todos los catálogos de errores
        GeneralErrors();
        // 2. Carga de errores de validacion de curp
        CurpErrors();
        // 3. Carga de errores de autenticacion
        AuthenticationErrors();
    }

    // Método privado para añadir un error al diccionario
    public void AddServiceError(string errorCode, string message, string description)
    {
        _errors[errorCode] = new ServiceError(errorCode, message, description);
    }

    /// <summary>
    /// Método público utilizado por el Middleware (capa .api) para obtener los detalles de un error.
    /// </summary>
    /// <param name="errorCode">El código de error constante (ej: "EM-MONITOR-DB-ERROR").</param>
    /// <returns>El objeto ServiceError con todos los detalles.</returns>
    public IServiceError GetError(string errorCode)
    {
        if (_errors.TryGetValue(errorCode, out var error))
        {
            return error;
        }

        // Retorna un error de configuración si el código no fue definido (Error 500)
        return new ServiceError(
            errorCode: "ERROR-CODE-NO-DEFINIDO",
            message: "Error de Configuración Interna",
            description: $"El código de error '{errorCode}' no fue definido en el catálogo.");
    }

    #region Constantes y Carga de Errores

    public const string ApiErrorNoManejado = "API-ERROR-NO-MANEJADO";
    private void GeneralErrors()
    {
        // Error interno no manejado
        AddServiceError(
            errorCode: ApiErrorNoManejado, // Usa la constante pública
            message: "Error Interno del Servidor",
            description: "Ocurrió un error inesperado que ha sido registrado. Inténtelo de nuevo más tarde.");
    }

    public const string NombreEstadoRequerido = "NOMBRE-ESTADO-REQUERIDO";
    public const string NombreEstadoInvalido = "NOMBRE-ESTADO-INVALIDO";
    private void CurpErrors()
    {
        // Error nombre estado requerido
        AddServiceError(
            errorCode: NombreEstadoRequerido,
            message: "El nombre del estado es requerido.",
            description: "El nombre del estado no puede ser nulo o vacío.");
        // Error nombre estado no valido
        AddServiceError(
            errorCode: NombreEstadoInvalido,
            message: "El nombre del estado no es válido.",
            description: "El nombre del estado no es válido.");
    }
   
    #endregion
    
    #region Autentication errors

    public const string EmClaimUserError = "EM-CLAIM-USER-ERROR";
    private void AuthenticationErrors()
    {
        // Error de autenticación
        AddServiceError(
            errorCode: EmClaimUserError,
            message: "Error de autenticación",
            description: "El user de autenticación no es válido o no fue encontrado");
    }
		
    #endregion
}