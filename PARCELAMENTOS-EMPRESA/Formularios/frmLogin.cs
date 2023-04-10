using PARCELAMENTOS_EMPRESA.Repositorios;
using System;
using System.Windows.Forms;

namespace PARCELAMENTOS_EMPRESA
{
    public partial class FrmLogin : Form
    {
        private RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
        private Criptografia criptografia = new Criptografia();
        public FrmLogin()
        {
            InitializeComponent();
            MapeiaNomeUsuarios();
        }
        private void BtnClose_Click(object sender, EventArgs e) => Application.Exit();

        private void ImgOlhosFechados_Click(object sender, EventArgs e) => VisualizarSenha();

        private void ImgOlhosAberto_Click(object sender, EventArgs e) => NaoVisualizarSenha();

        private void NaoVisualizarSenha()
        {
            txtSenha.UseSystemPasswordChar = true;
            imgOlhosAberto.Visible = false;
            imgOlhosFechados.Visible = true;
        }

        private void VisualizarSenha()
        {
            txtSenha.UseSystemPasswordChar = false;
            imgOlhosAberto.Visible = true;
            imgOlhosFechados.Visible = false;
        }

        private void BtnLogin_Click(object sender, EventArgs e) {
            string senha = criptografia.GerarHashMd5(txtSenha.Text);
            if(repositorioUsuario.Logar(comboBoxUsuarios.Text, senha, txtSenha.Text))
            {
                FrmControleParcelamentos frmControleParcelamentos = new FrmControleParcelamentos(comboBoxUsuarios.Text);
                this.Hide();
                frmControleParcelamentos.Show();
            }
            else
            {
                MessageBox.Show("Usuário ou senha incorreto!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            };
        }
            

        private void Timer1_Tick(object sender, EventArgs e) => 
            label2.Text = DateTime.Now.ToString("HH:mm:ss");

        private void MapeiaNomeUsuarios()
        {
            foreach (String nomeUsuarios in repositorioUsuario.ListaNomeUsuariosWL())
            {
                comboBoxUsuarios.Items.Add(nomeUsuarios);
            }
            comboBoxUsuarios.SelectedIndex = 0;
        }

    }
}
