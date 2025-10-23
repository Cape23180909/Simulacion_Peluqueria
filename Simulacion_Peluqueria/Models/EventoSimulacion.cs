namespace Simulacion_Peluqueria.Models;

public class EventoSimulacion
{
    public int Id { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public string TipoEvento { get; set; } = string.Empty;
    public double Tiempo { get; set; }
    public double? TiempoEspera { get; set; }
    public double? TiempoCorte { get; set; }
    public int ResultadoSimulacionId { get; set; }
    public ResultadoSimulacion ResultadoSimulacion { get; set; } = null!;
}