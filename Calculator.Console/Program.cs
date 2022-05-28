namespace Calculator.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] != "-h" && args[0] != "--help")
            {
                System.Console.WriteLine(@"Usage: {0} [formula]", AppDomain.CurrentDomain.FriendlyName);
                return;
            }
            else if (args.Length == 1)
            {
                System.Console.Write(Core.Calculator.Calculate(args[1]));
            }
            else
            {
                System.Console.Write("Enter formula: ");
                string formula = System.Console.ReadLine();
                System.Console.WriteLine("{0} = {1}", formula, Core.Calculator.Calculate(formula));
            }
        }
    }
}
