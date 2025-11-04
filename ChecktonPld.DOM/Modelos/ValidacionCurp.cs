using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChecktonPld.DOM.Comun;
using ChecktonPld.DOM.Enums;
using ChecktonPld.DOM.Errors;

namespace ChecktonPld.DOM.Modelos;

public class ValidacionCurp : ValidatablePersistentObjectLogicalDelete
{
    protected override List<PropertyConstraint> PropertyConstraints =>
    [
        PropertyConstraint.StringPropertyConstraint(
            propertyName: nameof(Nombre),
            isRequired: true,
            minimumLength: 1,
            maximumLength: 100),
        PropertyConstraint.StringPropertyConstraint(
            propertyName: nameof(PrimerApellido),
            isRequired: true,
            minimumLength: 1,
            maximumLength: 100),
        PropertyConstraint.StringPropertyConstraint(
            propertyName: nameof(SegundoApellido),
            isRequired: true,
            minimumLength: 1,
            maximumLength: 100),
        PropertyConstraint.DateOnlyPropertyConstraint(
            propertyName: nameof(FechaNacimiento),
            isRequired: true),
        PropertyConstraint.ObjectPropertyConstraint(
            propertyName: nameof(Genero),
            isRequired: true),
        PropertyConstraint.StringPropertyConstraint(
            propertyName: nameof(NombreEstado),
            isRequired: true,
            minimumLength: 1,
            maximumLength: 100),
        PropertyConstraint.StringPropertyConstraint(
            propertyName: nameof(CurpGenerada),
            isRequired: true,
            minimumLength: 18,
            maximumLength:18),
        PropertyConstraint.ObjectPropertyConstraint(
            propertyName: nameof(TipoCheckton),
            isRequired: true),
        PropertyConstraint.StringPropertyConstraint(
            propertyName: nameof(NombreServicioCliente),
            isRequired: true,
            minimumLength:1,
            maximumLength: 100)
    ];


    [Key]
    public int Id { get; private set; }


    [Required]
    [MaxLength(100)]
    public string? Nombre { get; private set; }

    [Required]
    [MaxLength(100)]
    public string? PrimerApellido { get; private set; }

    [Required]
    [MaxLength(100)]
    public string? SegundoApellido { get; private set; }
    [NotMapped]
    public string? NombreCompleto => $"{this.Nombre} {this.PrimerApellido} {this.SegundoApellido}";

    [Required]
    public DateOnly? FechaNacimiento { get; private set; }
    [Required]
    public Genero? Genero { get; private set; }
    [Required]
    [MaxLength(100)]
    public string NombreEstado { get; private set; }
    [Required]
    [MaxLength(18)]
    public string? CurpGenerada { get; private set; }
    [Required]
    public TipoCheckton TipoCheckton { get; private set; }
    [Required]
    [MaxLength(100)]
    public string NombreServicioCliente { get; private set; }

    public ValidacionCurp() : base()
    {

    }

    /// <summary>
    /// Nueva validacion de curp
    /// </summary>
    /// <param name="nombre"></param>
    /// <param name="primerApellido"></param>
    /// <param name="segundoApellido"></param>
    /// <param name="fechaNacimiento"></param>
    /// <param name="genero"></param>
    /// <param name="nombreEstado"></param>
    /// <param name="curpGenerada"></param>
    /// <param name="tipoCheckton"></param>
    /// <param name="nombreServicioCliente"></param>
    /// <param name="creationUser"></param>
    /// <param name="testCase"></param>
    /// <exception cref="EMGeneralAggregateException"></exception>
    public ValidacionCurp(
        string nombre,
        string primerApellido,
        string segundoApellido,
        DateOnly fechaNacimiento,
        Genero genero,
        string nombreEstado,
        string curpGenerada,
        TipoCheckton tipoCheckton,
        string nombreServicioCliente,
        Guid creationUser,
        string? testCase = null) : base(creationUser, testCase)
    {
        // Initialize the list of exceptions
        List<EMGeneralException> exceptions = new();
        // Validate the properties
        IsPropertyValid(propertyName: nameof(Nombre), value: nombre, exceptions: ref exceptions);
        IsPropertyValid(propertyName: nameof(PrimerApellido), value: primerApellido, exceptions: ref exceptions);
        IsPropertyValid(propertyName: nameof(SegundoApellido), value: segundoApellido, exceptions: ref exceptions);
        IsPropertyValid(propertyName: nameof(FechaNacimiento), value: fechaNacimiento, exceptions: ref exceptions);
        IsPropertyValid(propertyName: nameof(Genero), value: genero, exceptions: ref exceptions);
        IsPropertyValid(propertyName: nameof(NombreEstado), value: nombreEstado, exceptions: ref exceptions);
        IsPropertyValid(propertyName: nameof(CurpGenerada), value: curpGenerada, exceptions: ref exceptions);
        IsPropertyValid(propertyName: nameof(TipoCheckton), value: tipoCheckton, exceptions: ref exceptions);
        IsPropertyValid(propertyName: nameof(NombreServicioCliente), value: nombreServicioCliente, exceptions: ref exceptions);
        // If there are exceptions, throw them
        if (exceptions.Count > 0) throw new EMGeneralAggregateException(exceptions: exceptions);
        // Seteo de propiedades
        this.Nombre = nombre;
        this.PrimerApellido = primerApellido;
        this.SegundoApellido = segundoApellido;
        this.FechaNacimiento = fechaNacimiento;
        this.Genero = genero;
        this.NombreEstado = nombreEstado;
        this.CurpGenerada = curpGenerada;
        this.TipoCheckton = tipoCheckton;
        this.NombreServicioCliente = nombreServicioCliente;
    }

    
}


