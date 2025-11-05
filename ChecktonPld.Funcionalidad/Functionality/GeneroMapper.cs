using ChecktonPld.DOM.Enums;
using ChecktonPld.Funcionalidad.GeneradorCurp.Enums;

namespace ChecktonPld.Funcionalidad.Functionality;

// Función de mapeo (Ejemplo)
public static class GeneroMapper
{
    /// <summary>
    /// Convierte el enum Genero (int) al enum Sexo (char) compatible con CURP.
    /// </summary>
    /// <param name="genero">El valor del enum Genero original.</param>
    /// <returns>El valor correspondiente del enum Sexo.</returns>
    public static Sexo MapToSexo(Genero genero)
    {
        return genero switch
        {
            Genero.Masculino => Sexo.Hombre,
            Genero.Femenino => Sexo.Mujer,
            Genero.NoBinario => Sexo.NoBinario,
            _ => Sexo.NoBinario, // Valor por defecto si hay un caso no mapeado
        };
    }
}