namespace Ejercicio4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Dame la primera altitud: ");
            int PrimeraAltitud = int.Parse(Console.ReadLine());
            Console.WriteLine("Dame la segunda altitud: ");
            int SegundaAltitud = int.Parse(Console.ReadLine());
            int Diferencia = PrimeraAltitud - SegundaAltitud;
            Console.WriteLine("Esta es la ecalidad: " + Diferencia);
        }
    }
}