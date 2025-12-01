using ChecktonPld.DOM;
using ChecktonPld.DOM.Errors;
using ChecktonPld.Funcionalidad.GeneradorCurp.Enums;

namespace ChecktonPld.Funcionalidad.Functionality.CurpFacade;

// Clase para mapear un string al enum Estado
public static class EstadoMapper
{
    /// <summary>
    /// Convierte el nombre de un estado (con espacios) a su valor de enum Estado (con guiones bajos).
    /// </summary>
    /// <param name="estadoNombre">Nombre del estado (ej. "Baja California Sur" o "Yucatan").</param>
    /// <returns>El valor correspondiente del enum Estado.</returns>
    /// <exception cref="ArgumentException">Lanzada si el string de entrada no es un nombre de estado válido.</exception>
    public static Estado MapStringToEstado(string estadoNombre)
    {
        if (string.IsNullOrWhiteSpace(estadoNombre))
        {
            throw new EMGeneralAggregateException(DomCommon.BuildEmGeneralException(
                errorCode: ServiceErrorsBuilder.NombreEstadoRequerido,
                dynamicContent: [],
                module: "EstadoMapper"));
        }

        // 1. Normalización: Reemplazar espacios por guiones bajos para que coincida con el enum.
        // También reemplazamos el guion (si lo hay) para estandarizar.
        string normalizedName = estadoNombre.Trim()
            .Replace(" ", "_")
            .Replace("-", "_");
        
        // 2. Remover acentos
        normalizedName = normalizedName.RemoveAccentMarks();
        
        // La comparación se hace ignorando mayúsculas/minúsculas (ignoreCase: true)
        if (Enum.TryParse(normalizedName, ignoreCase: true, out Estado result))
        {
            return result;
        }

        // Si la conversión falla, se lanza una excepción indicando que el nombre no es válido.
        throw new EMGeneralAggregateException(DomCommon.BuildEmGeneralException(
            errorCode: ServiceErrorsBuilder.NombreEstadoInvalido,
            dynamicContent: [estadoNombre],
            module: "EstadoMapper"));
    }
}