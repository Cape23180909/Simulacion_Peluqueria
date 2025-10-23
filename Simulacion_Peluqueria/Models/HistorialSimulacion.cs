namespace Simulacion_Peluqueria.Models;


public class HistorialSimulacion
{
    public int Id { get; set; }
    public DateTime FechaEjecucion { get; set; }
    public int Semilla { get; set; }
    public int NumeroPeluqueros { get; set; }
    public int TotalClientes { get; set; }
    public double LongitudPromedioCola { get; set; }
    public double TiempoEsperaPromedio { get; set; }
    public double UsoPromedioInstalacion { get; set; }

    // Relaciones
    public int ConfiguracionId { get; set; }
    public Configuracion Configuracion { get; set; } = null!;

    public int ResultadoId { get; set; }
    public ResultadoSimulacion Resultado { get; set; } = null!;
}