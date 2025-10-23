using Simulacion_Peluqueria.DAL;
using Simulacion_Peluqueria.Models;
using Microsoft.EntityFrameworkCore;

namespace Simulacion_Peluqueria.Services;

public class SimulacionService
{
    private readonly Contexto _context;
    private readonly ILogger<SimulacionService> _logger;

    public SimulacionService(Contexto context, ILogger<SimulacionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ResultadoSimulacion> EjecutarSimulacionAsync(Configuracion config)
    {
        try
        {
            _logger.LogInformation("Iniciando simulacion con semilla: {Semilla}", config.Semilla);

            var resultado = await Task.Run(() => SimularPeluqueria(config));

            var historial = new HistorialSimulacion
            {
                FechaEjecucion = DateTime.Now,
                Configuracion = config,
                Resultado = resultado,
                Semilla = config.Semilla,
                NumeroPeluqueros = config.NumPeluqueros,
                TotalClientes = config.TotClientes,
                LongitudPromedioCola = resultado.Indicadores.LongitudPromedioCola,
                TiempoEsperaPromedio = resultado.Indicadores.TiempoEsperaPromedio,
                UsoPromedioInstalacion = resultado.Indicadores.UsoPromedioInstalacion
            };

            _context.HistorialSimulaciones.Add(historial);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Simulacion completada y guardada con ID: {Id}", historial.Id);

            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al ejecutar simulacion");
            throw;
        }
    }

    private ResultadoSimulacion SimularPeluqueria(Configuracion config)
    {
        var random = new Random(config.Semilla);
        var eventos = new List<EventoSimulacion>();
        var tiempoActual = 0.0;

        var llegadas = GenerarLlegadasClientes(config, random);

        foreach (var llegada in llegadas)
        {
            eventos.Add(new EventoSimulacion
            {
                Cliente = llegada.Nombre,
                TipoEvento = "Llegada",
                Tiempo = llegada.TiempoLlegada
            });
        }

        var cola = new Queue<Cliente>();
        var peluquerosLibres = config.NumPeluqueros;
        var clientesAtendidos = 0;

        while (clientesAtendidos < config.TotClientes)
        {
            var llegadasAhora = llegadas.Where(l => l.TiempoLlegada <= tiempoActual && !l.Atendido).ToList();
            foreach (var cliente in llegadasAhora)
            {
                cola.Enqueue(cliente);
                cliente.EnCola = true;
            }

            while (peluquerosLibres > 0 && cola.Count > 0)
            {
                var cliente = cola.Dequeue();
                var tiempoEspera = tiempoActual - cliente.TiempoLlegada;

                eventos.Add(new EventoSimulacion
                {
                    Cliente = cliente.Nombre,
                    TipoEvento = "Inicio Corte",
                    Tiempo = tiempoActual,
                    TiempoEspera = tiempoEspera
                });

                var tiempoCorte = GenerarTiempoCorte(config, random);

                var tiempoFinCorte = tiempoActual + tiempoCorte;
                eventos.Add(new EventoSimulacion
                {
                    Cliente = cliente.Nombre,
                    TipoEvento = "Fin Corte",
                    Tiempo = tiempoFinCorte,
                    TiempoCorte = tiempoCorte
                });

                cliente.Atendido = true;
                peluquerosLibres--;
                clientesAtendidos++;
            }

            var proximoEvento = eventos
                .Where(e => e.Tiempo > tiempoActual)
                .OrderBy(e => e.Tiempo)
                .FirstOrDefault();

            if (proximoEvento != null)
            {
                tiempoActual = proximoEvento.Tiempo;

                if (proximoEvento.TipoEvento == "Fin Corte")
                {
                    peluquerosLibres++;
                }
            }
            else if (clientesAtendidos < config.TotClientes)
            {
                var proximaLlegada = llegadas.FirstOrDefault(l => !l.Atendido);
                if (proximaLlegada != null)
                {
                    tiempoActual = proximaLlegada.TiempoLlegada;
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        var indicadores = CalcularIndicadores(eventos, config, tiempoActual);

        return new ResultadoSimulacion
        {
            Eventos = eventos.OrderBy(e => e.Tiempo).ToList(),
            Indicadores = indicadores,
            TiempoTotalSimulacion = tiempoActual
        };
    }

    private List<Cliente> GenerarLlegadasClientes(Configuracion config, Random random)
    {
        var llegadas = new List<Cliente>();
        double tiempoAcumulado = 0;

        for (int i = 0; i < config.TotClientes; i++)
        {
            var r = random.NextDouble();
            var tiempoEntreLlegadas = -config.TLlegadas * Math.Log(1 - r);
            tiempoAcumulado += tiempoEntreLlegadas;

            llegadas.Add(new Cliente
            {
                Nombre = $"Cliente {i + 1}",
                TiempoLlegada = tiempoAcumulado
            });
        }

        return llegadas.OrderBy(l => l.TiempoLlegada).ToList();
    }

    private double GenerarTiempoCorte(Configuracion config, Random random)
    {
        var r = random.NextDouble();
        var rango = config.TiempoCorteMax - config.TiempoCorteMin;
        return config.TiempoCorteMin + (rango * r);
    }

    private Indicadores CalcularIndicadores(List<EventoSimulacion> eventos, Configuracion config, double tiempoTotal)
    {
        if (!eventos.Any() || tiempoTotal == 0)
            return new Indicadores();

        var eventosInicioCorte = eventos.Where(e => e.TipoEvento == "Inicio Corte").ToList();
        var eventosFinCorte = eventos.Where(e => e.TipoEvento == "Fin Corte").ToList();

        var tiempoEsperaTotal = eventosInicioCorte.Sum(e => e.TiempoEspera ?? 0);
        var tiempoCorteTotal = eventosFinCorte.Sum(e => e.TiempoCorte ?? 0);

        return new Indicadores
        {
            LongitudPromedioCola = tiempoEsperaTotal / tiempoTotal,
            TiempoEsperaPromedio = tiempoEsperaTotal / config.TotClientes,
            UsoPromedioInstalacion = (tiempoCorteTotal / tiempoTotal) / config.NumPeluqueros
        };
    }

    public async Task<List<HistorialSimulacion>> ObtenerHistorialAsync()
    {
        return await _context.HistorialSimulaciones
            .Include(h => h.Configuracion)
            .Include(h => h.Resultado)
            .ThenInclude(r => r.Eventos)
            .OrderByDescending(h => h.FechaEjecucion)
            .ToListAsync();
    }

    public async Task<HistorialSimulacion> ObtenerSimulacionPorIdAsync(int id)
    {
        return await _context.HistorialSimulaciones
            .Include(h => h.Configuracion)
            .Include(h => h.Resultado)
            .ThenInclude(r => r.Eventos)
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<bool> EliminarSimulacionAsync(int id)
    {
        var simulacion = await _context.HistorialSimulaciones.FindAsync(id);
        if (simulacion != null)
        {
            _context.HistorialSimulaciones.Remove(simulacion);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}