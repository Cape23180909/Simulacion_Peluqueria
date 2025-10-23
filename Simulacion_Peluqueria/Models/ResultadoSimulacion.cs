namespace Simulacion_Peluqueria.Models;

public class ResultadoSimulacion
{
    public int Id { get; set; }
    public List<EventoSimulacion> Eventos { get; set; } = new();
    public Indicadores Indicadores { get; set; } = new();
    public double TiempoTotalSimulacion { get; set; }
}