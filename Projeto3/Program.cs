﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Projeto3
{
    class Program
    {
        static List<char> letras = new List<char>();
        static List<Estado> estados = new List<Estado>();
        static List<DFAedge> list_dfa = new List<DFAedge>();

        static void Main(string[] args)
        {
            bool repeatProg = true;

            Graph g = geraGraficoTeset();

            if (isAFD(g))
            {
                Graph g2 = miniminiza(g);
            }

            while (repeatProg)
            {
                string regex = "";
                List<string> strings = new List<string>();

                regex = receive();
                string alfabeto = Regex.Replace(regex, "[^A-Za-z0-9]+", "");
                alfabeto += " ";
                Console.WriteLine(alfabeto);
                string posfixa = Converter(regex);

                Graph result = Calcular(posfixa);
                Console.WriteLine("GRAFO AFE\n");
                printGraph(result);

                Graph afd = buildAFD(result);
                Console.WriteLine("GRAFO AFD\n");
                printGraph(afd);

                if (repeatProg = NovaOperacao())
                {
                    Console.Clear();
                    letras = new List<char>();
                    estados = new List<Estado>();
                    list_dfa = new List<DFAedge>();
                    Estado zerador = new Estado();
                    zerador.zeraEstado();
                }
            }
        }

        static string receive()
        {
            string expr = "";
            bool repeat = true;

            while (repeat)
            {
                repeat = false;
                Console.WriteLine("Operadores válidos: '.' '*' '|' '+' ");
                Console.WriteLine("Digite uma expressão regular: ");
                expr = Console.ReadLine().Replace(" ", string.Empty);

                expr = expr.Replace('{', '(');
                expr = expr.Replace('[', '(');
                expr = expr.Replace('}', ')');
                expr = expr.Replace(']', ')');

                for (int i = 0; i <= (expr.Length - 1); i++)
                {
                    char c = expr[i];
                    if (!isValidSimbol(c) && !(c >= 'a' && c <= 'z') && !(c >= 'A' && c <= 'Z'))
                    {
                        repeat = true;
                        Console.WriteLine("\n\tERRO: Entrada inválida!\n");
                    }
                }
            }
            Console.Clear();
            Console.WriteLine("Expressão regular: " + expr);
            return expr;
        }

        static Graph buildInitialGraph(char c)
        {
            Graph g = new Graph("graph_" + c);

            Node node1 = new Node();
            node1.initial = true;
            Node node2 = new Node(node1);
            node2.final = true;

            Edge initial = new Edge(node1, node2, c);
            node1.AddEdge(initial);

            g.AddNode(node1);
            g.AddNode(node2);
            return g;
        }

        static bool isOperator(char c)
        {
            char[] operators = { '.', '|', '*', '+' };
            return operators.Contains(c);
        }

        static bool isValidSimbol(char c)
        {
            char[] operators = { '(', ')', '.', '|', '*', '+' };
            return operators.Contains(c);
        }

        static bool hasOperator(string regex)
        {
            char[] operators = { '.', '|', '*', '+' };
            bool contain = false;
            foreach (char c in operators)
            {
                contain = regex.Contains(c);
            }
            return contain;
        }

        static string Converter(string expr)
        {
            string pos = "";
            Stack<char> s = new Stack<char>();

            expr = expr.Replace('{', '(');
            expr = expr.Replace('[', '(');
            expr = expr.Replace('}', ')');
            expr = expr.Replace(']', ')');

            for (int i = 0; i <= (expr.Length - 1); i++)
            {
                char c = expr[i];
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                {
                    pos += c;
                    letras.Add(c);
                }

                if (isOperator(c))
                {
                    while ((s.Count() > 0) && (priority((char)s.First()) >= priority(c)))
                        pos += (char)s.Pop();
                    s.Push(c);
                }
                if (c == '(')
                {
                    s.Push(c);
                }
                if (c == ')')
                {
                    char x = (char)s.Pop();
                    while (x != '(')
                    {
                        pos += x;
                        x = (char)s.Pop();
                    }
                }
            }

            while (s.Count() > 0)
            {
                pos += (char)s.Pop();
            }

            return pos;
        }

        static int priority(char c)
        {
            switch (c)
            {
                case '(': return 1;
                case '+': return 3;
                case '.': return 2;
                case '|': return 2;
                case '*': return 3;
            }
            return 0;
        }

        static Graph Calcular(string posfixa)
        {
            Stack<Graph> stack = new Stack<Graph>();
            Graph x = null;
            int size = posfixa.Length;
            foreach (char c in posfixa)
            {
                size--;
                if (isOperator(c))
                {
                    if (c != ' ')
                    {
                        if (c == '.' || c == '|')
                        {
                            Graph y = stack.Pop();
                            x = stack.Pop();
                            Graph r = buildGraph(x, y, c);
                            stack.Push(r);
                        }
                        else if (c == '*' || c == '+')
                        {
                            x = stack.Pop();
                            Graph r = buildGraph(x, null, c);
                            stack.Push(r);
                        }
                    }
                }
                else
                {
                    Graph g = buildInitialGraph(c);
                    stack.Push(g);

                    //caso o regex de entrada seja uma letra única
                    if (size == 0)
                    {
                        x = g;
                    }
                }
            }
            return x;
        }

        static Graph buildGraph(Graph x, Graph y, char operation)
        {
            if (operation == '.')
            {
                Node initialNodeX = x.getInitialNode();
                Node initialNodeY = y.getInitialNode();
                Node finalNodeX = x.getFinalNode();
                Node finalNodeY = y.getFinalNode();

                finalNodeX.AddEdge(new Edge(finalNodeX, initialNodeY, ' '));
                finalNodeX.final = false;
                initialNodeY.initial = false;

                foreach (Node n in y.nodes)
                {
                    x.AddNode(n);
                }
                addStartEnd(x);
            }
            else if (operation == '|')
            {
                Node initial = new Node();
                initial.initial = true;

                initial.AddEdge(new Edge(initial, x.getInitialNode(), ' '));
                initial.AddEdge(new Edge(initial, y.getInitialNode(), ' '));

                x.getInitialNode().initial = false;
                y.getInitialNode().initial = false;

                Node final = new Node();
                final.final = true;

                x.getFinalNode().AddEdge(new Edge(x.getFinalNode(), final, ' '));
                y.getFinalNode().AddEdge(new Edge(y.getFinalNode(), final, ' '));

                x.getFinalNode().final = false;
                y.getFinalNode().final = false;

                x.AddNode(initial);
                x.AddNode(final);

                foreach (Node n in y.nodes)
                {
                    x.AddNode(n);
                }
            }
            else if (operation == '*')
            {
                x.getFinalNode().AddEdge(new Edge(x.getFinalNode(), x.getInitialNode(), ' '));
                addStartEnd(x);
                x.getInitialNode().AddEdge(new Edge(x.getInitialNode(), x.getFinalNode(), ' '));
            }
            else if (operation == '+')
            {
                x.getFinalNode().AddEdge(new Edge(x.getFinalNode(), x.getInitialNode(), ' '));
                addStartEnd(x);
            }
            return x;

        }

        static void addStartEnd(Graph g)
        {
            Node initial = new Node();
            Node final = new Node(g.getFinalNode());

            initial.AddEdge(new Edge(initial, g.getInitialNode(), ' '));
            g.getInitialNode().initial = false;

            g.getFinalNode().AddEdge(new Edge(g.getFinalNode(), final, ' '));
            g.getFinalNode().final = false;

            initial.initial = true;
            final.final = true;

            g.AddNode(initial);
            g.AddNode(final);
        }

        public static List<Node> closure(Node n)
        {
            Queue<Node> fila = new Queue<Node>();
            fila.Enqueue(n);
            List<Node> nodes = new List<Node>();
            while (fila.Count > 0)
            {
                Node atual = fila.Dequeue();
                nodes.Add(atual);
                foreach (Edge e in atual.edges)
                {
                    if (e.value == ' ' && !fila.Contains(e.to))
                    {
                        fila.Enqueue(e.to);
                    }
                }
            }

            return nodes;
        }

        static Estado DFAedge(Estado e, char c)
        {
            List<Node> nodes = new List<Node>();
            Estado nodes_closure = new Estado();

            foreach (Node n in e.nodes)
            {
                nodes.AddRange(edge(n, c));
            }
            foreach (Node n in nodes)
            {
                nodes_closure.nodes.AddRange(closure(n));
            }
            return nodes_closure;
        }

        static List<Node> edge(Node n, char c)
        {
            List<Node> nodes = new List<Node>();
            foreach (Edge e in n.edges)
            {
                if (e.value == c)
                {
                    nodes.Add(e.to);
                }
            }
            return nodes;
        }

        static Graph buildAFD(Graph g)
        {
            Estado estado_inicial = new Estado(closure(g.getInitialNode()));
            //Adiciona o nó inicial S0
            estado_inicial.initial = true;
            estados.Add(estado_inicial);
            List<Estado> novos_estados = new List<Estado>();
            //Adiciona os nós gerados por S0: S1, S2
            estados.AddRange(buildDFAedge(estado_inicial));

            while (!verificarTodosEstados())
            {
                foreach (Estado estado in estados)
                {
                    if (!estado.visited)
                    {
                        novos_estados.AddRange(buildDFAedge(estado));
                    }
                }
                estados.AddRange(novos_estados);
            }
            marcarEstadosFinais();
            return buildGraphAFD();
        }

        static Graph buildGraphAFD()
        {
            Graph graph = new Graph("AFD");
            //adiciona primeiro somente os novos nós (S0, S1, ...)
            foreach (Estado estado in estados)
            {
                Node node = new Node(estado.name);
                if (estado.initial)
                {
                    node.initial = true;
                }
                if (estado.final)
                {
                    node.final = true;
                }
                graph.AddNode(node);
            }
            //adiciona as arestas de cada nó
            foreach (DFAedge edge in list_dfa)
            {
                //verifica se não é um nó que não chega em lugar nenhum
                if (edge.result != null)
                {
                    Node node_from = graph.findNode(edge.estado.name);
                    Node node_to = graph.findNode(edge.result.name);
                    node_from.AddEdge(new Edge(node_from, node_to, edge.simbolo));
                }
            }

            //remove nos sem pai, que nao sejam iniciais (isolados)
            graph.nodes.RemoveAll(n => n.parents.Count() == 0 && !n.initial);
            graph.nodes.RemoveAll(n => n.edges.Count() == 0 && !n.final);
            return graph;
        }

        //verifica se todos os estados da lista já foram processados (calculado o DFAedge)
        static bool verificarTodosEstados()
        {
            foreach (Estado e in estados)
            {
                if (!e.visited)
                {
                    return false;
                }
            }
            return true;
        }

        //monta o DFAedge de um estado para todo o alfabeto e retorna os estados novos que foram gerados
        static List<Estado> buildDFAedge(Estado estado)
        {
            List<Estado> estados_tmp = new List<Estado>();
            foreach (char letra in letras)
            {
                Estado novo = DFAedge(estado, letra);
                Estado verificaEstado = checarSeEstadoExiste(novo);
                if (novo.nodes.Count > 0)
                {
                    if (verificaEstado == null)
                    {
                        estados_tmp.Add(novo);
                    }
                    else
                    {
                        novo = verificaEstado;
                    }
                }
                else
                {
                    novo = null;
                }
                list_dfa.Add(new DFAedge(estado, letra, novo));
            }
            estado.visited = true;
            return estados_tmp;
        }

        static void marcarEstadosFinais()
        {
            foreach (Estado estado in estados)
            {
                foreach (Node n in estado.nodes)
                {
                    if (n.final)
                    {
                        estado.final = true;
                    }
                }
            }
        }

        static Estado checarSeEstadoExiste(Estado estado)
        {
            int aux = 0;
            foreach (Estado e in estados)
            {
                foreach (Node n in e.nodes)
                {
                    if (estado.nodes.Contains(n))
                    {
                        aux++;
                    }
                }
                if (aux == e.nodes.Count() && estado.nodes.Count == e.nodes.Count)
                {
                    return e;
                }
                aux = 0;
            }
            return null;
        }

        static bool NovaOperacao()
        {
            bool repeat = true;
            string repetir = "";

            while (repeat)
            {
                Console.WriteLine($"\nDeseja fazer uma nova operação? (S/N)");
                repetir = Console.ReadLine();

                if (repetir == "S" || repetir == "s")
                {
                    repeat = false;
                    return true;
                }
                else if (repetir == "N" || repetir == "n")
                {
                    repeat = false;
                    return false;
                }
                else
                {
                    repeat = true;
                }
            }
            return false;
        }

        static void printGraph(Graph result)
        {
            Console.WriteLine($"Nome do Grafo: {result.name}\n");
            foreach (Node n in result.nodes)
            {
                Console.WriteLine($"Nó: {n.name}");
                if (n.initial)
                {
                    Console.WriteLine("Nó Inicial");
                }
                if (n.final)
                {
                    Console.WriteLine("Nó Final");
                }
                Console.WriteLine("Pai(s):");
                foreach (Node p in n.parents)
                {
                    Console.WriteLine($"    Nó: {p.name}");
                }
                Console.WriteLine("Aresta(s):");
                foreach (Edge e in n.edges)
                {
                    Console.WriteLine($"    Nó: {e.to.name} - Valor: {e.value}");
                }
                Console.WriteLine("------------------------------------------------");
            }
        }

        static Graph geraGraficoTeset()
        {
            Graph g = new Graph("Mini");

            Node n0 = new Node();
            n0.initial = true;

            Node n1 = new Node();
            Node n2 = new Node();
            Node n3 = new Node();
            Node n4 = new Node();
            n4.final = true;
            Node n5 = new Node();
            n5.final = true;

            Edge e = new Edge(n0, n2, 'a');
            n0.AddEdge(e);

            e = new Edge(n0, n1, 'b');
            n0.AddEdge(e);

            e = new Edge(n1, n1, 'a');
            n1.AddEdge(e);

            e = new Edge(n1, n0, 'b');
            n1.AddEdge(e);

            e = new Edge(n2, n4, 'a');
            n2.AddEdge(e);

            e = new Edge(n2, n5, 'b');
            n2.AddEdge(e);

            e = new Edge(n3, n5, 'a');
            n3.AddEdge(e);

            e = new Edge(n3, n4, 'b');
            n3.AddEdge(e);

            e = new Edge(n4, n3, 'a');
            n4.AddEdge(e);

            e = new Edge(n4, n2, 'b');
            n3.AddEdge(e);

            e = new Edge(n5, n2, 'a');
            n5.AddEdge(e);

            e = new Edge(n5, n3, 'b');
            n5.AddEdge(e);

            return g; 
        }

        // Verifica se o Grafico é um AFD
        static bool isAFD(Graph g)
        {
            return true;
        }

        // Remove os estados inacessíveis
        static Graph removeInacessiveis(Graph g)
        {
            return g;
        }

        // Verifica se o Grafico é uma função programa total
        static bool isTotal(Graph g)
        {
            return true;
        }

        // Todos os estados que não são função programa total devem apontar para o estado d
        static Graph addEstadoD(Graph g)
        {
            Node d = new Node("d");
            Edge e = new Edge(d, d, ' ');
            foreach (char c in letras)
            {
                e = new Edge(d, d, c);
                d.AddEdge(e);
            }
            g.AddNode(d);
            return g;
        }

        //Se não possuir pelo menos um edge com cada letra do alfabeto, apontar para d com cada letra faltante
        static void apontaParaD(Node n)
        {

        }

        static Graph miniminiza(Graph g)
        {
            Graph g2 = new Graph("Minimizado");

            return g2;
        }


    }
}