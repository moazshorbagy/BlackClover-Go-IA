using BlackClover_Go_IA.monte_carlo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackClover_Go_IA
{
    class Program
    {
        static void Main(string[] args)
        {
            long before = GC.GetTotalMemory(true);

            State s = new State(0);
            DAG graph = new DAG(s);

            Node currNode = graph.root;

            for (int i = 1; i < 10000000; i++)
            {
                s = new State(i);
                currNode.AddChild(new Node(s));
                currNode = currNode.children[0];
            }

            graph.TraverseBFS();

            Console.Write("used memory = ");
            Console.Write((GC.GetTotalMemory(false) - before)/1024L);
            Console.WriteLine(" MB");
        }
    }
}
