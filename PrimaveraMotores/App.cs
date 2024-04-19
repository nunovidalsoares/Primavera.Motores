using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpBS100;
using Primavera.Extensibility.Engine;
using StdBE100;
using StdPlatBS100;
using static StdBE100.StdBETipos;

namespace PrimaveraMotor
{
    class App
    {
        StdPlatBS PSO = new StdPlatBS();
        ErpBS BSO = new ErpBS();

        public void Main()
        {
            string Empresa = "";
            string Utilizador = "";
            string Password = "";
            string Instancia = "default";

            EnumTipoPlataforma objTipoPlataforma = EnumTipoPlataforma.tpEmpresarial;

            if (AbreMotores(pCompany: Empresa, pUser: Utilizador, pPassword: Password, pInstance: Instancia, pTipoPlataforma: objTipoPlataforma))
            {
                Console.WriteLine($"Empresa aberta: {BSO.Contexto.CodEmp}");
            }
            else
            {
                Console.WriteLine($"Erro a abrir empresa");
            }

            Console.WriteLine("Press key to exit...");
            Console.ReadKey(true);
        }

        public bool AbreMotores(string pCompany, string pUser, string pPassword, string pInstance, EnumTipoPlataforma pTipoPlataforma)
        {
            StdBSConfApl objAplConf = new StdBSConfApl();
            
            StdBETransaccao objStdTransac = new StdBETransaccao();

            objAplConf.Instancia = pInstance;
            objAplConf.AbvtApl = "ERP";
            objAplConf.PwdUtilizador = pPassword;
            objAplConf.Utilizador = pUser;
            objAplConf.LicVersaoMinima = "10.00";
            
            try
            {
                Console.Write("A abrir Empresa...");
                PSO.AbrePlataformaEmpresa(pCompany, objStdTransac, objAplConf, pTipoPlataforma);
                
                Console.WriteLine("Done");

                if (PSO.Inicializada)
                {
                    Console.Write("A abrir Plataforma...");
                    BSO.AbreEmpresaTrabalho(pTipoPlataforma, pCompany, objAplConf.Utilizador, objAplConf.PwdUtilizador, objStdTransac, pInstance);
                    Console.WriteLine("Done");

                    Console.Write("A carregar extensibilidade...");
                    LoadExtensibility();
                    Console.WriteLine("Done");

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        private void LoadExtensibility()
        {
            // Use this service to trigger the API events.
            ExtensibilityService service = new ExtensibilityService();
            // Suppress all message box events from the API.
            // Plataforma.ExtensibilityLogger.AllowInteractivity = false;
            service.Initialize(BSO);
            // Check if service is operational
            if (service.IsOperational)
            {
                // Inshore that all extensions are loaded.
                service.LoadExtensions();
            }
        }
    }
}
