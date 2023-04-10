using Projeto_Construir_Desktops;
using System;
using System.Collections.Generic;

namespace PARCELAMENTOS_EMPRESA.Classes
{
    public class ParcelamentosEmpresa : IEntidade
    {
        public int Id { get; set; }
        public IEnumerable<Empresas> Empresa { get; set; }
        public string Cidade { get; set; }
        public string Atividade { get; set; }
        public string Regime { get; set; }
        public string Parcelamento { get; set; }
        public string Tipo { get; set; }
        public string Parcela { get; set; }
        public DateTime Data { get; set; }
        public IEnumerable<Usuarios> Usuario{ get; set; }
        public string Status { get; set; }
    }
}
