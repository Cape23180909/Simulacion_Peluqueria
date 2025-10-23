namespace Simulacion_Peluqueria.Models;

public class Cliente
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public double TiempoLlegada { get; set; }
    public bool Atendido { get; set; }
    public bool EnCola { get; set; }
}