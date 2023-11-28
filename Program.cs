using System.Reflection;

namespace first_console_app;

class Program
{
    static void Main(string[] args)
    {
        Game g = new(8, 8);
        g.run();
    }
}
