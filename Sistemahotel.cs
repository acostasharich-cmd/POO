using System;
using System.Collections.Generic;


var hab101 = new HabitacionHotel(101, "Individual", 50000);
var hab202 = new HabitacionHotel(202, "Doble", 80000);
var hab303 = new HabitacionHotel(303, "Suite", 150000);

var habitaciones = new List<HabitacionHotel> { hab101, hab202, hab303 };

Console.WriteLine("=== ESTADO INICIAL ===");
foreach (var h in habitaciones) Console.WriteLine(h.MostrarInfo());

Console.WriteLine("\n=== SIMULACION ===");
double costo = hab101.Reservar("Juana Perez", 3);
Console.WriteLine($"Reserva 101 Juana Perez 3 noches: ${costo:N0}");

costo = hab202.Reservar("Luis Gomez", 2);
Console.WriteLine($"Reserva 202 Luis Gomez 2 noches: ${costo:N0}");

costo = hab101.Reservar("Otro Cliente", 1);
if (costo == -1) Console.WriteLine("Intento reservar 101 de nuevo: FALLO -> -1");

bool lib = hab101.Liberar();
Console.WriteLine($"Liberar 101: {(lib ? "Exito" : "Ya estaba libre")}");

hab303.AplicarDescuento(10);
Console.WriteLine($"Descuento 10% a 303. Nuevo precio: ${hab303.PrecioPorNoche:N0}");

costo = hab101.Reservar("Carlos Ruiz", 1);
Console.WriteLine($"Reserva 101 Carlos Ruiz 1 noche: ${costo:N0}");

Console.WriteLine("\n=== ESTADO FINAL ===");
foreach (var h in habitaciones) Console.WriteLine(h.MostrarInfo());

int ocupadas = 0;
foreach (var h in habitaciones) if (!h.Disponible) ocupadas++;
Console.WriteLine($"\nFinal: {ocupadas} de {habitaciones.Count} habitaciones ocupadas.");


public class HabitacionHotel
{
    private int numero;
    private string tipo;
    private double precioPorNoche;
    private bool disponible;
    private string clienteActual;

    public int Numero => numero;
    public bool Disponible => disponible;
    public string ClienteActual => clienteActual;

    public string Tipo
    {
        get => tipo;
        set
        {
            if (value == "Individual" || value == "Doble" || value == "Suite")
                tipo = value;
            else
                throw new ArgumentException("Solo: Individual, Doble, Suite");
        }
    }

    public double PrecioPorNoche
    {
        get => precioPorNoche;
        set
        {
            if (value > 0) precioPorNoche = value;
            else throw new ArgumentException("Precio > 0");
        }
    }

    public HabitacionHotel(int numero, string tipo, double precioPorNoche)
    {
        this.numero = numero;
        Tipo = tipo;
        PrecioPorNoche = precioPorNoche;
        disponible = true;
        clienteActual = "";
    }

    public double Reservar(string nombreCliente, int noches)
    {
        if (disponible)
        {
            disponible = false;
            clienteActual = nombreCliente;
            return precioPorNoche * noches;
        }
        return -1;
    }

    public bool Liberar()
    {
        if (!disponible)
        {
            disponible = true;
            clienteActual = "";
            return true;
        }
        return false;
    }

    public string MostrarInfo()
    {
        string estado = disponible ? "Disponible" : $"Ocupada por {clienteActual}";
        return $"Habitacion {numero} | Tipo: {tipo} | Precio: ${precioPorNoche:N0} | Estado: {estado}";
    }

    public void AplicarDescuento(int porcentaje)
    {
        if (porcentaje < 0 || porcentaje > 100) return;
        double nuevo = precioPorNoche - (precioPorNoche * porcentaje / 100.0);
        if (nuevo >= 0) precioPorNoche = nuevo;
    }
}