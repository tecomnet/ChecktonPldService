using System.Text;
using System.Text.RegularExpressions;

namespace ChecktonPld.Funcionalidad.Helper;

public class GeneradorCURP
{
    public static string GenerarCURP(string primerApellido, string segundoApellido, string nombre, 
                                    DateTime fechaNacimiento, string sexo, string entidadFederativa)
    {
        // Validaciones básicas
        if (string.IsNullOrWhiteSpace(primerApellido) || 
            string.IsNullOrWhiteSpace(nombre) ||
            string.IsNullOrWhiteSpace(sexo) ||
            string.IsNullOrWhiteSpace(entidadFederativa))
        {
            throw new ArgumentException("Todos los campos son obligatorios");
        }

        var curpBuilder = new StringBuilder();

        // 1-4: Letras de apellidos y nombre
        curpBuilder.Append(ObtenerLetraApellido(primerApellido, 1));
        curpBuilder.Append(ObtenerVocalInterna(primerApellido));
        curpBuilder.Append(ObtenerLetraApellido(segundoApellido, 1));
        curpBuilder.Append(ObtenerPrimeraLetraNombre(nombre));

        // 5-10: Fecha de nacimiento (AAMMDD)
        curpBuilder.Append(fechaNacimiento.ToString("yyMMdd"));

        // 11: Sexo
        curpBuilder.Append(sexo.ToUpper() == "H" ? "H" : "M");

        // 12-13: Entidad federativa
        curpBuilder.Append(entidadFederativa.ToUpper());

        // 14-16: Consonantes internas
        curpBuilder.Append(ObtenerConsonanteInterna(primerApellido));
        curpBuilder.Append(ObtenerConsonanteInterna(segundoApellido));
        curpBuilder.Append(ObtenerConsonanteInterna(nombre));

        // 17: Diferencial de siglo
        curpBuilder.Append(ObtenerDiferencialSiglo(fechaNacimiento.Year));

        // 18: Dígito verificador
        string curpSinDigito = curpBuilder.ToString();
        curpBuilder.Append(CalcularDigitoVerificador(curpSinDigito));

        return curpBuilder.ToString();
    }

    private static char ObtenerLetraApellido(string apellido, int posicion)
    {
        if (string.IsNullOrWhiteSpace(apellido)) return 'X';
        
        // Remover caracteres especiales y convertir a mayúsculas
        string limpio = LimpiarTexto(apellido);
        
        // Para apellidos compuestos, tomar la primera parte
        if (limpio.Contains(' '))
        {
            limpio = limpio.Split(' ')[0];
        }

        return limpio.Length >= posicion ? limpio[posicion - 1] : 'X';
    }

    private static char ObtenerPrimeraLetraNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre)) return 'X';
        
        string limpio = LimpiarTexto(nombre);
        
        // Si el nombre es compuesto y comienza con "MARIA" o "JOSE", usar el segundo nombre
        if (limpio.StartsWith("MARIA") || limpio.StartsWith("JOSE"))
        {
            string[] partes = limpio.Split(' ');
            if (partes.Length > 1)
            {
                return partes[1][0];
            }
        }

        return limpio[0];
    }

    private static char ObtenerVocalInterna(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto)) return 'X';
        
        string limpio = LimpiarTexto(texto);
        
        // Buscar la primera vocal después de la primera letra
        for (int i = 1; i < limpio.Length; i++)
        {
            if (EsVocal(limpio[i]))
            {
                return limpio[i];
            }
        }
        
        return 'X';
    }

    private static char ObtenerConsonanteInterna(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto)) return 'X';
        
        string limpio = LimpiarTexto(texto);
        
        // Buscar la primera consonante después de la primera letra
        for (int i = 1; i < limpio.Length; i++)
        {
            if (!EsVocal(limpio[i]) && char.IsLetter(limpio[i]))
            {
                return limpio[i];
            }
        }
        
        return 'X';
    }

    private static char ObtenerDiferencialSiglo(int año)
    {
        return año >= 2000 ? 'A' : '0';
    }
    
    private static char CalcularDigitoVerificador(string curpSinDigito)
    {
        if (curpSinDigito.Length != 17)
            throw new ArgumentException("El CURP sin dígito debe tener 17 caracteres");

        // Tabla de valores para el cálculo
        string caracteres = "0123456789ABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
        int suma = 0;

        for (int i = 0; i < 17; i++)
        {
            int valor = caracteres.IndexOf(curpSinDigito[i]);
            if (valor == -1) valor = 0;
            
            suma += valor * (18 - i);
        }

        int residuo = suma % 10;
        int digito = (10 - residuo) % 10;

        return digito.ToString()[0];
    }

    private static string LimpiarTexto(string texto)
    {
        // Remover acentos y caracteres especiales
        texto = texto.ToUpper();
        
        // Reemplazar caracteres especiales
        texto = texto.Replace("Á", "A")
            .Replace("É", "E")
            .Replace("Í", "I")
            .Replace("Ó", "O")
            .Replace("Ú", "U")
            .Replace("Ü", "U")
            .Replace("Ñ", "X"); // La Ñ se convierte en X según reglas oficiales

        // Remover caracteres no alfabéticos (excepto espacios)
        texto = Regex.Replace(texto, @"[^A-Z\s]", "");
        
        return texto.Trim();
    }

    private static bool EsVocal(char c)
    {
        return "AEIOU".Contains(c);
    }

}