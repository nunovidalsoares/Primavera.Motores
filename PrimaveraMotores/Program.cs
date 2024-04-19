using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PrimaveraMotor
{
    class Program
    {
        static void Main(string[] args)
        {
            //Este handler tem que ser adicionado antes de existir qualquer referência para classes existentes.
            //isto é, no método Main() da aplicação NÃO PODERÁ EXISTIR DECLARAÇÕES DE VARIÁVEIS DE TIPOS EXISTENTES NAS Assemblies.
            //Com este método, na pasta da aplicação não deverão existir as assemblies e as referências para os mesmos deverão ser
            //adicionadas com Copy Local = False e Specific Version = false.
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            //executar o método de teste

            new App().Main();
        }

        static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string assemblyFullName;

            System.Reflection.AssemblyName assemblyName;

            assemblyName = new System.Reflection.AssemblyName(args.Name);
            assemblyFullName = System.IO.Path.Combine(Environment.GetEnvironmentVariable("PERCURSOSGE100", EnvironmentVariableTarget.Machine), assemblyName.Name + ".dll");

            if (System.IO.File.Exists(assemblyFullName))
                return System.Reflection.Assembly.LoadFile(assemblyFullName);
            else
                return null;
        }
    }
}
