namespace Simulacion_Peluqueria.Models;

public class Configuracion
{
    public int Id { get; set; }
    public int Semilla { get; set; }
    public int NumPeluqueros { get; set; }
    public int TiempoCorteMin { get; set; }
    public int TiempoCorteMax { get; set; }
    public int TLlegadas { get; set; }
    public int TotClientes { get; set; }
}