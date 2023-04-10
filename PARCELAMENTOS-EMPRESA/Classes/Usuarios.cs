using Projeto_Construir_Desktops;

namespace PARCELAMENTOS_EMPRESA
{
    public class Usuarios : IEntidade
    {
        public int Id { get; set; }
        public string NomeUsuario { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public int IdEmpresa { get; set; }
        public string Status { get; set; }
        public string FoneCelular { get; set; }
        public string NovaSenha { get; set; }
        public bool AlterarSenha { get; set; }
    }
}
