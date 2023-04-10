using Projeto_Construir_Desktops;

namespace PARCELAMENTOS_EMPRESA.Classes
{
    public class Empresas : IEntidade
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public string NomeEmpresa { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public int? DddFone { get; set; }
        public string Cnpj { get; set; }
        public string EmailSecundario { get; set; }
        public string Municipio { get; set; }
        public int Filial { get; set; }

        public string Tipo { get; set; }

        public string InscricaoMunicipal { get; set; }
        public string TipoEmpresa { get; set; }
        public string Enquadramento { get;set; }
    }
}
